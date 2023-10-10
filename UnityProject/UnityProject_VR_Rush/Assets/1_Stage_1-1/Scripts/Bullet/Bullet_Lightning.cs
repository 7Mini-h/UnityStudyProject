using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총알은 생성 후, 컨트롤러의 정면으로 날아간다.
// 총알이 적에게 맞는 순간
// 주변의 적들이 있는지를 확인하고
// 검색된 적들 중, 자기 자신을 제외한 적들에게 튕겨서 데미지를 주고 싶다.


public class Bullet_Lightning : MonoBehaviour
{
    // 들고있는 총 마다 데미지를 다르게 주기위한 싱글턴
    public static Bullet_Lightning instance;

    // 총에서 총알의 데미지를 결정할 수 있도록 함 => PlayerGun_FireShot에서 설정 중임
    public static float bulletDamage;

    TrailRenderer tr;

    public Collider[] cols;

    int hitCount;
    private void Awake()
    {
        instance = this;
    }

    public float speed = 20f;
    Vector3 dir;

    private void Start()
    {
        //dir = Camera.main.transform.forward;
        tr = GetComponentInChildren<TrailRenderer>();
        tr.enabled = false;

        hitCount = 0;
    }

    internal void ShootBullet(Vector3 direction)
    {
        dir = direction;
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    public void SpreadDamage(GameObject hitEnemy)
    {
        if (hitCount < 2)
        {
            // 2명이상의 적이 있으면 가장 가까운 적을 찾는다 == 맞은적
            int layer = 1 << LayerMask.NameToLayer("Enemy");
            cols = Physics.OverlapSphere(transform.position, 10f, layer);

            if (cols.Length == 1)
            {
                hitEnemy.GetComponent<Enemy>().enemyHP -= Bullet_Basic.bulletDamage;
                Destroy(gameObject);
            }

            else if (cols.Length >= 2)
            {
                int randIndex = UnityEngine.Random.Range(0, cols.Length);

                while (cols[randIndex].gameObject == hitEnemy)
                {
                    randIndex = UnityEngine.Random.Range(0, cols.Length);
                }

                dir = cols[randIndex].transform.position - transform.position;
                dir.Normalize();
                hitEnemy.GetComponent<Enemy>().enemyHP -= Bullet_Basic.bulletDamage;
                hitCount++;
            }
        }
        else if(hitCount >= 2)
        {
            hitEnemy.GetComponent<Enemy>().enemyHP -= Bullet_Basic.bulletDamage;
            Destroy(gameObject);
        }
    }
}
