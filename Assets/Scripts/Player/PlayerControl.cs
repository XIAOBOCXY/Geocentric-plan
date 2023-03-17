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
        //��������ƶ�
        MovementInput();
        //��������
        WeaponInput();

    }

    //��������ƶ�
    private void MovementInput()
    {
        player.idleEvent.CallIdleEvent();
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
}
