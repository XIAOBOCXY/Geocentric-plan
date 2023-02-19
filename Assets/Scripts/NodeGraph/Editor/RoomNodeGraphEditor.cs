using UnityEngine;
using UnityEditor;//EditorWindow
using UnityEditor.Callbacks;//OnOpenAssetAttribute
public class RoomNodeGraphEditor : EditorWindow //继承EditorWindow类
{
    private GUIStyle roomNodeStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeTypeListSO roomNodeTypeList;

    private const float nodeWidth = 160f;//结点宽
    private const float nodeHeight = 75f;//结点高
    private const int nodePadding = 25;//结点内边距
    private const int nodeBorder = 12;//结点边界
    [MenuItem("Room Node Graph Editor",menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]//添加菜单项
    private static void openWindow()//创建函数打开编辑器窗口
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");//返回窗口
    }

    private void OnEnable()
    {
        roomNodeStyle = new GUIStyle();//GUIStyle可以基于已经存在的实例new一个新的实例,只需对原有的效果中不符合自己需求的进行修改
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;//加载一个内置资源，样式为node1
        roomNodeStyle.normal.textColor = Color.white;//结点字体颜色为白色
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);//内边距
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);//边界

        //加载房间节点类型
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    //如果在inspector中双击房间节点图形脚本化资产，则打开房间节点图形编辑器窗口
    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID,int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;//根据instanceid获得hierarchy中的object，将实例 ID 转换为对对象的引用
        if (roomNodeGraph != null)//判断roomNodeGraph是否为空
        {
            openWindow();
            currentRoomNodeGraph = roomNodeGraph;
            return true;
        }
        return false;
    }

    private void OnGUI()
    {
        /*Debug.Log("OnGUI has been called");//调试日志

        //结点1
        GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);//位置，大小，样式
        EditorGUILayout.LabelField("Node 1");//文本
        GUILayout.EndArea();//结束区域

        //结点2
        GUILayout.BeginArea(new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);//位置，大小，样式
        EditorGUILayout.LabelField("Node 2");//文本
        GUILayout.EndArea();//结束区域*/

        //如果房间节点图形脚本化对象类型的一个脚本化对象被选择，会执行以下的步骤
        if (currentRoomNodeGraph != null)
        {
            //处理事件
            ProcessEvents(Event.current);//Event.current将被立即处理的当前事件
            //绘制房间节点
            DrawRoomNodes();
        }
        if (GUI.changed)
        {
            Repaint();//重新绘制
        }
    }

    //处理事件函数
    private void ProcessEvents(Event currentEvent)
    {
        //处理房间节点图形事件
        ProcessRoomNodeGraphEvents(currentEvent);
    }

    //处理房间节点图形事件函数
    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            //处理鼠标按下事件
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            default:
                break;
        }
    }

    //处理鼠标按下事件函数
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        //处理鼠标按下右键，显示上下文菜单
        if (currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
    }

    //显示上下文菜单函数
    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();//GenericMenu 允许您创建自定义上下文菜单和下拉菜单
        menu.AddItem(new GUIContent("create Room Node"), false, createRoomNode, mousePosition);//向菜单添加一个项
        menu.ShowAsContext();//右键单击时在鼠标下显示菜单
    }

    //在鼠标点击的位置创建一个房间节点
    private void createRoomNode(object mousePositionObject)
    {
        createRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone)); ;
    }

    //重载 在鼠标点击的位置创建一个房间节点
    private void createRoomNode(object mousePositionObject,RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;//把鼠标位置对象转换为向量2d

        //创建房间节点脚本化对象资产
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();//创建脚本化对象的实例
        //把房间节点加入到当前房间节点图形的房间节点列表中
        currentRoomNodeGraph.roomNodeList.Add(roomNode);
        //设置房间节点值
        roomNode.Initialise(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);
        //把房间节点加入到房间节点图形脚本化对象资产数据库中
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);//AssetDatabase.AddObjectToAsset将 objectToAdd 添加到 path 下的现有资源中

        AssetDatabase.SaveAssets();//将所有未保存的资源更改写入磁盘
    }

    //在图形编辑器中绘制房间节点
    private void DrawRoomNodes()
    {
        //循环所有的房间节点并且绘制
        foreach(RoomNodeSO roomnode in currentRoomNodeGraph.roomNodeList)
        {
            roomnode.Draw(roomNodeStyle);
        }
        GUI.changed = true;
    }
}