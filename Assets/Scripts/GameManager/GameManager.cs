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

    [HideInInspector] public GameState gameState;

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
    //开始地牢当前level
    private void playDungeonLevel(int dungeonLevelListIndex)
    {
        //为当前level创建dungeon
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);
        if (!dungeonBuiltSuccessfully)
        {
            Debug.Log("创建失败");
        }
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
