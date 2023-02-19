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

    //初始化节点
    public void Initialise(Rect rect,RoomNodeGraphSO nodeGraph,RoomNodeTypeSO roomNodeType)
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
        int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
        int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());//EditorGUILayout.Popup以参数形式获取当前所选的索引，并返回用户选择的索引
        roomNodeType = roomNodeTypeList.list[selection];//获取到选择的房间节点类型
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

#endif
    #endregion
}
