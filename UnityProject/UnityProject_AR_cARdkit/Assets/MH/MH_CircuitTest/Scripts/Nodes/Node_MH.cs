using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 노드에 접촉함으로써 P는 전압과 P접촉상태를 전달, M도 GND상태를 전달 = POW, GND

// 병렬: 접촉한 소자끼리 2개의 노드를 공유하는 상태 -> 노드는 접촉해 있는 소자를 확인, IsMine이라는 성분을 통해 자기 객체를 구분해야함

public class Node_MH : MonoBehaviour
{
    public bool isConnected;
    public bool isPowerSupplied;
    public bool isGrounded;

    public bool isSeparated;

    // 이 Node의 부모 소자
    public NodeManager_MH nm_part;
    // 이 Node와 같은 부모의 다른 소자
    public Node_MH oppositeNode;

    // 이 Node와 접촉해있는 Node들
    public Node_MH[] ContactNodes;
    // 이 Node와 접촉해있는 Node들의 반대쪽 Node들
    public Node_MH[] ContactOppositeNodes;

    public GameObject checker;

    // 나중에 사용 할 수도 있는 소자 전압
    public float nodeVoltage;

    private void Awake()
    {

    }

    void Start()
    {
        checker = GetComponentInChildren<Checker_MH>().checker;

        oppositeNode = nm_part.nodes.Where(opp => opp != this).ToArray()[0];
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            int layer = 1 << LayerMask.NameToLayer("Node");
            ContactNodes = Physics.OverlapBox(checker.transform.position, checker.transform.lossyScale / 2, Quaternion.identity, layer)
                                    .Select(obj => obj.gameObject)
                                    .Where(notThis => notThis != this.gameObject)
                                    .Select(cols => cols.gameObject.GetComponent<Node_MH>())
                                    .ToArray();

            if (ContactNodes != null && ContactNodes.Length > 0)
            {
                ContactOppositeNodes = ContactNodes.Select(opp => opp.oppositeNode).ToArray();
            }

            if (isSeparated == true)
            {
                for (int i = 0; i < ContactNodes.Length; i++)
                {
                    ContactNodes[i].Initialize();
                }
                isSeparated = false;
            }
            isConnected = true;


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Node"))
        {
            CircuitManager_MH.instance.NodeInitialize();
            isSeparated = true;
        }
    }

    // 최초 실행 시 소자에서 이 함수를 실행해서 Node_MH에 소자 NodeManager_MH정보가 담겨짐
    public void CallNodeManager(NodeManager_MH target)
    {
        nm_part = target;
    }

    public void Initialize()
    {
        isConnected = false;
        isPowerSupplied = false;
        isGrounded = false;

        if (nm_part.gameObject.layer != LayerMask.NameToLayer("Power"))
        {
            gameObject.tag = "Node";
            nodeVoltage = 0;
        }

        ContactNodes = null;
        ContactOppositeNodes = null;
    }

}
