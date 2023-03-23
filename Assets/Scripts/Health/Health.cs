using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    private int startingHealth;
    private int currentHealth;

    public event Action<int> OnHealthChangingEvent;

    //���ÿ�ʼ����ֵ
    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    //��ÿ�ʼ����ֵ
    public int GetStartingHealth()
    {
        return startingHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetHealthRatio()
    {
        return currentHealth * 1f / startingHealth;
    }


    public bool SetHealth(int value)
    {
        if (currentHealth <= 0) { return true; }

        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
        OnHealthChangingEvent?.Invoke(currentHealth);
        return currentHealth <= 0;
    }

    private void OnDestroy()
    {
        OnHealthChangingEvent = null;
    }

}
