using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NpcMove : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView PV;

    float speed = 1f;

    Vector3 moveDirection;

    Animator NpcAC;

    BoxCollider2D NpcCol;

    bool isBlocked;

    Vector3 currentPos;

    enum Move
    {
        Up,
        Down,
        Right,
        Left,
        Idle,
    }

    Move move;

    float currentTime;
    float RandMoveTime;
    int RandState;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        NpcCol = GetComponent<BoxCollider2D>();
        NpcAC = GetComponent<Animator>();
        RandState = Random.Range(0, 5);
        isBlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            RandomMoveFunction();
            currentTime += Time.deltaTime;

            switch (move)
            {
                case Move.Up:
                    // Y축 + 방향으로 이동
                    moveDirection = Vector3.up;
                    // Animation 변경
                    AnimationReset();
                    NpcAC.SetBool("isUp", true);
                    // Colider2D 크기 조정
                    VerticalColider();
                    break;
                case Move.Down:
                    // Y축 - 방향으로 이동
                    moveDirection = Vector3.down;
                    // Animation 변경
                    AnimationReset();
                    NpcAC.SetBool("isDown", true);
                    // Colider2D 크기 조정
                    VerticalColider();
                    break;
                case Move.Right:
                    // X축 + 방향으로 이동
                    moveDirection = Vector3.right;
                    // Animation 변경
                    AnimationReset();
                    NpcAC.SetBool("isRight", true);
                    // Colider2D 크기 조정
                    HorizonColider();
                    break;
                case Move.Left:
                    // X축 - 방향으로 이동
                    moveDirection = Vector3.left;
                    // Animation 변경
                    AnimationReset();
                    NpcAC.SetBool("isLeft", true);
                    // Colider2D 크기 조정
                    HorizonColider();
                    break;
                case Move.Idle:
                    // X축, Y축 고정
                    moveDirection = Vector3.zero;
                    // Animation 변경
                    AnimationReset();
                    NpcAC.SetBool("isIdle", true);
                    break;
            }

            PlayerMoving();
        }
        //IsMine이 아닌 것들은 부드럽게 위치 동기화
        else if ((transform.localPosition - currentPos).sqrMagnitude >= 100)
        {
            transform.localPosition = currentPos;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * 20);
        }
    }

    void RandomMoveFunction()
    {
        if (currentTime > RandMoveTime || isBlocked)
        {
            isBlocked = false;
            RandMoveTime = Random.Range(0.5f, 5f);
            RandState = Random.Range(0, 5);

            if (RandState == 0)
            {
                move = Move.Right;
            }
            else if (RandState == 1)
            {
                move = Move.Left;
            }
            else if (RandState == 2)
            {
                move = Move.Up;
            }
            else if (RandState == 3)
            {
                move = Move.Down;
            }
            else
            {
                move = Move.Idle;
            }
            currentTime = 0;
        }

    }

    void PlayerMoving()
    {
        transform.localPosition += moveDirection * speed * Time.deltaTime;
    }
    void AnimationReset()
    {
        NpcAC.SetBool("isUp", false);
        NpcAC.SetBool("isDown", false);
        NpcAC.SetBool("isRight", false);
        NpcAC.SetBool("isLeft", false);
        NpcAC.SetBool("isIdle", false);
    }
    void VerticalColider()
    {
        NpcCol.size = new Vector2(0.17f, 0.35f);
    }
    void HorizonColider()
    {
        NpcCol.size = new Vector2(0.37f, 0.26f);
    }

    private void OnCollision2D(Collision2D other)
    {
        isBlocked = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.localPosition);
        }
        else
        {
            currentPos = (Vector3)stream.ReceiveNext();
        }
    }

    public void OnAttacked()
    {
        PV.RPC("DestroyRPC", RpcTarget.AllBufferedViaServer);
    }

    [PunRPC]
    void DestroyRPC()
    {
        Destroy(gameObject);
    }

}
