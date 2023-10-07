using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friend3 : MonoBehaviour
{
    public Animator friend3anim;

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.CompareTag("Friend"))
        {
            friend3anim.SetBool("IsTail", true);
            return;
        }
    }
}
