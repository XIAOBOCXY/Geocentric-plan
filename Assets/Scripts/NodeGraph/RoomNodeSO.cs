//房间节点脚本化对象类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNodeSO : ScriptableObject //继承ScriptableObject类
{
    [HideInInspector] public string id; //房间节点的id
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>(); //父亲房间节点id列表
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();  //子房间节点id列表
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;  //房间节点图
    public RoomNodeTypeSO roomNodeType;  //房间节点类型
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;  //房间节点类型列表

}
