using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Copy_SkillbarControl : MonoBehaviour
{
    public bool[] skActs = new bool[3];

    public Image imageSkill1;
    //bool isSkill1Act = true; // 스킬1이 사용할 수 있으면 true, 사용 못하면 false;
    float Skill1CoolTime = 3f; // 스킬1의 쿨타임
    IEnumerator sk1Cortine;

    public Image imageSkill2;
    //bool isSkill1Act = true; // 스킬2이 사용할 수 있으면 true, 사용 못하면 false;
    float Skill2CoolTime = 5f; // 스킬2의 쿨타임
    IEnumerator sk2Cortine;

    public Image imageSkill3;
    //bool isSkill1Act = true; // 스킬3이 사용할 수 있으면 true, 사용 못하면 false;
    float Skill3CoolTime = 2f; // 스킬3의 쿨타임
    IEnumerator sk3Cortine;

    // Start is called before the first frame update
    void Start()
    {
        imageSkill1.fillAmount = 1;
        imageSkill2.fillAmount = 1;
        imageSkill3.fillAmount = 1;
        for (int i = 0; i < skActs.Length; i++)
        {
            skActs[i] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) // LeftShift를 누르면 스킬 1이 발동해라
        {
            if (skActs[0])
            {
                sk1Cortine = SkillCoolTimeCheck(Skill1CoolTime, 0, imageSkill1);
                // 스킬1을 실행해라
                StartCoroutine(sk1Cortine);
            }
            else
            {
                StopCoroutine(sk1Cortine);
                skActs[0] = true;
                imageSkill1.fillAmount = 1;
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) // E를 누르면 스킬 2이 발동해라
        {
            if (skActs[1])
            {
                sk2Cortine = SkillCoolTimeCheck(Skill2CoolTime, 1, imageSkill2);
                // 스킬2을 실행해라
                StartCoroutine(sk2Cortine);
            }
            else
            {
                StopCoroutine(sk2Cortine);
                skActs[1] = true;
                imageSkill2.fillAmount = 1;
            }
        }

        if (Input.GetMouseButtonDown(1)) // 마우스 우클릭을 누르면 스킬 3이 발동해라
        {
            if (skActs[2])
            {
                sk3Cortine = SkillCoolTimeCheck(Skill3CoolTime, 2, imageSkill3);
                // 스킬3을 실행해라
                StartCoroutine(sk3Cortine);
            }
            else
            {
                StopCoroutine(sk3Cortine);
                skActs[2] = true;
                imageSkill3.fillAmount = 1;
            }
        }
    }

    IEnumerator SkillCoolTimeCheck(float coolTime, int boolindex, Image target)
    {
        target.fillAmount = 0; //스킬을 사용했으므로 fillAmount를 0으로 만듦
        skActs[boolindex] = false;

        float checkTime = 0;

        while (checkTime <= coolTime)
        {
            checkTime += Time.deltaTime; // 시간증가
            target.fillAmount = checkTime / coolTime; // 0~1사이 값으로 변환
            yield return null;
        }

        skActs[boolindex] = true;
    }
}
