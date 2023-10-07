using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameoverUIButtons;
    public GameObject pauseMenuUI;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        gameoverUIButtons.SetActive(false);
        pauseMenuUI.SetActive(false);
    }

    public void OnClickPause()
    {
        Time.timeScale = 0;
        pauseMenuUI.SetActive(true);
    }

    public void OnClickPlay()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
    }

    public void OnClickRestartMenu()
    {
        SceneManager.LoadScene("ExplainScene");
    }
    public void OnClickExit()
    {
        //SceneManager.LoadScene("TitleScene");
        Application.Quit();
    }
    public void OnClickRestart()
    {
        SceneManager.LoadScene("ExplainScene");
    }
    public void OnClickQuit()
    {
        Application.Quit();
    }
    
}
