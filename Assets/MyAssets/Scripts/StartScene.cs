using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{

    private void Awake()
    {
        Const.ClearLevelPrefs();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }
    /// <summary>
    /// ÍË³öÓÎÏ·
    /// </summary>
    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
