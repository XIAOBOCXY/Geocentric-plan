using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable
{
    //��ʼ����ҩ
    void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false);
    //��ȡ��Ϸ����
    GameObject GetGameObject();

}
