using UnityEngine;
using UnityEditor;//EditorWindow
using UnityEditor.Callbacks;//OnOpenAssetAttribute
public class RoomNodeGraphEditor : EditorWindow //继承EditorWindow类
{
    private GUIStyle roomNodeStyle;
    private GUIStyle roomNodeSelectedStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeSO currentRoomNode = null;//当前选择的房间节点
    private RoomNodeTypeListSO roomNodeTypeList;

    private const float nodeWidth = 160f;//结点宽
    private const float nodeHeight = 75f;//结点高
    private const int nodePadding = 25;//结点内边距
    private const int nodeBorder = 12;//结点边界

    //连接线值
    private const float connectingLineWidth = 3f;//连接线宽度
    private const float connectingLineArrowSize = 6f;//连接线箭头大小

    [MenuItem("Room Node Graph Editor",menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]//添加菜单项
    private static void openWindow()//创建函数打开编辑器窗口
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");//返回窗口
    }

    private void OnEnable()
    {
        //Selection.selectionChanged选择的东西变化的时候调用
        Selection.selectionChanged += InspectorSelectionChanged;

        //定义节点布局样式
        roomNodeStyle = new GUIStyle();//GUIStyle可以基于已经存在的实例new一个新的实例,只需对原有的效果中不符合自己需求的进行修改
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;//加载一个内置资源，样式为node1
        roomNodeStyle.normal.textColor = Color.white;//结点字体颜色为白色
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);//内边距
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);//边界

        //定义被选中的节点样式
        roomNodeSelectedStyle = new GUIStyle();//GUIStyle可以基于已经存在的实例new一个新的实例,只需对原有的效果中不符合自己需求的进行修改
        roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;//加载一个内置资源，样式为node1 on
        roomNodeSelectedStyle.normal.textColor = Color.white;//结点字体颜色为白色
        roomNodeSelectedStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);//内边距
        roomNodeSelectedStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);//边界

        //加载房间节点类型
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    private void OnDisable()
    {
        //取消委托，选择的东西变化的时候不再调用
        Selection.selectionChanged -= InspectorSelectionChanged;
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
            //如果被拖动，则画连接线
            DrawDraggedLine();
            //处理事件
            ProcessEvents(Event.current);//Event.current将被立即处理的当前事件
            //绘制房间节点之间的连接线
            DrawRoomConnections();
            //绘制房间节点
            DrawRoomNodes();
        }
        if (GUI.changed)
        {
            Repaint();//重新绘制
        }
    }

    //如果被拖动，则画连接线
    private void DrawDraggedLine()
    {
        if (currentRoomNodeGraph.linePosition != Vector2.zero)
        {
            //绘制连接线，从房间节点到连接线末尾
            Handles.DrawBezier(currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition,
                currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, Color.white, null, connectingLineWidth);
        }
    }

    //处理事件函数
    private void ProcessEvents(Event currentEvent)
    {
        //如果当前房间节点为空或当前房间节点未被拖动，则获取鼠标所在的房间节点
        if (currentRoomNode==null || currentRoomNode.isLeftClickDragging == false)
        {
            //返回当前鼠标悬停的房间节点
            currentRoomNode = IsMouseOverRoomNode(currentEvent);
        }
        //如果鼠标没有悬停在任意一个房间节点上 或 连接线起始房间节点不为空
        if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom!=null)
        {
            //处理房间节点图形事件
            ProcessRoomNodeGraphEvents(currentEvent);
        }
        //否则运行房间节点事件
        else
        {
            //运行房间节点事件
            currentRoomNode.ProcessEvents(currentEvent);
        }
    }

    //检查鼠标是否悬停在房间节点上，如果是则返回房间节点，如果不是则返回null
    private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
    {
        for(int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >= 0; i--)//遍历当前房间节点图中的每一个房间节点
        {
            if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition))//判断当前的鼠标坐标是否在房间节点的矩形中
            {
                return currentRoomNodeGraph.roomNodeList[i];//如果在则返回该房间节点，说明鼠标悬停在当前房间节点矩形上
            }
        }
        return null;//不在则返回null,说明鼠标没有悬停在任意一个房间节点矩形上
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

    //处理鼠标按下事件函数
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        //处理鼠标按下右键，显示上下文菜单
        if (currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
        //处理鼠标左键按下事件
        else if (currentEvent.button == 0)
        {
            //清除连接线
            ClearLineDrag();
            //清除所有选择的房间节点
            ClearAllSelectedRoomNodes();
        }
    }

    //显示上下文菜单函数
    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();//GenericMenu 允许您创建自定义上下文菜单和下拉菜单
        menu.AddItem(new GUIContent("create Room Node"), false, createRoomNode, mousePosition);//向菜单添加一个项
        menu.AddSeparator("");//分隔
        menu.AddItem(new GUIContent("Select All Room Nodes"), false, SelectAllRoomNodes);//添加 选择所有房间节点 选项
        menu.ShowAsContext();//右键单击时在鼠标下显示菜单
    }

    //在鼠标点击的位置创建一个房间节点
    private void createRoomNode(object mousePositionObject)
    {
        //如果当前房间节点图的房间节点列表为空，即没有一个房间节点
        if (currentRoomNodeGraph.roomNodeList.Count == 0)
        {
            //在（200,200）创建入口节点
            createRoomNode(new Vector2(200f, 200f), roomNodeTypeList.list.Find(x => x.isEntrance));
        }
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

        //更新当前房间节点图形的 房间节点字典
        currentRoomNodeGraph.OnValidate();
    }

    //清除所有选择的房间节点
    private void ClearAllSelectedRoomNodes()
    {
        //循环遍历房间节点图的每个房间节点
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            //如果该房间节点被选择
            if (roomNode.isSelected)
            {
                //取消选择该房间节点
                roomNode.isSelected = false;
                GUI.changed = true;
            }
        }
    }

    //选择所有节点
    private void SelectAllRoomNodes()
    {
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.isSelected = true;
        }
        GUI.changed = true;
    }

    //处理鼠标抬起事件
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        //正在拖动连接线时释放鼠标右键
        if(currentEvent.button==1 && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            //roomNode是释放鼠标时悬停的房间节点，即连接线末端的房间节点
            RoomNodeSO roomNode = IsMouseOverRoomNode(currentEvent);
            //如果连接线末端的房间节点存在
            if (roomNode != null)
            {
                //如果能把末端房间节点加入到起始房间节点的子房间列表中
                if (currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(roomNode.id))
                {
                    //把起始房间节点加入到末端房间节点的父房间节点列表中
                    roomNode.AddParentRoomNodeIDToRoomNode(currentRoomNodeGraph.roomNodeToDrawLineFrom.id);
                }
            }
            //清除连接线拖拽
            ClearLineDrag();
        }
    }

    //处理鼠标拖动事件
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        //如果是鼠标右键
        if (currentEvent.button == 1)
        {
            //处理鼠标右键拖动事件
            ProcessRightMouseDragEvent(currentEvent);
        }
    }

    //处理鼠标右键拖动事件
    private void ProcessRightMouseDragEvent(Event currentEvent)
    {
        //如果连接线起始房间节点不为空
        if (currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            //拖动连接线方法
            DragConnectingLine(currentEvent.delta);
            GUI.changed = true;
        }
    }

    //从起始房间节点拖动连接线
    public void DragConnectingLine(Vector2 delta)
    {
        //更新连接线末端的位置
        currentRoomNodeGraph.linePosition += delta;
    }

    //清除连接线
    private void ClearLineDrag()
    {
        //连接线的起始房间节点为null
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
        //连接线为（0,0）
        currentRoomNodeGraph.linePosition = Vector2.zero;
        //任何控件更改了输入数据的值，返回true
        GUI.changed = true;
    }

    //绘制房间节点之间的连接线
    private void DrawRoomConnections()
    {
        //循环遍历当前房间节点图的房间节点列表中所有的房间节点
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            //如果该房间节点有子房间节点
            if (roomNode.childRoomNodeIDList.Count > 0)
            {
                //循环遍历该房间节点的所有子房间节点
                foreach(string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    //如果子房间节点ID在房间节点字典中
                    if (currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childRoomNodeID))
                    {
                        //绘制房间节点和子房间节点之间的连接线
                        DrawConnectionLine(roomNode, currentRoomNodeGraph.roomNodeDictionary[childRoomNodeID]);
                        GUI.changed = true;
                    }
                }
            }
        }
    }

    //父房间节点和子房间节点之间画连接线
    private void DrawConnectionLine(RoomNodeSO parentRoomNode,RoomNodeSO childRoomNode)
    {
        //获取连接线起始和末尾位置
        Vector2 startPosition = parentRoomNode.rect.center;
        Vector2 endPosition = childRoomNode.rect.center;

        //计算中点
        Vector2 midPosition = (endPosition + startPosition) / 2f;
        //从开始到末尾的向量
        Vector2 direction = endPosition - startPosition;
        //计算距离中点的归一化（长度为1）垂直向量
        Vector2 arrowTailPoint1 = midPosition - new Vector2(direction.y, direction.x).normalized * connectingLineArrowSize;//下方的箭头尾
        Vector2 arrowTailPoint2 = midPosition + new Vector2(direction.y, direction.x).normalized * connectingLineArrowSize;//上方的箭头尾
        //计算箭头头部
        Vector2 arrowHeadPoint = midPosition + direction.normalized * connectingLineArrowSize;//箭头头部
        
        //画箭头
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, arrowTailPoint1, Color.white, null, connectingLineWidth);
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, arrowTailPoint2, Color.white, null, connectingLineWidth);
        //画线
        Handles.DrawBezier(startPosition, endPosition, startPosition, endPosition, Color.white, null, connectingLineWidth);
        GUI.changed = true;
    }



    //在图形编辑器中绘制房间节点
    private void DrawRoomNodes()
    {
        //循环所有的房间节点并且绘制
        foreach(RoomNodeSO roomnode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomnode.isSelected)
            {
                roomnode.Draw(roomNodeSelectedStyle);
            }
            else
            {
                roomnode.Draw(roomNodeStyle);
            }
        }
        GUI.changed = true;
    }

    //面板中选项改变，即窗口更换资源
    private void InspectorSelectionChanged()
    {
        //获取当前激活的房间节点图
        RoomNodeGraphSO roomNodeGraph = Selection.activeObject as RoomNodeGraphSO;
        if (roomNodeGraph != null)
        {
            //更改当前选择的房间节点图
            currentRoomNodeGraph = roomNodeGraph;
            GUI.changed = true;
        }
    }
}