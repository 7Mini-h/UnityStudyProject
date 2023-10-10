using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestCenterManager : MonoBehaviour
{
    #region #싱글턴
    public static QuestCenterManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject questBoard;
    public GameObject StageBoard;
    public CanvasGroup questCG;
    public CanvasGroup stageCG;

    public RectTransform image_RightArrow;
    public AnimationCurve img_arrowAC;

    public Text text_Indicate;
    public AnimationCurve textAC;

    Coroutine questShow;
    Coroutine questHide;
    Coroutine stageShow;
    Coroutine stageHide;

    // Start is called before the first frame update
    void Start()
    {
        questBoard.SetActive(false);
        StageBoard.SetActive(false);
        questCG.alpha = 0;
        stageCG.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            questBoard.SetActive(true);
            StopCoroutine("IEHideBoard");
            StartCoroutine("IEShowBoard", questCG);

            StopCoroutine("IEMenuIndicate");
            StartCoroutine("IEMenuIndicate");
        }
    }

    public IEnumerator IEShowBoard(CanvasGroup canvasGroup)
    {
        while(canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        canvasGroup.alpha = 1;
    }

    public IEnumerator IEHideBoard(CanvasGroup canvasGroup)
    {
        while (0 < canvasGroup.alpha)
        {
            canvasGroup.alpha -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        canvasGroup.alpha = 0;
    }

    IEnumerator IEMenuIndicate()
    {
        while (true)
        {
            Vector3 move = image_RightArrow.localPosition;
            move.x = img_arrowAC.Evaluate(Time.time);
            image_RightArrow.localPosition = move;

            Color color = text_Indicate.color;
            color.a = textAC.Evaluate(Time.time);
            text_Indicate.color = color;

            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            questBoard.SetActive(false);
            StageBoard.SetActive(false);
            StopCoroutine("IEShowBoard");
            StartCoroutine("IEHideBoard",questCG);
            StartCoroutine("IEHideBoard",stageCG);
        }
        StopCoroutine("IEMenuIndicate");
    }
}
