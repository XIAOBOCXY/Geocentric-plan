using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField] private float m_FollowDist = 20f;
    [SerializeField] private float m_MoveSpeed = 1f;

    private Animator m_Anim;

    protected override void Awake()
    {
        base.Awake();
        m_Anim = GetComponentInChildren<Animator>();
    }

    protected override void OnCheckDistWithPlayer(float dist)
    {
        if (dist <= m_FollowDist && dist > m_DistWithPlayer)
        {
            Vector3 dir = m_Player.transform.position - transform.position;
            dir.z = 0;
            transform.position += dir.normalized * Time.deltaTime * m_MoveSpeed;
            m_Anim.SetBool("IsMoving", true);
        }
        else
        {
            base.OnCheckDistWithPlayer(dist);
            m_Anim.SetBool("IsMoving", false);
        }
    }
}
