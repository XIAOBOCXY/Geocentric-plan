using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Room_", menuName = "Scriptable Objects/Dungeon/Room")]
public class RoomTemplateSO : ScriptableObject
{
    [HideInInspector] public string guid;
    #region Header ROOM PREFAB
    [Space(10)]
    [Header("ROOM PREFAB")]
    #endregion Header ROOM PREFAB
    #region Tooltip
    [Tooltip("�������Ϸ����Ԥ���壨���������װ����")]
    #endregion Tooltip
    public GameObject prefab;
    [HideInInspector] public GameObject previousPrefab;//������󱻸��ƻ���Ԥ����ı䣬����������guid
    #region Header ROOM CONFIGURATION
    [Space(10)]
    [Header("ROOM CONFIGURATION")]
    #endregion Header ROOM CONFIGURATION
    #region Tooltip
    [Tooltip("����ڵ�����SO")]
    #endregion Tooltip
    public RoomNodeTypeSO roomNodeType;
    #region Tooltip
    [Tooltip("��������")]
    #endregion Tooltip
    public Vector2Int lowerBounds;
    #region Tooltip
    [Tooltip("��������")]
    #endregion Tooltip
    public Vector2Int upperBounds;
    #region Tooltip
    [Tooltip("ÿ�������ĸ��Ŷ�Ҫ��3���ש��С")]
    #endregion Tooltip
    [SerializeField] public List<Doorway> doorwayList;
    #region Tooltip
    [Tooltip("�������ɵ��˺ͱ����λ�üӵ�������")]
    #endregion Tooltip   
    public Vector2Int[] spawnPositionArray;


    // ����roomtemplate������б�ķ���
    public List<Doorway> GetDoorwayList()
    {
        return doorwayList;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        //���guid�ǿջ���Ԥ����ı䣬������Ψһ��guid
        if(guid=="" || previousPrefab != prefab)
        {
            guid = GUID.Generate().ToString();//using UnityEditor;
            previousPrefab = prefab;
            EditorUtility.SetDirty(this);//using UnityEditor;
        }
        //���doorwayList�Ƿ�Ϊ��
        HelperUtlities.ValidateCheckEnumerableValues(this, nameof(doorwayList), doorwayList);
        //���spawnPosition�Ƿ����
        HelperUtlities.ValidateCheckEnumerableValues(this, nameof(spawnPositionArray), spawnPositionArray);
    }
#endif
    #endregion Validation
}
