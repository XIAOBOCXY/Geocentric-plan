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

    //脚本加载的时候执行的代码
    private void Awake()
    {
        //加载房间节点字典
        LoadRoomNodeDictionary();
    }

    //加载房间节点字典函数
    private void LoadRoomNodeDictionary()
    {
        //清空房间节点字典
        roomNodeDictionary.Clear();
        //将房间节点列表内的房间节点与id一一对应
        foreach(RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node;
        }
    }

    //通过roomnode ID获取room node
    public RoomNodeSO GetRoomNode(string roomNodeID)
    {
        //在roomNodeDictionary中查找roomnode ID对应的room node
        if (roomNodeDictionary.TryGetValue(roomNodeID,out RoomNodeSO roomNode))//不确定是否存在时加out
        {
            return roomNode;
        }
        return null;
    }

    #region Editor Code
#if UNITY_EDITOR
    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom = null;//连接线的起始房间节点
    [HideInInspector] public Vector2 linePosition;//连接线位置

    //OnValidate可以用来验证一些数据，脚本加载或Inspector中的任何值被修改时会调用
    public void OnValidate()
    {
        //加载房间节点字典
        LoadRoomNodeDictionary();
    }

    //设置连接线的起始房间节点
    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node,Vector2 position)
    {
        roomNodeToDrawLineFrom = node;//连接线的起始房间节点
        linePosition = position;//连接线位置
    }

#endif
#endregion Editor Code
}
