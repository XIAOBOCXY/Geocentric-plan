using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;
    [SerializeField] private Transform weaponShootPosition;
    private Player player;
    private float moveSpeed;
    private void Awake()
    {
        player = GetComponent<Player>();
        moveSpeed = movementDetails.GetMoveSpeed();
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
        //获取移动输入
        //1.Vertical  对应键盘上面的上下箭头，当按下上或下箭头时触发
        //2.Horizontal  对应键盘上面的左右箭头，当按下左或右箭头时触发
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");


        //根据键盘的输入得到方向向量
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        //当斜着运动的时候，三角形为1 0.7 0.7 方向向量乘以0.7才能保证斜着移动的距离是1，否则会多移动距离
        if (horizontalMovement != 0f && verticalMovement != 0f)
        {
            direction *= 0.7f;
        }

        //按下了键盘，方向向量不为0
        if (direction != Vector2.zero)
        {
            //调用移动事件
            player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);

        }
        //没有移动，调用空闲事件
        else
        {
            player.idleEvent.CallIdleEvent();
        }
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

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtlities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion Validation

}
