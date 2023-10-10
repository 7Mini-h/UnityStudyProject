using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 처음 시작할 때 Enemy의 체력을 정해주고
// 총알에 부딯혔을 때, 총알의 데미지만큼 HP를 깎는다
// 만약 HP가 0이 되거나 더 작아지면 HP를 0으로 만들고 사라진다.

public class Enemy : MonoBehaviour
{
    GameObject target;

    public float enemyHP;
    public int attackDamage;

    Vector3 dir;
    public float speed = 0.8f;

    float distance2Player = 3f;

    float currentTime = 0;
    float attackTime = 2f;

    enum State
    {
        Move,
        Attack,
        Die,
        Stop
    }

    State state;

    Animator[] enemyAC;

    bool enemyDie;
    private void Start()
    {
        // 처음 시작할 때 Enemy의 체력을 정해주고
        enemyHP = 100;
        state = State.Move;
        enemyAC = GetComponentsInChildren<Animator>();
        enemyDie = false;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Move: EnemyMove(); break;
            case State.Attack: EnemyAttack(); break;
            case State.Die: EnemyDie(); break;
            case State.Stop: EnemyStop(); break;
        }
        currentTime += Time.deltaTime;
    }

    private void EnemyMove()
    {
        StopCoroutine("IEIceGun");
        dir = target.transform.position - transform.position;
        dir.Normalize();

        transform.position += dir * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.transform.position) < distance2Player)
        {
            state = State.Attack;
            enemyAC[0].SetBool("IsAttack", true);
        }
    }

    private void EnemyAttack()
    {
        StopCoroutine("IEIceGun");
        if (Vector3.Distance(transform.position, target.transform.position) > distance2Player)
        {
            state = State.Move;
        }
        if (currentTime >= attackTime)
        {
            HitPlayer(attackDamage);
            currentTime = 0f;

            // 플레이어 HP가 0이되면 죽음
            if (PlayerHP.instance.playerCurrentHp <= 0 && PlayerHP.instance.isAlive == true)
            {
                PlayerHP.instance.PlayerDie();
                PlayerHP.instance.isAlive = false;
            }
        }
    }

    private void HitPlayer(int damage)
    {
        print("공격 중");
        PlayerHP.instance.Damaged(damage);
    }

    private void EnemyDie()
    {
        // 죽는 조건은 OntriggerEnter에서 구현 됨
        // 죽는 순간 제자리에 멈춤
        transform.position = transform.position;
        GetComponent<Collider>().enabled = false;
    }

    bool isIceStop = false;
    State beforeIceState;
    private void EnemyStop()
    {
        if (isIceStop)
        {
            StartCoroutine("IEIceGun");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 기본 총알에 부딯혔을 때
        if (other.gameObject.CompareTag("Bullet_Basic"))
        {
            // 총알의 데미지만큼 HP를 깎는다
            enemyHP -= Bullet_Basic.bulletDamage;
            Destroy(other.gameObject);
        }

        // 관통탄에 맞았을 때 총알이 파괴되지않고 관통된다
        if (other.gameObject.CompareTag("Bullet_Piercer"))
        {
            // 총알의 데미지만큼 HP를 깎는다
            enemyHP -= Bullet_Basic.bulletDamage;
        }

        // 얼음탄에 맞았을때, 현재 상태에서 멈추고 싶다.
        if (other.gameObject.CompareTag("Bullet_Ice"))
        {
            // 총알의 데미지만큼 HP를 깎는다
            enemyHP -= Bullet_Basic.bulletDamage;
            if (isIceStop == false)
            {
                isIceStop = true;
                beforeIceState = state;
                state = State.Stop;
            }
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Bullet_Lightning"))
        {
            other.GetComponentInParent<Bullet_Lightning>().SpreadDamage(gameObject);
        }

        // 만약 HP가 0이 되거나 더 작아지면 HP를 0으로 만들고 사라진다.
        if (enemyHP <= 0)
        {
            enemyAC[0].StopPlayback();
            StopCoroutine("IEIceGun");
            enemyHP = 0;
            if (enemyDie == false)
            {
                state = State.Die;
                enemyAC[0].SetBool("IsDie", true);
                enemyDie = true;
                Destroy(gameObject, 1.5f);
            }
        }

    }

    internal void GotoPlayer(GameObject player)
    {
        target = player;
    }

    IEnumerator IEIceGun()
    {
        //transform.position = transform.position;
        enemyAC[0].StartPlayback();
        yield return new WaitForSeconds(3);
        enemyAC[0].StopPlayback();
        state = beforeIceState;
        isIceStop = false;
    }

}
