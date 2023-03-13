//����ڵ����ͽű���������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType", menuName = "Scriptable Objects/Dungeon/Room Node Type")] //���Assets�˵���ť
public class RoomNodeTypeSO : ScriptableObject  //�̳�ScriptableObject��
{
    public string roomNodeTypeName;

    #region Header
    [Header("ֻ�б���˷���ڵ����Ͳ����ڱ༭���п���")]
    #endregion Header
    public bool displayInNodeGraphEditor = true;
    #region Header
    [Header("�Ƿ�������")]
    #endregion Header
    public bool isCorridor;
    #region Header
    [Header("�Ƿ����ϱ�����")]
    #endregion Header
    public bool isCorridorNS;
    #region Header
    [Header("�Ƿ��Ƕ�������")]
    #endregion Header
    public bool isCorridorEW;
    #region Header
    [Header("�Ƿ������")]
    #endregion Header
    public bool isEntrance;
    #region Header
    [Header("�Ƿ���boss����")]
    #endregion Header
    public bool isBossRoom;
    #region Header
    [Header("û�б����䷿��ڵ�����")]
    #endregion Header
    public bool isNone;

    #region Validation 
#if UNITY_EDITOR //ƽ̨�жϣ�ֻ����unity�༭����ִ�У��Ż�ִ�����´���
    private void OnValidate() //������֤һЩ���ݣ��ű����ػ�Inspector�е��κ�ֵ���޸�ʱ�����
    {
        HelperUtlities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName); //�жϷ���ڵ����������Ƿ�Ϊ���ַ���
    }
#endif
    #endregion
}
