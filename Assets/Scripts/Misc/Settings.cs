using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings//添加static,将其改为静态方法，删除MonoBehaviour脚本基类
{
    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion 


    #region ROOM SETTINGS
    public const int maxChildCorridors = 3;//一个房间最多可以连接的孩子走廊数量
    #endregion

    #region ANIMATOR PARAMETERS
    //player的动画参数
    public static int aimUp = Animator.StringToHash("aimUp");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpRight = Animator.StringToHash("aimUpRight");
    public static int aimUpLeft = Animator.StringToHash("aimUpLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int isIdle = Animator.StringToHash("isIdle");
    public static int isMoving = Animator.StringToHash("isMoving");
    #endregion
}
