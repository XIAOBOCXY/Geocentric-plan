using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private MovementDetailsSO movementDetails;
    [SerializeField] private Transform weaponShootPosition;
    private Player player;
    private float moveSpeed;

    private Coroutine playerRollCoroutine;//Э��
    private WaitForFixedUpdate waitForFixedUpdate;//�ȴ���ֱ����һ���̶�֡�ʸ��º���
    private float playerRollCooldownTimer = 0f;
    public bool isPlayerRolling = false;


    private void Awake()
    {
        player = GetComponent<Player>();
        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        //����waitforfixed update��Э��ʹ��
        waitForFixedUpdate = new WaitForFixedUpdate();

    }

    private void Update()
    {
        //���ڹ���
        if (isPlayerRolling) return;
        //��������ƶ�
        MovementInput();
        //��������
        WeaponInput();
        //��ҹ�����ȴ
        PlayerRollCooldownTimer();

    }

    //��������ƶ�
    private void MovementInput()
    {
        //��ȡ�ƶ�����
        //1.Vertical  ��Ӧ������������¼�ͷ���������ϻ��¼�ͷʱ����
        //2.Horizontal  ��Ӧ������������Ҽ�ͷ������������Ҽ�ͷʱ����
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        //����Ҽ�����
        bool rightMouseButtonDown = Input.GetMouseButtonDown(1);


        //���ݼ��̵�����õ���������
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        //��б���˶���ʱ��������Ϊ1 0.7 0.7 ������������0.7���ܱ�֤б���ƶ��ľ�����1���������ƶ�����
        if (horizontalMovement != 0f && verticalMovement != 0f)
        {
            direction *= 0.7f;
        }

        //�����˼��̻���꣬����������Ϊ0
        if (direction != Vector2.zero)
        {
            if (!rightMouseButtonDown)
            {
                //�����ƶ��¼�
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }
            // ������ڹ�����ȴʱ�䣬�͹�
            else if (playerRollCooldownTimer <= 0f)
            {
                PlayerRoll((Vector3)direction);
            }

        }
        //û���ƶ������ÿ����¼�
        else
        {
            player.idleEvent.CallIdleEvent();
        }
    }

    //��ҹ���
    private void PlayerRoll(Vector3 direction)
    {
        //��ʼЭ��
        playerRollCoroutine = StartCoroutine(PlayerRollRoutine(direction));
    }

    //��ҹ���Э��
    private IEnumerator PlayerRollRoutine(Vector3 direction)
    {
        //�˳�Э�̵���С����
        float minDistance = 0.2f;
        //�������ڹ���
        isPlayerRolling = true;
        //�������Ŀ��λ��
        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.rollDistance;
        //��С����С����ʱ�˳�
        while (Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            //Ŀ��λ�ã����λ�ã������ٶȣ��������ڹ�������
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.rollSpeed, direction, isPlayerRolling);
            yield return waitForFixedUpdate;

        }

        isPlayerRolling = false;

        //������ȴʱ��
        playerRollCooldownTimer = movementDetails.rollCooldownTime;
        player.transform.position = targetPosition;

    }

    //�����ȴʱ��
    private void PlayerRollCooldownTimer()
    {
        if (playerRollCooldownTimer >= 0f)
        {
            //Time.deltaTime ��ǰ֡����һ֮֡���ʱ��
            playerRollCooldownTimer -= Time.deltaTime;
        }
    }

    //��������
    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDirection;
        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection);
    }
    //�������
    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, out AimDirection playerAimDirection)
    {
        //��ȡ�����������
        Vector3 mouseWorldPosition = HelperUtlities.GetMouseWorldPosition();

        //����������Ŀ���ľ�������
        weaponDirection = (mouseWorldPosition - weaponShootPosition.position);

        //�������λ�õ�Ŀ���ľ�������
        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        //�����������Ƕȶ���
        weaponAngleDegrees = HelperUtlities.GetAngleFromVector(weaponDirection);

        //��ȡ��Һ�Ŀ���ĽǶȶ���
        playerAngleDegrees = HelperUtlities.GetAngleFromVector(playerDirection);

        //��ȡ����������
        playerAimDirection = HelperUtlities.GetAimDirection(playerAngleDegrees);

        // ������������¼�
        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
    }

    //������ײ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //ֹͣ��ҹ���
        StopPlayerRollRoutine();
    }

    //ͣ����ײ��
    private void OnCollisionStay2D(Collision2D collision)
    {
        //ֹͣ��ҹ���
        StopPlayerRollRoutine();
    }

    //ֹͣ��ҹ���
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
