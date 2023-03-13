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
    [Tooltip("房间的游戏对象预制体（包括房间和装饰物")]
    #endregion Tooltip
    public GameObject prefab;
    [HideInInspector] public GameObject previousPrefab;//如果对象被复制或者预制体改变，会重新生成guid
    #region Header ROOM CONFIGURATION
    [Space(10)]
    [Header("ROOM CONFIGURATION")]
    #endregion Header ROOM CONFIGURATION
    #region Tooltip
    [Tooltip("房间节点类型SO")]
    #endregion Tooltip
    public RoomNodeTypeSO roomNodeType;
    #region Tooltip
    [Tooltip("房间下限")]
    #endregion Tooltip
    public Vector2Int lowerBounds;
    #region Tooltip
    [Tooltip("房间上限")]
    #endregion Tooltip
    public Vector2Int upperBounds;
    #region Tooltip
    [Tooltip("每个房间四个门都要有3块瓷砖大小")]
    #endregion Tooltip
    [SerializeField] public List<Doorway> doorwayList;
    #region Tooltip
    [Tooltip("可能生成敌人和宝箱的位置加到数组中")]
    #endregion Tooltip   
    public Vector2Int[] spawnPositionArray;


    // 返回roomtemplate的入口列表的方法
    public List<Doorway> GetDoorwayList()
    {
        return doorwayList;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        //如果guid是空或者预制体改变，则设置唯一的guid
        if(guid=="" || previousPrefab != prefab)
        {
            guid = GUID.Generate().ToString();//using UnityEditor;
            previousPrefab = prefab;
            EditorUtility.SetDirty(this);//using UnityEditor;
        }
        //检查doorwayList是否为空
        HelperUtlities.ValidateCheckEnumerableValues(this, nameof(doorwayList), doorwayList);
        //检查spawnPosition是否填充
        HelperUtlities.ValidateCheckEnumerableValues(this, nameof(spawnPositionArray), spawnPositionArray);
    }
#endif
    #endregion Validation
}
