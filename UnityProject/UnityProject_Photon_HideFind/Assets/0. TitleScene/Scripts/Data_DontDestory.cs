using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_DontDestory : MonoBehaviour
{
    public int npcCount;
    public string nickName;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DataCarry_npcCount(int value)
    {
        npcCount = value;
    }

    public void DataCarry_LocalNickName(string localNickName)
    {
        nickName = localNickName;
    }

}
