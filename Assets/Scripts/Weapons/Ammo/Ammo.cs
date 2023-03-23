using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{
    [SerializeField] private TrailRenderer trailRenderer;
    private float ammoRange = 0f;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private SpriteRenderer spriteRenderer;
    private AmmoDetailsSO ammoDetails;
    private float ammoChargeTimer;
    private bool isAmmoMaterialSet = false;
    private bool overrideAmmoMovement;


    //实现接口
    public GameObject GetGameObject()
    {
        throw new System.NotImplementedException();
    }


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (ammoChargeTimer > 0f)
        {
            ammoChargeTimer -= Time.deltaTime;
            return;
        }
        else if (!isAmmoMaterialSet)
        {
            //设置弹药材质
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        //计算弹药移动距离
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        //超出最大范围则禁用组件
        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            DisableAmmo();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //禁用子弹
        //DisableAmmo();
    }

    //初始化弹药
    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false)
    {
        this.ammoDetails = ammoDetails;
        //设置发射弹药方向
        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);
        //设置弹药sprite
        spriteRenderer.sprite = ammoDetails.ammoSprite;

        //设置充能材质
        if (ammoDetails.ammoChargeTime > 0f)
        {
            ammoChargeTimer = ammoDetails.ammoChargeTime;
            SetAmmoMaterial(ammoDetails.ammoChargeMaterial);
            isAmmoMaterialSet = false;
        }
        else
        {
            ammoChargeTimer = 0f;
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }
        ammoRange = ammoDetails.ammoRange;
        this.ammoSpeed = ammoSpeed;
        //重写子弹运动轨迹
        this.overrideAmmoMovement = overrideAmmoMovement;
        //激活游戏对象
        gameObject.SetActive(true);

 


        //子弹轨迹
        if (ammoDetails.isAmmoTrail)
        {
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.emitting = true;
            trailRenderer.material = ammoDetails.ammoTrailMaterial;
            trailRenderer.startWidth = ammoDetails.ammoTrailStartWidth;
            trailRenderer.endWidth = ammoDetails.ammoTrailEndWidth;
            trailRenderer.time = ammoDetails.ammoTrailTime;
        }
        else
        {
            trailRenderer.emitting = false;
            trailRenderer.gameObject.SetActive(false);
        }
    }

    //设置发射弹药方向
    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        //随机取一个发散的范围值
        float randomSpread = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);
        // 随机取1或-1，用于乘以发散值，来随机方向
        int spreadToggle = Random.Range(0, 2) * 2 - 1;
        //小于最小角度，则使用玩家秒赚角度，否则使用武器瞄准角度
        if (weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle;
        }
        else
        {
            fireDirectionAngle = weaponAimAngle;
        }
        //计算随机反射角度
        fireDirectionAngle += spreadToggle * randomSpread;
        //子弹旋转
        transform.eulerAngles = new Vector3(0f, 0f, fireDirectionAngle);
        //根据角度计算方向向量
        fireDirectionVector = HelperUtlities.GetDirectionVectorFromAngle(fireDirectionAngle);

    }

    //禁用子弹，即不激活
    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    //设置子弹材质
    public void SetAmmoMaterial(Material material)
    {
        spriteRenderer.material = material;
    }


    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtlities.ValidateCheckNullValue(this, nameof(trailRenderer), trailRenderer);
    }

#endif
    #endregion Validation
}
