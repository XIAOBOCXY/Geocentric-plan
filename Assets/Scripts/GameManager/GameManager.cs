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
    [Tooltip("���dungeon level scriptable object")]
    #endregion Tooltip
    [SerializeField] private List<DungeonLevelSO> dungeonLevelList;

    #region Tooltip
    [Tooltip("���level���ڲ���,�ʼ��0")]
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
        //���������ϸ��Ϣ
        playerDetails = GameResources.Instance.currentPlayer.playerDetails;
        //ʵ����player
        InstantiatePlayer();
    }

    //ʵ����player
    private void InstantiatePlayer()
    {
        //ʵ����player
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);
        //��ʼ��player
        player = playerGameObject.GetComponent<Player>();
        player.Initialize(playerDetails);
    }

    private void Start()
    {
        gameState = GameState.gameStarted;
    }
    private void Update()
    {
        //������Ϸ����
        HandleGameState();

        //���ڲ���
        if (Input.GetKeyDown(KeyCode.R))//������r
        {
            //��Ϸ״̬Ϊ��Ϸ��ʼ
            gameState = GameState.gameStarted;
        }
    }
    //������Ϸ����
    private void HandleGameState()
    {
        switch (gameState)
        {
            //��ʼ��Ϸ
            case GameState.gameStarted:
                //��ʼ���ε�ǰlevel
                playDungeonLevel(currentDungeonLevelListIndex);
                //������Ϸ״̬ΪplayingLevel
                gameState = GameState.playingLevel;
                break;
        }
    }
    //������ҽ��뵱ǰ�ķ���
    public void SetCurrentRoom(Room room)
    {
        previousRoom = currentRoom;
        currentRoom = room;
    }

    //��ʼ���ε�ǰlevel
    private void playDungeonLevel(int dungeonLevelListIndex)
    {
        //Ϊ��ǰlevel����dungeon
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);
        if (!dungeonBuiltSuccessfully)
        {
            Debug.Log("����ʧ��");
        }

        //���÷���ı��¼���ʵ�ֵ���
        StaticEventHandler.CallRoomChangedEvent(currentRoom);

        // ��������ڵ�ǰ��������м�
        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f, (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        // ��ȡ������������ spawn point 
        player.gameObject.transform.position = HelperUtlities.GetSpawnPositionNearestToPlayer(player.gameObject.transform.position);
    }

    //��ȡplayer
    public Player GetPlayer()
    {
        return player;
    }

    //��ȡplayer�����ڵķ���
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        //��֤dungeonLevelList�Ƿ����
        HelperUtlities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelList), dungeonLevelList);
    }
#endif
    #endregion Validation
}
