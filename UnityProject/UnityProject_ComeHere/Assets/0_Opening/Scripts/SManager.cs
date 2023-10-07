using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 시작 애니메이션이 지나가고나면 버튼을 활성화하고 아무곳이나 누르세요라는 버튼이 깜빡이도록하고 싶다.

public class SManager : MonoBehaviour
{
    public GameObject buttonNextScene;
    public Text textNextScene;

    public AnimationCurve ac;

    float currentTime;
    float animTime = 10.6f;

    // Start is called before the first frame update
    void Start()
    {
        buttonNextScene.SetActive(false);
        Color c = textNextScene.color;
        c.a = 0f;
        textNextScene.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime <= animTime) return;

        buttonNextScene.SetActive(true);
        Color c = textNextScene.color;
        c.a = ac.Evaluate(Time.time);
        textNextScene.color = c;
    }

    public void OnclickPressAnyWhere()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
