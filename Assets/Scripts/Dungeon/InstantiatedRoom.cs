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
        //����collider bounds ��ײ����Χ��
        roomColliderBounds = boxCollider2D.bounds;
    }

    //����ҽ��뷿�� ���÷���ı��¼�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��ҽ����·���
        if (collision.tag == Settings.playerTag && room != GameManager.Instance.GetCurrentRoom())
        {
            //���÷��䱻����
            this.room.isPreviouslyVisited = true;
            //���÷���ı��¼�
            StaticEventHandler.CallRoomChangedEvent(room);
        }
    }


    //��ʼ�� ʵ�����ķ���
    public void Initialise(GameObject roomGameobject)
    {
        //���tilemap��grid ����
        PopulateTilemapMemberbariables(roomGameobject);
        //��סû���õ���
        BlockOffUnusedDoorWays();
        //������ӵ�������
        AddDoorsToRooms();
        //��ֹ��ײ��Ƭ��ͼ��Ⱦ��
        DisableCollisionTilemapRenderer();
    }

    //���tilemap��grid ����
    private void PopulateTilemapMemberbariables(GameObject roomGameobject)
    {
        //��ȡgrid���
        grid = roomGameobject.GetComponentInChildren<Grid>();

        //��ȡgrid�������tilemaps
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


    //��סû���õ���
    private void BlockOffUnusedDoorWays()
    {
        //ѭ�����е���
        foreach (Doorway doorway in room.doorWayList)
        {
            if (doorway.isConnected)
            {
                continue;
            }
            //ʹ��tilemap��tiles������ûʹ�õ���
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

    //��ͼ���Ϸ�ס��
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

    //�����ϱ�����
    private void BlockDoorwayHorizontally(Tilemap tilemap,Doorway doorway)
    {
        //�ڷ���ģ���л�ȡ�ķ�������ʼ���Ƶ����Ͻ�����
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;
        //ѭ������Ҫ��ӵ�tiles
        for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
        {
            for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
            {
                //��ȡ��Ƭ�ı任����
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));
                //����tile(��Ƭ�� Tilemap �ϵ�λ��,Ҫ���ڵ�Ԫ�񴦵� Tile)
                tilemap.SetTile(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));
                //������Ƭ(��Ƭ�� Tilemap �ϵ�λ��,�任����)
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + 1 + xPos, startPosition.y - yPos, 0), transformMatrix);
            }
        }
    }

    //������������
    private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
    {
        //�ڷ���ģ���л�ȡ�ķ�������ʼ���Ƶ����Ͻ�����
        Vector2Int startPosition = doorway.doorwayStartCopyPosition;
        //ѭ������Ҫ��ӵ�tiles
        for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
        {
            for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
            {
                //��ȡ��Ƭ�ı任����
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0));
                //����tile(��Ƭ�� Tilemap �ϵ�λ��,Ҫ���ڵ�Ԫ�񴦵� Tile)
                tilemap.SetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), tilemap.GetTile(new Vector3Int(startPosition.x + xPos, startPosition.y - yPos, 0)));
                //������Ƭ(��Ƭ�� Tilemap �ϵ�λ��,�任����)
                tilemap.SetTransformMatrix(new Vector3Int(startPosition.x + xPos, startPosition.y - 1 - yPos, 0), transformMatrix);
            }
        }
    }

    //����ŵ���������Ȳ���Ҫ
    private void AddDoorsToRooms()
    {
        //���Ȳ���Ҫ��
        if (room.roomNodeType.isCorridorEW || room.roomNodeType.isCorridorNS) return;

        //�����ŷ���ʵ������Ԥ����
        foreach (Doorway doorway in room.doorWayList)
        {

            // ��Ԥ���岻Ϊ�ղ�����������������
            if (doorway.doorPrefab != null && doorway.isConnected)
            {
                //����ÿ��tile�ľ���
                float tileDistance = Settings.tileSizePixels / Settings.pixelsPerUnit;

                GameObject door = null;

                if (doorway.orientation == Orientation.north)
                {
                    //���ƶ��󣨱����ƵĶ��󣬸��Ƴ�����������ĸ����壩
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y + tileDistance, 0f);
                }
                else if (doorway.orientation == Orientation.south)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y, 0f);
                }
                else if (doorway.orientation == Orientation.east)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x + tileDistance, doorway.position.y + tileDistance * 1.25f, 0f);
                }
                else if (doorway.orientation == Orientation.west)
                {
                    door = Instantiate(doorway.doorPrefab, gameObject.transform);
                    door.transform.localPosition = new Vector3(doorway.position.x, doorway.position.y + tileDistance * 1.25f, 0f);
                }

                //��������
                Door doorComponent = door.GetComponent<Door>();

                //�����boss����
                if (room.roomNodeType.isBossRoom)
                {
                    doorComponent.isBossRoomDoor = true;

                    //����
                    doorComponent.LockDoor();


                }
            }
        }
    }

    //��ֹ��ײ��Ƭ��ͼ��Ⱦ��
    private void DisableCollisionTilemapRenderer()
    {
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }

}
