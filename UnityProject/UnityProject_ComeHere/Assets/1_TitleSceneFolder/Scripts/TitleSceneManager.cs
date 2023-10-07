using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNextScene()
    {
        SceneManager.LoadScene("ExplainScene");
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
