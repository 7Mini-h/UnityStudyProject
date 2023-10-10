using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    float currentTime, startTimer;

    public GameObject GameoverUI;
    public Image image_Fader;
    public Text text_Winner;
    public Button button_GoTitle;
    public Button button_Watching;
    public Text text_CurrentPlayerCount;

    GameObject[] PlayerCountForWinner;

    PhotonView PV;

    bool isGameoverAll;
    bool isGameoverOne;

    int preCount;
    int curCount;
    // Start is called before the first frame update
    void Start()
    {
        image_Fader.color = new Color(0, 0, 0, 0);
        GameoverUI.SetActive(false);
        text_Winner.gameObject.SetActive(false);
        button_GoTitle.gameObject.SetActive(false);
        button_Watching.gameObject.SetActive(false);

        PV = GetComponent<PhotonView>();
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        startTimer = 1f;

        isGameoverAll = true;
        isGameoverOne = true;

        PhotonNetwork.AutomaticallySyncScene = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime <= startTimer)
        {
            currentTime += Time.deltaTime;
            return;
        }

        preCount = curCount;
        PlayerCountForWinner = GameObject.FindGameObjectsWithTag("Player");
        text_CurrentPlayerCount.text = PlayerCountForWinner.Length.ToString();

        curCount = PlayerCountForWinner.Length;
        // 전체 게임 오버
        if (PlayerCountForWinner.Length <= 1)
        {
            if (isGameoverAll) PV.RPC("GameOverCall_All", RpcTarget.All);
        }
        else if (preCount != curCount)
        {
            int count = 0;
            for (int i = 0; i < PlayerCountForWinner.Length; i++)
            {
                if (PlayerCountForWinner[i].GetComponent<PhotonView>().IsMine)
                {
                    count++;
                }
            }

            if (count < 1)
            {
                if (isGameoverOne) GameOverCall_One();
            }
        }
        else if (PlayerCountForWinner.Length < 1)
        {
            print("DDDDD");
        }

        if(isGameoverOne == false && Input.GetKeyDown(KeyCode.Escape))
        {
            GameOverCall_One();
        }
    }

    [PunRPC]
    void GameOverCall_All()
    {
        isGameoverAll = false;
        GameoverUI.SetActive(true);
        StopCoroutine("IEFaderOut_GameOverAll");
        StartCoroutine("IEFaderOut_GameOverAll");
    }

    void GameOverCall_One()
    {
        isGameoverOne = false;
        GameoverUI.SetActive(true);
        StopCoroutine("IEFaderOut_GameOverOne");
        StartCoroutine("IEFaderOut_GameOverOne");
    }

    IEnumerator IEFaderOut_GameOverAll()
    {
        Color c = new Color(0, 0, 0);
        for (float i = 0; i < 0.3; i += Time.deltaTime * 0.4f)
        {
            c.a = i;
            image_Fader.color = c;
            yield return new WaitForSeconds(Time.deltaTime * 0.4f);
        }
        c.a = 0.3f;
        image_Fader.color = c;

        text_Winner.gameObject.SetActive(true);
        button_Watching.gameObject.SetActive(false);
        button_GoTitle.gameObject.SetActive(true);
        text_Winner.text = $"{PlayerCountForWinner[0].GetComponent<PhotonView>().Owner.NickName} Win!!";
    }
    IEnumerator IEFaderOut_GameOverOne()
    {
        Color c = new Color(0, 0, 0);
        for (float i = 0; i < 0.3; i += Time.deltaTime * 0.4f)
        {
            c.a = i;
            image_Fader.color = c;
            yield return new WaitForSeconds(Time.deltaTime * 0.4f);
        }
        c.a = 0.3f;
        image_Fader.color = c;

        text_Winner.gameObject.SetActive(true);
        button_Watching.gameObject.SetActive(true);
        button_GoTitle.gameObject.SetActive(true);
        text_Winner.text = $"You die";
    }

    // 타이틀로 버튼 클릭 시 동작 할 내용
    public void OnClickGoTitle()
    {
        StartCoroutine("DisconnectAndLoad");
        //PhotonNetwork.LoadLevel(0);
    }
    
    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected) yield return null;
        SceneManager.LoadScene(0);
    }

    // 관전하기 버튼 클릭 시 동작 할 내용
    public void OnClickWatching()
    {
        image_Fader.color = new Color(0, 0, 0, 0);

        text_Winner.gameObject.SetActive(false);
        button_GoTitle.gameObject.SetActive(false);
        button_Watching.gameObject.SetActive(false);
        GameoverUI.SetActive(false);
    }
}
