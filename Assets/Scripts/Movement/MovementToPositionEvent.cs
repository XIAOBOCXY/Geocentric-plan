using System;
using UnityEngine;

[DisallowMultipleComponent]
public class MovementToPositionEvent : MonoBehaviour
{
    public event Action<MovementToPositionEvent, MovementToPositionArgs> OnMovementToPosition;

    public void CallMovementToPositionEvent(Vector3 movePosition, Vector3 currentPosition, float moveSpeed, Vector2 moveDirection, bool isRolling = false)
    {
        OnMovementToPosition?.Invoke(this, new MovementToPositionArgs() { movePosition = movePosition, currentPosition = currentPosition, moveSpeed = moveSpeed, moveDirection = moveDirection, isRolling = isRolling });
    }
}

public class MovementToPositionArgs : EventArgs
{
    //移动到达的位置
    public Vector3 movePosition;
    //当前位置
    public Vector3 currentPosition;
    //移动速度
    public float moveSpeed;
    //移动方向向量
    public Vector2 moveDirection;
    //是否滚动
    public bool isRolling;
}