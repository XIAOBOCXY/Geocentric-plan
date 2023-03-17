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
    //�ƶ������λ��
    public Vector3 movePosition;
    //��ǰλ��
    public Vector3 currentPosition;
    //�ƶ��ٶ�
    public float moveSpeed;
    //�ƶ���������
    public Vector2 moveDirection;
    //�Ƿ����
    public bool isRolling;
}