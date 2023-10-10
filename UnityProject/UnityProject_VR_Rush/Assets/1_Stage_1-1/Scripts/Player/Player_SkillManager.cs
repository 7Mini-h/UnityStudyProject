using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class Player_SkillManager : MonoBehaviour
{
    public SteamVR_Action_Boolean getButton_R, getButton_L;

    SubCharacter_Type_Heal character_heal_R, character_heal_L;
    SubCharacter_Type_Attack character_attack_R, character_attack_L;

    public GameObject Assistant_R, Assistant_L;
    public Image imageSub_R, imageSub_L;

    public float time_Skill_R = 5f;
    public float time_Skill_L = 3f;

    // 0: R, 1: L
    bool[] IsSkillAct = new bool[2];
    //bool IsSkillAct_R = true;
    //bool IsSkillAct_L = true;

    IEnumerator skRCortine;
    IEnumerator skLCortine;

    enum AssistantType
    {
        Heal,
        Attack,
    }

    AssistantType r_Type, l_Type;


    // Start is called before the first frame update
    void Start()
    {
        Assistant_R = GameObject.Find("AssistantR");
        Assistant_L = GameObject.Find("AssistantL");

        #region #오른쪽 보조캐릭터 타입확인
        if (Assistant_R.CompareTag("Type_Heal"))
        {
            character_heal_R = Assistant_R.GetComponent<SubCharacter_Type_Heal>();
            r_Type = AssistantType.Heal;
        }
        else if (Assistant_R.CompareTag("Type_Attack"))
        {
            character_attack_R = Assistant_R.GetComponent<SubCharacter_Type_Attack>();
            r_Type = AssistantType.Attack;
        }
        #endregion

        #region #왼쪽 보조캐릭터 타입확인
        if (Assistant_L.CompareTag("Type_Heal"))
        {
            character_heal_L = Assistant_L.GetComponent<SubCharacter_Type_Heal>();
            l_Type = AssistantType.Heal;
        }
        else if (Assistant_L.CompareTag("Type_Attack"))
        {
            character_attack_L = Assistant_L.GetComponent<SubCharacter_Type_Attack>();
            l_Type = AssistantType.Attack;
        }
        #endregion

        imageSub_L.fillAmount = 1;
        imageSub_R.fillAmount = 1;
        for (int i = 0; i < IsSkillAct.Length; i++)
        {
            IsSkillAct[i] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSkillAct[0])
        //if(Input.GetKeyDown(KeyCode.X))
        {
            if (getButton_R.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                if (r_Type == AssistantType.Heal)
                {
                    character_heal_R.SkillHeal();
                }
                else if (r_Type == AssistantType.Attack)
                {
                    character_attack_R.SkillAttack();
                }
                skRCortine = SkillCoolTimeCheck(time_Skill_R, 0, imageSub_R);
                StartCoroutine(skRCortine);
            }
        }
        if (IsSkillAct[1])
        {
            if (getButton_L.GetStateDown(SteamVR_Input_Sources.LeftHand))
            //if (Input.GetKeyDown(KeyCode.Z))
            {
                    if (l_Type == AssistantType.Heal)
                    {
                        character_heal_L.SkillHeal();
                    }
                    else if (l_Type == AssistantType.Attack)
                    {
                        character_attack_L.SkillAttack();
                    }
                    skLCortine = SkillCoolTimeCheck(time_Skill_L, 1, imageSub_L);
                    StartCoroutine(skLCortine);
            }
        }
    }
    IEnumerator SkillCoolTimeCheck(float coolTime, int index, Image target)
    {
        target.fillAmount = 0; //스킬을 사용했으므로 fillAmount를 0으로 만듦
        IsSkillAct[index] = false;

        float checkTime = 0;

        while (checkTime <= coolTime)
        {
            checkTime += Time.deltaTime; // 시간증가
            target.fillAmount = checkTime / coolTime; // 0~1사이 값으로 변환
            yield return null;
        }

        IsSkillAct[index] = true;
        target.fillAmount = 1;
    }
}
