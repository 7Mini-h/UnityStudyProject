using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendIndicateArrow : MonoBehaviour
{
    public static FriendIndicateArrow instance;

    private void Awake()
    {
        instance = this;
    }

    // 방향이 위쪽인지 아래쪽인지를 구분하기 위한 bool타입변수
    bool isDown = true;

    // 현재방향을 나타내기위한 변수
    Vector3 dir;
    // 현재 이동하고있는 거리가 얼마나 되는지 비교하기위한 변수
    Vector3 temp;

    // 이동 속도
    public float speed = 2f;

    void Start()
    {
        temp = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라에서 보면 UI가 계속 앞이 보이게끔 설정
        Billboard();

        if (Friends.instance.friends == null) return;

        Vector3 refX = Friends.instance.friends.transform.position;

        transform.position = new Vector3(refX.x, transform.position.y, transform.position.z);

        // 현재 UI 이동방향 설정
        if (isDown)
            dir = Vector3.back;
        else
            dir = Vector3.forward;

        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(temp, transform.position) > 1)
        {
            isDown = !isDown;
            temp = transform.position;
        }

    }


    void Billboard()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
