using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Bullet m_Bullet;//×Óµ¯
    [SerializeField] private float m_AttackInterval = 0.2f;//¹¥»÷¼ä¸ô

    private float m_Timer;
    
    private void Update()
    {
        if (m_Timer > 0) { m_Timer -= Time.deltaTime; }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            if (m_Timer <= 0f) { Fire(); }
        }
    }

    private void Fire()
    {
        m_Timer = m_AttackInterval;
        Bullet bullet = Instantiate(m_Bullet, transform.position, transform.rotation);
        bullet.Fire();
    }

}
