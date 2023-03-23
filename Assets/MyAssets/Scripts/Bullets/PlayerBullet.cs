using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    private TrailRenderer m_Trail;

    protected override void Awake()
    {
        base.Awake();
        m_Trail = GetComponentInChildren<TrailRenderer>();
    }

    protected override void OnBulletDestroy()
    {
        if (m_Trail.isVisible)
        {
            Destroy(gameObject);
        }
    }


    protected override void OnTrigger(Collider2D collider)
    {
        switch (collider.tag)
        {
            case Const.ENEMY:
                var enemy = collider.GetComponentInParent<Enemy>();
                if (enemy.BeHit(m_DamageValue))
                {
                    if (enemy.IsBoss)
                    {
                        Global.PassLevel();
                    }
                }
                Destroy();
                break;
        }
    }

}
