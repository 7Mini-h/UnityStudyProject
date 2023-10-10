using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 컨트롤러의 특정 키를 누르면 3초간 공격속도를 2배로 빠르게 함

public class SubCharacter_Type_Attack : MonoBehaviour
{
    internal void SkillAttack()
    {
        StartCoroutine(AttackAccel());
    }

    IEnumerator AttackAccel()
    {
        GetComponent<SubCharacter_AutoAttack>().skill_attackAccel = 0.5f;
        yield return new WaitForSeconds(3f);
        GetComponent<SubCharacter_AutoAttack>().skill_attackAccel = 1f;

    }

}