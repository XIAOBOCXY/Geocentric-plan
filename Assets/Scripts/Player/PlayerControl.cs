using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private Transform weaponShootPosition;
    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        //处理玩家移动
        MovementInput();
        //处理武器
        WeaponInput();

    }

    //处理玩家移动
    private void MovementInput()
    {
        player.idleEvent.CallIdleEvent();
    }

    //处理武器
    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDirection;
        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection);
    }
    //武器射击
    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out AimDirection playerAimDirection)
    {
        //获取鼠标世界坐标
        Vector3 mouseWorldPosition = HelperUtlities.GetMouseWorldPosition();

        //计算武器到目标点的距离向量
        weaponDirection = (mouseWorldPosition - weaponShootPosition.position);

        //计算玩家位置到目标点的距离向量
        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        //获得武器射击角度度数
        weaponAngleDegrees = HelperUtlities.GetAngleFromVector(weaponDirection);

        //获取玩家和目标点的角度度数
        playerAngleDegrees = HelperUtlities.GetAngleFromVector(playerDirection);

        //获取玩家射击方向
        playerAimDirection = HelperUtlities.GetAimDirection(playerAngleDegrees);

        // 调用武器射击事件
        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
    }
}
