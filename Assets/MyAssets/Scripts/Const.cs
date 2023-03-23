using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Const
{
    public const string TITE_MAP = "collitionTilemap";
    public const string ENEMY = "Enemy";
    public const string PLAYER = "Player";

    private const string LEVEL_PREFS = "Level";

    public static void SetLevelPrefs(int index)
    {
        PlayerPrefs.SetInt("Level", index);
    }

    public static int GetLevelPrefs()
    {
        return PlayerPrefs.GetInt("Level", 1);
    }

    public static void ClearLevelPrefs()
    {
        PlayerPrefs.SetInt("Level", 1);
    }

}
