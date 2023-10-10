using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class PlayerAim : MonoBehaviour
{
    #region #SteamVR로 부터 가져온 자료
    public SteamVR_Action_Boolean pinch;
    #endregion

    #region #VR컨트롤러
    public Transform left_Hand;
    public Transform left_Hand_FirePoint;
    public Transform right_Hand;
    public Transform right_Hand_FirePoint;
    #endregion

    #region #LineRenderer를 가져옴
    public LineRenderer left_LR;
    public LineRenderer right_LR;
    #endregion

    Ray ray;
    RaycastHit hitinfo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //========== Line Renderer ==========
        // - 왼손 -
        left_LR.SetPosition(0, left_Hand_FirePoint.position);
        left_LR.SetPosition(1, left_Hand_FirePoint.position + left_Hand_FirePoint.forward * 10f);
        // - 오른손 -
        right_LR.SetPosition(0, right_Hand_FirePoint.position);
        right_LR.SetPosition(1, right_Hand_FirePoint.position + right_Hand_FirePoint.forward * 10f);

        //========== Vive 컨트롤러로 버튼 누르기 ==========
        ray = new Ray(left_Hand_FirePoint.position, left_Hand_FirePoint.forward);
        if (Physics.Raycast(ray, out hitinfo, float.MaxValue))
        {

            if (pinch.GetStateDown(SteamVR_Input_Sources.LeftHand))
            {
                Button btn = hitinfo.transform.GetComponent<Button>();
                if (btn != null)
                {
                    print("눌림");
                    btn.onClick.Invoke();
                }
            }
            if (pinch.GetStateUp(SteamVR_Input_Sources.LeftHand))
            {

            }
        }
        ray = new Ray(right_Hand_FirePoint.position, right_Hand_FirePoint.forward);
        if (Physics.Raycast(ray, out hitinfo, float.MaxValue))
        {

            if (pinch.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                Button btn = hitinfo.transform.GetComponent<Button>();
                if (btn != null)
                {
                    print("눌림");
                    btn.onClick.Invoke();
                }
            }
            if (pinch.GetStateUp(SteamVR_Input_Sources.RightHand))
            {

            }
        }
    }
}
