//房间节点脚本化对象类
using System;//Guid
using System.Collections.Generic;
using UnityEditor;//EditorGUI
using UnityEngine;

public class RoomNodeSO : ScriptableObject //继承ScriptableObject类
{
    [HideInInspector] public string id; //房间节点的id
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>(); //父亲房间节点id列表
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();  //子房间节点id列表
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;  //房间节点图
    public RoomNodeTypeSO roomNodeType;  //房间节点类型
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;  //房间节点类型列表


    #region Editor Code
#if UNITY_EDITOR

    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false;//左键拖动房间节点
    [HideInInspector] public bool isSelected = false;//当前房间节点被选中
    //初始化节点
    public void Initialise(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;
        //加载房间节点类型列表
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    //绘制nodestyle类型的节点
    public void Draw(GUIStyle nodeStyle)
    {
        //通过begin area来绘制节点
        GUILayout.BeginArea(rect, nodeStyle);//在一个固定的屏幕区域中开始 GUI 控件的 GUILayout 块
        EditorGUI.BeginChangeCheck();//检查代码块中是否有任何控件被更改

        // 如果父节点存在或者房间节点是入口
        if (parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
        {
            //锁定节点标签，不能更改
            EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
        }
        else
        {
            int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
            int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());//EditorGUILayout.Popup以参数形式获取当前所选的索引，并返回用户选择的索引
            roomNodeType = roomNodeTypeList.list[selection];//获取到选择的房间节点类型

            //如果房间类型更改，那么连接不合法
            if(roomNodeTypeList.list[selected].isCorridor && !roomNodeTypeList.list[selection].isCorridor || !roomNodeTypeList.list[selected].isCorridor
                && roomNodeTypeList.list[selection].isCorridor || !roomNodeTypeList.list[selected].isBossRoom && roomNodeTypeList.list[selection].isBossRoom){
                //如果有子房间节点
                if (childRoomNodeIDList.Count > 0)
                {
                    //遍历子房间节点
                    for(int i = childRoomNodeIDList.Count - 1; i >= 0; i--)
                    {
                        //获取子房间节点
                        RoomNodeSO childRoomNode = roomNodeGraph.GetRoomNode(childRoomNodeIDList[i]);
                        if (childRoomNode != null)
                        {
                            //子房间和该房间节点双向断绝父子关系
                            RemoveChildRoomNodeIDFromRoomNode(childRoomNode.id);
                            childRoomNode.RemoveParentRoomNodeIDFromRoomNode(id);
                        }
                    }
                }
            }
        }

        if (EditorGUI.EndChangeCheck())//如果在BeginChangeCheck和EndChangeCheck之间的代码块中，有控件被更改，就把它设置为脏
        {
            EditorUtility.SetDirty(this);//将 target 对象标记为“脏”（仅适用于非场景对象）
        }
        GUILayout.EndArea();
    }

    //创建一个填充了可被选择的房间节点类型的字符串数组
    public string[] GetRoomNodeTypesToDisplay()
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];
        for(int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }
        }
        return roomArray;
    }

    //节点的处理事件方法
    public void ProcessEvents(Event currentEvent)
    {
        switch (currentEvent.type) {
            //处理鼠标按下事件
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            //处理鼠标抬起事件
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;
            //处理鼠标拖动事件
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
            default:
                break;
        }
    }

    //处理鼠标按下事件
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        //如果是鼠标左键
        if (currentEvent.button == 0)
        {
            //处理左键点击事件
            ProcessLeftClickDownEvent();
        }
        //如果是鼠标右键
        else if (currentEvent.button == 1)
        {
            //处理右键点击事件
            ProcessRightClickDownEvent(currentEvent);
        }
    }

    //处理左键点击事件
    private void ProcessLeftClickDownEvent()
    {
        Selection.activeObject = this;//返回当前选择的物体，激活对象
        //切换 节点是否被选择的状态
        isSelected = !isSelected;
    }

    //处理右键点击事件
    private void ProcessRightClickDownEvent(Event currentEvent)
    {
        //设置连接线起始房间节点和连接线的位置
        roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
    }

    //处理鼠标抬起事件
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        //如果是鼠标左键
        if (currentEvent.button == 0)
        {
            //处理鼠标左键抬起事件
            ProcessLeftClickUpEvent();
        }
    }

    //处理鼠标左键抬起事件
    private void ProcessLeftClickUpEvent()
    {
        //如果左键点击拖动是true
        if (isLeftClickDragging)
        {
            //将左键点击拖动设为false，即取消左键点击拖动
            isLeftClickDragging = false;
        }
    }

    //处理鼠标拖动事件
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        //如果是鼠标左键
        if(currentEvent.button == 0){
            //处理鼠标左键拖动事件
            ProcessLeftMouseDragEvent(currentEvent);
        }
    }

    //处理鼠标左键拖动事件
    private void ProcessLeftMouseDragEvent(Event currentEvent)
    {
        isLeftClickDragging = true;//当前处于点击左键拖动状态
        DragNode(currentEvent.delta);//Event.delta 与上次事件相比该鼠标的相对移动
        GUI.changed = true;//GUI.changed 如果任何控件更改了输入数据的值，则返回 true
    }

    //拖动节点
    public void DragNode(Vector2 delta)
    {
        rect.position += delta;//节点矩形位置改变
        EditorUtility.SetDirty(this);//将 target 对象标记为“脏”
    }

    //把子房间节点id添加到房间节点，如果成功添加返回true
    public bool AddChildRoomNodeIDToRoomNode(string childID)
    {
        //判断子房间节点是否可以合法的加入到父房间
        if (IsChildRoomVaild(childID))
        {
            childRoomNodeIDList.Add(childID);
            return true;
        }
        else
        {
            return false;
        }
    }

    //判断子房间节点是否可以合法的加入到父房间，可以return true，否则false
    public bool IsChildRoomVaild(string childID)
    {
        //是否已经有连通的Boss房间
        bool isConnectedBossNodeAlready = false;
        foreach(RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
        {
            //有Boss房间，并且Boss房间有父房间
            if(roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0)
            {
                //已经有连通的Boss房间
                isConnectedBossNodeAlready = true;
            }
        }

        //验证合法性
        //要连接的子房间是Boss房间，并且已经有Boss连通了
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isBossRoom && isConnectedBossNodeAlready)
            return false;
        //要连接的子房间是None
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isNone)
            return false;
        //该房间节点的子房间中已经有这个要连接的子房间了，重复了
        if (childRoomNodeIDList.Contains(childID))
            return false;
        //子房间节点和该房间节点相同
        if (id == childID)
            return false;
        //要连接的和该房间节点的父房间相同
        if (parentRoomNodeIDList.Contains(childID))
            return false;
        //要连接的这个房间节点已经有父房间了
        if (roomNodeGraph.GetRoomNode(childID).parentRoomNodeIDList.Count > 0)
            return false;
        //走廊+走廊
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && roomNodeType.isCorridor)
            return false;
        //不是走廊+不是走廊
        if (!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && !roomNodeType.isCorridor)
            return false;
        //要连接的是走廊，但是当前房间节点的走廊数已经最大了
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count >= Settings.maxChildCorridors)
            return false;
        //加入的是入口
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isEntrance)
            return false;
        //加入的不是走廊，要把房间连接到走廊，但是走廊已经有房间了
        if (!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count > 0)
            return false;


        return true;
    }

    //把父房间节点id添加到房间节点，如果成功添加返回true
    public bool AddParentRoomNodeIDToRoomNode(string parentID)
    {
        parentRoomNodeIDList.Add(parentID);
        return true;
    }

    //如果有子房间节点就删除
    public bool RemoveChildRoomNodeIDFromRoomNode(String childID)
    {
        //如果子房间列表中有childID
        if (childRoomNodeIDList.Contains(childID))
        {
            childRoomNodeIDList.Remove(childID);
            return true;
        }
        return false;
    }

    //如果有父房间节点就删除
    public bool RemoveParentRoomNodeIDFromRoomNode(string parentID)
    {
        //如果父房间列表中有parentID
        if (parentRoomNodeIDList.Contains(parentID))
        {
            parentRoomNodeIDList.Remove(parentID);
            return true;
        }
        return false;
    }
#endif
    #endregion
}
