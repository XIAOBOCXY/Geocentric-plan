using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DungeonLevel_",menuName ="Scriptable Objects/Dungeon/Dungeon Level")]
public class DungeonLevelSO : ScriptableObject
{
    #region Header BASIC LEVEL DETAILS
    [Space(10)]
    [Header("BASIC LEVEL DETAILS")]
    #endregion Header BASIC LEVEL DETAILS

    #region Tooltip
    [Tooltip("level名称")]
    #endregion Tooltip
    public string levelName;

    #region Header ROOM TEMPLATES FOR LEVEL
    [Space(10)]
    [Header("ROOM TEMPLATES FOR LEVEL")]
    #endregion Header ROOM TEMPLATES FOR LEVEL
    #region Tooltip
    [Tooltip("把不同level的room template填充到list中")]
    #endregion Tooltip
    public List<RoomTemplateSO> roomTemplateList;

    #region Header ROOM NODE GRAPHS FOR LEVEL
    [Space(10)]
    [Header("ROOM NODE GRAPHS FOR LEVEL")]
    #endregion Header ROOM NODE GRAPHS FOR LEVEL
    #region Tooltip
    [Tooltip("在list中填充level可以随机选择的房间节点图")]
    #endregion Tooltip
    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation
#if UNITY_EDITOR
    //验证可编写脚本对象详细信息不为空
    private void OnValidate()
    {
        HelperUtlities.ValidateCheckEmptyString(this, nameof(levelName), levelName);
        if (HelperUtlities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList))
            return;
        if (HelperUtlities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList))
            return;
        //接下来确保指定的房间节点图中的节点类型的房间模板存在

        //首先确保南北走廊、东西走廊和入口类型都存在
        bool isEWCorrider = false;//是东西走廊
        bool isNSCorrider = false;//是南北走廊
        bool isEntrance = false;//是入口

        //循环遍历所有的房间模板，确保这个节点类型存在
        foreach(RoomTemplateSO roomTemplateSO in roomTemplateList)
        {
            if (roomTemplateSO == null)
                return;
            if (roomTemplateSO.roomNodeType.isCorridorEW)
                isEWCorrider = true;
            if (roomTemplateSO.roomNodeType.isCorridorNS)
                isNSCorrider = true;
            if (roomTemplateSO.roomNodeType.isEntrance)
                isEntrance = true;
        }
        if (isEWCorrider == false)
        {
            Debug.Log("在" + this.name.ToString() + "中，没有东西走廊类型的房间");
        }
        if (isNSCorrider == false)
        {
            Debug.Log("在" + this.name.ToString() + "中，没有南北走廊类型的房间");
        }
        if (isEntrance == false)
        {
            Debug.Log("在" + this.name.ToString() + "中，没有入口类型的房间");
        }

        //循环遍历所有房间节点图
        foreach(RoomNodeGraphSO roomNodeGraph in roomNodeGraphList)
        {
            if (roomNodeGraph == null)
                return;
            //循环遍历该房间节点图中的所有房间节点
            foreach(RoomNodeSO roomNodeSO in roomNodeGraph.roomNodeList)
            {
                if (roomNodeSO == null)
                    continue;

                //接下来确保每一种房间类型，都存在至少一个房间模板

                //走廊和入口已经确定存在了，直接continue
                if(roomNodeSO.roomNodeType.isEntrance || roomNodeSO.roomNodeType.isCorridorEW || roomNodeSO.roomNodeType.isCorridorNS || roomNodeSO.roomNodeType.isCorridor || roomNodeSO.roomNodeType.isNone)
                {
                    continue;
                }

                bool isRoomNodeTypeFound = false;
                //循环遍历所有的房间模板，确保每种房间类型都有房间模板
                foreach(RoomTemplateSO roomTemplateSO in roomTemplateList)
                {
                    if (roomTemplateSO == null)
                        continue;
                    if (roomTemplateSO.roomNodeType == roomNodeSO.roomNodeType)
                    {
                        isRoomNodeTypeFound = true;
                        break;
                    }
                }
                if (!isRoomNodeTypeFound)
                {
                    Debug.Log("在" + this.name.ToString() + "中，房间节点图" + roomNodeGraph.name.ToString() + "中的房间模板类型" + roomNodeSO.roomNodeType.name.ToString() + "没有找到");
                }
            }
        }
    }
#endif
    #endregion Validation
}
