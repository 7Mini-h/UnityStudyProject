using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어가 Map의 특정 포인트에 도착하면 CreateEnemyWave라는 코드를 실행하라는 명령을 함

public class SpawnManager : MonoBehaviour
{
    #region #싱글턴
    public static SpawnManager instance;

    private void Awake()
    {
        instance = this;
        spawnPoints = GetComponentsInChildren<Transform>();
    }
    #endregion

    public GameObject[] enemies;
    public Transform[] spawnPoints;


    // 적들의 위치를 확인는 화살표를 표시하기위한 적 관리
    public GameObject[] enemyManager = new GameObject[50];

    public GameObject indicator;
    public GameObject[] indicatorManager = new GameObject[50];
    
    
    
    
    
    
    internal void CreateEnemyWave1(GameObject player)
    {
        StartCoroutine(IEWave1(player));
    }
    internal void CreateEnemyWave2(GameObject player)
    {
        StartCoroutine(IEWave2(player));
    }
    internal void CreateEnemyWave3(GameObject player)
    {
        StartCoroutine(IEWave3(player));
    }

    IEnumerator IEWave1(GameObject player)
    {
        yield return new WaitForSeconds(1f);

        GameObject enemy1 = Instantiate(enemies[0]);
        enemy1.transform.position = spawnPoints[1].position;
        enemy1.transform.LookAt(player.transform.position);
        enemy1.GetComponent<Enemy>().GotoPlayer(player);

        GameObject enemy2 = Instantiate(enemies[1]);
        enemy2.transform.position = spawnPoints[2].position;
        enemy2.transform.LookAt(player.transform.position);
        enemy2.GetComponent<Enemy>().GotoPlayer(player);

        GameObject enemy3 = Instantiate(enemies[2]);
        enemy3.transform.position = spawnPoints[3].position;
        enemy3.transform.LookAt(player.transform.position);
        enemy3.GetComponent<Enemy>().GotoPlayer(player);

        // 생성 종료
        player.GetComponent<PlayerMove>().EnemyCreateDone();
    }


    IEnumerator IEWave2(GameObject player)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 2; i++)
        {
            GameObject enemy1 = Instantiate(enemies[0]);
            enemy1.transform.position = spawnPoints[4].position;
            enemy1.transform.LookAt(player.transform.position);
            enemy1.GetComponent<Enemy>().GotoPlayer(player);

            GameObject enemy2 = Instantiate(enemies[1]);
            enemy2.transform.position = spawnPoints[5].position;
            enemy2.transform.LookAt(player.transform.position);
            enemy2.GetComponent<Enemy>().GotoPlayer(player);

            GameObject enemy3 = Instantiate(enemies[2]);
            enemy3.transform.position = spawnPoints[6].position;
            enemy3.transform.LookAt(player.transform.position);
            enemy3.GetComponent<Enemy>().GotoPlayer(player);
            yield return new WaitForSeconds(2f);
        }

        // 생성 종료
        player.GetComponent<PlayerMove>().EnemyCreateDone();
    }


    IEnumerator IEWave3(GameObject player)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++)
        {

            GameObject enemy1 = Instantiate(enemies[0]);
            enemy1.transform.position = spawnPoints[7].position;
            enemy1.transform.LookAt(player.transform.position);
            enemy1.GetComponent<Enemy>().GotoPlayer(player);

            GameObject enemy2 = Instantiate(enemies[1]);
            enemy2.transform.position = spawnPoints[8].position;
            enemy2.transform.LookAt(player.transform.position);
            enemy2.GetComponent<Enemy>().GotoPlayer(player);

            GameObject enemy3 = Instantiate(enemies[2]);
            enemy3.transform.position = spawnPoints[9].position;
            enemy3.transform.LookAt(player.transform.position);
            enemy3.GetComponent<Enemy>().GotoPlayer(player);
            yield return new WaitForSeconds(2f);
        }

        // 생성 종료
        player.GetComponent<PlayerMove>().EnemyCreateDone();
    }

}
