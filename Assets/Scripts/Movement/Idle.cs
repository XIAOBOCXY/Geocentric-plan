using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IdleEvent))]
[DisallowMultipleComponent]
public class Idle : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private IdleEvent idleEvent;

    private void Awake()
    {
        //�������
        rigidBody2D = GetComponent<Rigidbody2D>();
        idleEvent = GetComponent<IdleEvent>();

    }

    private void OnEnable()
    {
        //����idle event
        idleEvent.OnIdle += IdleEvent_OnIdle;
    }

    private void OnDisable()
    {
        //ȡ������idle event
        idleEvent.OnIdle -= IdleEvent_OnIdle;
    }

    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        //�ƶ��������
        MoveRigidBody();
    }

    //�ƶ��������
    private void MoveRigidBody()
    {
        // Rigidbody2D.velocity:�����������ƶ���ĳһλ��
        rigidBody2D.velocity = Vector2.zero;
    }
}
