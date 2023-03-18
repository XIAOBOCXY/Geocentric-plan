using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails_", menuName = "Scriptable Objects/Weapons/Ammo Details")]
public class AmmoDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("BASIC AMMO DETAILS")]
    public string ammoName;
    public bool isPlayerAmmo;

    [Space(10)]
    [Header("AMMO SPRITE, PREFAB & MATERIALS")]
    public Sprite ammoSprite;
    public GameObject[] ammoPrefabArray;
    public Material ammoMaterial;
    public float ammoChargeTime = 0.1f;
    public Material ammoChargeMaterial;


    [Space(10)]
    [Header("AMMO BASE PARAMETERS")]
    public int ammoDamage = 1;
    public float ammoSpeedMin = 20f;
    public float ammoSpeedMax = 20f;
    public float ammoRange = 20f;
    public float ammoRotationSpeed = 1f;

    [Space(10)]
    [Header("AMMO SPREAD DETAILS")]
    public float ammoSpreadMin = 0f;
    public float ammoSpreadMax = 0f;

    [Space(10)]
    [Header("AMMO SPAWN DETAILS")]

    public int ammoSpawnAmountMin = 1;
    public int ammoSpawnAmountMax = 1;
    public float ammoSpawnIntervalMin = 0f;
    public float ammoSpawnIntervalMax = 0f;

    [Space(10)]
    [Header("AMMO TRAIL DETAILS")]
    public bool isAmmoTrail = false;
    public float ammoTrailTime = 3f;
    public Material ammoTrailMaterial;
    [Range(0f, 1f)] public float ammoTrailStartWidth;
    [Range(0f, 1f)] public float ammoTrailEndWidth;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtlities.ValidateCheckEmptyString(this, nameof(ammoName), ammoName);
        HelperUtlities.ValidateCheckNullValue(this, nameof(ammoSprite), ammoSprite);
        HelperUtlities.ValidateCheckEnumerableValues(this, nameof(ammoPrefabArray), ammoPrefabArray);
        HelperUtlities.ValidateCheckNullValue(this, nameof(ammoMaterial), ammoMaterial);
        if (ammoChargeTime > 0)
            HelperUtlities.ValidateCheckNullValue(this, nameof(ammoChargeMaterial), ammoChargeMaterial);
        HelperUtlities.ValidateCheckPositiveValue(this, nameof(ammoDamage), ammoDamage, false);
        HelperUtlities.ValidateCheckPositiveRange(this, nameof(ammoSpeedMin), ammoSpeedMin, nameof(ammoSpeedMax), ammoSpeedMax, false);
        HelperUtlities.ValidateCheckPositiveValue(this, nameof(ammoRange), ammoRange, false);
        HelperUtlities.ValidateCheckPositiveRange(this, nameof(ammoSpreadMin), ammoSpreadMin, nameof(ammoSpreadMax), ammoSpreadMax, true);
        HelperUtlities.ValidateCheckPositiveRange(this, nameof(ammoSpawnAmountMin), ammoSpawnAmountMin, nameof(ammoSpawnAmountMax), ammoSpawnAmountMax, false);
        HelperUtlities.ValidateCheckPositiveRange(this, nameof(ammoSpawnIntervalMin), ammoSpawnIntervalMin, nameof(ammoSpawnIntervalMax), ammoSpawnIntervalMax, true);
        if (isAmmoTrail)
        {
            HelperUtlities.ValidateCheckPositiveValue(this, nameof(ammoTrailTime), ammoTrailTime, false);
            HelperUtlities.ValidateCheckNullValue(this, nameof(ammoTrailMaterial), ammoTrailMaterial);
            HelperUtlities.ValidateCheckPositiveValue(this, nameof(ammoTrailStartWidth), ammoTrailStartWidth, false);
            HelperUtlities.ValidateCheckPositiveValue(this, nameof(ammoTrailEndWidth), ammoTrailEndWidth, false);
        }
    }

#endif
    #endregion
}
