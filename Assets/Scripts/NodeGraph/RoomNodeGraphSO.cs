//房间节点图脚本化对象类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RoomNodeGraph",menuName ="Scriptable Object/Dungeon/Room Node Graph")] //添加Assets菜单按钮
public class RoomNodeGraphSO : ScriptableObject   //继承ScriptableObject类
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;//房间节点类型的列表
    [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();//房间节点列表
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();//房间节点字典，guid为关键字，类型为string

}
