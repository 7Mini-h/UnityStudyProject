using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    // =============== 게임오버 시 나타나는 UI와 튕기는 효과를 위한 선언 ===============
    public GameObject gameOverUIPoint;

    Transform[] movePoint;

    int step = 1;
    float speed = 0.25f;

    // =============== 게임오버 시 화면을 어둡게 하기위한 선언 ===============
    public Image imageBackgroundAlpha;
    Color color;

    // =============== 게임오버 UI를 장면을 진행하기위한 선언 ===============
    public GameObject f1, f2, f3, f4, f5, f6, f7, f8, f9, f10, f11, f12, f13, f14, f15, f16;
    GameObject go;

    enum State
    {
        s0, s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13
    }

    State state;

    float waitTime = 0;
    float intervalTime = 1.5f;

    public AudioSource audio_blop;
    GameObject Flow(int index)
    {
        switch (index)
        {
            case 1: go = f1; break;
            case 2: go = f2; break;
            case 3: go = f3; break;
            case 4: go = f4; break;
            case 5: go = f5; break;
            case 6: go = f6; break;
            case 7: go = f7; break;
            case 8: go = f8; break;
            case 9: go = f9; break;
            case 10: go = f10; break;
            case 11: go = f11; break;
            case 12: go = f12; break;
            case 13: go = f13; break;
            case 14: go = f14; break;
            case 15: go = f15; break;
            case 16: go = f16; break;
        }
        return go;
    }

    private void Start()
    {
        movePoint = gameOverUIPoint.GetComponentsInChildren<Transform>();
        //for (int i = 0; i < movePoint.Length; i++)
        //{
        //    print($"{movePoint[i]}");
        //    print($"{movePoint[i].position}");
        //}

        // - 값 복사(컴포넌트의 Value를 복사, 주소X)
        color = imageBackgroundAlpha.color;
        // - 복사된 값 변경
        color.a = 0;
        // - 변경한 값 삽입
        imageBackgroundAlpha.color = color;

        for (int i = 1; i < 17; i++)
        {
            Flow(i); go.SetActive(false);
        }

        state = State.s0;
        //{
        //    s0 = false; s1 = false; s2 = false; s3 = false; s4 = false; s5 = false; s6 = false; s7 = false;
        //    s8 = false; s9 = false; s10 = false; s11 = false; s12 = false; s13 = false;
        //}
    }

    // 일단 전부 표시되는 것을 우선의 목적으로 하고 각 시간등을 설정하도록한다.

    // Update is called once per frame
    void Update()
    {

        FadeOutAlpha();

        if (imageBackgroundAlpha.color.a <= 0.47f) return;

        if (state == State.s0)
        {
            UIMove_n_Bounce();
        }
        else if (state == State.s1)
        {
            audio_blop.Stop();
            audio_blop.Play();
            Flow(1); go.SetActive(true);
            Flow(2); go.SetActive(true);
            // 소라게를 먹은 수를 표시
            Flow(3);
            go.GetComponent<Text>().text = $"{PlayerTrigger.instance.count_f1}";
            go.SetActive(true);
            state = State.s2;
        }
        else if (state == State.s2)
        {
            if(waitTime < intervalTime)
            {
                waitTime += 0.02f;
                return;
            }
            audio_blop.Stop();
            audio_blop.Play();
            Flow(4); go.SetActive(true);
            Flow(5); go.SetActive(true);
            // 늑대를 먹은 수 표시
            Flow(6);
            go.GetComponent<Text>().text = $"{PlayerTrigger.instance.count_f2}";
            go.SetActive(true);
            state = State.s3;

            waitTime = 0;
        }
        else if (state == State.s3)
        {
            if (waitTime < intervalTime)
            {
                waitTime += 0.02f;
                return;
            }
            audio_blop.Stop();
            audio_blop.Play();
            Flow(7); go.SetActive(true);
            Flow(8); go.SetActive(true);
            // 허수아비를 먹은 수 표시
            Flow(9);
            go.GetComponent<Text>().text = $"{PlayerTrigger.instance.count_f3}";
            go.SetActive(true);
            state = State.s4;

            waitTime = 0;
        }
        else if (state == State.s4)
        {
            if (waitTime < intervalTime)
            {
                waitTime += 0.02f;
                return;
            }
            audio_blop.Stop();
            audio_blop.Play();
            Flow(10); go.SetActive(true);
            Flow(11); go.SetActive(true);
            // 고기를 먹은 수 표시
            Flow(12);
            go.GetComponent<Text>().text = $"{PlayerTrigger.instance.count_isub}";
            go.SetActive(true);
            state = State.s5;

            waitTime = 0;
        }
        else if (state == State.s5)
        {
            if (waitTime < intervalTime)
            {
                waitTime += 0.02f;
                return;
            }
            audio_blop.Stop();
            audio_blop.Play();
            Flow(13); go.SetActive(true);
            // Score표시
            Flow(14);
            go.GetComponent<Text>().text = $"{ScoreManager.instance.SCORE}";
            go.SetActive(true);
            state = State.s6;

            waitTime = 0;
        }
        else if (state == State.s6)
        {
            if (waitTime < intervalTime)
            {
                waitTime += 0.02f;
                return;
            }
            audio_blop.Stop();
            audio_blop.Play();
            Flow(15); go.SetActive(true);
            // HighScore 표시
            Flow(16);
            go.GetComponent<Text>().text = $"{ScoreManager.instance.HIGHSCORE}";
            go.SetActive(true);
            state = State.s7;
            
            waitTime = 0;
        }
        else if (state == State.s7)
        {
            if (waitTime < intervalTime)
            {
                waitTime += 0.02f;
                return;
            }
            audio_blop.Stop();
            audio_blop.Play();
            GameManager.instance.gameoverUIButtons.SetActive(true);
        }
    }

    void FadeOutAlpha()
    {
        if (PlayerTrigger.instance.isGameOver == true)
        {
            color = imageBackgroundAlpha.color;
            color.a = Mathf.Lerp(color.a, 0.5f, 0.02f);

            imageBackgroundAlpha.color = color;
        }
    }

    void UIMove_n_Bounce()
    {
        if (PlayerTrigger.instance.isGameOver == true)
        {
            if (Vector3.Distance(transform.position, movePoint[step].position) >= 0.1f && (step == 1 || step == 3))
            {
                transform.position += (-Vector3.forward) * 2f * speed;
                if (step == 3) speed += 0.00032f;
                if (Vector3.Distance(transform.position, movePoint[step].position) <= 0.2f)
                {
                    if (step == 1) speed *= 0.06f;
                    step++;
                }
            }


            if (Vector3.Distance(transform.position, movePoint[step].position) >= 0.1f && (step == 2 || step == 4))
            {
                //transform.position += (Vector3.forward) * tempTime * speed;
                transform.position = Vector3.Lerp(transform.position, movePoint[step].position, 0.15f);
                if (Vector3.Distance(transform.position, movePoint[step].position) <= 0.1f)
                {
                    speed *= 0.0001f;
                    step++;
                }
            }

            if (step == 5)
            {
                transform.position += (-Vector3.forward) * 2f * 0.0045f;

                if (Vector3.Distance(transform.position, movePoint[step].position) <= 0.1f)
                {
                    transform.position = movePoint[step].position;
                    state = State.s1;
                }
            }

        }
    }

}
