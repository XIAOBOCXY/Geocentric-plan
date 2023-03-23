using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{

    protected override void OnBulletDestroy()
    {
        Destroy(gameObject);
    }

    protected override void OnTrigger(Collider2D collider)
    {
        switch (collider.tag)
        {
            case Const.PLAYER:
                collider.GetComponent<Player>().health.SetHealth(-m_DamageValue);
                Destroy();
                break;
        }
    }

}
