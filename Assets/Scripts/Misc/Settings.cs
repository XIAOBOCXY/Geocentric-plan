using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings//添加static,将其改为静态方法，删除MonoBehaviour脚本基类
{

    public const float pixelsPerUnit = 16f;
    public const float tileSizePixels = 16f;

    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion



    #region ROOM SETTINGS
    public const float fadeInTime = 0.5f; // 淡入房间时间
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
    public static int rollUp = Animator.StringToHash("rollUp");
    public static int rollRight = Animator.StringToHash("rollRight");
    public static int rollLeft = Animator.StringToHash("rollLeft");
    public static int rollDown = Animator.StringToHash("rollDown");


    //门的动画参数
    public static int open = Animator.StringToHash("open");

    #endregion

    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";

}
