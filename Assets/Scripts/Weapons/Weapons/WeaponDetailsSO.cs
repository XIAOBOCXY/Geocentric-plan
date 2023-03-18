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
    public bool hasInfiniteAmmo = false;//�����ӵ�
    public bool hasInfiniteClipCapacity = false;//���޵�������
    public int weaponClipAmmoCapacity = 6;//��������
    public int weaponAmmoCapacity = 100;//����
    public float weaponFireRate = 0.2f;//����
    public float weaponPrechargeTime = 0f;//����ʱ��
    public float weaponReloadTime = 0f;//����ʱ��

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
