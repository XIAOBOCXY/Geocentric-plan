using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class DungeonBuilder :SingletonMonobehaviour<DungeonBuilder>
{
    public Dictionary<string, Room> dungeonBuilderRoomDictionary = new Dictionary<string, Room>();
    private Dictionary<string, RoomTemplateSO> roomTemplateDictionary = new Dictionary<string, RoomTemplateSO>();
    private List<RoomTemplateSO> roomTemplateList = null;
    private RoomNodeTypeListSO roomNodeTypeList;
    private bool dungeonBuildSuccessful;

    private void OnEnable()
    {
        GameResources.Instance.dimmerMaterial.SetFloat("Alpha_Slider", 0f);
    }

    private void OnDisable()
    {
        GameResources.Instance.dimmerMaterial.SetFloat("Alpha_Slider", 1f);
    }


    protected override void Awake()
    {
        base.Awake();
        //���ط���ڵ������б�
        LoadRoomNodeTypeList();

    }

    //���ط���ڵ������б�
    private void LoadRoomNodeTypeList()
    {
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    //����������Σ��ɹ�����true,���򷵻�false
    public bool GenerateDungeon(DungeonLevelSO currentDungeonLevel)
    {
        roomTemplateList = currentDungeonLevel.roomTemplateList;
        //����scriptable object room templates���ֵ���
        LoadRoomTemplatesIntoDictionary();
        //��ʼ��
        dungeonBuildSuccessful = false;
        int dungeonBuildAttempts = 0;
        //����û���ɲ��ҳ��Դ�����û�����
        while(!dungeonBuildSuccessful&& dungeonBuildAttempts < Settings.maxDungeonBuildAttempts)
        {
            dungeonBuildAttempts++;
            //���б������ѡ��һ������ڵ�ͼ
            RoomNodeGraphSO roomNodeGraph = SelectRandomRoomNodeGraph(currentDungeonLevel.roomNodeGraphList);
            int dungeonRebuildAttemptsForNodeGraph = 0;
            dungeonBuildSuccessful = false;
            //ѭ��ֱ�����ι����ɹ����߷���ڵ�ͼ���ԵĴ����������ֵ
            while(!dungeonBuildSuccessful && dungeonRebuildAttemptsForNodeGraph <= Settings.maxDungeonRebuildAttemptsForRoomGraph)
            {
                //���gameobjects�͵��η����ֵ�
                ClearDungeon();
                dungeonRebuildAttemptsForNodeGraph++;
                //����Ϊѡ��ķ���ڵ�ͼ����һ������ĵ���
                dungeonBuildSuccessful = AttemptToBuildRandomDungeon(roomNodeGraph);
            }
            if (dungeonBuildSuccessful)
            {
                //ʵ����������Ϸ����
                InstantiateRoomGameobjects();
            }
        }
        return dungeonBuildSuccessful;
    }

    //����scriptable object room templates���ֵ���
    private void LoadRoomTemplatesIntoDictionary()
    {
        //��շ���ģ���ֵ�
        roomTemplateDictionary.Clear();
        //�ѷ���ģ���б�ŵ��ֵ���
        foreach(RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if (!roomTemplateDictionary.ContainsKey(roomTemplate.guid))
            {
                roomTemplateDictionary.Add(roomTemplate.guid, roomTemplate);
            }
            else
            {
                Debug.Log(roomTemplateList + "���Ѿ�����ͬ��roomTemplate");
            }
        }
    }

    //����Ϊѡ��ķ���ڵ�ͼ����һ������ĵ���
    private bool AttemptToBuildRandomDungeon(RoomNodeGraphSO roomNodeGraph)
    {
        //����һ������ڵ����
        Queue<RoomNodeSO> openRoomNodeQueue = new Queue<RoomNodeSO>();

        //�ӷ���ڵ�ͼ���ҵ���ڽڵ�
        RoomNodeSO entranceNode = roomNodeGraph.GetRoomNode(roomNodeTypeList.list.Find(x => x.isEntrance));
        if (entranceNode != null)
        {
            //����ڷ�����ӵ�����ڵ������
            openRoomNodeQueue.Enqueue(entranceNode);
        }
        else
        {
            //û����ڷ���ڵ㣬�޷�����
            Debug.Log("û����ڷ���ڵ�");
            return false;
        }

        //��ʼ��û�з����ص�
        bool noRoomOverlaps = true;
        //������ڵ����
        noRoomOverlaps = ProcessRoomsInOpenRoomNodeQueue(roomNodeGraph, openRoomNodeQueue, noRoomOverlaps);
        //������з���ڵ㶼�����ˣ�����û�з����ص�
        if(openRoomNodeQueue.Count==0 && noRoomOverlaps)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //������ڵ����,���û�з����ص����򷵻�true,��֮
    private bool ProcessRoomsInOpenRoomNodeQueue(RoomNodeGraphSO roomNodeGraph,Queue<RoomNodeSO> openRoomNodeQueue,bool noRoomOverlaps)
    {
        //�������л��з���ڵ㲢��û�з����ص�
        while(openRoomNodeQueue.Count>0 && noRoomOverlaps == true)
        {
            //�Ӷ����л�ȡ��һ������ڵ�
            RoomNodeSO roomNode = openRoomNodeQueue.Dequeue();
            //����������ĺ��ӽڵ㵽������
            foreach(RoomNodeSO childRoomNode in roomNodeGraph.GetChildRoomNodes(roomNode))
            {
                openRoomNodeQueue.Enqueue(childRoomNode);
            }
            //���������������ڣ�����ӵ������ֵ���
            if (roomNode.roomNodeType.isEntrance)
            {
                //������ĳ���͵ķ���template
                RoomTemplateSO roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
                //��ѡ���room template����һ������
                Room room = CreateRoomFromTemplate(roomTemplate, roomNode);
                room.isPositioned = true;
                //���room��room dictionary��
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }
            //����������Ͳ������
            else
            {
                //��ȡ�������ĸ�����
                Room parentRoom = dungeonBuilderRoomDictionary[roomNode.parentRoomNodeIDList[0]];
                //�ж�Ҫ�ŵķ���͸�������û���ص�
                noRoomOverlaps = CanPlaceRoomWithNoOverlaps(roomNode, parentRoom);
            }
        }
        return noRoomOverlaps;
    }

    //�����ڵ����з��÷��䣬������Ա������򷵻�room,��֮����null
    private bool CanPlaceRoomWithNoOverlaps(RoomNodeSO roomNode,Room parentRoom)
    {
        bool roomOverlaps = true;
        while (roomOverlaps)
        {
            //ѡ�񸸷�����δ���ӵĿ��õķ�����
            List<Doorway> unconnectedAvailableParentDoorways = GetUnconnectedAvauladbleDoorways(parentRoom.doorWayList).ToList();
            //û�п��õķ�������
            if (unconnectedAvailableParentDoorways.Count == 0)
            {
                return false;
            }
            //���ѡ��һ����������
            Doorway doorwayParent = unconnectedAvailableParentDoorways[UnityEngine.Random.Range(0, unconnectedAvailableParentDoorways.Count)];
            //��ȡ�븸�����ŷ���һ�µķ���ģ��
            RoomTemplateSO roomTemplate = GetRandomTemplateForRoomConsistentWithParent(roomNode, doorwayParent);
            //��������
            Room room = CreateRoomFromTemplate(roomTemplate, roomNode);
            //���ݸ������λ�ã���������λ�ã�Ҫ���õķ��� �����÷��䣬���û���ص��򷵻�true,��֮
            if (PlaceTheRoom(parentRoom, doorwayParent, room))
            {
                //�������û���ص�����roomOverlaps����Ϊfalse���˳�ѭ��
                roomOverlaps = false;
                //��Ƿ����Ѿ�����ʽ��
                room.isPositioned = true;
                //�ѷ���ӵ��ֵ���
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }
            else
            {
                roomOverlaps = true;
            }
        }
        return true;
    }

    //���ݸ������λ�ã���������λ�ã�Ҫ���õķ��� �����÷��䣬���û���ص��򷵻�true,��֮
    private bool PlaceTheRoom(Room parentRoom,Doorway doorwayParent, Room room)
    {
        //��õ�ǰ������
        Doorway doorway = GetOppositeDoorway(doorwayParent, room.doorWayList);
        if (doorway == null)
        {
            //���øø�����û���ſ��ã�����������ȥ����������
            doorwayParent.isUnavailable = true;
        }
        //���㸸�����ŵ�λ��
        Vector2Int parentDoorwayPosition = parentRoom.lowerBounds + doorwayParent.position - parentRoom.templateLowerBounds;
        //ƫ����
        Vector2Int adjustment = Vector2Int.zero;
        switch (doorway.orientation)
        {
            case Orientation.north:
                adjustment = new Vector2Int(0, -1);
                break;
            case Orientation.east:
                adjustment = new Vector2Int(-1, 0);
                break; 
            case Orientation.south:
                adjustment = new Vector2Int(0, 1);
                break;
            case Orientation.west:
                adjustment = new Vector2Int(1, 0);
                break;
            case Orientation.none:
                break;
            default:
                break;
        }
        //���ݸ��������������Ҫ���õķ����������
        room.lowerBounds = parentDoorwayPosition + adjustment + room.templateLowerBounds - doorway.position;
        room.upperBounds = room.lowerBounds + room.templateUpperBounds - room.templateLowerBounds;

        //���ݷ����������ж��Ƿ��ص�,����ص��򷵻��ص����䣬û�ص��ͷ���null
        Room overlappingRoom = CheckForRoomOverlap(room);

        if (overlappingRoom == null)
        {
            //��Ǹ��ű����Ӳ��Ҳ�����
            doorwayParent.isConnected = true;
            doorwayParent.isUnavailable = true;
            doorway.isConnected = true;
            doorway.isUnavailable = true;
            //����true��ʾ�����Ѿ������Ӳ���û���ص�
            return true;
        }
        else
        {
            //��Ǹ������Ų����ã����������ٴ�ȥ������
            doorwayParent.isUnavailable = true;
            return false;
        }
    }
    //��õ�ǰ�����ŵ�λ��
    private Doorway GetOppositeDoorway(Doorway parentDoorway,List<Doorway> dooewayList)
    {
        foreach(Doorway doorwayToCheck in dooewayList)
        {
            if(parentDoorway.orientation==Orientation.east && doorwayToCheck.orientation == Orientation.west)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.orientation == Orientation.west && doorwayToCheck.orientation == Orientation.east)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.orientation == Orientation.north && doorwayToCheck.orientation == Orientation.south)
            {
                return doorwayToCheck;
            }
            else if (parentDoorway.orientation == Orientation.south && doorwayToCheck.orientation == Orientation.north)
            {
                return doorwayToCheck;
            }
        }
        return null;
    }

    //���ݷ����������ж��Ƿ��ص�,����ص��򷵻��ص����䣬û�ص��ͷ���null
    private Room CheckForRoomOverlap(Room roomToTest)
    {
        foreach(KeyValuePair<string,Room> keyValuePair in dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;
            //��������б��еķ�����������Ҫ�жϵķ��� ���� ���䲻�ܱ�����
            if(room.id==roomToTest.id || !room.isPositioned)
            {
                continue;
            }
            //�ж����������Ƿ��ص�,�ص�����true,���򷵻�false
            if (IsOverLappingRoom(roomToTest, room))
            {
                return room;
            }
        }
        return null;
    }

    //�ж����������Ƿ��ص�,�ص�����true,���򷵻�false
    private bool IsOverLappingRoom(Room room1,Room room2)
    {
        bool isOverlappingX = IsOverLappingInterval(room1.lowerBounds.x, room1.upperBounds.x, room2.lowerBounds.x, room2.upperBounds.x);
        bool isOverlappingY = IsOverLappingInterval(room1.lowerBounds.y, room1.upperBounds.y, room2.lowerBounds.y, room2.upperBounds.y);
        if(isOverlappingX && isOverlappingY)
        {
            //�ص�
            return true;
        }
        else
        {
            return false;
        }
    }

    //�ж���������С
    private bool IsOverLappingInterval(int imin1,int imax1,int imin2,int imax2)
    {
        if (Mathf.Max(imin1, imin2) <= Mathf.Min(imax1, imax2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //��ȡ�븸�����ŷ���һ�µķ���ģ��
    private RoomTemplateSO GetRandomTemplateForRoomConsistentWithParent(RoomNodeSO roomNode,Doorway doorwayParent)
    {
        RoomTemplateSO roomTemplate = null;
        //��ӵķ���ʱ����
        if (roomNode.roomNodeType.isCorridor)
        {
            switch (doorwayParent.orientation)
            {
                case Orientation.north:
                case Orientation.south:
                    roomTemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorNS));
                    break;
                case Orientation.east:
                case Orientation.west:
                    roomTemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorEW));
                    break;
                case Orientation.none:
                    break;
                default:
                    break;
            }
        }
        //��ӵķ��䲻������
        else
        {
            roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
        }
        return roomTemplate;
    }

    //��ȡ������û���ӵĿ��õ���
    private IEnumerable<Doorway> GetUnconnectedAvauladbleDoorways(List<Doorway> roomDoorwayList)
    {
        //ѭ�����б�
        foreach(Doorway doorway in roomDoorwayList)
        {
            if(!doorway.isConnected && !doorway.isUnavailable)
            {
                yield return doorway;
            }
        }
    }

    //���һ�������ĳ����room template
    private RoomTemplateSO GetRandomRoomTemplate(RoomNodeTypeSO roomNodeType)
    {
        //�����Ǹ����͵ķ���ģ��
        List<RoomTemplateSO> matchingRoomTemplateList = new List<RoomTemplateSO>();
        //ѭ��room template�б�
        foreach(RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if (roomTemplate.roomNodeType == roomNodeType)
            {
                matchingRoomTemplateList.Add(roomTemplate);
            }
        }
        if (matchingRoomTemplateList.Count == 0)
        {
            return null;
        }
        //�������һ�������͵ķ���ģ��
        return matchingRoomTemplateList[UnityEngine.Random.Range(0, matchingRoomTemplateList.Count)];
    }

    //����roomtemplate��������
    private Room CreateRoomFromTemplate(RoomTemplateSO roomTemplate,RoomNodeSO roomNode)
    {
        //��ʼ��roomtemplate�е�����
        Room room = new Room();
        room.templateID = roomTemplate.guid;
        room.id = roomNode.id;
        room.prefab = roomTemplate.prefab;
        room.roomNodeType = roomTemplate.roomNodeType;
        room.lowerBounds = roomTemplate.lowerBounds;
        room.upperBounds = roomTemplate.upperBounds;
        room.spawnPositionArray = roomTemplate.spawnPositionArray;
        room.templateLowerBounds = roomTemplate.lowerBounds;
        room.templateUpperBounds = roomTemplate.upperBounds;

        room.childRoomIDList = CopyStringList(roomNode.childRoomNodeIDList);//ʵ��childRoomIDList���
        room.doorWayList = CopyDoorwayList(roomTemplate.doorwayList);//ʵ��doorwayList���

        //Ϊ��������parent id
        if (roomNode.parentRoomNodeIDList.Count == 0)//���
        {
            room.parentRoomID = "";
            room.isPreviouslyVisited = true;
            //�������
            GameManager.Instance.SetCurrentRoom(room);
        }
        else
        {
            room.parentRoomID = roomNode.parentRoomNodeIDList[0];
        }
        return room;
    }


    //���б������ѡ��һ������ڵ�ͼ
    private RoomNodeGraphSO SelectRandomRoomNodeGraph(List<RoomNodeGraphSO> roomNodeGraphList)
    {
        if (roomNodeGraphList.Count > 0)
        {
            return roomNodeGraphList[UnityEngine.Random.Range(0, roomNodeGraphList.Count)];
        }
        else
        {
            Debug.Log("�б���û�з���ڵ�ͼ");
            return null;
        }
    }

    //��Ԥ������ʵ����������Ϸ����
    private void InstantiateRoomGameobjects()
    {
        //ѭ������room
        foreach (KeyValuePair<string,Room> keyValuePair in dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;
            //���㷿��λ��
            Vector3 roomPosition = new Vector3(room.lowerBounds.x - room.templateLowerBounds.x, room.lowerBounds.y - room.templateLowerBounds.y, 0f);
            //ʵ��������
            GameObject roomGameobject = Instantiate(room.prefab, roomPosition, Quaternion.identity, transform);
            //��ʵ����Ԥ������ʵ�����������
            InstantiatedRoom instantiatedRoom = roomGameobject.GetComponentInChildren<InstantiatedRoom>();
            instantiatedRoom.room = room;
            //��ʼ��ʵ�����ķ���
            instantiatedRoom.Initialise(roomGameobject);
            room.instantiatedRoom = instantiatedRoom;
        }
    }

    //ͨ������ģ��id��÷���ģ�壬��������ڷ���null
    public RoomTemplateSO GetRoomTemplate(string roomTemplateID)
    {
        if(roomTemplateDictionary.TryGetValue(roomTemplateID,out RoomTemplateSO roomTemplate))
        {
            return roomTemplate;
        }
        else
        {
            return null;
        }
    }

    //ͨ������id��÷��䣬����������򷵻�null
    public Room GetRoomByRoomID(string roomID)
    {
        if(dungeonBuilderRoomDictionary.TryGetValue(roomID,out Room room))
        {
            return room;
        }
        else
        {
            return null;
        }
    }

    //���gameobjects�͵��η����ֵ�
    private void ClearDungeon()
    {
        if (dungeonBuilderRoomDictionary.Count > 0)
        {
            foreach(KeyValuePair<string,Room> keyValuePair in dungeonBuilderRoomDictionary)
            {
                Room room = keyValuePair.Value;
                //����Ѿ�ʵ�����˷�����Ϸ������Ҫ����ɾ��
                if (room.instantiatedRoom != null)
                {
                    Destroy(room.instantiatedRoom.gameObject);
                }
            }
            dungeonBuilderRoomDictionary.Clear();
        }
    }


    //ʵ��doorwayList���
    private List<Doorway> CopyDoorwayList(List<Doorway> oldDoorwayList)
    {
        List<Doorway> newDoorwayList = new List<Doorway>();
        foreach (Doorway doorway in oldDoorwayList)
        {
            Doorway newDoorway = new Doorway();
            newDoorway.position = doorway.position;
            newDoorway.orientation = doorway.orientation;
            newDoorway.doorPrefab = doorway.doorPrefab;
            newDoorway.isConnected = doorway.isConnected;
            newDoorway.isUnavailable = doorway.isUnavailable;
            newDoorway.doorwayStartCopyPosition = doorway.doorwayStartCopyPosition;
            newDoorway.doorwayCopyTileWidth = doorway.doorwayCopyTileWidth;
            newDoorway.doorwayCopyTileHeight = doorway.doorwayCopyTileHeight;

            newDoorwayList.Add(newDoorway);
        }
        return newDoorwayList;
    }


    //ʵ��childRoomIDList���
    private List<string> CopyStringList(List<string> oldStringList)
    {
        List<string> newStringList = new List<string>();
        foreach(string stringValue in oldStringList)
        {
            newStringList.Add(stringValue);
        }
        return newStringList;
    }
}
