
using UnityEngine;
[System.Serializable]
public class Doorway
{
    public Vector2 position;
    public Orientation orientation;
    public GameObject doorPrefab;

    #region Header
    [Header("��ʼ���Ƶ����Ͻ�λ��")]
    #endregion
    public Vector2Int doorwayStartCopyPosition;
    #region
    [Header("�ſڵĿ��")]
    #endregion
    public int doorwayCopyTileWidth;
    #region Header
    [Header("�ſڵĸ߶�")]
    #endregion
    public int doorwayCopyTileHeight;
    [HideInInspector]
    public bool isConnected = false;
    [HideInInspector]
    public bool isUnavailable = false;
}
