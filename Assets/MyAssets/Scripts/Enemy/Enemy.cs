using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool m_IsBoss;//是否是Boss
    [SerializeField] private int m_Health = 50;//生命值
    [SerializeField] protected float m_DistWithPlayer = 20f;
    [SerializeField] private float m_AttackInterval = 1f;
    [SerializeField] private Bullet m_Bullet;
    [SerializeField] private Transform m_MuzzlePos;

    private int m_HealthRecord;
    private Vector3 m_DefaultPoint;
    private UI_EnemyHealth m_HealthUI;

    private float m_Timer;

    public bool IsBoss
    {
        get { return m_IsBoss; }
    }

    public bool IsDeath
    {
        get;
        private set;
    }

    protected Player m_Player;

    protected virtual void Awake()
    {
        m_HealthRecord = m_Health;
        m_HealthUI = GetComponentInChildren<UI_EnemyHealth>();
    }

    public void Init(Vector3 point)
    {
        m_DefaultPoint = point;
        transform.position = point;
    }

    private void Update()
    {
        if (IsDeath) { return; }
        if (m_Player == null)
        {
            m_Player = FindObjectOfType<Player>();
        }
        else
        {
            SetDirect();
            if (m_Timer < m_AttackInterval) { m_Timer += Time.deltaTime; }
            float dist = Vector2.Distance(transform.position, m_Player.transform.position);
            OnCheckDistWithPlayer(dist);
        }
    }

    private void Attack()
    {
        m_Timer = 0f;
        Bullet bullet = Instantiate(m_Bullet, m_MuzzlePos.position, m_MuzzlePos.rotation);
        Vector3 v = m_Player.transform.position - transform.position;
        v.z = 0;
        float angle = Vector3.SignedAngle(Vector3.up, v, Vector3.forward);
        Quaternion rotation = Quaternion.Euler(Vector3.forward * (angle + 90f));
        bullet.transform.rotation = rotation;
        bullet.Fire();
    }

    private float m_InterDirect = 0.2f;
    private void SetDirect()
    {
        if (m_InterDirect > 0) { m_InterDirect -= Time.deltaTime; }
        else
        {
            m_InterDirect = 0.2f;
            Vector3 v = Vector3.Cross(m_Player.transform.up,  transform.position - m_Player.transform.position);
            if (v.z != 0)
            {
                transform.localEulerAngles = Vector3.up * 180f * (v.z > 0 ? 1f : 0f);
            }
        }
    }

    /// <summary>
    /// 受伤
    /// </summary>
    public bool BeHit(int value)
    {
        if (IsDeath) { return true; }
        m_Health = Mathf.Max(0, m_Health - value);
        m_HealthUI.UpdateHeath(m_Health * 1f / m_HealthRecord);
        if (m_Health <= 0)
        {
            IsDeath = true;
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    protected virtual void OnCheckDistWithPlayer(float dist)
    {
        if (dist <= m_DistWithPlayer)
        {
            if (m_Timer >= m_AttackInterval) { Attack(); }
        }
    }

}
