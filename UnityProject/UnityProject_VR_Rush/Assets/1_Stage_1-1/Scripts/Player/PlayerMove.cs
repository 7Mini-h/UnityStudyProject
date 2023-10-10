using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Stage의 특정 Point 마다 멈춰서 Wave를 시작함
// 조건을 만족하면 다음 Wave로 이동
// - 조건1. Wave가 시작되어야 함
// - 조건2. Wave에 나올 적들이 모두 출현해야 함
// - 조건3. Wave시 출현하는 적을 모두 없애야 함

// 각 Stage(Map)마다 Point를 찾아서 매번 할당할 필요 없이 플레이 됨

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    float currentTime;
    float startTime = 5f;

    public GameObject[] aliveEnemies;
    public Transform[] movePoints;

    Vector3 moveDir;

    bool isEnemyCreateStart;
    bool isEnemyCreateDone;

    public Transform cameraRig;
    public Text text_nowState;

    // Fader 관련
    public Image image_fader;

    float fadeInTime;
    float fadeOutTime;

    bool isClear = false;

    enum Wave
    {
        Wave1,
        Wave2,
        Wave3,
        Boss,
        Clear
    }

    Wave wave;

    // Start is called before the first frame update
    void Start()
    {
        wave = Wave.Wave1;
        Player_PanelManager.instance.ChangeWaveNumber("1 / 3");
        movePoints = GameObject.Find("MovePoint").GetComponentsInChildren<Transform>();
        isEnemyCreateStart = false;
        isEnemyCreateDone = false;

        currentTime = 0;

        Color c = image_fader.color;
        c.a = 0;
        image_fader.color = c;
        FadeIn();

        text_nowState.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 시작 후 n초 후에 시작되도록 구현
        currentTime += Time.deltaTime;
        if (currentTime <= startTime)
        {
            if (currentTime < startTime - 3.2)
            {
                text_nowState.text = $"";
            }
            else if (startTime - 3.2f <= currentTime && currentTime < startTime - 3f)
            {
                text_nowState.text = $"S";
            }
            else if (startTime - 3f <= currentTime && currentTime < startTime - 2.8f)
            {
                text_nowState.text = $"St";
            }
            else if (startTime - 2.8f <= currentTime && currentTime < startTime - 2.6f)
            {
                text_nowState.text = $"St";
            }
            else if (startTime - 2.6f <= currentTime && currentTime < startTime - 2.4f)
            {
                text_nowState.text = $"Sta";
            }
            else if (startTime - 2.4f <= currentTime && currentTime < startTime - 2.2f)
            {
                text_nowState.text = $"Stag";
            }
            else if (startTime - 2.2f <= currentTime && currentTime < startTime - 2f)
            {
                text_nowState.text = $"Stage";
            }
            else if (startTime - 2f <= currentTime && currentTime < startTime - 1.8f)
            {
                text_nowState.text = $"Stage S";
            }
            else if (startTime - 1.8f <= currentTime && currentTime < startTime - 1.6f)
            {
                text_nowState.text = $"Stage St";
            }
            else if (startTime - 1.6f <= currentTime && currentTime < startTime - 1.4f)
            {
                text_nowState.text = $"Stage Sta";
            }
            else if (startTime - 1.4f <= currentTime && currentTime < startTime - 1.2f)
            {
                text_nowState.text = $"Stage Star";
            }
            else if (startTime - 1.2f <= currentTime && currentTime < startTime - 1f)
            {
                text_nowState.text = $"Stage Start";
            }
            else if (startTime - 0.6f <= currentTime && currentTime < startTime - 0.4f)
            {
                text_nowState.enabled = false;
            }
            return;
        }


        // ===================== 자동으로 움직이는 코드 ===================== 
        // 계속해서 적이 있는지 검색
        aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // 현재 wave에 따른 코드 구현
        switch (wave)
        {
            case Wave.Wave1: Wave1(); break;
            case Wave.Wave2: Wave2(); break;
            case Wave.Wave3: Wave3(); break;
            case Wave.Boss: break;
            case Wave.Clear: WaveClear(); break;
        }

        // 화면 흔들기
        if (Input.GetButtonDown("Jump"))
        {
            StartCoroutine("ShakeCameraRig");
        }

        #region #키보드로 직접입력 받기
        // ===================== 입력을 받아 움직이는 코드 ===================== 
        /*
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        transform.position += dir * Time.deltaTime * speed;
        */
        #endregion
    }

    int sym4D_state;

    private void Wave1()
    {
        if (Vector3.Distance(movePoints[1].position, transform.position) > 0.1f)
        {
            moveDir = movePoints[1].position - transform.position;
            moveDir.Normalize();
            Sym4DController.instance.OnFan(true);
            sym4D_state = 0;

            transform.position += moveDir * speed * Time.deltaTime;
            if (Vector3.Distance(movePoints[1].position, transform.position) < 0.1f)
            {
                StartCoroutine(IECreateEnemy1());
                Sym4DController.instance.OffFan(true);
                sym4D_state = 1;
            }
        }
        //pointDistance = Vector3.Distance(transform.position, GameObject.Find("Point1").transform.position);
        //transform.position = Vector3.Lerp(transform.position, GameObject.Find("Point1").transform.position, 0.005f);
        if (isEnemyCreateStart == true && isEnemyCreateDone == true && aliveEnemies.Length <= 0)
        {
            isEnemyCreateStart = false;
            isEnemyCreateDone = false;
            wave = Wave.Wave2;
            Player_PanelManager.instance.ChangeWaveNumber("2 / 3");
        }

        if(sym4D_state == 0)
        {
            Sym4DController.instance.Move(0, 0.5f);
        }
        else
        {
            Sym4DController.instance.Move(0, 0);
        }
    }

    private void Wave2()
    {
        if (Vector3.Distance(movePoints[2].position, transform.position) > 0.1f)
        {
            moveDir = movePoints[2].position - transform.position;
            moveDir.Normalize();
            Sym4DController.instance.OnFan(true);
            sym4D_state = 0;

            transform.position += moveDir * speed * Time.deltaTime;
            if (Vector3.Distance(movePoints[2].position, transform.position) < 0.1f)
            {
                StartCoroutine(IECreateEnemy2());
                Sym4DController.instance.OffFan(true);
                sym4D_state = 1;
            }
        }
        if (isEnemyCreateStart == true && isEnemyCreateDone == true && aliveEnemies.Length <= 0)
        {
            isEnemyCreateStart = false;
            isEnemyCreateDone = false;
            wave = Wave.Wave3;
            Player_PanelManager.instance.ChangeWaveNumber("3 / 3");
        }

        if (sym4D_state == 0)
        {
            Sym4DController.instance.Move(0, 0.5f);
        }
        else
        {
            Sym4DController.instance.Move(0, 0);
        }

    }

    private void Wave3()
    {
        if (Vector3.Distance(movePoints[3].position, transform.position) > 0.1f)
        {
            moveDir = movePoints[3].position - transform.position;
            moveDir.Normalize();
            Sym4DController.instance.OnFan(true);
            sym4D_state = 0;

            transform.position += moveDir * speed * Time.deltaTime;
            if (Vector3.Distance(movePoints[3].position, transform.position) < 0.1f)
            {
                StartCoroutine(IECreateEnemy3());
                Sym4DController.instance.OffFan(true);
                sym4D_state = 1;
            }
        }
        if (isEnemyCreateStart == true && isEnemyCreateDone == true && aliveEnemies.Length <= 0)
        {
            isEnemyCreateStart = false;
            isEnemyCreateDone = false;
            isClear = true;
            wave = Wave.Clear;
            Player_PanelManager.instance.ChangeWaveNumber("Clear");
        }

        if (sym4D_state == 0)
        {
            Sym4DController.instance.Move(0, 0.5f);
        }
        else
        {
            Sym4DController.instance.Move(0, 0);
        }
    }
    private void WaveClear()
    {
        if (isClear == true)
        {
            isClear = false;
            FadeOut();
            StartCoroutine(IEWaveClear());
        }
    }

    IEnumerator IECreateEnemy1()
    {
        transform.position = transform.position;
        yield return new WaitForSeconds(2f);
        SpawnManager.instance.CreateEnemyWave1(gameObject);
        isEnemyCreateStart = true;
    }

    IEnumerator IECreateEnemy2()
    {
        transform.position = transform.position;
        yield return new WaitForSeconds(2f);
        SpawnManager.instance.CreateEnemyWave2(gameObject);
        isEnemyCreateStart = true;
    }
    IEnumerator IECreateEnemy3()
    {
        transform.position = transform.position;
        yield return new WaitForSeconds(2f);
        SpawnManager.instance.CreateEnemyWave3(gameObject);
        isEnemyCreateStart = true;
    }

    IEnumerator IEWaveClear()
    {
        text_nowState.enabled = true;
        text_nowState.text = "Stage Clear";
        yield return new WaitForSeconds(2f);
        text_nowState.enabled = false;
        SceneManager.LoadScene("0_OfficeScene");
    }

    internal void EnemyCreateDone()
    {
        isEnemyCreateDone = true;
    }


    IEnumerator ShakeCameraRig()
    {
        Vector3 rigOrigin = cameraRig.position;
        float timeGap = 0.01f;
        for (float t = 0; t <= 2; t += timeGap)
        {
            cameraRig.position = rigOrigin + Random.insideUnitSphere * 0.1f;
            yield return new WaitForSeconds(timeGap);
        }
        cameraRig.position = rigOrigin;
    }

    #region #FadeIn
    public void FadeIn()
    {
        StartCoroutine("IEFadeIn");
    }


    IEnumerator IEFadeIn()
    {
        fadeInTime = 1f;
        Color c = image_fader.color;
        c.a = 1;
        image_fader.color = c;

        float add = 0.01f;
        c = image_fader.color;
        for (float a = 0; a <= fadeInTime; a += add)
        {
            c.a = 1 - (a / fadeInTime);
            image_fader.color = c;
            yield return new WaitForSeconds(add);
        }
        c.a = 0;
        image_fader.color = c;
    }
    #endregion
    #region #FadeOut
    public void FadeOut()
    {
        StartCoroutine("IEFadeOut");
    }

    IEnumerator IEFadeOut()
    {
        fadeOutTime = 1f;
        Color c = image_fader.color;
        c.a = 0;
        image_fader.color = c;

        float add = 0.01f;
        c = image_fader.color;
        for (float a = 0; a <= fadeOutTime; a += add)
        {
            c.a = a / fadeOutTime;
            image_fader.color = c;
            yield return new WaitForSeconds(add);
        }
        c.a = 1;
        image_fader.color = c;
    }
    #endregion

}
