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
    //是否打开
    private bool isOpen = false;
    //之前有没有打开
    private bool previouslyOpened = false;
    private Animator animator;

    private void Awake()
    {
        //默认不开门的collider
        doorCollider.enabled = false;

        animator = GetComponent<Animator>();
        doorTrigger = GetComponent<BoxCollider2D>();
    }

    //进入trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果是玩家或者玩家的武器进入门的trigger
        if (collision.tag == Settings.playerTag || collision.tag == Settings.playerWeapon)
        {
            //开门
            OpenDoor();
        }
    }
    private void OnEnable()
    {
        //保存下门动画参数的状态
        animator.SetBool(Settings.open, isOpen);
    }


    //开门
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            previouslyOpened = true;
            doorCollider.enabled = false;
            doorTrigger.enabled = false;
            //设置门动画中的参数
            animator.SetBool(Settings.open, true);
        }
    }

    /// <summary>
    /// 锁门
    /// </summary>
    public void LockDoor()
    {
        isOpen = false;
        doorCollider.enabled = true;
        doorTrigger.enabled = false;

        //设置门动画中的参数
        animator.SetBool(Settings.open, false);
    }

    /// <summary>
    /// 解锁门
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
