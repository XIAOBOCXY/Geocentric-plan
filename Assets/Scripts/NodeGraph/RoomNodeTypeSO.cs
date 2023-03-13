//房间节点类型脚本化对象类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType", menuName = "Scriptable Objects/Dungeon/Room Node Type")] //添加Assets菜单按钮
public class RoomNodeTypeSO : ScriptableObject  //继承ScriptableObject类
{
    public string roomNodeTypeName;

    #region Header
    [Header("只有标记了房间节点类型才能在编辑器中看到")]
    #endregion Header
    public bool displayInNodeGraphEditor = true;
    #region Header
    [Header("是否是走廊")]
    #endregion Header
    public bool isCorridor;
    #region Header
    [Header("是否是南北走廊")]
    #endregion Header
    public bool isCorridorNS;
    #region Header
    [Header("是否是东西走廊")]
    #endregion Header
    public bool isCorridorEW;
    #region Header
    [Header("是否是入口")]
    #endregion Header
    public bool isEntrance;
    #region Header
    [Header("是否是boss房间")]
    #endregion Header
    public bool isBossRoom;
    #region Header
    [Header("没有被分配房间节点类型")]
    #endregion Header
    public bool isNone;

    #region Validation 
#if UNITY_EDITOR //平台判断，只有在unity编辑器中执行，才会执行以下代码
    private void OnValidate() //用来验证一些数据，脚本加载或Inspector中的任何值被修改时会调用
    {
        HelperUtlities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName); //判断房间节点类型名称是否为空字符串
    }
#endif
    #endregion
}
