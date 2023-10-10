using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    public void OnClickQuestAccept()
    {
        QuestCenterManager.instance.questBoard.SetActive(false);
        QuestCenterManager.instance.StartCoroutine("IEHideBoard", QuestCenterManager.instance.questCG);
        QuestCenterManager.instance.StageBoard.SetActive(true);
        QuestCenterManager.instance.StartCoroutine("IEShowBoard", QuestCenterManager.instance.stageCG);
    }

    public void OnClickStage_Back()
    {
        QuestCenterManager.instance.StageBoard.SetActive(false);
        QuestCenterManager.instance.StartCoroutine("IEHideBoard", QuestCenterManager.instance.stageCG);
        QuestCenterManager.instance.questBoard.SetActive(true);
        QuestCenterManager.instance.StartCoroutine("IEShowBoard", QuestCenterManager.instance.questCG);
    }

    public void OnClickStage1_1()
    {
        StartCoroutine(IEGoStage());
    }

    IEnumerator IEGoStage()
    {
        PlayerMove_InOffice.instance.FadeOut();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("1_Stage_1-1");
    }

    public void OnAdventure()
    {

    }

    public void OnQuit()
    {
        Application.Quit();
    }

}
