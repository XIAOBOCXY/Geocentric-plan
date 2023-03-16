//游戏资源脚本
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;//实例
    public static GameResources Instance
    {
        get
        {
            if (instance == null)//检查实例是否为空
            {
                instance = Resources.Load<GameResources>("GameResources");//资源加载方法，将游戏资源类型的对象加载到实例中
            }
            return instance;//返回实例
        }
    }

    #region Header DUNGEON
    [Space(10)]
    [Header("DUNGEON")]
    #endregion
    #region Tooltip
    [Tooltip("用房间节点类型列表填充地牢")]
    #endregion

    public RoomNodeTypeListSO roomNodeTypeList;


    #region Header Player
    [Space(10)]
    [Header("PLAYER")]
    #endregion
    #region Tooltip
    [Tooltip("当前选择的玩家")]
    #endregion

    public CurrentPlayerSO currentPlayer;

    #region Header MATERIALS
    [Space(10)]
    [Header("MATERIALS")]
    #endregion
    #region Tooltip
    [Tooltip("变暗的材质")]
    #endregion
    public Material dimmerMaterial;
}
