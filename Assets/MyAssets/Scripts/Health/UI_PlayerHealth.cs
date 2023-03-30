
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHealth : UI_Health
{

    private Player m_Player;
    private bool m_Init;

    private void OnEnable()
    {
        InitPlayer();
    }

    private void InitPlayer()
    {
        if (m_Player != null) { return; }
        m_Player = FindObjectOfType<Player>();
    }

    private void OnDisable()
    {
        if (m_Player.health != null)
        {
            m_Player.health.OnHealthChangingEvent -= OnHealthChangingEvent;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (m_Player.health != null && !m_Init)
        {
            m_Init = true;
            m_Player.health.OnHealthChangingEvent += OnHealthChangingEvent;
        }
    }

    /// <summary>
    /// 玩家健康值更新，当健康值为0，死亡并重新开始关卡
    /// </summary>
    /// <param name="health"></param>
    private void OnHealthChangingEvent(int health)
    {
        UpdateHeath(m_Player.health.GetHealthRatio());
        if (health <= 0)
        {
            Destroy(m_Player.gameObject);
            Global.Instance.SetMask(true, onComplete: () => Global.LoadSelf());
        }
    }

    protected override Transform GetTarget()
    {
        InitPlayer();
        return m_Player.transform;
    }
}
