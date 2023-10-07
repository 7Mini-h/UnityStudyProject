using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friends : MonoBehaviour
{
    // 싱글턴
    public static Friends instance;

    private void Awake()
    {
        instance = this;
    }

    // 아이템이 생성되는데까지 시간을 세기위한 변수 선언
    public float creatTime = 10;
    float currentTIme;


    // 친구공장
    public GameObject friendsPrefab1;
    public GameObject friendsPrefab2;
    public GameObject friendsPrefab3;

    // 생성되는 친구를 저장하기위한 변수 선언
    public GameObject friends;

    // 친구가 생성 되어있는지 상태를 확인하기위한 변수 선언
    public int friendsCount;

    // 친구를 랜덤하게 출현시키기 위한 랜덤값 설정
    int Ran_fri;

    public GameObject indicateArrowPrefab;
    GameObject indicateArrow;

    private void Start()
    {

    }

    void Update()
    {
        // 친구가 맵상에 아직 있으면 생성하지 않고
        // 친구가 필드에서 사라지면 바로 나온다.
        if (friendsCount <= 0)
        {
            // 친구를 랜덤하게 출현시키기 위한 랜덤값 설정
            Ran_fri = Random.Range(0, 3);
            // 랜덤 값에 맞는 친구를 출현시키기위한 조건문 실행
            switch (Ran_fri)
            {
                case 0:
                    friends = Instantiate(friendsPrefab1);
                    break;
                case 1:
                    friends = Instantiate(friendsPrefab2);
                    break;
                case 2:
                    friends = Instantiate(friendsPrefab3);
                    break;
            }

            // 생성되는 친구의 위치를 -14~14까지 랜덤하게 출현시키기
            int rx = Random.Range(-13, 14);
            int ry = Random.Range(-13, 14);
            friends.transform.position = new Vector3(rx, 0.7f, ry);
            friends.transform.Rotate(new Vector3(0, 180, 0));

            friendsCount++;

            indicateArrow =  Instantiate(indicateArrowPrefab);
            indicateArrow.transform.position = friends.transform.position + new Vector3(0,1,2.5f);
        }
    }

    public void InitFriendsCount()

    {
        Destroy(indicateArrow);
        friendsCount = 0;
    }
}
