//����ڵ������б�ű���������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeTypeListSO", menuName = "Scriptable Object/Dungeon/Room Node Type List")] //���Assets�˵���ť
public class RoomNodeTypeListSO : ScriptableObject  //�̳�ScriptableObject��
{
    #region Header ROOM NODE TYPE LIST
    [Space(10)]
    [Header("����ڵ������б�")]
    #endregion
    #region Tooltip
    [Tooltip("���б�Ӧ������е���Ϸ����ڵ����Ϳɱ�д�ű��Ķ���")]
    #endregion
    public List<RoomNodeTypeSO> list;

    #region Validation 
#if UNITY_EDITOR //ƽ̨�жϣ�ֻ����unity�༭����ִ�У��Ż�ִ�����´���
    private void OnValidate() //������֤һЩ���ݣ��ű����ػ�Inspector�е��κ�ֵ���޸�ʱ�����
    {
        HelperUtlities.ValidateCheckEnumerableValues(this, nameof(list), list); //�жϷ���ڵ������б��Ƿ�Ϊ���ַ���
    }
#endif
    #endregion
}
