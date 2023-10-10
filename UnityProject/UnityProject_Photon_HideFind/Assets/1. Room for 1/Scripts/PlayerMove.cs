using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerMove : MonoBehaviourPunCallbacks, IPunObservable //, IInRoomCallbacks
{
    PhotonView PV;

    float speed = 1f;

    Vector3 moveDirection;

    Animator playerAC;

    BoxCollider2D[] PlayerCol;

    Rigidbody2D PlayerRb;

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

    float attackCurrentTime = 0;
    float attackIntervalTime = 2f;

    bool isAttackPosible = false;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        PlayerCol = GetComponentsInChildren<BoxCollider2D>();
        playerAC = GetComponent<Animator>();
        PlayerRb = GetComponent<Rigidbody2D>();
        move = Move.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            attackCurrentTime += Time.deltaTime;
            InputFunction();
            AttackFunction();
            //PV.RPC("AttackFunction", RpcTarget.AllBuffered);


            switch (move)
            {
                case Move.Up:
                    // Y축 + 방향으로 이동
                    moveDirection = Vector3.up;
                    // Animation 변경
                    AnimationReset();
                    playerAC.SetBool("isUp", true);
                    // Colider2D 크기 조정
                    VerticalColider();
                    break;
                case Move.Down:
                    // Y축 - 방향으로 이동
                    moveDirection = Vector3.down;
                    // Animation 변경
                    AnimationReset();
                    playerAC.SetBool("isDown", true);
                    // Colider2D 크기 조정
                    VerticalColider();
                    break;
                case Move.Right:
                    // X축 + 방향으로 이동
                    moveDirection = Vector3.right;
                    // Animation 변경
                    AnimationReset();
                    playerAC.SetBool("isRight", true);
                    // Colider2D 크기 조정
                    HorizonColider();
                    break;
                case Move.Left:
                    // X축 - 방향으로 이동
                    moveDirection = Vector3.left;
                    // Animation 변경
                    AnimationReset();
                    playerAC.SetBool("isLeft", true);
                    // Colider2D 크기 조정
                    HorizonColider();
                    break;
                case Move.Idle:
                    // X축, Y축 고정
                    moveDirection = Vector3.zero;
                    // Animation 변경
                    AnimationReset();
                    playerAC.SetBool("isIdle", true);
                    break;
            }

            PlayerMoving();

        }
        // IsMine이 아닌 것들은 부드럽게 위치 동기화
        else if ((transform.localPosition - currentPos).sqrMagnitude >= 100)
        {
            transform.localPosition = currentPos;
        }

        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, currentPos, Time.deltaTime * 20);
        }
    }

    void InputFunction()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move = Move.Right;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            move = Move.Left;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            move = Move.Up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            move = Move.Down;
        }
        else
        {
            move = Move.Idle;
        }

        // Dash = 스피드 X 2
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed /= 2;
        }
    }


    void PlayerMoving()
    {
        transform.localPosition += moveDirection * speed * Time.deltaTime;
    }
    void AnimationReset()
    {
        playerAC.SetBool("isUp", false);
        playerAC.SetBool("isDown", false);
        playerAC.SetBool("isRight", false);
        playerAC.SetBool("isLeft", false);
        playerAC.SetBool("isIdle", false);
    }

    void VerticalColider()
    {
        for (int i = 0; i < PlayerCol.Length; i++)
        {
            PlayerCol[i].size = new Vector2(0.17f, 0.35f);
        }
    }
    void HorizonColider()
    {
        for (int i = 0; i < PlayerCol.Length; i++)
        {
            PlayerCol[i].size = new Vector2(0.37f, 0.26f);
        }
    }

    GameObject tempContacted;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("NPC"))
        {
            isAttackPosible = true;
            tempContacted = other.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        isAttackPosible = false;
        tempContacted = null;
    }

    // [ContextMenu("Attact")] // 함수를 Instpector 창에서 활용할 수 있게 됨


    void AttackFunction()
    {
        if (Input.GetKeyDown(KeyCode.Space) && attackCurrentTime > attackIntervalTime)
        {
            if (isAttackPosible)
            {
                if (tempContacted.GetComponent<PlayerMove>() != null)
                {
                    tempContacted.GetComponent<PlayerMove>().OnAttacked();
                }
                else if (tempContacted.GetComponent<NpcMove>() != null)
                {
                    tempContacted.GetComponent<NpcMove>().OnAttacked();
                }
                // 공격하려는 상대가 플레이어일때
            }
            //isAttackPosible = false;
            //tempContacted = null;
            attackCurrentTime = 0;
            StopCoroutine("IEAddForce");
            StartCoroutine("IEAddForce");
        }
    }

    IEnumerator IEAddForce()
    {
        PlayerRb.AddForce(Vector2.up, ForceMode2D.Impulse);
        PlayerRb.gravityScale = 1;
        yield return new WaitForSeconds(0.2f);
        PlayerRb.gravityScale = 0;
        PlayerRb.velocity = Vector2.zero;
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


    // Migration

    //public override void OnPlayerLeftRoom(Player otherPlayer)
    //{
    //    print(otherPlayer.NickName);
    //    if (otherPlayer.IsMasterClient)
    //    {       
    //        Player player = PhotonNetwork.CurrentRoom.Players.ToList().Select(kv => kv.Value).First();//First(p => !p.IsMasterClient);
    //        PhotonNetwork.MasterClient.GetNext();
    //        PhotonNetwork.SetMasterClient(player);

    //        GameObject.FindGameObjectsWithTag("NPC")
    //                .Select(NPC => NPC.GetComponent<PhotonView>())
    //                .ToList()
    //                .ForEach(PV => PV.TransferOwnership(player));
    //    }
    //}
}
