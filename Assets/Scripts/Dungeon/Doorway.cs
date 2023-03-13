
using UnityEngine;
[System.Serializable]
public class Doorway
{
    public Vector2 position;
    public Orientation orientation;
    public GameObject doorPrefab;

    #region Header
    [Header("开始复制的左上角位置")]
    #endregion
    public Vector2Int doorwayStartCopyPosition;
    #region
    [Header("门口的宽度")]
    #endregion
    public int doorwayCopyTileWidth;
    #region Header
    [Header("门口的高度")]
    #endregion
    public int doorwayCopyTileHeight;
    [HideInInspector]
    public bool isConnected = false;
    [HideInInspector]
    public bool isUnavailable = false;
}
