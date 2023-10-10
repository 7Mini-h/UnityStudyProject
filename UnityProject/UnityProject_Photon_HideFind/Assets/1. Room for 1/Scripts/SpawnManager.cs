using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    public GameObject prefabNpc;
    public GameObject prefabPlayer;
    Transform[] spawnPoints;

    public int NpcCount;

    GameObject dontDestroy_dataCarrier;

    PhotonView PV;

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;

        dontDestroy_dataCarrier = GameObject.FindWithTag("DontDestroy");
        if (dontDestroy_dataCarrier != null)
        {
            NpcCount = dontDestroy_dataCarrier.GetComponent<Data_DontDestory>().npcCount;
        }
        else
        {
            NpcCount = 0;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        int rand;
        spawnPoints = GetComponentsInChildren<Transform>();

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < NpcCount; i++)
            {
                rand = Random.Range(1, spawnPoints.Length);
                GameObject npc = PhotonNetwork.InstantiateRoomObject("G_cat_NPC", spawnPoints[rand].position, Quaternion.identity);
                //GameObject npc = PhotonNetwork.Instantiate("G_cat_NPC", spawnPoints[rand].position, Quaternion.identity);
                //GameObject npc = Instantiate(prefabNpc, spawnPoints[rand].position,Quaternion.identity);
            }
        }
        rand = Random.Range(1, spawnPoints.Length);
        GameObject player = PhotonNetwork.Instantiate("G_cat_Player", spawnPoints[rand].position, Quaternion.identity);

    }
}
