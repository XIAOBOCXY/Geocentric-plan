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

    //��ʼ���ڵ�
    public void Initialise(Rect rect,RoomNodeGraphSO nodeGraph,RoomNodeTypeSO roomNodeType)
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
        int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
        int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());//EditorGUILayout.Popup�Բ�����ʽ��ȡ��ǰ��ѡ���������������û�ѡ�������
        roomNodeType = roomNodeTypeList.list[selection];//��ȡ��ѡ��ķ���ڵ�����
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

#endif
    #endregion
}
