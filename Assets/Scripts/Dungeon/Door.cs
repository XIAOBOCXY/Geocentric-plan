using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class Door : MonoBehaviour
{
    [Space(10)]
    [Header("OBJECT REFERENCES")]

    [SerializeField] private BoxCollider2D doorCollider;

    [HideInInspector] public bool isBossRoomDoor = false;
    private BoxCollider2D doorTrigger;
    //�Ƿ��
    private bool isOpen = false;
    //֮ǰ��û�д�
    private bool previouslyOpened = false;
    private Animator animator;

    private void Awake()
    {
        //Ĭ�ϲ����ŵ�collider
        doorCollider.enabled = false;

        animator = GetComponent<Animator>();
        doorTrigger = GetComponent<BoxCollider2D>();
    }

    //����trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�������һ�����ҵ����������ŵ�trigger
        if (collision.tag == Settings.playerTag || collision.tag == Settings.playerWeapon)
        {
            //����
            OpenDoor();
        }
    }
    private void OnEnable()
    {
        //�������Ŷ���������״̬
        animator.SetBool(Settings.open, isOpen);
    }


    //����
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            previouslyOpened = true;
            doorCollider.enabled = false;
            doorTrigger.enabled = false;
            //�����Ŷ����еĲ���
            animator.SetBool(Settings.open, true);
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void LockDoor()
    {
        isOpen = false;
        doorCollider.enabled = true;
        doorTrigger.enabled = false;

        //�����Ŷ����еĲ���
        animator.SetBool(Settings.open, false);
    }

    /// <summary>
    /// ������
    /// </summary>
    public void UnlockDoor()
    {
        doorCollider.enabled = false;
        doorTrigger.enabled = true;

        if (previouslyOpened == true)
        {
            isOpen = false;
            OpenDoor();
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtlities.ValidateCheckNullValue(this, nameof(doorCollider), doorCollider);
    }
#endif
    #endregion

}
