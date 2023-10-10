using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 총알은 생성 후, 컨트롤러의 정면으로 날아간다.

public class Bullet_Ice : MonoBehaviour
{
    // 들고있는 총 마다 데미지를 다르게 주기위한 싱글턴
    public static Bullet_Ice instance;

    // 총에서 총알의 데미지를 결정할 수 있도록 함 => PlayerGun_FireShot에서 설정 중임
    public static float bulletDamage;

    private void Awake()
    {
        instance = this;
    }

    public float speed = 20f;
    Vector3 dir;

    private void Start()
    {
        //dir = Camera.main.transform.forward;
    }

    internal void ShootBullet(Vector3 direction)
    {
        dir = direction;
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

}
