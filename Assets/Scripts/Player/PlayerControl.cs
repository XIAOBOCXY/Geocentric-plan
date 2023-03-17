using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;
    [SerializeField] private Transform weaponShootPosition;
    private Player player;
    private float moveSpeed;

    private Coroutine playerRollCoroutine;//协程
    private WaitForFixedUpdate waitForFixedUpdate;//等待，直到下一个固定帧率更新函数
    private float playerRollCooldownTimer = 0f;
    public bool isPlayerRolling = false;


    private void Awake()
    {
        player = GetComponent<Player>();
        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        //创建waitforfixed update供协程使用
        waitForFixedUpdate = new WaitForFixedUpdate();

    }

    private void Update()
    {
        //正在滚动
        if (isPlayerRolling) return;
        //处理玩家移动
        MovementInput();
        //处理武器
        WeaponInput();
        //玩家滚动冷却
        PlayerRollCooldownTimer();

    }

    //处理玩家移动
    private void MovementInput()
    {
        //获取移动输入
        //1.Vertical  对应键盘上面的上下箭头，当按下上或下箭头时触发
        //2.Horizontal  对应键盘上面的左右箭头，当按下左或右箭头时触发
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        //鼠标右键按下
        bool rightMouseButtonDown = Input.GetMouseButtonDown(1);


        //根据键盘的输入得到方向向量
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        //当斜着运动的时候，三角形为1 0.7 0.7 方向向量乘以0.7才能保证斜着移动的距离是1，否则会多移动距离
        if (horizontalMovement != 0f && verticalMovement != 0f)
        {
            direction *= 0.7f;
        }

        //按下了键盘或鼠标，方向向量不为0
        if (direction != Vector2.zero)
        {
            if (!rightMouseButtonDown)
            {
                //调用移动事件
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }
            // 如果不在滚动冷却时间，就滚
            else if (playerRollCooldownTimer <= 0f)
            {
                PlayerRoll((Vector3)direction);
            }

        }
        //没有移动，调用空闲事件
        else
        {
            player.idleEvent.CallIdleEvent();
        }
    }

    //玩家滚动
    private void PlayerRoll(Vector3 direction)
    {
        //开始协程
        playerRollCoroutine = StartCoroutine(PlayerRollRoutine(direction));
    }

    //玩家滚动协程
    private IEnumerator PlayerRollRoutine(Vector3 direction)
    {
        //退出协程的最小距离
        float minDistance = 0.2f;
        //设置正在滚动
        isPlayerRolling = true;
        //计算滚动目标位置
        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.rollDistance;
        //当小于最小距离时退出
        while (Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            //目标位置，玩家位置，滚动速度，方向，正在滚动参数
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.rollSpeed, direction, isPlayerRolling);
            yield return waitForFixedUpdate;

        }

        isPlayerRolling = false;

        //设置冷却时间
        playerRollCooldownTimer = movementDetails.rollCooldownTime;
        player.transform.position = targetPosition;

    }

    //玩家冷却时间
    private void PlayerRollCooldownTimer()
    {
        if (playerRollCooldownTimer >= 0f)
        {
            //Time.deltaTime 当前帧和上一帧之间的时间
            playerRollCooldownTimer -= Time.deltaTime;
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

    //进入碰撞器
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //停止玩家滚动
        StopPlayerRollRoutine();
    }

    //停留碰撞器
    private void OnCollisionStay2D(Collision2D collision)
    {
        //停止玩家滚动
        StopPlayerRollRoutine();
    }

    //停止玩家滚动
    private void StopPlayerRollRoutine()
    {
        if (playerRollCoroutine != null)
        {
            StopCoroutine(playerRollCoroutine);

            isPlayerRolling = false;
        }
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
