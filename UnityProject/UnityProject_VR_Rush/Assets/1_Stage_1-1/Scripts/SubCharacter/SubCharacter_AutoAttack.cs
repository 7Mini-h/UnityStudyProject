using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 1초 주기로 반경 10m 범위내의 적들중 가장 가까운 적을 공격하는 것을 반복
// 플레이어가 컨트롤러의 특정 키를 누르면 플레이어의 HP를 회복시켜줌

public class SubCharacter_AutoAttack : MonoBehaviour
{

    public GameObject subBullet;
    public Transform firePoint;

    float distance;
    float temp_distance = float.MaxValue;
    int nearest_Enemy_index;
    Vector3 fireDirection;

    Collider[] enemiesColiders;

    public Coroutine autoAttack;

    bool isAttackStart = false;

    public float skill_attackAccel = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int layer = 1 << LayerMask.NameToLayer("Enemy");
        enemiesColiders = Physics.OverlapSphere(transform.position, 10f, layer);

        if (enemiesColiders != null && enemiesColiders.Length > 0 && isAttackStart == false)
        {
            isAttackStart = true;
            StartCoroutine("IEAutoAttack");
            print("공격시작");
        }
        else if (enemiesColiders.Length <= 0 && isAttackStart == true)
        {
            isAttackStart = false;
            StopCoroutine("IEAutoAttack");
            print("공격멈춤");
        }
    }

    IEnumerator IEAutoAttack()
    {
        while (true)
        {
            for (int i = 0; i < enemiesColiders.Length; i++)
            {
                if (enemiesColiders[i] != null)
                {
                    distance = Vector3.Distance(transform.position, enemiesColiders[i].transform.position);

                    if (distance < temp_distance)
                    {
                        temp_distance = distance;
                        nearest_Enemy_index = i;
                    }
                }
            }
            fireDirection = enemiesColiders[nearest_Enemy_index].gameObject.transform.position - transform.position;
            fireDirection.Normalize();

            GameObject bullet = Instantiate(subBullet);
            bullet.transform.position = firePoint.position;
            bullet.GetComponent<Bullet_Basic>().ShootBullet(fireDirection);
            Destroy(bullet, 3f);
            // 주변 적 검색할 값 초기화
            nearest_Enemy_index = 0;
            temp_distance = float.MaxValue;

            yield return new WaitForSeconds(1f * skill_attackAccel);
        }
    }

    public void StopAutoAttack()
    {
        StopCoroutine("IEAutoAttack");
    }
}
