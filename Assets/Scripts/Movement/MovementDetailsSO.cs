using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails_", menuName = "Scriptable Objects/Movement/MovementDetails")]
public class MovementDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("MOVEMENT DETAILS")]
    public float minMoveSpeed = 8f;
    public float maxMoveSpeed = 8f;
    //玩家滚动速度
    public float rollSpeed; 
    //玩家滚动距离
    public float rollDistance; 
    //玩家滚动冷却时间
    public float rollCooldownTime; 


    //最大速度与最小速度之间取随机值
    public float GetMoveSpeed()
    {
        if (minMoveSpeed == maxMoveSpeed)
        {
            return minMoveSpeed;
        }
        else
        {
            return Random.Range(minMoveSpeed, maxMoveSpeed);
        }
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtlities.ValidateCheckPositiveRange(this, nameof(minMoveSpeed), minMoveSpeed, nameof(maxMoveSpeed), maxMoveSpeed, false);
        if (rollDistance != 0f || rollSpeed != 0 || rollCooldownTime != 0)
        {
            //确保是正数
            HelperUtlities.ValidateCheckPositiveValue(this, nameof(rollDistance), rollDistance, false);
            HelperUtlities.ValidateCheckPositiveValue(this, nameof(rollSpeed), rollSpeed, false);
            HelperUtlities.ValidateCheckPositiveValue(this, nameof(rollCooldownTime), rollCooldownTime, false);
        }
    }

#endif
    #endregion Validation
}
