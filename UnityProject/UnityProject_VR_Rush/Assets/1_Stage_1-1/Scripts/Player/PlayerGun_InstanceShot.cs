using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun_InstanceShot : MonoBehaviour
{
    RaycastHit left_hitinfo;
    RaycastHit right_hitinfo;

    public GameObject BulletFactory_L;
    public GameObject BulletFactory_R;

    public Transform left_Hand;
    public Transform right_Hand;

    // Start is called before the first frame update
    void Start()
    {
        left_Hand = Camera.main.transform;
        right_Hand = Camera.main.transform;

        SetBulletDamage(10f);
    }

    // - bool값과 if문을 사용한 다른 기법
    //bool isPressMouseL;

    // - Coroutine을 Playe할 때의 변수 값을 가져오고, 마우스를 뗐을 때 Coroutine을 정지하기 위한 변수
    Coroutine coFireBullet_L;
    Coroutine coFireBullet_R;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //isPressMouseL = true;
            coFireBullet_L = StartCoroutine(FireBullet_L());
        }

        //if(isPressMouseL)
        //{
        //    coFireBullet = StartCoroutine(FireBullet());
        //}

        if (Input.GetMouseButtonUp(0))
        {
            //isPressMouseL = false;
            StopCoroutine(coFireBullet_L);
        }

        if (Input.GetMouseButtonDown(1))
        {
            coFireBullet_R = StartCoroutine(FireBullet_R());
        }

        if (Input.GetMouseButtonUp(1))
        {
            //isPressMouseL = false;
            StopCoroutine(coFireBullet_R);
        }
    }

    IEnumerator FireBullet_L()
    {
        while (true)
        {
            Ray left_ray = new Ray(left_Hand.position, left_Hand.forward);
            if (Physics.Raycast(left_ray, out left_hitinfo))
            {
                GameObject bullet_L = Instantiate(BulletFactory_L);
                bullet_L.transform.position = left_hitinfo.point;
                Destroy(bullet_L, 1f);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator FireBullet_R()
    {
        while (true)
        {
            Ray right_ray = new Ray(right_Hand.position, right_Hand.forward);
            if (Physics.Raycast(right_ray, out right_hitinfo))
            {
                GameObject bullet_R = Instantiate(BulletFactory_R);
                bullet_R.transform.position = right_hitinfo.point;
                Destroy(bullet_R, 1f);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    void SetBulletDamage(float damage)
    {
        Bullet_Basic.bulletDamage = damage;
    }
}
