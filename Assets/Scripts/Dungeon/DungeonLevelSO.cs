using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DungeonLevel_",menuName ="Scriptable Objects/Dungeon/Dungeon Level")]
public class DungeonLevelSO : ScriptableObject
{
    #region Header BASIC LEVEL DETAILS
    [Space(10)]
    [Header("BASIC LEVEL DETAILS")]
    #endregion Header BASIC LEVEL DETAILS

    #region Tooltip
    [Tooltip("level����")]
    #endregion Tooltip
    public string levelName;

    #region Header ROOM TEMPLATES FOR LEVEL
    [Space(10)]
    [Header("ROOM TEMPLATES FOR LEVEL")]
    #endregion Header ROOM TEMPLATES FOR LEVEL
    #region Tooltip
    [Tooltip("�Ѳ�ͬlevel��room template��䵽list��")]
    #endregion Tooltip
    public List<RoomTemplateSO> roomTemplateList;

    #region Header ROOM NODE GRAPHS FOR LEVEL
    [Space(10)]
    [Header("ROOM NODE GRAPHS FOR LEVEL")]
    #endregion Header ROOM NODE GRAPHS FOR LEVEL
    #region Tooltip
    [Tooltip("��list�����level�������ѡ��ķ���ڵ�ͼ")]
    #endregion Tooltip
    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation
#if UNITY_EDITOR
    //��֤�ɱ�д�ű�������ϸ��Ϣ��Ϊ��
    private void OnValidate()
    {
        HelperUtlities.ValidateCheckEmptyString(this, nameof(levelName), levelName);
        if (HelperUtlities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList))
            return;
        if (HelperUtlities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList))
            return;
        //������ȷ��ָ���ķ���ڵ�ͼ�еĽڵ����͵ķ���ģ�����

        //����ȷ���ϱ����ȡ��������Ⱥ�������Ͷ�����
        bool isEWCorrider = false;//�Ƕ�������
        bool isNSCorrider = false;//���ϱ�����
        bool isEntrance = false;//�����

        //ѭ���������еķ���ģ�壬ȷ������ڵ����ʹ���
        foreach(RoomTemplateSO roomTemplateSO in roomTemplateList)
        {
            if (roomTemplateSO == null)
                return;
            if (roomTemplateSO.roomNodeType.isCorridorEW)
                isEWCorrider = true;
            if (roomTemplateSO.roomNodeType.isCorridorNS)
                isNSCorrider = true;
            if (roomTemplateSO.roomNodeType.isEntrance)
                isEntrance = true;
        }
        if (isEWCorrider == false)
        {
            Debug.Log("��" + this.name.ToString() + "�У�û�ж����������͵ķ���");
        }
        if (isNSCorrider == false)
        {
            Debug.Log("��" + this.name.ToString() + "�У�û���ϱ��������͵ķ���");
        }
        if (isEntrance == false)
        {
            Debug.Log("��" + this.name.ToString() + "�У�û��������͵ķ���");
        }

        //ѭ���������з���ڵ�ͼ
        foreach(RoomNodeGraphSO roomNodeGraph in roomNodeGraphList)
        {
            if (roomNodeGraph == null)
                return;
            //ѭ�������÷���ڵ�ͼ�е����з���ڵ�
            foreach(RoomNodeSO roomNodeSO in roomNodeGraph.roomNodeList)
            {
                if (roomNodeSO == null)
                    continue;

                //������ȷ��ÿһ�ַ������ͣ�����������һ������ģ��

                //���Ⱥ�����Ѿ�ȷ�������ˣ�ֱ��continue
                if(roomNodeSO.roomNodeType.isEntrance || roomNodeSO.roomNodeType.isCorridorEW || roomNodeSO.roomNodeType.isCorridorNS || roomNodeSO.roomNodeType.isCorridor || roomNodeSO.roomNodeType.isNone)
                {
                    continue;
                }

                bool isRoomNodeTypeFound = false;
                //ѭ���������еķ���ģ�壬ȷ��ÿ�ַ������Ͷ��з���ģ��
                foreach(RoomTemplateSO roomTemplateSO in roomTemplateList)
                {
                    if (roomTemplateSO == null)
                        continue;
                    if (roomTemplateSO.roomNodeType == roomNodeSO.roomNodeType)
                    {
                        isRoomNodeTypeFound = true;
                        break;
                    }
                }
                if (!isRoomNodeTypeFound)
                {
                    Debug.Log("��" + this.name.ToString() + "�У�����ڵ�ͼ" + roomNodeGraph.name.ToString() + "�еķ���ģ������" + roomNodeSO.roomNodeType.name.ToString() + "û���ҵ�");
                }
            }
        }
    }
#endif
    #endregion Validation
}
