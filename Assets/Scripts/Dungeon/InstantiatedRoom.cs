using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
public class InstantiatedRoom : MonoBehaviour
{
    [HideInInspector] public Room room;
    [HideInInspector] public Grid grid;
    [HideInInspector] public Tilemap groundTilemap;
    [HideInInspector] public Tilemap decoration1Tilemap;
    [HideInInspector] public Tilemap decoration2Tilemap;
    [HideInInspector] public Tilemap frontTilemap;
    [HideInInspector] public Tilemap collisionTilemap;
    [HideInInspector] public Tilemap minimapTilemap;
    [HideInInspector] public Bounds roomColliderBounds;

    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        //保存collider bounds 碰撞器包围盒
        roomColliderBounds = boxCollider2D.bounds;
    }

    //初始化 实例化的房间
    public void Initialise(GameObject roomGameobject)
    {
        //填充tilemap和grid 变量
        PopulateTilemapMemberbariables(roomGameobject);
        //封住没有用的门
        BlockOffUnusedDoorWays();
        //禁止碰撞瓦片地图渲染器
        DisableCollisionTilemapRenderer();
    }

    //填充tilemap和grid 变量
    private void PopulateTilemapMemberbariables(GameObject roomGameobject)
    {
        //获取grid组件
        grid = roomGameobject.GetComponentInChildren<Grid>();

        //获取grid的子组件tilemaps
        Tilemap[] tilemaps = roomGameobject.GetComponentsInChildren<Tilemap>();
        foreach(Tilemap tilemap in tilemaps)
        {
            if (tilemap.gameObject.tag == "groundTilemap")
            {
                groundTilemap = tilemap;
            }
            else if(tilemap.gameObject.tag == "decoration1Tilemap")
            {
                decoration1Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "decoration2Tilemap")
            {
                decoration2Tilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "frontTilemap")
            {
                frontTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "collitionTilemap")
            {
                collisionTilemap = tilemap;
            }
            else if (tilemap.gameObject.tag == "minimapTilemap")
            {
                minimapTilemap = tilemap;
            }
        }
    }


    //封住没有用的门
    private void BlockOffUnusedDoorWays()
    {
        //循环所有的门
        foreach (Doorway doorway in room.doorWayList)
        {
            if (doorway.isConnected)
            {
                continue;
            }
            //使用tilemap的tiles来封锁没使用的门
            if (collisionTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(collisionTilemap, doorway);
            }
            if (minimapTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(minimapTilemap, doorway);
            }
            if (groundTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(groundTilemap, doorway);
            }
            if (decoration1Tilemap != null)
            {
                BlockADoorwayOnTilemapLayer(decoration1Tilemap, doorway);
            }
            if (decoration2Tilemap != null)
            {
                BlockADoorwayOnTilemapLayer(decoration2Tilemap, doorway);
            }
            if (frontTilemap != null)
            {
                BlockADoorwayOnTilemapLayer(frontTilemap, doorway);
            }
        }
    }

    //在图层上封住门
    private void BlockADoorwayOnTilemapLayer(Tilemap tilemap,Doorway doorway)
    {
        switch (doorway.orientation)
        {
            case Orientation.north:
            case Orientation.south:
                BlockDoorwayHorizontally(tilemap, doorway);
                break;
            case Orientation.east:
            case Orientation.west:
                BlockDoorwayVertically(tilemap, doorway);
                break;
            case Orientation.none:
                break;
        }
    }

    //封锁南北的门
    private void BlockDoorwayHorizontally(Tilemap tilemap,Doorway doorway)
    {
        //在房间模板中获取的房间门起始复制的左上角坐标
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;
        //循环所有要添加的tiles
        for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
        {
            for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
            {
                //获取瓦片的变换矩阵
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));
                //设置tile(瓦片在 Tilemap 上的位置,要置于单元格处的 Tile)
                tilemap.SetTile(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));
                //设置瓦片(瓦片在 Tilemap 上的位置,变换矩阵)
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), transformMatrix);
            }
        }
    }

    //封锁东西的门
    private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
    {
        //在房间模板中获取的房间门起始复制的左上角坐标
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;
        //循环所有要添加的tiles
        for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
        {
            for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
            {
                //获取瓦片的变换矩阵
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));
                //设置tile(瓦片在 Tilemap 上的位置,要置于单元格处的 Tile)
                tilemap.SetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));
                //设置瓦片(瓦片在 Tilemap 上的位置,变换矩阵)
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), transformMatrix);
            }
        }
    }


    //禁止碰撞瓦片地图渲染器
    private void DisableCollisionTilemapRenderer()
    {
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }

}
