using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapons/Weapon Details")]
public class WeaponDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("WEAPON BASE DETAILS")]

    public string weaponName;
    public Sprite weaponSprite;

    [Space(10)]
    [Header("WEAPON CONFIGURATION")]
    public Vector3 weaponShootPosition;
    public AmmoDetailsSO weaponCurrentAmmo;

    [Space(10)]
    [Header("WEAPON OPERATING VALUES")]
    public bool hasInfiniteAmmo = false;//有限子弹
    public bool hasInfiniteClipCapacity = false;//有限弹夹容量
    public int weaponClipAmmoCapacity = 6;//弹夹容量
    public int weaponAmmoCapacity = 100;//总量
    public float weaponFireRate = 0.2f;//射速
    public float weaponPrechargeTime = 0f;//充能时间
    public float weaponReloadTime = 0f;//换弹时间

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtlities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        HelperUtlities.ValidateCheckNullValue(this, nameof(weaponCurrentAmmo), weaponCurrentAmmo);
        HelperUtlities.ValidateCheckPositiveValue(this, nameof(weaponFireRate), weaponFireRate, false);
        HelperUtlities.ValidateCheckPositiveValue(this, nameof(weaponPrechargeTime), weaponPrechargeTime, true);

        if (!hasInfiniteAmmo)
        {
            HelperUtlities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        }

        if (!hasInfiniteClipCapacity)
        {
            HelperUtlities.ValidateCheckPositiveValue(this, nameof(weaponClipAmmoCapacity), weaponClipAmmoCapacity, false);
        }
    }

#endif
    #endregion Validation
}
