using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

// 왼쪽 컨트롤러 trackpad의 버튼을 누르는 것으로 움직이고싶다.

public class PlayerMove_InOffice : MonoBehaviour
{
    #region #싱글턴
    public static PlayerMove_InOffice instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public SteamVR_Action_Vector2 trackPadMove;
    public Image image_fader;

    float h, v;
    float rotY;
    public float speed = 1;
    public float rotspeed = 10f;

    float fadeInTime;
    float fadeOutTime;
    // Start is called before the first frame update
    void Start()
    {
        FadeIn();
    }

    #region #FadeIn
    public void FadeIn()
    {
        StartCoroutine("IEFadeIn");
    }

    IEnumerator IEFadeIn()
    {
        fadeInTime = 1.5f;
        Color c = image_fader.color;
        c.a = 1;
        image_fader.color = c;

        float add = 0.01f;
        c = image_fader.color;
        for (float a = 0; a <= fadeInTime; a += add)
        {
            c.a = 1 - (a / fadeInTime);
            image_fader.color = c;
            yield return new WaitForSeconds(add);
        }
        c.a = 0;
        image_fader.color = c;
    }
    #endregion
    #region #FadeOut
    public void FadeOut()
    {
        StartCoroutine("IEFadeOut");
    }

    IEnumerator IEFadeOut()
    {
        fadeOutTime = 1f;
        Color c = image_fader.color;
        c.a = 0;
        image_fader.color = c;

        float add = 0.01f;
        c = image_fader.color;
        for (float a = 0; a <= fadeOutTime; a += add)
        {
            c.a = a / fadeOutTime;
            image_fader.color = c;
            yield return new WaitForSeconds(add);
        }
        c.a = 1;
        image_fader.color = c;
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        h = trackPadMove.GetAxis(SteamVR_Input_Sources.LeftHand).x;
        v = trackPadMove.GetAxis(SteamVR_Input_Sources.LeftHand).y;

        rotY = trackPadMove.GetAxis(SteamVR_Input_Sources.RightHand).x;

        transform.Rotate(new Vector3(0, rotY, 0) * Time.deltaTime * rotspeed);

        Vector3 dir = new Vector3(h, 0, v);
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;

        transform.position += dir * speed * Time.deltaTime;

    }

}
