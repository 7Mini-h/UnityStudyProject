using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

// 플레이어를 따라다니다가 플레이어가 죽으면 멀리서 바라보고 싶다.

public class CameraMove : MonoBehaviour
{
    GameObject player;

    private void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < objs.Length; i++)
        {
            PhotonView PV = objs[i].GetComponent<PhotonView>();
            if (PV.IsMine)
            {
                player = objs[i];
            }
        }
        
    }

    private void Update()
    {
        if (player != null)
        {
            transform.localPosition = player.transform.localPosition + new Vector3(0, 0, -10);
            Camera.main.orthographicSize = 1.5f;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,new Vector3(0, 0, -10),0.05f);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5, 0.05f);
        }
    }

}
