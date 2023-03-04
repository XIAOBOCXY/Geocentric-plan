//����ڵ�ű���������
using System;//Guid
using System.Collections.Generic;
using UnityEditor;//EditorGUI
using UnityEngine;

public class RoomNodeSO : ScriptableObject //�̳�ScriptableObject��
{
    [HideInInspector] public string id; //����ڵ��id
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>(); //���׷���ڵ�id�б�
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();  //�ӷ���ڵ�id�б�
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;  //����ڵ�ͼ
    public RoomNodeTypeSO roomNodeType;  //����ڵ�����
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;  //����ڵ������б�


    #region Editor Code
#if UNITY_EDITOR

    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false;//����϶�����ڵ�
    [HideInInspector] public bool isSelected = false;//��ǰ����ڵ㱻ѡ��
    //��ʼ���ڵ�
    public void Initialise(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;
        //���ط���ڵ������б�
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    //����nodestyle���͵Ľڵ�
    public void Draw(GUIStyle nodeStyle)
    {
        //ͨ��begin area�����ƽڵ�
        GUILayout.BeginArea(rect, nodeStyle);//��һ���̶�����Ļ�����п�ʼ GUI �ؼ��� GUILayout ��
        EditorGUI.BeginChangeCheck();//����������Ƿ����κοؼ�������

        // ������ڵ���ڻ��߷���ڵ������
        if (parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
        {
            //�����ڵ��ǩ�����ܸ���
            EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
        }
        else
        {
            int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
            int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());//EditorGUILayout.Popup�Բ�����ʽ��ȡ��ǰ��ѡ���������������û�ѡ�������
            roomNodeType = roomNodeTypeList.list[selection];//��ȡ��ѡ��ķ���ڵ�����

            //����������͸��ģ���ô���Ӳ��Ϸ�
            if(roomNodeTypeList.list[selected].isCorridor && !roomNodeTypeList.list[selection].isCorridor || !roomNodeTypeList.list[selected].isCorridor
                && roomNodeTypeList.list[selection].isCorridor || !roomNodeTypeList.list[selected].isBossRoom && roomNodeTypeList.list[selection].isBossRoom){
                //������ӷ���ڵ�
                if (childRoomNodeIDList.Count > 0)
                {
                    //�����ӷ���ڵ�
                    for(int i = childRoomNodeIDList.Count - 1; i >= 0; i--)
                    {
                        //��ȡ�ӷ���ڵ�
                        RoomNodeSO childRoomNode = roomNodeGraph.GetRoomNode(childRoomNodeIDList[i]);
                        if (childRoomNode != null)
                        {
                            //�ӷ���͸÷���ڵ�˫��Ͼ����ӹ�ϵ
                            RemoveChildRoomNodeIDFromRoomNode(childRoomNode.id);
                            childRoomNode.RemoveParentRoomNodeIDFromRoomNode(id);
                        }
                    }
                }
            }
        }

        if (EditorGUI.EndChangeCheck())//�����BeginChangeCheck��EndChangeCheck֮��Ĵ�����У��пؼ������ģ��Ͱ�������Ϊ��
        {
            EditorUtility.SetDirty(this);//�� target ������Ϊ���ࡱ���������ڷǳ�������
        }
        GUILayout.EndArea();
    }

    //����һ������˿ɱ�ѡ��ķ���ڵ����͵��ַ�������
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

    //�ڵ�Ĵ����¼�����
    public void ProcessEvents(Event currentEvent)
    {
        switch (currentEvent.type) {
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

    //������갴���¼�
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        //�����������
        if (currentEvent.button == 0)
        {
            //�����������¼�
            ProcessLeftClickDownEvent();
        }
        //���������Ҽ�
        else if (currentEvent.button == 1)
        {
            //�����Ҽ�����¼�
            ProcessRightClickDownEvent(currentEvent);
        }
    }

    //�����������¼�
    private void ProcessLeftClickDownEvent()
    {
        Selection.activeObject = this;//���ص�ǰѡ������壬�������
        //�л� �ڵ��Ƿ�ѡ���״̬
        isSelected = !isSelected;
    }

    //�����Ҽ�����¼�
    private void ProcessRightClickDownEvent(Event currentEvent)
    {
        //������������ʼ����ڵ�������ߵ�λ��
        roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
    }

    //�������̧���¼�
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        //�����������
        if (currentEvent.button == 0)
        {
            //����������̧���¼�
            ProcessLeftClickUpEvent();
        }
    }

    //����������̧���¼�
    private void ProcessLeftClickUpEvent()
    {
        //����������϶���true
        if (isLeftClickDragging)
        {
            //���������϶���Ϊfalse����ȡ���������϶�
            isLeftClickDragging = false;
        }
    }

    //��������϶��¼�
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        //�����������
        if(currentEvent.button == 0){
            //�����������϶��¼�
            ProcessLeftMouseDragEvent(currentEvent);
        }
    }

