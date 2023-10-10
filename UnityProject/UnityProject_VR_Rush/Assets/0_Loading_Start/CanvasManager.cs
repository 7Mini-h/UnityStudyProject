using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;

// 유니티 로고가 끝나면 메인 제목이 서서히 FadeIn된 후
// PressAny Button 문구가 깜빡이도록 하고 싶다.

public class CanvasManager : MonoBehaviour
{
    // 1. 시작 Fadein
    public Text text_Title;
    public Text text_Press;
    public AnimationCurve textAC;

    float fadeInTime;

    // 2. Scene 넘기기
    public SteamVR_Action_Boolean key;

    bool isTitleAct;

    // 3. FadeOut
    public Image img_FadeOut;
    float fadeOutTime;
    bool isSceneChangeReady;

    // Start is called before the first frame update
    void Start()
    {
        isSceneChangeReady = true;
        isTitleAct = false;

        Color titleColor = text_Title.color;
        titleColor.a = 0;
        text_Title.color = titleColor;

        Color pressColor = text_Press.color;
        pressColor.a = 0;
        text_Press.color = pressColor;

        Color fader = img_FadeOut.color;
        fader.a = 0;
        img_FadeOut.color = fader;

        TitleFadeIn();
    }

    private void Update()
    {
        if (key.GetStateDown(SteamVR_Input_Sources.Any) && isTitleAct && isSceneChangeReady)
        {
            FadeOut();
        }
    }

    public void TitleFadeIn()
    {
        StartCoroutine("IETitleFadeIn");
    }

    IEnumerator IETitleFadeIn()
    {
        yield return new WaitForSeconds(2f);
        fadeInTime = 2f;
        Color c = text_Title.color;
        c.a = 0;
        text_Title.color = c;

        float add = 0.01f;
        c = text_Title.color;
        for (float a = 0; a <= fadeInTime; a += add)
        {
            c.a = a / fadeInTime;
            text_Title.color = c;
            yield return new WaitForSeconds(add);
        }
        c.a = 1;
        text_Title.color = c;

        isTitleAct = true;

        while (true)
        {
            Color color = text_Press.color;
            color.a = textAC.Evaluate(Time.time);
            text_Press.color = color;

            yield return new WaitForEndOfFrame();
        }
    }

    public void FadeOut()
    {
        // 씬이 2번이상 넘어가는 것을 방지하기위한 isSceneChangeReady 비활성화
        isSceneChangeReady = false;

        StartCoroutine("IEFadeOut");
    }

    IEnumerator IEFadeOut()
    {
        fadeOutTime = 1f;
        Color c = img_FadeOut.color;
        c.a = 0;
        img_FadeOut.color = c;

        float add = 0.01f;
        c = img_FadeOut.color;
        for (float a = 0; a <= fadeOutTime; a += add)
        {
            c.a =a / fadeOutTime;
            img_FadeOut.color = c;
            yield return new WaitForSeconds(add);
        }
        c.a = 1;
        img_FadeOut.color = c;

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("0_OfficeScene");
    }

}
