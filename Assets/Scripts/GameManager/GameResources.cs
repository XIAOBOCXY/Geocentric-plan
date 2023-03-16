//��Ϸ��Դ�ű�
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;//ʵ��
    public static GameResources Instance
    {
        get
        {
            if (instance == null)//���ʵ���Ƿ�Ϊ��
            {
                instance = Resources.Load<GameResources>("GameResources");//��Դ���ط���������Ϸ��Դ���͵Ķ�����ص�ʵ����
            }
            return instance;//����ʵ��
        }
    }

    #region Header DUNGEON
    [Space(10)]
    [Header("DUNGEON")]
    #endregion
    #region Tooltip
    [Tooltip("�÷���ڵ������б�������")]
    #endregion

    public RoomNodeTypeListSO roomNodeTypeList;


    #region Header Player
    [Space(10)]
    [Header("PLAYER")]
    #endregion
    #region Tooltip
    [Tooltip("��ǰѡ������")]
    #endregion

    public CurrentPlayerSO currentPlayer;

    #region Header MATERIALS
    [Space(10)]
    [Header("MATERIALS")]
    #endregion
    #region Tooltip
    [Tooltip("�䰵�Ĳ���")]
    #endregion
    public Material dimmerMaterial;
}
