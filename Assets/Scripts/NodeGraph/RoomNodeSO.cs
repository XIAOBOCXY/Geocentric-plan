//����ڵ�ű���������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNodeSO : ScriptableObject //�̳�ScriptableObject��
{
    [HideInInspector] public string id; //����ڵ��id
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>(); //���׷���ڵ�id�б�
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();  //�ӷ���ڵ�id�б�
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;  //����ڵ�ͼ
    public RoomNodeTypeSO roomNodeType;  //����ڵ�����
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;  //����ڵ������б�

}
