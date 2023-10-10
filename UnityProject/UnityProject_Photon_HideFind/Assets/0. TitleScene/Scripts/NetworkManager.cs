using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    public GameObject button_Connect;
    public GameObject input_NickName;
    public GameObject text_NickName;

    public GameObject button_CreateRoom;
    public GameObject button_Join;
    public GameObject input_RoomName;
    public GameObject text_RoomName;

    public GameObject toggle_Ready;
    Toggle toggle;
    GameObject toggleObj;
    int toggleCount;

    public GameObject button_Disconnect;
    public GameObject button_Escape;
    public Text text_CurrentRoomCount;
    public Text text_ReadyCount;
    public GameObject button_Start;

    public Text text_State;
    public GameObject UI_NpcCount;
    public Text text_npcCount;
    int npcCount;

    bool isAnnounce = false;

    GameObject dontDestroy_dataCarrier;
    public GameObject prefab_dontDestroy;

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        PhotonNetwork.AutomaticallySyncScene = true;

        dontDestroy_dataCarrier = GameObject.FindWithTag("DontDestroy");
        if (dontDestroy_dataCarrier == null)
        {
            dontDestroy_dataCarrier = Instantiate(prefab_dontDestroy);
            dontDestroy_dataCarrier.transform.position = Vector3.zero;
        }

    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();

        button_Connect.SetActive(true);
        input_NickName.SetActive(true);
        text_NickName.SetActive(false);

        button_CreateRoom.SetActive(false);
        button_Join.SetActive(false);
        input_RoomName.SetActive(false);
        text_RoomName.SetActive(false);
        toggle_Ready.SetActive(false);
        toggle = toggle_Ready.GetComponent<Toggle>();

        button_Disconnect.SetActive(false);
        button_Escape.SetActive(false);
        button_Start.SetActive(false);

        npcCount = 20;
        text_npcCount.text = npcCount.ToString();
        UI_NpcCount.SetActive(false);

        // 한 게임이 끝나고 나면 연결을 끊고 방에서 나온 뒤 재접속 한다.
        if (dontDestroy_dataCarrier.GetComponent<Data_DontDestory>().nickName != "")
        {
            PhotonNetwork.ConnectUsingSettings();
        }


    }

    private void Update()
    {
        if (!isAnnounce)
        {
            text_State.text = PhotonNetwork.NetworkClientState.ToString();
        }

        if (PhotonNetwork.InRoom)
        {
            toggleCount = GameObject.FindGameObjectsWithTag("Toggle").Length;

            string addText;
            text_CurrentRoomCount.text = $"참가 인원 : {PhotonNetwork.CurrentRoom.PlayerCount}명";
            if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            {
                addText = " - 최소 2명의 플레이어가 필요합니다.";
            }
            else
            {
                addText = "";
            }

            text_ReadyCount.text = $"( Ready:  {toggleCount} / {PhotonNetwork.CurrentRoom.PlayerCount + addText})";

            if (PhotonNetwork.IsMasterClient)
            {
                UI_NpcCount.SetActive(true);
                if (toggleCount == PhotonNetwork.CurrentRoom.PlayerCount && 1 < PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    button_Start.SetActive(true);
                }
                else
                {
                    button_Start.SetActive(false);
                }
            }
            else
            {
                print("I'm not Master");
                UI_NpcCount.SetActive(false);
                button_Start.SetActive(false);
            }

        }

    }

    IEnumerator IEAnnounceSomeThing()
    {
        isAnnounce = true;
        text_State.color = Color.red;
        yield return new WaitForSeconds(2f);
        isAnnounce = false;
        text_State.color = Color.yellow;
    }

    #region Button 작동 함수

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings(); // 이 함수가 실행되고 성공하면 OnConnectedToMaster() 콜백함수가 실행됨

        print("서버에 접속 중입니다.");
    }

    public void OnClickDisconnect()
    {
        PhotonNetwork.Disconnect(); // 이 함수가 실행되고 성공하면 OnDisconnected() 콜백함수가 실행됨
        button_Connect.SetActive(true);
        input_NickName.SetActive(true);
        text_NickName.SetActive(false);
        button_CreateRoom.SetActive(false);
        button_Join.SetActive(false);
        input_RoomName.SetActive(false);
        text_RoomName.SetActive(false);
        toggle_Ready.SetActive(false);
        button_Disconnect.SetActive(false);
        button_Escape.SetActive(false);
        button_Join.SetActive(false);
        UI_NpcCount.SetActive(false);

        dontDestroy_dataCarrier.GetComponent<Data_DontDestory>().nickName = "";

        print("서버 연결을 종료하였습니다.");
    }

    public void OnClickCreateRoom()
    {
        if (input_RoomName.GetComponent<InputField>().text == "")
        {
            print("방 제목을 입력하세요.");
            StopCoroutine("IEAnnounceSomeThing");
            StartCoroutine("IEAnnounceSomeThing");
            text_State.text = "생성할 방의 이름을 입력하세요";
        }
        else
        {
            text_RoomName.GetComponent<Text>().text = "방 제목 : " + input_RoomName.GetComponent<InputField>().text;
            PhotonNetwork.CreateRoom(input_RoomName.GetComponent<InputField>().text, new RoomOptions { MaxPlayers = 4 }, null);// 이 함수가 실행되고 성공하면 OnCreateRoom(), OnJoinedRoom() / 실패하면 OnCreateRoomFailed() 콜백함수가 실행됨

            print("방 제목을 저장했습니다.");
        }
    }

    public void OnClickJoinRoom()
    {
        if (input_RoomName.GetComponent<InputField>().text == "")
        {
            print("방 제목을 입력하세요.");
            StopCoroutine("IEAnnounceSomeThing");
            StartCoroutine("IEAnnounceSomeThing");
            text_State.text = "참여할 방의 이름을 입력하세요";
        }
        else
        {
            text_RoomName.GetComponent<Text>().text = "방 제목 : " + input_RoomName.GetComponent<InputField>().text;
            PhotonNetwork.JoinRoom(input_RoomName.GetComponent<InputField>().text, null);// 이 함수가 실행되고 성공하면 OnCreateRoom(), OnJoinedRoom() / 실패하면 OnCreateRoomFailed() 콜백함수가 실행됨

            print("방 제목을 저장했습니다.");
        }

    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        if (toggle.isOn)
        {
            toggle.isOn = false;
        }
    }

    public void OnClickToggle()
    {
        if (toggle.isOn)
        {
            toggleObj = PhotonNetwork.Instantiate("ToggleCounter", Vector3.zero, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Destroy(toggleObj);
        }
    }

    public void OnClickNpcCountMinus()
    {
        PV.RPC("NpcCountMinus", RpcTarget.AllBuffered, null);
    }

    public void OnClickNpcCountPlus()
    {
        PV.RPC("NpcCountPlus", RpcTarget.AllBuffered, null);
    }

    public void OnClickStart()
    {
        print("게임을 시작합니다.");
        if (dontDestroy_dataCarrier.GetComponent<Data_DontDestory>() != null)
        {
            dontDestroy_dataCarrier.GetComponent<Data_DontDestory>().DataCarry_npcCount(npcCount);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("Room for 1");
        }
        //PV.RPC("StartGame", RpcTarget.All);
    }

    #endregion

    public override void OnConnectedToMaster()
    {
        if (input_NickName.GetComponent<InputField>().text == "" && dontDestroy_dataCarrier.GetComponent<Data_DontDestory>().nickName == "")
        {
            PhotonNetwork.Disconnect(); // 이 함수가 실행되고 성공하면 OnDisconnected() 콜백함수가 실행됨

            print("Player 이름을 입력해주세요.");
            StopCoroutine("IEAnnounceSomeThing");
            StartCoroutine("IEAnnounceSomeThing");
            text_State.text = "플레이어 이름을 입력하세요";
        }
        else
        {
            print("서버 접속 완료");

            PhotonNetwork.LocalPlayer.NickName = input_NickName.GetComponent<InputField>().text;

            // 2회차: 게임이 끝나고 마스터 서버로 돌아왔을 때, 닉네임을 저장하고 있다가 값을 부여한다.
            if (dontDestroy_dataCarrier.GetComponent<Data_DontDestory>().nickName != "")
            {
                PhotonNetwork.LocalPlayer.NickName = dontDestroy_dataCarrier.GetComponent<Data_DontDestory>().nickName;
                input_NickName.GetComponent<InputField>().text = dontDestroy_dataCarrier.GetComponent<Data_DontDestory>().nickName;
            }

            dontDestroy_dataCarrier.GetComponent<Data_DontDestory>().DataCarry_LocalNickName(PhotonNetwork.LocalPlayer.NickName);

            input_NickName.SetActive(false);
            text_NickName.SetActive(true);
            text_NickName.GetComponent<Text>().text = "Player : " + PhotonNetwork.LocalPlayer.NickName;
            print("입력한 닉네임이 Local Player의 이름으로 저장되었습니다.");
            button_Connect.SetActive(false);

            button_CreateRoom.SetActive(true);
            button_Join.SetActive(true);
            input_RoomName.SetActive(true);
            text_RoomName.SetActive(false);

            button_Disconnect.SetActive(true);
            button_Escape.SetActive(false);
            toggle_Ready.SetActive(false);
            UI_NpcCount.SetActive(false);

        }
    }

    public override void OnCreatedRoom()
    {
        button_CreateRoom.SetActive(false);
        button_Join.SetActive(false);
        input_RoomName.SetActive(false);

        text_RoomName.SetActive(true);
        toggle_Ready.SetActive(true);

        button_Disconnect.SetActive(false);
        button_Escape.SetActive(true);

        print("방 만들기 성공");
    }
    public override void OnJoinedRoom()
    {
        button_CreateRoom.SetActive(false);
        button_Join.SetActive(false);
        input_RoomName.SetActive(false);

        text_RoomName.SetActive(true);
        toggle_Ready.SetActive(true);

        button_Disconnect.SetActive(false);
        button_Escape.SetActive(true);

        print("방 참가 완료");
    }

    public override void OnLeftRoom()
    {
        print("현재 방을 나갑니다");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("방 만들기에 실패하였습니다." + returnCode.ToString() + message);
        StopCoroutine("IEAnnounceSomeThing");
        StartCoroutine("IEAnnounceSomeThing");
        if (message == "A game with the specified id already exist." || returnCode == 32766)
        {
            text_State.text = "만들려는 이름의 방이 이미 존재합니다.";
        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("방에 참여할 수 없습니다." + returnCode.ToString() + message);
        StopCoroutine("IEAnnounceSomeThing");
        StartCoroutine("IEAnnounceSomeThing");
        if (message == "Game does not exist" || returnCode == 32758)
        {
            text_State.text = "입력하신 이름의 방이 존재 하지 않습니다.";
        }
        else if (message == "Game full" || returnCode == 32765)
        {
            text_State.text = "정원이 초과되었습니다.";
        }
        else if(message == "Game closed" || returnCode == 32764)
        {
            text_State.text = "진행 중인 게임에 참여할 수 없습니다.";
        }
    }

    [PunRPC]
    void NpcCountMinus()
    {
        npcCount--;
        if (npcCount < 0) npcCount = 0;
        text_npcCount.text = npcCount.ToString();
    }

    [PunRPC]
    void NpcCountPlus()
    {
        npcCount++;
        //if (npcCount > 100) npcCount = 100;
        text_npcCount.text = npcCount.ToString();
    }

    //[PunRPC]
    //void StartGame()
    //{
    //    PhotonNetwork.LoadLevel("Room for 1");
    //}



    [ContextMenu("정보")] // 함수를 Instpector 창에서 활용할 수 있게 됨
    void Info()
    {

        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원 수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 인원 최대 인원 수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string PlayerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                PlayerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            }
            print(PlayerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비에 연결되어 있는지? : " + PhotonNetwork.InLobby);
            print("서버에 연결되어 있는지? : " + PhotonNetwork.IsConnected);
        }
    }

}
