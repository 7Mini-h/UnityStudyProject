using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 적으로 부터 공격을 받으며
// HP가 0이 되면 죽음

// - HP
// - 실드

public class PlayerHP : MonoBehaviour
{
    // 보조캐릭터와 적들이 체력 정보에 접근하기위한 Single Turn 기법
    #region #싱글턴
    public static PlayerHP instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    public int playerCurrentHp = 100;
    public int playerMaxHp = 100;
    public int playerShield = 0;

    public bool isAlive;
    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHp = playerMaxHp;
        isAlive = true;
        Player_PanelManager.instance.UpdateHPGage(playerCurrentHp, playerMaxHp);
    }

    // Update is called once per frame
    void Update()
    {

    }
    internal void Damaged(int damage)
    {
        playerCurrentHp -= damage;
        if (playerCurrentHp <= 0)
        {
            playerCurrentHp = 0;
        }
        Player_PanelManager.instance.UpdateHPGage(playerCurrentHp, playerMaxHp);
    }

    internal void Healed(int HealValue)
    {
        playerCurrentHp += HealValue;
        if (playerCurrentHp >= 100)
        {
            playerCurrentHp = 100;
        }
        Player_PanelManager.instance.UpdateHPGage(playerCurrentHp, playerMaxHp);
    }

    public void PlayerDie()
    {
        GetComponent<PlayerMove>().FadeOut();
        StartCoroutine(IEDieWord());
        print("쥬굼");
        if (GameObject.Find("AssistantL").GetComponent<SubCharacter_AutoAttack>().autoAttack != null)
            GameObject.Find("AssistantL").GetComponent<SubCharacter_AutoAttack>().StopAutoAttack();
        if (GameObject.Find("AssistantR").GetComponent<SubCharacter_AutoAttack>().autoAttack != null)
            GameObject.Find("AssistantR").GetComponent<SubCharacter_AutoAttack>().StopAutoAttack();
        GameObject.Find("AssistantL").GetComponent<SubCharacter_AutoAttack>().enabled = false;
        GameObject.Find("AssistantR").GetComponent<SubCharacter_AutoAttack>().enabled = false;
        GetComponent<Player_SkillManager>().enabled = false;
        StartCoroutine(IEPlayerDie());
    }

    IEnumerator IEPlayerDie()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("0_OfficeScene");
    }

    IEnumerator IEDieWord()
    {
        PlayerMove pm = GetComponent<PlayerMove>();
        pm.text_nowState.enabled = true;
        pm.text_nowState.text = $"You Die";

        float fadeInTime = 1f;
        Color c = pm.text_nowState.color;
        c.a = 0;
        pm.text_nowState.color = c;

        float add = 0.01f;
        c = pm.text_nowState.color;
        for (float a = 0; a <= fadeInTime; a += add)
        {
            c.a = a / fadeInTime;
            pm.text_nowState.color = c;
            yield return new WaitForSeconds(add);
        }
        c.a = 1;
        pm.text_nowState.color = c;

    }

}
