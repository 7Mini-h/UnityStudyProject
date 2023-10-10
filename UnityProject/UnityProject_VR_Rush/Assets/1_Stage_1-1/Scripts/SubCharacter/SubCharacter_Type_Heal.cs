using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 컨트롤러의 특정 키를 누르면 플레이어의 HP를 회복시켜줌

public class SubCharacter_Type_Heal : MonoBehaviour
{
    internal void SkillHeal()
    {
        PlayerHP.instance.Healed(10);
    }
}