using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 일정시간동안 나타났다 먹지 않으면 없어지는 아이템을 소환하고싶다.
// 아이템 종류: 1. 점수, 2. 동료 차감
// 아이템 확률 : 1: 70%, 2: 30%

// - ItemFactory1
// - ItemFactory2

// - 시작하고 3초뒤에 아이템이 나오게 하고싶다.
// 다음 조건 중 1가지라도 만족하면 코드를 실행한다

// 1. 필드 위의 아이템이 하나도 없다 + 아이템을 먹고 3초 경과

// 2. 필드 위의 아이템을 플레이어가 먹지 않았을 경우 15초 후에 해당 아이템을 없애고 랜덤한 위치에 다시 생성 -> 아이템에서 구현


public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    private void Awake()
    {
        instance = this;
    }

    // 아이템 공장
    public GameObject itemScore;
    public GameObject itemSubtract;

    // 시간 지정을 위한 변수
    float currentTime;
    float intervalTime;
    public float destroyCountTime;
    public float destroyTime;
    public float blinkTime;

    // 현재 필드에 아이템이 있는지 없는지를 확인하기위한 변수 선언
    int countItem;

    // 생성된 아이템을 저장하기위한 변수 선언
    public GameObject item;

    // 아이템 생성에 확률을 부여하기 위한 변수 선언
    int randItem;

    // 필드위에 아이템이 나타날 위치를 랜덤하게 나타내는 변수 선언
    int ranX, ranZ;

    // 아이템을 깜빡이게 하기 위해 필요한 변수들 선언
    float meshRendererTime;
    bool meshOn;
    bool meshOff;

    MeshRenderer[] itemMesh;

    void Start()
    {
        currentTime = 0;
        destroyCountTime = 0;

        intervalTime = 3f;
        destroyTime = intervalTime + 20f;
        blinkTime = destroyTime - 3f;

        meshRendererTime = 0f;

        // 깜빡일때 꺼진는 것 부터 실행
        meshOn = false;
        meshOff = true;
    }

    // Update is called once per frame
    void Update()
    {

        currentTime += Time.deltaTime;

        // 1. 아이템 생성
        // 1-1. 시작 후 3초뒤에 아이템 생성
        // 1-2. 필드 위의 아이템이 하나도 없고, 아이템을 먹고서 3초 이상 지났을 경우
        if (currentTime >= intervalTime && countItem <= 0)
        {
            // 1-3. 확률에 따라 아이템 생산(생산 비율 1번: 70%, 2번: 30%)
            randItem = Random.Range(0, 10);
            if (randItem < 7)
            {
                item = GameObject.Instantiate(itemScore);
                itemMesh = item.GetComponentsInChildren<MeshRenderer>();
            }
            else if (7 <= randItem && randItem < 10)
            {
                item = GameObject.Instantiate(itemSubtract);
                itemMesh = item.GetComponentsInChildren<MeshRenderer>();
            }
            // 1-4. 생산된 아이템을 랜덤 위치에 배치
            ranX = Random.Range(-13, 14);
            ranZ = Random.Range(-13, 14);
            item.transform.position = new Vector3(ranX, 0.5f, ranZ);

            countItem++;
        }

        // 생성되고 15초동안 플레이어가 아이템을 먹지 않을 경우
        destroyCountTime += Time.deltaTime;



        // 아이템이 사라지기 3초 전부터 깜빡거린다
        if (destroyCountTime >= blinkTime)
        {
            if (meshOff)
            {
                //item.GetComponentInChildren<MeshRenderer>().enabled = false;

                for (int i = 0; i < itemMesh.Length; i++)
                {
                    itemMesh[i].enabled = false;
                }

                if (meshRendererTime >= 0.3f)
                {
                    meshOff = false;
                    meshOn = true;
                    meshRendererTime = 0;
                }
            }

            else if (meshOn)
            {
                //item.GetComponentInChildren<MeshRenderer>().enabled = true;

                for (int i = 0; i < itemMesh.Length; i++)
                {
                    itemMesh[i].enabled = true;
                }
                if (meshRendererTime >= 0.3f)
                {
                    meshOff = true;
                    meshOn = false;
                    meshRendererTime = 0;
                }
            }

            meshRendererTime += Time.deltaTime;
        }

        // 스스로를 파괴한다.
        if (destroyCountTime >= destroyTime)
        {
            Destroy(item);
            InitItemCount();
        }
    }

    public void InitItemCount()

    {
        currentTime = 0;
        destroyCountTime = 0;
        countItem = 0;
        meshOff = true;
        meshOn = false;
        meshRendererTime = 0;
    }
}
