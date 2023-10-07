using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 등장 후 2초마다 왼쪽으로 1번 오른쪽으로 1번 왔다갔다 움직이는 친구를 만들고 싶다.

// 현재위치에서 왼쪽으로 출발해 1m 이동 후, 다시 오른쪽으로 1m를 이동한다

public class Friend1 : MonoBehaviour
{
    // 방향이 왼쪽인지 오른쪽인지를 구분하기 위한 bool타입변수
    bool isLeft = true;

    // 현재방향을 나타내기위한 변수
    Vector3 dir;
    // 현재 이동하고있는 거리가 얼마나 되는지 비교하기위한 변수
    Vector3 temp;

    // 이동 속도
    public float speed = 0.5f;

    public Animator friend1anim;

    private void Start()
    {
        temp = transform.position;
    }

    private void Update()
    {
        if (!gameObject.CompareTag("Friend"))
        {
            friend1anim.SetBool("IsTail", true);
            return;
        }
        
        if (isLeft)
            dir = Vector3.left;
        else
            dir = Vector3.right;

        transform.position += dir * speed * Time.deltaTime;

        transform.LookAt(transform.position + dir);

        if (Vector3.Distance(temp, transform.position) > 1)
        {
            isLeft = !isLeft;
            temp = transform.position;
        }
    }
}
