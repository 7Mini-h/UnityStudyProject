using Sym4D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sym4DController : MonoBehaviour
{
    public static Sym4DController instance;
    private void Awake()
    {
        instance = this;
    }

    // 코루틴에서 공용으로 사용될 딜레이 시간
    WaitForSeconds ws = new WaitForSeconds(0.1f);

    // Sym4D 움직임 제어 Port
    int movePort;

    // Sym4D Fan(바람) 제어 Port
    int fanPort;


    void Start()
    {
        StartCoroutine(init());
    }


    IEnumerator init()
    {
        //시트 포트번호 추출
        movePort = Sym4DEmulator.Sym4D_X_Find();
        yield return ws;

        //펜 포트번호 추출
        fanPort = Sym4DEmulator.Sym4D_W_Find();
        yield return ws;

        //Sym4D - X100 COM Port Open 및 컨텐츠 시작을 장치에 전달
        Sym4DEmulator.Sym4D_X_StartContents(movePort);
        yield return ws;

        //시트의 Roll, Pitch의 최대 회전각도(10도, 10도), 100을 입력하면 10도
        Sym4DEmulator.Sym4D_X_SetConfig(100, 100);
        yield return ws;

        Sym4DEmulator.Sym4D_W_StartContents(fanPort);
        //펜의 최대회전수 (Max 100)
        Sym4DEmulator.Sym4D_W_SetConfig(100);
        yield return ws;
    }


    // 현재 게임오브젝트의 각도, Sym4D도 이 각도로 동기화
    public Vector3 rot;
    // 이전 조이스틱 값 저장
    float prevH, prevV;
    // 각 축마다 움직이는 시간 체크
    float timeH, timeV;
    // 각 축이 움직이기 시작하는 초기 값
    float startH, startV;

    // Fan이 켜져있는지 꺼져있는지
    bool isFan;

    void Update()
    {

    }

    public void Move(float x, float y)
    {
        float h = x;
        float v = y;

        // 조이스틱 신호값이 달라지면 시간과 초기 위치를 초기화
        if (prevH != h)
        {
            timeH = 0;
            startH = rot.z;
        }

        if (prevV != v)
        {
            timeV = 0;
            startV = rot.x;
        }

        // 목표 각도까지 일정하게 움직이게 하느 코드
        timeH += Time.deltaTime;
        timeV += Time.deltaTime;

        rot.z = Mathf.Lerp(startH, -10 * h, timeH);
        rot.x = Mathf.Lerp(startV, 10 * v, timeV);

        // Sym4D는 int 값으로 각도를 설정하기 때문에 소수점까지 체크할 필요 없음
        // 현재 물체의 각도와 계산된 각도의 값이 달라지면 Sym4D에게 움직임 명령 
        if ((int)transform.eulerAngles.x != (int)rot.x || (int)transform.eulerAngles.z != (int)rot.z)
        {
            StartCoroutine(SetMotion());
        }

        //// 현재 게임오브젝트도 계산된 각도로 변경
        //transform.eulerAngles = rot;

        // 현재 조이스틱 신호값을 저장
        prevH = h;
        prevV = v;
    }


    IEnumerator SetMotion()
    {
        Sym4DEmulator.Sym4D_X_StartContents(movePort);
        yield return ws;

        // 첫번째 인자 : roll (z축 회전) , 두번째 인자 : pitch (x축 회전)
        Sym4DEmulator.Sym4D_X_SendMosionData((int)rot.z * 10, (int)rot.x * 10);
        yield return ws;
    }



    // 조이스틱 위 버튼
    public void OnFan(bool on_Condition)
    {
        if (on_Condition)
        {
            if (isFan) return;
            isFan = true;
            StartCoroutine(SetFan());
        }

    }

    // 조이스틱 앞 버튼
    public void OffFan(bool off_Condition)
    {
        if (off_Condition)
        {
            if (!isFan) return;
            isFan = false;
            StartCoroutine(SetFan());
        }
    }

    IEnumerator SetFan()
    {
        Sym4DEmulator.Sym4D_W_StartContents(fanPort);
        yield return ws;

        if (isFan)
        {
            Sym4DEmulator.Sym4D_W_SendMosionData(100);
        }
        else
        {
            Sym4DEmulator.Sym4D_W_SendMosionData(0);
        }
    }

    private void OnDestroy()
    {
        Sym4DEmulator.Sym4D_W_EndContents();
        Sym4DEmulator.Sym4D_X_EndContents();
    }
}
