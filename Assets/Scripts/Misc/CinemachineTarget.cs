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
        //加载组件
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //设置CinemachineTargetGroup
        SetCinemachineTargetGroup();
    }

    //设置CinemachineTargetGroup
    private void SetCinemachineTargetGroup()
    {
        // 为电影机设置目标组，让其跟随玩家和鼠标
        CinemachineTargetGroup.Target cinemachineGroupTarget_player = new CinemachineTargetGroup.Target { weight = 1f, radius = 2.5f, target = GameManager.Instance.GetPlayer().transform };

        CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[] { cinemachineGroupTarget_player};

        cinemachineTargetGroup.m_Targets = cinemachineTargetArray;

    }

}
