using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class PlayerGun_FireShot : MonoBehaviour
{
    #region #SteamVR로 부터 가져온 자료
    public SteamVR_Action_Boolean pinch;
    public SteamVR_Action_Boolean switchBulletL;
    public SteamVR_Action_Boolean switchBulletR;
    #endregion

    #region #총알공장
    public GameObject[] BulletL;
    public GameObject[] BulletR;

    GameObject currentBulletFactory_L;
    GameObject currentBulletFactory_R;

    enum BulletMagic
    {
        Basic,
        Ice,
        Lightning
    }
    BulletMagic bulletM;
    int indexM = 0;
    enum BulletWeapon
    {
        Basic,
        Shotgun,
        Piercer
    }
    BulletWeapon bulletW;
    int indexW = 0;
    #endregion

    #region #VR컨트롤러
    public Transform left_Hand;
    public Transform left_Hand_FirePoint;
    public Transform right_Hand;
    public Transform right_Hand_FirePoint;
    #endregion

    #region #총알UI
    public GameObject[] pos_UI;
    public GameObject[] size_Font;
    public Image[] R_Image, L_Image;
    public Text[] R_Text_Cur, R_Text_Max, L_Text_Cur, L_Text_Max, R_Slash, L_Slash;
    #endregion

    #region #R_총알UI
    void R_ChangeBulletUI1(int indexUI)
    {
        // 총알 이미지
        Color c = R_Image[indexUI].color;
        c = new Color(0f, 0f, 0f, 1f);
        R_Image[indexUI].color = c;

        // 총알 개수
        R_Slash[indexUI].text = "∞";
        R_Text_Cur[indexUI].text = "";
        R_Text_Max[indexUI].text = "";
    }
    void R_ChangeBulletUI2(int indexUI)
    {
        if (indexUI > 2)
        {
            if (indexUI == 3)
            {
                indexUI = 0;
            }
            else if (indexUI == 4)
            {
                indexUI = 1;
            }
        }
        // 총알 이미지
        Color c = R_Image[indexUI].color;
        c = new Color(0.5f, 0f, 1f, 1f);
        R_Image[indexUI].color = c;

        // 총알 개수
        R_Slash[indexUI].text = "/";
        R_Text_Cur[indexUI].text = $"5";
        R_Text_Max[indexUI].text = $"5";
    }
    void R_ChangeBulletUI3(int indexUI)
    {
        if (indexUI > 2)
        {
            if (indexUI == 3)
            {
                indexUI = 0;
            }
            else if (indexUI == 4)
            {
                indexUI = 1;
            }
        }
        // 총알 이미지
        Color c = R_Image[indexUI].color;
        c = new Color(0.5f, 0.5f, 0.5f, 1f);
        R_Image[indexUI].color = c;

        // 총알 개수
        R_Slash[indexUI].text = "/";
        R_Text_Cur[indexUI].text = $"10";
        R_Text_Max[indexUI].text = $"10";
    }
    #endregion

    #region #L_총알UI
    void L_ChangeBulletUI1(int indexUI)
    {
        // 총알 이미지
        Color c = L_Image[indexUI].color;
        c = new Color(0f, 0f, 0f, 1f);
        L_Image[indexUI].color = c;

        // 총알 개수
        L_Slash[indexUI].text = "∞";
        L_Text_Cur[indexUI].text = "";
        L_Text_Max[indexUI].text = "";
    }
    void L_ChangeBulletUI2(int indexUI)
    {
        if (indexUI > 2)
        {
            if (indexUI == 3)
            {
                indexUI = 0;
            }
            else if (indexUI == 4)
            {
                indexUI = 1;
            }
        }
        // 총알 이미지
        Color c = L_Image[indexUI].color;
        c = new Color(1f, 1f, 0f, 1f);
        L_Image[indexUI].color = c;

        // 총알 개수
        L_Slash[indexUI].text = "/";
        L_Text_Cur[indexUI].text = $"5";
        L_Text_Max[indexUI].text = $"5";
    }
    void L_ChangeBulletUI3(int indexUI)
    {
        if (indexUI > 2)
        {
            if (indexUI == 3)
            {
                indexUI = 0;
            }
            else if (indexUI == 4)
            {
                indexUI = 1;
            }
        }
        // 총알 이미지
        Color c = L_Image[indexUI].color;
        c = new Color(0f, 1f, 1f, 1f);
        L_Image[indexUI].color = c;

        // 총알 개수
        L_Slash[indexUI].text = "/";
        L_Text_Cur[indexUI].text = $"10";
        L_Text_Max[indexUI].text = $"10";
    }
    #endregion

    #region #LineRenderer를 가져옴
    public LineRenderer left_LR;
    public LineRenderer right_LR;
    #endregion

    // - bool값과 if문을 사용한 다른 기법
    //bool isPressMouseL;

    // - Coroutine을 Playe할 때의 변수 값을 가져오고, 마우스를 뗐을 때 Coroutine을 정지하기 위한 변수
    Coroutine coFireBullet_L;
    Coroutine coFireBullet_R;

    // - 총알 발사 속도
    public float bulletIntervalTime = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        bulletM = BulletMagic.Basic;
        bulletW = BulletWeapon.Basic;

        R_ChangeBulletUI1(indexW);
        R_ChangeBulletUI2(indexW + 1);
        R_ChangeBulletUI3(indexW + 2);
        L_ChangeBulletUI1(indexM);
        L_ChangeBulletUI2(indexM + 1);
        L_ChangeBulletUI3(indexM + 2);

        // 총마다 데미지를 다르게 하여 시작 시 총알 데미지를 지정해 줌
        SetBulletDamage(20f);
    }

    // Update is called once per frame
    void Update()
    {
        #region #총알 종류 변경
        switch (bulletM)
        {
            case BulletMagic.Basic: currentBulletFactory_L = BulletL[0]; break;
            case BulletMagic.Ice: currentBulletFactory_L = BulletL[1]; break;
            case BulletMagic.Lightning: currentBulletFactory_L = BulletL[2]; break;
        }
        switch (bulletW)
        {
            case BulletWeapon.Basic: currentBulletFactory_R = BulletR[0]; break;
            case BulletWeapon.Shotgun: currentBulletFactory_R = BulletR[1]; break;
            case BulletWeapon.Piercer: currentBulletFactory_R = BulletR[2]; break;
        }
        //========== 총알 종류 변경 ==========
        if (switchBulletR.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            indexM++;
            if (indexM > BulletL.Length - 1)
            {
                indexM = 0;
            }
            bulletM = (BulletMagic)indexM;
            L_ChangeBulletUI1(indexM);
            L_ChangeBulletUI2(indexM + 1);
            L_ChangeBulletUI3(indexM + 2);
        }
        if (switchBulletL.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            indexM--;
            if (indexM < 0)
            {
                indexM = BulletL.Length - 1;
            }
            bulletM = (BulletMagic)indexM;
            L_ChangeBulletUI1(indexM);
            L_ChangeBulletUI2(indexM + 1);
            L_ChangeBulletUI3(indexM + 2);
        }

        if (switchBulletR.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            indexW++;
            if (indexW > BulletR.Length - 1)
            {
                indexW = 0;
            }
            bulletW = (BulletWeapon)indexW;
            R_ChangeBulletUI1(indexW);
            R_ChangeBulletUI2(indexW + 1);
            R_ChangeBulletUI3(indexW + 2);
        }
        if (switchBulletL.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            indexW--;
            if (indexW < 0)
            {
                indexW = BulletR.Length - 1;
            }
            bulletW = (BulletWeapon)indexW;
            R_ChangeBulletUI1(indexW);
            R_ChangeBulletUI2(indexW + 1);
            R_ChangeBulletUI3(indexW + 2);
        }
        #endregion

        //========== Line Renderer ==========
        // - 왼손 -
        left_LR.SetPosition(0, left_Hand_FirePoint.position);
        left_LR.SetPosition(1, left_Hand_FirePoint.position + left_Hand_FirePoint.forward * 10f);
        // - 오른손 -
        right_LR.SetPosition(0, right_Hand_FirePoint.position);
        right_LR.SetPosition(1, right_Hand_FirePoint.position + right_Hand_FirePoint.forward * 10f);

        //========== Vive 컨트롤러로 총알 발사하기 ==========
        if (pinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            coFireBullet_L = StartCoroutine(FireBullet_L());
            HapticVibration.instance.PlayVibration(SteamVR_Input_Sources.LeftHand);
        }
        if (pinch.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            StopCoroutine(coFireBullet_L);
        }

        if (pinch.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            coFireBullet_R = StartCoroutine(FireBullet_R());
            HapticVibration.instance.PlayVibration(SteamVR_Input_Sources.RightHand);
        }
        if (pinch.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            StopCoroutine(coFireBullet_R);
        }

        //========== 총알 종류 변경하기 ==========

        #region #Mouse로 총알 발사 하기
        //if (Input.GetMouseButtonDown(0))
        //{
        //    //isPressMouseL = true;
        //    coFireBullet_L = StartCoroutine(FireBullet_L());
        //}

        ////if(isPressMouseL)
        ////{
        ////    coFireBullet = StartCoroutine(FireBullet());
        ////}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    //isPressMouseL = false;
        //    StopCoroutine(coFireBullet_L);
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    coFireBullet_R = StartCoroutine(FireBullet_R());
        //}

        //if (Input.GetMouseButtonUp(1))
        //{
        //    //isPressMouseL = false;
        //    StopCoroutine(coFireBullet_R);
        //}
        #endregion
    }

    IEnumerator FireBullet_L()
    {
        while (true)
        {
            if (currentBulletFactory_L == BulletL[2])
            {
                GameObject bullet = Instantiate(currentBulletFactory_L);
                bullet.transform.position = left_Hand_FirePoint.position;

                Bullet_Lightning shotBullet = bullet.GetComponent<Bullet_Lightning>();
                Vector3 bulletDirection = left_Hand_FirePoint.forward;//left_Hand.forward;// - new Vector3(0, 0.6f, 0);
                shotBullet.ShootBullet(bulletDirection);
                Destroy(bullet, 3f);

                yield return new WaitForSeconds(bulletIntervalTime);
            }
            else
            {
                GameObject bullet = Instantiate(currentBulletFactory_L);
                bullet.transform.position = left_Hand_FirePoint.position;

                Bullet_Basic shotBullet = bullet.GetComponent<Bullet_Basic>();
                Vector3 bulletDirection = left_Hand_FirePoint.forward;//left_Hand.forward;// - new Vector3(0, 0.6f, 0);
                shotBullet.ShootBullet(bulletDirection);
                Destroy(bullet, 3f);

                yield return new WaitForSeconds(bulletIntervalTime);

            }
        }
    }

    IEnumerator FireBullet_R()
    {
        while (true)
        {
            if (currentBulletFactory_R == BulletR[1])
            {
                for (int i = 0; i < 5; i++)
                {
                    GameObject bullet_S = Instantiate(currentBulletFactory_R);
                    bullet_S.transform.position = right_Hand_FirePoint.position;

                    Bullet_Shotgun shotBullet = bullet_S.GetComponent<Bullet_Shotgun>();
                    Vector3 bulletDirection = right_Hand_FirePoint.forward; //right_Hand.forward;// - new Vector3(0, 0.6f, 0);
                    shotBullet.ShootBullet(bulletDirection);
                    Destroy(bullet_S, 1f);
                }
            }
            else
            {
                GameObject bullet = Instantiate(currentBulletFactory_R);
                bullet.transform.position = right_Hand_FirePoint.position;

                Bullet_Basic shotBullet = bullet.GetComponent<Bullet_Basic>();
                Vector3 bulletDirection = right_Hand_FirePoint.forward; //right_Hand.forward;// - new Vector3(0, 0.6f, 0);
                shotBullet.ShootBullet(bulletDirection);
                Destroy(bullet, 3f);

            }

            yield return new WaitForSeconds(bulletIntervalTime);
        }
    }

    void SetBulletDamage(float damage)
    {
        Bullet_Basic.bulletDamage = damage;
    }
}
