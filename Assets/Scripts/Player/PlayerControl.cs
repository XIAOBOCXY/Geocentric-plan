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
        //��������ƶ�
        MovementInput();
        //��������
        WeaponInput();

    }

    //��������ƶ�
    private void MovementInput()
    {
        //��ȡ�ƶ�����
        //1.Vertical  ��Ӧ������������¼�ͷ���������ϻ��¼�ͷʱ����
        //2.Horizontal  ��Ӧ������������Ҽ�ͷ������������Ҽ�ͷʱ����
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");


        //���ݼ��̵�����õ���������
        Vector2 direction = new Vector2(horizontalMovement, verticalMovement);

        //��б���˶���ʱ��������Ϊ1 0.7 0.7 ������������0.7���ܱ�֤б���ƶ��ľ�����1���������ƶ�����
        if (horizontalMovement != 0f && verticalMovement != 0f)
        {
            direction *= 0.7f;
        }

        //�����˼��̣�����������Ϊ0
        if (direction != Vector2.zero)
        {
            //�����ƶ��¼�
            player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);

        }
        //û���ƶ������ÿ����¼�
        else
        {
            player.idleEvent.CallIdleEvent();
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

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtlities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }
#endif
    #endregion Validation

}
