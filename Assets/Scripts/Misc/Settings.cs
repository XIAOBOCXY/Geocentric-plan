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
}