    //�����������϶��¼�
    private void ProcessLeftMouseDragEvent(Event currentEvent)
    {
        isLeftClickDragging = true;//��ǰ���ڵ������϶�״̬
        DragNode(currentEvent.delta);//Event.delta ���ϴ��¼���ȸ���������ƶ�
        GUI.changed = true;//GUI.changed ����κοؼ��������������ݵ�ֵ���򷵻� true
    }

    //�϶��ڵ�
    public void DragNode(Vector2 delta)
    {
        rect.position += delta;//�ڵ����λ�øı�
        EditorUtility.SetDirty(this);//�� target ������Ϊ���ࡱ
    }

    //���ӷ���ڵ�id��ӵ�����ڵ㣬����ɹ���ӷ���true
    public bool AddChildRoomNodeIDToRoomNode(string childID)
    {
        //�ж��ӷ���ڵ��Ƿ���ԺϷ��ļ��뵽������
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

    //�ж��ӷ���ڵ��Ƿ���ԺϷ��ļ��뵽�����䣬����return true������false
    public bool IsChildRoomVaild(string childID)
    {
        //�Ƿ��Ѿ�����ͨ��Boss����
        bool isConnectedBossNodeAlready = false;
        foreach(RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
        {
            //��Boss���䣬����Boss�����и�����
            if(roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0)
            {
                //�Ѿ�����ͨ��Boss����
                isConnectedBossNodeAlready = true;
            }
        }

        //��֤�Ϸ���
        //Ҫ���ӵ��ӷ�����Boss���䣬�����Ѿ���Boss��ͨ��
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isBossRoom && isConnectedBossNodeAlready)
            return false;
        //Ҫ���ӵ��ӷ�����None
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isNone)
            return false;
        //�÷���ڵ���ӷ������Ѿ������Ҫ���ӵ��ӷ����ˣ��ظ���
        if (childRoomNodeIDList.Contains(childID))
            return false;
        //�ӷ���ڵ�͸÷���ڵ���ͬ
        if (id == childID)
            return false;
        //Ҫ���ӵĺ͸÷���ڵ�ĸ�������ͬ
        if (parentRoomNodeIDList.Contains(childID))
            return false;
        //Ҫ���ӵ��������ڵ��Ѿ��и�������
        if (roomNodeGraph.GetRoomNode(childID).parentRoomNodeIDList.Count > 0)
            return false;
        //����+����
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && roomNodeType.isCorridor)
            return false;
        //��������+��������
        if (!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && !roomNodeType.isCorridor)
            return false;
        //Ҫ���ӵ������ȣ����ǵ�ǰ����ڵ���������Ѿ������
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count >= Settings.maxChildCorridors)
            return false;
        //����������
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isEntrance)
            return false;
        //����Ĳ������ȣ�Ҫ�ѷ������ӵ����ȣ����������Ѿ��з�����
        if (!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count > 0)
            return false;


        return true;
    }

    //�Ѹ�����ڵ�id��ӵ�����ڵ㣬����ɹ���ӷ���true
    public bool AddParentRoomNodeIDToRoomNode(string parentID)
    {
        parentRoomNodeIDList.Add(parentID);
        return true;
    }

    //������ӷ���ڵ��ɾ��
    public bool RemoveChildRoomNodeIDFromRoomNode(String childID)
    {
        //����ӷ����б�����childID
        if (childRoomNodeIDList.Contains(childID))
        {
            childRoomNodeIDList.Remove(childID);
            return true;
        }
        return false;
    }

    //����и�����ڵ��ɾ��
    public bool RemoveParentRoomNodeIDFromRoomNode(string parentID)
    {
        //����������б�����parentID
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
