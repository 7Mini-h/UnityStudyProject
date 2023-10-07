using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend2 : MonoBehaviour
{
    public float speed = 1f;

    int ranNum;

    Vector3 dir;

    public Animator friend2anim;
    // Start is called before the first frame update
    void Start()
    {
        ranNum = Random.Range(0, 2);

        if (ranNum == 0)
        {
            dir = Vector3.left;
        }
        else
        {
            dir = Vector3.right;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 15f || transform.position.x < -15f)
        {
            Destroy(gameObject);
            Friends.instance.InitFriendsCount();
        }

        if (!gameObject.CompareTag("Friend"))
        {
            friend2anim.SetBool("IsTail", true);
            return;
        }
        transform.position += dir * speed * Time.deltaTime;
        transform.LookAt(transform.position + dir);
    }

}
