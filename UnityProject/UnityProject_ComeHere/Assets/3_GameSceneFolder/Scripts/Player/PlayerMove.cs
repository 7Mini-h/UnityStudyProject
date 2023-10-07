using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ( ○ ) 1. 플레이어가 필드내에서 멈추지 않고 움직인다.

// ( ○ ) 2. 플레이어가 동료와 부딯히면 동료가 플레이어 뒤를 따라온다.
// 중요 Point: 플레이어의 이동경로를 List로 저장한다.

// ( ○ ) 3. 플레이어는 꼬리 동료에게 부딯히면 게임오버

// ( ○ ) 4. 플레이어는 필드 벽에 부딯히면 게임오버


public class PlayerMove : MonoBehaviour
{
    // PlayerTrigger에서 PlayerMove의 정보를 사용할 수 있도록 하게해주기 위한 싱글턴
    public static PlayerMove instance;
    private void Awake()
    {
        instance = this;
    }

    // 플레이어 이동 ==============================================
    // - 원 운동
    public float circle_speed = 6; // 원호
    public float circle_w = 275; // 각속도

    Vector3 dir_straight;
    Vector3 dir_radius;
    //=============================================================

    // 동료의 꼬리물기 ============================================
    // - player의 이동장소를 따라오도록함
    public List<Vector3> pos_player = new List<Vector3>(); // 플레이어의 이동경로를 저장할 List를 생성
    public List<Vector3> rot_player = new List<Vector3>(); // 플레이어의 이동경로의 각도를 저장할 List를 생성
    public List<GameObject> gofriends = new List<GameObject>(); // n번째 동료들이 플레이어 이동경로의 n번 위치에서 따라오도록 따라오게 하기위한 List생성

    // - 플레이어와 동료와의 간격 변수
    public int distancePlayer2Friends = 13;
    public int distancePlayer2Guide = 12;
    int LargeDist;
    // - 플레이어 몸체를 받아오기위한 변수 선언
    public GameObject player;

    // - 컴퓨터 성능에의해 position을 저장하지 않도록 사용자 정의 시간마다 기록하도록 함 -> Fixed Update를 이용해 해결

    // - 방향이 바뀔 때 나오는 소리를 위한 AudioSorce 선언
    AudioSource audio_move;
    private void Start()
    {
        audio_move = gameObject.GetComponent<AudioSource>();

        if (distancePlayer2Friends <= distancePlayer2Guide)
            LargeDist = distancePlayer2Guide;
        else
            LargeDist = distancePlayer2Friends;

        // 처음생성되는 동료가 플레이어 위치에 있으면 이동경로 스택이 없으므로 index오류가나는 것을 방지하기위함
        for (int i = 0; i < (LargeDist * 2); i++)
        {
            pos_player.Insert(0, transform.position);
            rot_player.Insert(0, transform.position);
        }
        gofriends.Add(player);
    }

    private void Update()
    {
        // 1. 플레이어가 필드내에서 멈추지 않고 움직인다.
        CircleMove();
    }

    // 시간을 이용해서 
    private void FixedUpdate()
    {
        // 2. 친구가 플레이어의 이동경로를 따라서 따라온다.

        // 2-1. 친구들이 따라올 플레이어의 이동경로를 쌓음
        pos_player.Insert(0, gameObject.transform.position);
        rot_player.Insert(0, gameObject.transform.eulerAngles);


        // 2-2. 쌓인 경로 값이 너무 커지면 안됨으로 갯수를 제한
        if (pos_player.Count > 10000)
        {
            pos_player.RemoveAt(pos_player.Count - 1);
            rot_player.RemoveAt(rot_player.Count - 1);
        }

        gofriends[0].transform.position = pos_player[distancePlayer2Guide];
        gofriends[0].transform.eulerAngles = rot_player[distancePlayer2Guide];

        // 2-3. 따라오는 친구의 수가 1개 이상이 되었을 때, 따라오도록 함
        if (gofriends.Count > 1)
        {
            // 2-4. 뒤에 붙은 친구들이 모두 이동경로를 밟을 수 있도록 반복
            for (int i = 1; i < gofriends.Count; i++)
            {
                gofriends[i].transform.position = pos_player[(i + 1) * distancePlayer2Friends];
                gofriends[i].transform.eulerAngles = rot_player[(i + 1) * distancePlayer2Friends];
            }
        }
    }

    void CircleMove()
    {
        // 1. 플레이어의 이동 = 직진, 회전 //

        // 1-1. 직진 운동
        dir_straight = Vector3.forward;
        dir_straight = dir_straight.normalized * circle_speed; // 방향 크기의 정규화 * 속도

        // 1-2. 회전 운동
        dir_radius = new Vector3(0, circle_w, 0);

        // 1-3. 원운동 구현 코드
        transform.Translate(dir_straight * Time.deltaTime); // 속도 * 시간
        transform.Rotate(dir_radius * Time.deltaTime); // 각속도 * 시간

        // 1-4. 플레이어의 방향 전환

        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 누를 시 각속도를 조정해 반대방향으로 이동
        {
            circle_w *= (-1);
            audio_move.Stop();
            audio_move.Play();
        }
    }
}
