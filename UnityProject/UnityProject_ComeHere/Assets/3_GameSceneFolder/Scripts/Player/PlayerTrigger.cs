using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public bool isGameOver;

    public static PlayerTrigger instance;

    public AudioSource[] friendsAudios;
    
    private void Awake()
    {
        instance = this;
        isGameOver = false;
    }

    public int count_f1;
    public int count_f2;
    public int count_f3;
    public int count_isub;

    public GameObject buttonPause;

    private void Start()
    {
        friendsAudios[5].Play();
        count_f1 = 0;
        count_f2 = 0;
        count_f3 = 0;
        count_isub = 0;
    }

    // 물리충돌이 일어나면 부자연스러운 움직임이 되므로
    // 감지충돌이 일어났을 경우를 사용
    private void OnTriggerEnter(Collider other)
    {
        PlayerMove move = PlayerMove.instance;

        // ※ 2,3번의 위치가 바뀌면 태그를 먼저바꾸고 바뀐상태에서 파괴됨으로 코드 순서를 유의 해야 한다.

        // - 먹은 게임오브젝트 카운트 및 소리 구현
        {
            if (other.gameObject.name.Contains("Friend1"))
            { friendsAudios[0].Play(); count_f1++; print("소라게"); }
            if (other.gameObject.name.Contains("Friend2"))
            { friendsAudios[1].Play(); count_f2++; print("댕댕이"); }
            if (other.gameObject.name.Contains("Friend3"))
            { friendsAudios[2].Play(); count_f3++; print("허수아비"); }
            if (other.gameObject.CompareTag("Item_Subtract"))
            { friendsAudios[3].Play(); count_isub++; print("고기"); }
            if (other.gameObject.CompareTag("Item_Score"))
            { friendsAudios[4].Play();}
        }

        // 3. 꼬리에 따라오는 동료에세 부딯히면 게임오버
        if (other.gameObject.CompareTag("Tail"))
        {
            GameOver();
        }

        // 4. 벽에 부딯히면 게임오버
        if (other.gameObject.CompareTag("Wall"))
        {
            GameOver();
        }

        // 2. 친구와 부딯혔을 경우 친구의 태그가 Tail로 바뀌며 플레이어를 따라오게 됨
        if (other.gameObject.CompareTag("Friend"))
        {
            move.gofriends.Add(other.gameObject);

            other.gameObject.transform.position = move.pos_player[move.gofriends.Count * move.distancePlayer2Friends];
            other.tag = "Tail";

            // 필드에 존재하는 친구 수를 초기화
            Friends.instance.InitFriendsCount();

            // 친구와 부딯히면서 점수가 올라감
            ScoreManager.instance.SCORE += 100;
        }

        if (other.gameObject.CompareTag("Item_Score"))
        {
            // 점수를 10점 증가 시킴
            ScoreManager.instance.SCORE += 10;
            // 아이템을 제거
            Destroy(ItemManager.instance.item);
            // 필드에 존재하는 아이템 수를 초기화
            ItemManager.instance.InitItemCount();
        }
        // 친구 차감 아이템을 먹었을 경우
        if (other.gameObject.CompareTag("Item_Subtract"))
        {
            // move.gofriends.Count가 1명 이하 일때 = 친구가 없을 경우
            if (move.gofriends.Count <= 1) { print("함께하는 동료가 없습니다."); }
            // 현재 친구의 수가 1명 이상이 있을 때
            else
            {
                // 맨 마지막 친구를 리스트에서 삭제하고, 오브젝트를 파괴
                Destroy(move.gofriends[move.gofriends.Count - 1], Time.deltaTime);
                move.gofriends.RemoveAt(move.gofriends.Count - 1);
            }
            // 아이템을 제거
            Destroy(ItemManager.instance.item);
            // 필드에 존재하는 아이템 수를 초기화
            ItemManager.instance.InitItemCount();
        }
    }
    void GameOver()
    {
        print("GameOver...");
        print($"{count_f1},{count_f2},{count_f3},{count_isub}");

        friendsAudios[5].Stop();
        GameObject.Find("PlayerMove").GetComponent<PlayerMove>().enabled = false;

        buttonPause.SetActive(false);

        Time.timeScale = 0;
        ChangeBoolState();
        friendsAudios[6].Play();

        // ======= GameOverManager에서 관리하도록 기능 구현 옮김 =======
        //StartCoroutine(OnGameOverButton());

    }

    void ChangeBoolState()
    {
        isGameOver = true;
    }

    // ======= GameOverManager에서 관리하도록 기능 구현 옮김 =======

    //IEnumerator OnGameOverButton()
    //{
    //    yield return new WaitForSecondsRealtime(3f);
    //    ActiveGameOverButton();
    //}

    //public void ActiveGameOverButton()
    //{
    //    GameManager.instance.gameoverUIButtons.SetActive(true);
    //}

}
