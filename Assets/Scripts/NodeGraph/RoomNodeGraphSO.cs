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

}
