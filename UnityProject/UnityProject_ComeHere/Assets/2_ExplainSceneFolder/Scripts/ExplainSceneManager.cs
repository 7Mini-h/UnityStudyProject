using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 설명창이 나오고 확인을 누르면 3 → 1 카운트 후 GameScene이 실행된다

public class ExplainSceneManager : MonoBehaviour
{
    public GameObject image;
    public GameObject startCount;

    public AudioSource audio_start;

    public AnimationCurve ac;

    Text text;

    float currentTime;

    bool isCountStart = false;
    // Start is called before the first frame update
    void Start()
    {
        text = startCount.GetComponentInChildren<Text>();

        image.SetActive(true);
        startCount.SetActive(false);
    }

    public void OnClickEnter()
    {
        image.SetActive(false);
        startCount.SetActive(true);

        currentTime = 0f;

        Time.timeScale = 1;

        isCountStart = true;
    }

    private void Update()
    {
        if (!isCountStart) return;

        text.fontSize = (int)ac.Evaluate(currentTime);

        if (currentTime <= 1f)
        {
            text.text = $"3";
        }
        else if (currentTime <= 2f)
        {
            text.text = $"2";
        }
        else if (currentTime <= 3f)
        {
            text.text = $"1";
        }
        else if (currentTime <= 3.5f)
        {
            text.text = $"땅";
            audio_start.Play();
        }
        if (currentTime > 3.5f)
        {
            isCountStart = true;
            SceneManager.LoadScene("GameScene");

        }

        currentTime += Time.deltaTime;

    }

    public void OnClickGoToMenu()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
