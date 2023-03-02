//����ڵ�ͼ�ű���������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RoomNodeGraph",menuName ="Scriptable Object/Dungeon/Room Node Graph")] //���Assets�˵���ť
public class RoomNodeGraphSO : ScriptableObject   //�̳�ScriptableObject��
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;//����ڵ����͵��б�
    [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();//����ڵ��б�
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();//����ڵ��ֵ䣬guidΪ�ؼ��֣�����Ϊstring

    //�ű����ص�ʱ��ִ�еĴ���
    private void Awake()
    {
        //���ط���ڵ��ֵ�
        LoadRoomNodeDictionary();
    }

    //���ط���ڵ��ֵ亯��
    private void LoadRoomNodeDictionary()
    {
        //��շ���ڵ��ֵ�
        roomNodeDictionary.Clear();
        //������ڵ��б��ڵķ���ڵ���idһһ��Ӧ
        foreach(RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node;
        }
    }

    //ͨ��roomnode ID��ȡroom node
    public RoomNodeSO GetRoomNode(string roomNodeID)
    {
        //��roomNodeDictionary�в���roomnode ID��Ӧ��room node
        if (roomNodeDictionary.TryGetValue(roomNodeID,out RoomNodeSO roomNode))//��ȷ���Ƿ����ʱ��out
        {
            return roomNode;
        }
        return null;
    }

    #region Editor Code
#if UNITY_EDITOR
    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom = null;//�����ߵ���ʼ����ڵ�
    [HideInInspector] public Vector2 linePosition;//������λ��

    //OnValidate����������֤һЩ���ݣ��ű����ػ�Inspector�е��κ�ֵ���޸�ʱ�����
    public void OnValidate()
    {
        //���ط���ڵ��ֵ�
        LoadRoomNodeDictionary();
    }

    //���������ߵ���ʼ����ڵ�
    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node,Vector2 position)
    {
        roomNodeToDrawLineFrom = node;//�����ߵ���ʼ����ڵ�
        linePosition = position;//������λ��
    }

#endif
#endregion Editor Code
}
