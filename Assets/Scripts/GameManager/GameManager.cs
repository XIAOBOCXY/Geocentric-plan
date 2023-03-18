using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class GameManager : SingletonMonobehaviour<GameManager>
{
    #region Header DUNGEON LEVELS
    [Space(10)]
    [Header("DUNGEON LEVELS")]
    #endregion Header DUNGEON LEVELS

    #region Tooltip
    [Tooltip("填充dungeon level scriptable object")]
    #endregion Tooltip
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip
    [Tooltip("填充level用于测试,最开始是0")]
    #endregion Tooltip
    [SerializeField] private int currentDungeonLevelListIndex = 0;

    private Room currentRoom;
    private Room previousRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;


    [HideInInspector] public GameState gameState;


    protected override void Awake()
    {
        base.Awake();
        //设置玩家详细信息
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;
        //实例化player
        InstantiatePlayer();
    }

    //实例化player
    private void InstantiatePlayer()
    {
        //实例化player
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);
        //初始化player
        player = playerGameObject.GetComponent<Player>();
        player.Initialize(playerDetails);
    }

    private void Start()
    {
        gameState = GameState.gameStarted;
    }
    private void Update()
    {
        //处理游戏数据
        HandleGameState();

        //用于测试
        if (Input.GetKeyDown(KeyCode.R))//当按下r
        {
            //游戏状态为游戏开始
            gameState = GameState.gameStarted;
        }
    }
    //处理游戏数据
    private void HandleGameState()
    {
        switch (gameState)
        {
            //开始游戏
            case GameState.gameStarted:
                //开始地牢当前level
                playDungeonLevel(currentDungeonLevelListIndex);
                //更新游戏状态为playingLevel
                gameState = GameState.playingLevel;
                break;
        }
    }
    //设置玩家进入当前的房间
    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom;
        currentRoom = room;
    }

    //开始地牢当前level
    private void playDungeonLevel(int dungeonLevelListIndex)
    {
        //为当前level创建dungeon
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);
        if (!dungeonBuiltSuccessfully)
        {
            Debug.Log("创建失败");
        }

        //调用房间改变事件，实现淡入
        StaticEventHandler.CallRoomChangedEvent(currentRoom);

        // 设置玩家在当前房间的正中间
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        // 获取距离玩家最近的 spawn point 
        player.gameObject.transform.position = HelperUtlities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);
    }

    //获取player
    public Player GetPlayer()
    {
        return player;
    }

    //获取player现在在的房间
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        //验证dungeonLevelList是否被填充
        HelperUtlities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }
#endif
    #endregion Validation
}
