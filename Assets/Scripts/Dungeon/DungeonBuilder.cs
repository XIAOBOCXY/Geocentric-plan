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
        //加载房间节点类型列表
        LoadRoomNodeTypeList();

    }

    //加载房间节点类型列表
    private void LoadRoomNodeTypeList()
    {
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    //生成随机地牢，成功返回true,否则返回false
    public bool GenerateDungeon(DungeonLevelSO currentDungeonLevel)
    {
        roomTemplateList = currentDungeonLevel.roomTemplateList;
        //加载scriptable object room templates到字典中
        LoadRoomTemplatesIntoDictionary();
        //初始化
        dungeonBuildSuccessful = false;
        int dungeonBuildAttempts = 0;
        //当还没建成并且尝试次数还没到最大
        while(!dungeonBuildSuccessful&& dungeonBuildAttempts < Settings.maxDungeonBuildAttempts)
        {
            dungeonBuildAttempts++;
            //从列表中随机选择一个房间节点图
            RoomNodeGraphSO roomNodeGraph = SelectRandomRoomNodeGraph(currentDungeonLevel.roomNodeGraphList);
            int dungeonRebuildAttemptsForNodeGraph = 0;
            dungeonBuildSuccessful = false;
            //循环直到地牢构建成功或者房间节点图尝试的次数超过最大值
            while(!dungeonBuildSuccessful && dungeonRebuildAttemptsForNodeGraph <= Settings.maxDungeonRebuildAttemptsForRoomGraph)
            {
                //清空gameobjects和地牢房间字典
                ClearDungeon();
                dungeonRebuildAttemptsForNodeGraph++;
                //尝试为选择的房间节点图建造一个随机的地牢
                dungeonBuildSuccessful = AttemptToBuildRandomDungeon(roomNodeGraph);
            }
            if (dungeonBuildSuccessful)
            {
                //实例化房间游戏对象
                InstantiateRoomGameobjects();
            }
        }
        return dungeonBuildSuccessful;
    }

    //加载scriptable object room templates到字典中
    private void LoadRoomTemplatesIntoDictionary()
    {
        //清空房间模板字典
        roomTemplateDictionary.Clear();
        //把房间模板列表放到字典中
        foreach(RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if (!roomTemplateDictionary.ContainsKey(roomTemplate.guid))
            {
                roomTemplateDictionary.Add(roomTemplate.guid, roomTemplate);
            }
            else
            {
                Debug.Log(roomTemplateList + "中已经有相同的roomTemplate");
            }
        }
    }

    //尝试为选择的房间节点图建造一个随机的地牢
    private bool AttemptToBuildRandomDungeon(RoomNodeGraphSO roomNodeGraph)
    {
        //创建一个房间节点队列
        Queue<RoomNodeSO> openRoomNodeQueue = new Queue<RoomNodeSO>();

        //从房间节点图中找到入口节点
        RoomNodeSO entranceNode = roomNodeGraph.GetRoomNode(roomNodeTypeList.list.Find(x => x.isEntrance));
        if (entranceNode != null)
        {
            //把入口房间添加到房间节点队列中
            openRoomNodeQueue.Enqueue(entranceNode);
        }
        else
        {
            //没有入口房间节点，无法构建
            Debug.Log("没有入口房间节点");
            return false;
        }

        //初始化没有房间重叠
        bool noRoomOverlaps = true;
        //处理房间节点队列
        noRoomOverlaps = ProcessRoomsInOpenRoomNodeQueue(roomNodeGraph, openRoomNodeQueue, noRoomOverlaps);
        //如果所有房间节点都放完了，并且没有房间重叠
        if(openRoomNodeQueue.Count==0 && noRoomOverlaps)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    //处理房间节点队列,如果没有房间重叠，则返回true,反之
    private bool ProcessRoomsInOpenRoomNodeQueue(RoomNodeGraphSO roomNodeGraph,Queue<RoomNodeSO> openRoomNodeQueue,bool noRoomOverlaps)
    {
        //当队列中还有房间节点并且没有房间重叠
        while(openRoomNodeQueue.Count>0 && noRoomOverlaps == true)
        {
            //从队列中获取下一个房间节点
            RoomNodeSO roomNode = openRoomNodeQueue.Dequeue();
            //添加这个房间的孩子节点到队列中
            foreach(RoomNodeSO childRoomNode in roomNodeGraph.GetChildRoomNodes(roomNode))
            {
                openRoomNodeQueue.Enqueue(childRoomNode);
            }
            //如果房间类型是入口，则添加到房间字典中
            if (roomNode.roomNodeType.isEntrance)
            {
                //随机获得某类型的房间template
                RoomTemplateSO roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
                //用选择的room template创建一个房间
                Room room = CreateRoomFromTemplate(roomTemplate, roomNode);
                room.isPositioned = true;
                //添加room到room dictionary中
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }
            //如果房间类型不是入口
            else
            {
                //获取这个房间的父房间
                Room parentRoom = dungeonBuilderRoomDictionary[roomNode.parentRoomNodeIDList[0]];
                //判断要放的房间和父房间有没有重叠
                noRoomOverlaps = CanPlaceRoomWithNoOverlaps(roomNode, parentRoom);
            }
        }
        return noRoomOverlaps;
    }

    //尝试在地牢中放置房间，如果可以被放置则返回room,反之返回null
    private bool CanPlaceRoomWithNoOverlaps(RoomNodeSO roomNode,Room parentRoom)
    {
        bool roomOverlaps = true;
        while (roomOverlaps)
        {
            //选择父房间中未连接的可用的房间门
            List<Doorway> unconnectedAvailableParentDoorways = GetUnconnectedAvauladbleDoorways(parentRoom.doorWayList).ToList();
            //没有可用的房间门了
            if (unconnectedAvailableParentDoorways.Count == 0)
            {
                return false;
            }
            //随机选择一个父房间门
            Doorway doorwayParent = unconnectedAvailableParentDoorways[UnityEngine.Random.Range(0, unconnectedAvailableParentDoorways.Count)];
            //获取与父房间门方向一致的房间模板
            RoomTemplateSO roomTemplate = GetRandomTemplateForRoomConsistentWithParent(roomNode, doorwayParent);
            //创建房间
            Room room = CreateRoomFromTemplate(roomTemplate, roomNode);
            //根据父房间的位置，父房间门位置，要放置的房间 来放置房间，如果没有重叠则返回true,反之
            if (PlaceTheRoom(parentRoom, doorwayParent, room))
            {
                //如果房间没有重叠，则将roomOverlaps设置为false来退出循环
                roomOverlaps = false;
                //标记房间已经被方式了
                room.isPositioned = true;
                //把房间加到字典中
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }
            else
            {
                roomOverlaps = true;
            }
        }
        return true;
    }

    //根据父房间的位置，父房间门位置，要放置的房间 来放置房间，如果没有重叠则返回true,反之
    private bool PlaceTheRoom(Room parentRoom,Doorway doorwayParent, Room room)
    {
        //获得当前房间门
        Doorway doorway = GetOppositeDoorway(doorwayParent, room.doorWayList);
        if (doorway == null)
        {
            //设置该父房间没有门可用，这样不用再去尝试连接它
            doorwayParent.isUnavailable = true;
        }
        //计算父房间门的位置
        Vector2Int parentDoorwayPosition = parentRoom.lowerBounds + doorwayParent.position - parentRoom.templateLowerBounds;
        //偏移量
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
        //根据父房间的门来计算要放置的房间的上下限
        room.lowerBounds = parentDoorwayPosition + adjustment + room.templateLowerBounds - doorway.position;
        room.upperBounds = room.lowerBounds + room.templateUpperBounds - room.templateLowerBounds;

        //根据房间上下限判断是否重叠,如果重叠则返回重叠房间，没重叠就返回null
        Room overlappingRoom = CheckForRoomOverlap(room);

        if (overlappingRoom == null)
        {
            //标记该门被连接并且不可用
            doorwayParent.isConnected = true;
            doorwayParent.isUnavailable = true;
            doorway.isConnected = true;
            doorway.isUnavailable = true;
            //返回true表示房间已经被连接并且没有重叠
            return true;
        }
        else
        {
            //标记父房间门不可用，这样不会再次去连接它
            doorwayParent.isUnavailable = true;
            return false;
        }
    }
    //获得当前房间门的位置
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

    //根据房间上下限判断是否重叠,如果重叠则返回重叠房间，没重叠就返回null
    private Room CheckForRoomOverlap(Room roomToTest)
    {
        foreach(KeyValuePair<string,Room> keyValuePair in dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;
            //如果房间列表中的房间是我现在要判断的房间 或者 房间不能被放置
            if(room.id==roomToTest.id || !room.isPositioned)
            {
                continue;
            }
            //判断两个房间是否重叠,重叠返回true,否则返回false
            if (IsOverLappingRoom(roomToTest, room))
            {
                return room;
            }
        }
        return null;
    }

    //判断两个房间是否重叠,重叠返回true,否则返回false
    private bool IsOverLappingRoom(Room room1,Room room2)
    {
        bool isOverlappingX = IsOverLappingInterval(room1.lowerBounds.x, room1.upperBounds.x, room2.lowerBounds.x, room2.upperBounds.x);
        bool isOverlappingY = IsOverLappingInterval(room1.lowerBounds.y, room1.upperBounds.y, room2.lowerBounds.y, room2.upperBounds.y);
        if(isOverlappingX && isOverlappingY)
        {
            //重叠
            return true;
        }
        else
        {
            return false;
        }
    }

    //判断坐标间隔大小
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


    //获取与父房间门方向一致的房间模板
    private RoomTemplateSO GetRandomTemplateForRoomConsistentWithParent(RoomNodeSO roomNode,Doorway doorwayParent)
    {
        RoomTemplateSO roomTemplate = null;
        //添加的房间时走廊
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
        //添加的房间不是走廊
        else
        {
            roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
        }
        return roomTemplate;
    }

    //获取房间中没连接的可用的门
    private IEnumerable<Doorway> GetUnconnectedAvauladbleDoorways(List<Doorway> roomDoorwayList)
    {
        //循环门列表
        foreach(Doorway doorway in roomDoorwayList)
        {
            if(!doorway.isConnected && !doorway.isUnavailable)
            {
                yield return doorway;
            }
        }
    }

    //获得一个随机的某类型room template
    private RoomTemplateSO GetRandomRoomTemplate(RoomNodeTypeSO roomNodeType)
    {
        //所有是该类型的房间模板
        List<RoomTemplateSO> matchingRoomTemplateList = new List<RoomTemplateSO>();
        //循环room template列表
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
        //随机返回一个该类型的房间模板
        return matchingRoomTemplateList[UnityEngine.Random.Range(0, matchingRoomTemplateList.Count)];
    }

    //根据roomtemplate创建房间
    private Room CreateRoomFromTemplate(RoomTemplateSO roomTemplate,RoomNodeSO roomNode)
    {
        //初始化roomtemplate中的数据
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

        room.childRoomIDList = CopyStringList(roomNode.childRoomNodeIDList);//实现childRoomIDList深拷贝
        room.doorWayList = CopyDoorwayList(roomTemplate.doorwayList);//实现doorwayList深拷贝

        //为房间设置parent id
        if (roomNode.parentRoomNodeIDList.Count == 0)//入口
        {
            room.parentRoomID = "";
            room.isPreviouslyVisited = true;
            //设置入口
            GameManager.Instance.SetCurrentRoom(room);
        }
        else
        {
            room.parentRoomID = roomNode.parentRoomNodeIDList[0];
        }
        return room;
    }


    //从列表中随机选择一个房间节点图
    private RoomNodeGraphSO SelectRandomRoomNodeGraph(List<RoomNodeGraphSO> roomNodeGraphList)
    {
        if (roomNodeGraphList.Count > 0)
        {
            return roomNodeGraphList[UnityEngine.Random.Range(0, roomNodeGraphList.Count)];
        }
        else
        {
            Debug.Log("列表中没有房间节点图");
            return null;
        }
    }

    //从预制体中实例化房间游戏对象
    private void InstantiateRoomGameobjects()
    {
        //循环遍历room
        foreach (KeyValuePair<string,Room> keyValuePair in dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;
            //计算房间位置
            Vector3 roomPosition = new Vector3(room.lowerBounds.x - room.templateLowerBounds.x, room.lowerBounds.y - room.templateLowerBounds.y, 0f);
            //实例化房间
            GameObject roomGameobject = Instantiate(room.prefab, roomPosition, Quaternion.identity, transform);
            //从实例化预制物获得实例化房间组件
            InstantiatedRoom instantiatedRoom = roomGameobject.GetComponentInChildren<InstantiatedRoom>();
            instantiatedRoom.room = room;
            //初始化实例化的房间
            instantiatedRoom.Initialise(roomGameobject);
            room.instantiatedRoom = instantiatedRoom;
        }
    }

    //通过房间模板id获得房间模板，如果不存在返回null
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

    //通过房间id获得房间，如果不存在则返回null
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

    //清空gameobjects和地牢房间字典
    private void ClearDungeon()
    {
        if (dungeonBuilderRoomDictionary.Count > 0)
        {
            foreach(KeyValuePair<string,Room> keyValuePair in dungeonBuilderRoomDictionary)
            {
                Room room = keyValuePair.Value;
                //如果已经实例化了房间游戏对象，则要把它删除
                if (room.instantiatedRoom != null)
                {
                    Destroy(room.instantiatedRoom.gameObject);
                }
            }
            dungeonBuilderRoomDictionary.Clear();
        }
    }


    //实现doorwayList深拷贝
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


    //实现childRoomIDList深拷贝
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
