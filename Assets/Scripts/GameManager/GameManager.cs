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

    [HideInInspector] public GameState gameState;

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
    //��ʼ���ε�ǰlevel
    private void playDungeonLevel(int dungeonLevelListIndex)
    {
        //Ϊ��ǰlevel����dungeon
        bool dungeonBuiltSuccessfully = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelList[dungeonLevelListIndex]);
        if (!dungeonBuiltSuccessfully)
        {
            Debug.Log("����ʧ��");
        }
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
