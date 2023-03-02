using UnityEngine;
using UnityEditor;//EditorWindow
using UnityEditor.Callbacks;//OnOpenAssetAttribute
public class RoomNodeGraphEditor : EditorWindow //�̳�EditorWindow��
{
    private GUIStyle roomNodeStyle;
    private GUIStyle roomNodeSelectedStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeSO currentRoomNode = null;//��ǰѡ��ķ���ڵ�
    private RoomNodeTypeListSO roomNodeTypeList;

    private const float nodeWidth = 160f;//����
    private const float nodeHeight = 75f;//����
    private const int nodePadding = 25;//����ڱ߾�
    private const int nodeBorder = 12;//���߽�

    //������ֵ
    private const float connectingLineWidth = 3f;//�����߿��
    private const float connectingLineArrowSize = 6f;//�����߼�ͷ��С

    [MenuItem("Room Node Graph Editor",menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]//��Ӳ˵���
    private static void openWindow()//���������򿪱༭������
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");//���ش���
    }

    private void OnEnable()
    {
        //Selection.selectionChangedѡ��Ķ����仯��ʱ�����
        Selection.selectionChanged += InspectorSelectionChanged;

        //����ڵ㲼����ʽ
        roomNodeStyle = new GUIStyle();//GUIStyle���Ի����Ѿ����ڵ�ʵ��newһ���µ�ʵ��,ֻ���ԭ�е�Ч���в������Լ�����Ľ����޸�
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;//����һ��������Դ����ʽΪnode1
        roomNodeStyle.normal.textColor = Color.white;//���������ɫΪ��ɫ
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);//�ڱ߾�
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);//�߽�

        //���屻ѡ�еĽڵ���ʽ
        roomNodeSelectedStyle = new GUIStyle();//GUIStyle���Ի����Ѿ����ڵ�ʵ��newһ���µ�ʵ��,ֻ���ԭ�е�Ч���в������Լ�����Ľ����޸�
        roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;//����һ��������Դ����ʽΪnode1 on
        roomNodeSelectedStyle.normal.textColor = Color.white;//���������ɫΪ��ɫ
        roomNodeSelectedStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);//�ڱ߾�
        roomNodeSelectedStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);//�߽�

        //���ط���ڵ�����
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    private void OnDisable()
    {
        //ȡ��ί�У�ѡ��Ķ����仯��ʱ���ٵ���
        Selection.selectionChanged -= InspectorSelectionChanged;
    }

    //�����inspector��˫������ڵ�ͼ�νű����ʲ�����򿪷���ڵ�ͼ�α༭������
    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID,int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;//����instanceid���hierarchy�е�object����ʵ�� ID ת��Ϊ�Զ��������
        if (roomNodeGraph != null)//�ж�roomNodeGraph�Ƿ�Ϊ��
        {
            openWindow();
            currentRoomNodeGraph = roomNodeGraph;
            return true;
        }
        return false;
    }

    private void OnGUI()
    {
        /*Debug.Log("OnGUI has been called");//������־

        //���1
        GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);//λ�ã���С����ʽ
        EditorGUILayout.LabelField("Node 1");//�ı�
        GUILayout.EndArea();//��������

        //���2
        GUILayout.BeginArea(new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);//λ�ã���С����ʽ
        EditorGUILayout.LabelField("Node 2");//�ı�
        GUILayout.EndArea();//��������*/

        //�������ڵ�ͼ�νű����������͵�һ���ű�������ѡ�񣬻�ִ�����µĲ���
        if (currentRoomNodeGraph != null)
        {
            //������϶�����������
            DrawDraggedLine();
            //�����¼�
            ProcessEvents(Event.current);//Event.current������������ĵ�ǰ�¼�
            //���Ʒ���ڵ�֮���������
            DrawRoomConnections();
            //���Ʒ���ڵ�
            DrawRoomNodes();
        }
        if (GUI.changed)
        {
            Repaint();//���»���
        }
    }

    //������϶�����������
    private void DrawDraggedLine()
    {
        if (currentRoomNodeGraph.linePosition != Vector2.zero)
        {
            //���������ߣ��ӷ���ڵ㵽������ĩβ
            Handles.DrawBezier(currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition,
                currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, Color.white, null, connectingLineWidth);
        }
    }

    //�����¼�����
    private void ProcessEvents(Event currentEvent)
    {
        //�����ǰ����ڵ�Ϊ�ջ�ǰ����ڵ�δ���϶������ȡ������ڵķ���ڵ�
        if (currentRoomNode==null || currentRoomNode.isLeftClickDragging == false)
        {
            //���ص�ǰ�����ͣ�ķ���ڵ�
            currentRoomNode = IsMouseOverRoomNode(currentEvent);
        }
        //������û����ͣ������һ������ڵ��� �� ��������ʼ����ڵ㲻Ϊ��
        if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom!=null)
        {
            //������ڵ�ͼ���¼�
            ProcessRoomNodeGraphEvents(currentEvent);
        }
        //�������з���ڵ��¼�
        else
        {
            //���з���ڵ��¼�
            currentRoomNode.ProcessEvents(currentEvent);
        }
    }

    //�������Ƿ���ͣ�ڷ���ڵ��ϣ�������򷵻ط���ڵ㣬��������򷵻�null
    private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
    {
        for(int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >= 0; i--)//������ǰ����ڵ�ͼ�е�ÿһ������ڵ�
        {
            if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition))//�жϵ�ǰ����������Ƿ��ڷ���ڵ�ľ�����
            {
                return currentRoomNodeGraph.roomNodeList[i];//������򷵻ظ÷���ڵ㣬˵�������ͣ�ڵ�ǰ����ڵ������
            }
        }
        return null;//�����򷵻�null,˵�����û����ͣ������һ������ڵ������
    }

    //������ڵ�ͼ���¼�����
    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            //������갴���¼�
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            //�������̧���¼�
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;
            //��������϶��¼�
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
            default:
                break;
        }
    }

    //������갴���¼�����
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        //������갴���Ҽ�����ʾ�����Ĳ˵�
        if (currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
        //���������������¼�
        else if (currentEvent.button == 0)
        {
            //���������
            ClearLineDrag();
            //�������ѡ��ķ���ڵ�
            ClearAllSelectedRoomNodes();
        }
    }

    //��ʾ�����Ĳ˵�����
    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();//GenericMenu �����������Զ��������Ĳ˵��������˵�
        menu.AddItem(new GUIContent("create Room Node"), false, createRoomNode, mousePosition);//��˵����һ����
        menu.AddSeparator("");//�ָ�
        menu.AddItem(new GUIContent("Select All Room Nodes"), false, SelectAllRoomNodes);//��� ѡ�����з���ڵ� ѡ��
        menu.ShowAsContext();//�Ҽ�����ʱ���������ʾ�˵�
    }

    //���������λ�ô���һ������ڵ�
    private void createRoomNode(object mousePositionObject)
    {
        //�����ǰ����ڵ�ͼ�ķ���ڵ��б�Ϊ�գ���û��һ������ڵ�
        if (currentRoomNodeGraph.roomNodeList.Count == 0)
        {
            //�ڣ�200,200��������ڽڵ�
            createRoomNode(new Vector2(200f, 200f), roomNodeTypeList.list.Find(x => x.isEntrance));
        }
        createRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone)); ;
    }

    //���� ���������λ�ô���һ������ڵ�
    private void createRoomNode(object mousePositionObject,RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;//�����λ�ö���ת��Ϊ����2d

        //��������ڵ�ű��������ʲ�
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();//�����ű��������ʵ��
        //�ѷ���ڵ���뵽��ǰ����ڵ�ͼ�εķ���ڵ��б���
        currentRoomNodeGraph.roomNodeList.Add(roomNode);
        //���÷���ڵ�ֵ
        roomNode.Initialise(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);
        //�ѷ���ڵ���뵽����ڵ�ͼ�νű��������ʲ����ݿ���
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);//AssetDatabase.AddObjectToAsset�� objectToAdd ��ӵ� path �µ�������Դ��

        AssetDatabase.SaveAssets();//������δ�������Դ����д�����

        //���µ�ǰ����ڵ�ͼ�ε� ����ڵ��ֵ�
        currentRoomNodeGraph.OnValidate();
    }

    //�������ѡ��ķ���ڵ�
    private void ClearAllSelectedRoomNodes()
    {
        //ѭ����������ڵ�ͼ��ÿ������ڵ�
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            //����÷���ڵ㱻ѡ��
            if (roomNode.isSelected)
            {
                //ȡ��ѡ��÷���ڵ�
                roomNode.isSelected = false;
                GUI.changed = true;
            }
        }
    }

    //ѡ�����нڵ�
    private void SelectAllRoomNodes()
    {
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.isSelected = true;
        }
        GUI.changed = true;
    }

    //�������̧���¼�
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        //�����϶�������ʱ�ͷ�����Ҽ�
        if(currentEvent.button==1 && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            //roomNode���ͷ����ʱ��ͣ�ķ���ڵ㣬��������ĩ�˵ķ���ڵ�
            RoomNodeSO roomNode = IsMouseOverRoomNode(currentEvent);
            //���������ĩ�˵ķ���ڵ����
            if (roomNode != null)
            {
                //����ܰ�ĩ�˷���ڵ���뵽��ʼ����ڵ���ӷ����б���
                if (currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(roomNode.id))
                {
                    //����ʼ����ڵ���뵽ĩ�˷���ڵ�ĸ�����ڵ��б���
                    roomNode.AddParentRoomNodeIDToRoomNode(currentRoomNodeGraph.roomNodeToDrawLineFrom.id);
                }
            }
            //�����������ק
            ClearLineDrag();
        }
    }

    //��������϶��¼�
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        //���������Ҽ�
        if (currentEvent.button == 1)
        {
            //��������Ҽ��϶��¼�
            ProcessRightMouseDragEvent(currentEvent);
        }
    }

    //��������Ҽ��϶��¼�
    private void ProcessRightMouseDragEvent(Event currentEvent)
    {
        //�����������ʼ����ڵ㲻Ϊ��
        if (currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            //�϶������߷���
            DragConnectingLine(currentEvent.delta);
            GUI.changed = true;
        }
    }

    //����ʼ����ڵ��϶�������
    public void DragConnectingLine(Vector2 delta)
    {
        //����������ĩ�˵�λ��
        currentRoomNodeGraph.linePosition += delta;
    }

    //���������
    private void ClearLineDrag()
    {
        //�����ߵ���ʼ����ڵ�Ϊnull
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
        //������Ϊ��0,0��
        currentRoomNodeGraph.linePosition = Vector2.zero;
        //�κοؼ��������������ݵ�ֵ������true
        GUI.changed = true;
    }

    //���Ʒ���ڵ�֮���������
    private void DrawRoomConnections()
    {
        //ѭ��������ǰ����ڵ�ͼ�ķ���ڵ��б������еķ���ڵ�
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            //����÷���ڵ����ӷ���ڵ�
            if (roomNode.childRoomNodeIDList.Count > 0)
            {
                //ѭ�������÷���ڵ�������ӷ���ڵ�
                foreach(string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    //����ӷ���ڵ�ID�ڷ���ڵ��ֵ���
                    if (currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childRoomNodeID))
                    {
                        //���Ʒ���ڵ���ӷ���ڵ�֮���������
                        DrawConnectionLine(roomNode, currentRoomNodeGraph.roomNodeDictionary[childRoomNodeID]);
                        GUI.changed = true;
                    }
                }
            }
        }
    }

    //������ڵ���ӷ���ڵ�֮�仭������
    private void DrawConnectionLine(RoomNodeSO parentRoomNode,RoomNodeSO childRoomNode)
    {
        //��ȡ��������ʼ��ĩβλ��
        Vector2 startPosition = parentRoomNode.rect.center;
        Vector2 endPosition = childRoomNode.rect.center;

        //�����е�
        Vector2 midPosition = (endPosition + startPosition) / 2f;
        //�ӿ�ʼ��ĩβ������
        Vector2 direction = endPosition - startPosition;
        //��������е�Ĺ�һ��������Ϊ1����ֱ����
        Vector2 arrowTailPoint1 = midPosition - new Vector2(direction.y, direction.x).normalized * connectingLineArrowSize;//�·��ļ�ͷβ
        Vector2 arrowTailPoint2 = midPosition + new Vector2(direction.y, direction.x).normalized * connectingLineArrowSize;//�Ϸ��ļ�ͷβ
        //�����ͷͷ��
        Vector2 arrowHeadPoint = midPosition + direction.normalized * connectingLineArrowSize;//��ͷͷ��
        
        //����ͷ
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, arrowTailPoint1, Color.white, null, connectingLineWidth);
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, arrowTailPoint2, Color.white, null, connectingLineWidth);
        //����
        Handles.DrawBezier(startPosition, endPosition, startPosition, endPosition, Color.white, null, connectingLineWidth);
        GUI.changed = true;
    }



    //��ͼ�α༭���л��Ʒ���ڵ�
    private void DrawRoomNodes()
    {
        //ѭ�����еķ���ڵ㲢�һ���
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

    //�����ѡ��ı䣬�����ڸ�����Դ
    private void InspectorSelectionChanged()
    {
        //��ȡ��ǰ����ķ���ڵ�ͼ
        RoomNodeGraphSO roomNodeGraph = Selection.activeObject as RoomNodeGraphSO;
        if (roomNodeGraph != null)
        {
            //���ĵ�ǰѡ��ķ���ڵ�ͼ
            currentRoomNodeGraph = roomNodeGraph;
            GUI.changed = true;
        }
    }
}