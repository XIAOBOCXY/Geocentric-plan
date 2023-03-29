using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected int m_DamageValue = 5;//伤害值
    [SerializeField] protected float m_FireForce = 30f;//发射力

    private Rigidbody2D m_Rigid;
    private Collider2D m_Col;

    protected bool m_IsTrigger;

    protected virtual void Awake()
    {
        m_Rigid = GetComponentInChildren<Rigidbody2D>();
        m_Col = GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        if (m_IsTrigger)
        {
            OnBulletDestroy();
        }
    }

    /// <summary>
    /// 发射弹药
    /// </summary>
    public void Fire()
    {
        //给刚体（弹药）添加一个力（参数为：方向*力的大小，力的模式）
        m_Rigid.AddForce(transform.right * m_FireForce, ForceMode2D.Impulse);
        //生成两秒后销毁
        Destroy(gameObject, 2f);
    }

    /// <summary>
    /// 弹药销毁
    /// </summary>
    protected void Destroy()
    {
        if (m_IsTrigger) { return; }

        m_IsTrigger = true;
        m_Rigid.velocity = Vector2.zero;
        m_Rigid.bodyType = RigidbodyType2D.Kinematic;
        m_Col.enabled = false;
    }

    /// <summary>
    /// 触发检测
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case Const.TITE_MAP:
                Destroy();
                break;
        }
        OnTrigger(collision);
    }

    protected abstract void OnBulletDestroy();
    protected abstract void OnTrigger(Collider2D collider);

}
