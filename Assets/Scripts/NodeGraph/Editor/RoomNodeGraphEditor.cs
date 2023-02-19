using UnityEngine;
using UnityEditor;//EditorWindow
using UnityEditor.Callbacks;//OnOpenAssetAttribute
public class RoomNodeGraphEditor : EditorWindow //�̳�EditorWindow��
{
    private GUIStyle roomNodeStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeTypeListSO roomNodeTypeList;

    private const float nodeWidth = 160f;//����
    private const float nodeHeight = 75f;//����
    private const int nodePadding = 25;//����ڱ߾�
    private const int nodeBorder = 12;//���߽�
    [MenuItem("Room Node Graph Editor",menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]//��Ӳ˵���
    private static void openWindow()//���������򿪱༭������
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");//���ش���
    }

    private void OnEnable()
    {
        roomNodeStyle = new GUIStyle();//GUIStyle���Ի����Ѿ����ڵ�ʵ��newһ���µ�ʵ��,ֻ���ԭ�е�Ч���в������Լ�����Ľ����޸�
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;//����һ��������Դ����ʽΪnode1
        roomNodeStyle.normal.textColor = Color.white;//���������ɫΪ��ɫ
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);//�ڱ߾�
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);//�߽�

        //���ط���ڵ�����
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
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
            //�����¼�
            ProcessEvents(Event.current);//Event.current������������ĵ�ǰ�¼�
            //���Ʒ���ڵ�
            DrawRoomNodes();
        }
        if (GUI.changed)
        {
            Repaint();//���»���
        }
    }

    //�����¼�����
    private void ProcessEvents(Event currentEvent)
    {
        //������ڵ�ͼ���¼�
        ProcessRoomNodeGraphEvents(currentEvent);
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
    }

    //��ʾ�����Ĳ˵�����
    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();//GenericMenu �����������Զ��������Ĳ˵��������˵�
        menu.AddItem(new GUIContent("create Room Node"), false, createRoomNode, mousePosition);//��˵����һ����
        menu.ShowAsContext();//�Ҽ�����ʱ���������ʾ�˵�
    }

    //���������λ�ô���һ������ڵ�
    private void createRoomNode(object mousePositionObject)
    {
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
    }

    //��ͼ�α༭���л��Ʒ���ڵ�
    private void DrawRoomNodes()
    {
        //ѭ�����еķ���ڵ㲢�һ���
        foreach(RoomNodeSO roomnode in currentRoomNodeGraph.roomNodeList)
        {
            roomnode.Draw(roomNodeStyle);
        }
        GUI.changed = true;
    }
}