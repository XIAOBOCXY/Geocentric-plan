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

    /// <summary>
    /// 设置开始健康值
    /// </summary>
    /// <param name="startingHealth"></param>
    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    /// <summary>
    /// 获取开始健康值
    /// </summary>
    /// <returns></returns>
    public int GetStartingHealth()
    {
        return startingHealth;
    }

    /// <summary>
    /// 获取当前健康值
    /// </summary>
    /// <returns></returns>
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// 获取当前健康值比率
    /// </summary>
    /// <returns></returns>
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
