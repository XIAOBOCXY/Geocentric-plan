using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineTargetGroup))]

public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;
    private void Awake()
    {
        //�������
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //����CinemachineTargetGroup
        SetCinemachineTargetGroup();
    }

    //����CinemachineTargetGroup
    private void SetCinemachineTargetGroup()
    {
        // Ϊ��Ӱ������Ŀ���飬���������Һ����
        CinemachineTargetGroup.Target cinemachineGroupTarget_player = new CinemachineTargetGroup.Target { weight = 1f, radius = 2.5f, target = GameManager.Instance.GetPlayer().transform };

        CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[] { cinemachineGroupTarget_player};

        cinemachineTargetGroup.m_Targets = cinemachineTargetArray;

    }

}