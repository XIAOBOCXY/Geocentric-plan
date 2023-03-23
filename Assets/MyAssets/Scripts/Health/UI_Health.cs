using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UI_Health : MonoBehaviour
{
    [SerializeField] private float m_OffsetHeight = 5f;
    [SerializeField] private Image m_Health;

    protected

    private Transform m_Target;

    private void Start()
    {
        m_Target = GetTarget();
        UpdateHeath();
    }

    protected virtual void Update()
    {
        if (m_Target == null) { return; }
        transform.position = m_Target.position + Vector3.up * m_OffsetHeight / transform.localScale.x;
    }

    public void UpdateHeath(float ratio = 1f)
    {
        m_Health.fillAmount = ratio;
    }

    protected abstract Transform GetTarget();


}
