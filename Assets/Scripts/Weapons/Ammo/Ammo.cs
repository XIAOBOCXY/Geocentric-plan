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


    //ʵ�ֽӿ�
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
            //���õ�ҩ����
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        //���㵯ҩ�ƶ�����
        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;

        transform.position += distanceVector;

        //�������Χ��������
        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            DisableAmmo();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�����ӵ�
        //DisableAmmo();
    }

    //��ʼ����ҩ
    public void InitialiseAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false)
    {
        this.ammoDetails = ammoDetails;
        //���÷��䵯ҩ����
        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);
        //���õ�ҩsprite
        spriteRenderer.sprite = ammoDetails.ammoSprite;

        //���ó��ܲ���
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
        //��д�ӵ��˶��켣
        this.overrideAmmoMovement = overrideAmmoMovement;
        //������Ϸ����
        gameObject.SetActive(true);

 


        //�ӵ��켣
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

    //���÷��䵯ҩ����
    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        //���ȡһ����ɢ�ķ�Χֵ
        float randomSpread = Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);
        // ���ȡ1��-1�����ڳ��Է�ɢֵ�����������
        int spreadToggle = Random.Range(0, 2) * 2 - 1;
        //С����С�Ƕȣ���ʹ�������׬�Ƕȣ�����ʹ��������׼�Ƕ�
        if (weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle;
        }
        else
        {
            fireDirectionAngle = weaponAimAngle;
        }
        //�����������Ƕ�
        fireDirectionAngle += spreadToggle * randomSpread;
        //�ӵ���ת
        transform.eulerAngles = new Vector3(0f, 0f, fireDirectionAngle);
        //���ݽǶȼ��㷽������
        fireDirectionVector = HelperUtlities.GetDirectionVectorFromAngle(fireDirectionAngle);

    }

    //�����ӵ�����������
    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }

    //�����ӵ�����
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
