using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 친구를 먹을때마다 Score를 100점씩 추가 하고 싶다

// - Text
// - 추가하는 기능

public class ScoreManager : MonoBehaviour
{
    // 싱글턴
    public static ScoreManager instance;
    private void Awake()
    {
        ScoreManager.instance = this;
    }

    // property -> score 표시 text 갱신
    int score;
    int highScore;

    // - 점수를 증가시키기 위한 변수
    public Text textScore;
    public Text textHighScore;


    // property : 함수인데 변수처럼 사용할 수 있는 캡슐화 전용 함수다.
    public int SCORE
    {
        get { return score; }
        set
        {
            score = value;
            textScore.text = $"{score}";
            // 1. 만약 score가 highScore보다 크다면
            if (score > highScore)
            {
                // 2. 최고 점수를 score로 갱신하고
                HIGHSCORE = score;
                // 3. 최고점수를 저장하고싶다.
                PlayerPrefs.SetInt("HIGHSCORE", highScore);
            }
        }
    }
    public int HIGHSCORE
    {
        get { return highScore; }
        set
        {
            highScore = value;
            textHighScore.text = $"{highScore}";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 1. 태어날때 점수UI를 " Score: 0" 이라고 표시하고싶다.
        SCORE = 0;

        // 1. 태어날 때 저장되어있는 최고점수 값을 highScore 변수에 넣어주고 싶다.
        HIGHSCORE = PlayerPrefs.GetInt("HIGHSCORE", 0);
    }




    

}
