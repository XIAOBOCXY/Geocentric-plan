using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable
{
    //初始化弹药
    void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false);
    //获取游戏对象
    GameObject GetGameObject();

}
