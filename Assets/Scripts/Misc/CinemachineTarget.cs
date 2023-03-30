using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineTargetGroup))]

public class CinemachineTarget : MonoBehaviour
{
    private CinemachineTargetGroup cinemachineTargetGroup;
    //光标
    [SerializeField] private Transform cursorTarget;
    private void Awake()
    {
        //加载组件
        cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
    }

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
        CinemachineTargetGroup.Target cinemachineGroupTarget_cursor = new CinemachineTargetGroup.Target { weight = 1f, radius = 1f, target = cursorTarget };
        CinemachineTargetGroup.Target[] cinemachineTargetArray = new CinemachineTargetGroup.Target[] { cinemachineGroupTarget_player, cinemachineGroupTarget_cursor };

        cinemachineTargetGroup.m_Targets = cinemachineTargetArray;

    }
    private void Update()
    {
        //光标位置设置为鼠标世界坐标
        cursorTarget.position = HelperUtlities.GetMouseWorldPosition();
    }

}
