using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{

    public static Global Instance
    {
        get; private set;
    }

    [SerializeField] private CanvasGroup m_Mask;//遮罩
    [SerializeField] private Text m_Level;
    [SerializeField] private Text m_Monster;

    public const int TOTAL_LEVEL_COUNT = 6;//总关卡数

    private int m_CurrentLevel;

    private void Awake()
    {
        Instance = this;
        m_CurrentLevel = Const.GetLevelPrefs();
        UpdateLevel();
        SetMask(false); 
    }

    private void Start()
    {
        GameManager.Instance.SetCurrentLevelIndex(m_CurrentLevel - 1);
    }

    /// <summary>
    /// 设置遮罩
    /// </summary>
    public void SetMask(bool fadeIn, float duration = 1f, TweenCallback onComplete = null)
    {
        if (fadeIn) { m_Mask.gameObject.SetActive(true); }
        m_Mask.DOKill();
        m_Mask.alpha = fadeIn ? 0f : 1f;
        m_Mask.DOFade(fadeIn ? 1f : 0f, duration).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() =>
        {
            onComplete?.Invoke();
            if (!fadeIn) { m_Mask.gameObject.SetActive(false); }
        });
    }

    /// <summary>
    /// 更新关卡数量的显示
    /// </summary>
    private void UpdateLevel()
    {
        m_Level.text = string.Format("Level: " + m_CurrentLevel);
    }

    /// <summary>
    /// 更新敌人数量的显示
    /// </summary>
    /// <param name="value"></param>
    public void UpdateMonster(int value)
    {
        m_Monster.text = string.Format("Monster: " + value);
    }

    /// <summary>
    /// 通关游戏
    /// </summary>
    public static void PassLevel()
    {
        int count = Instance.m_CurrentLevel += 1;
        Const.SetLevelPrefs(count);

        bool isAll = count > TOTAL_LEVEL_COUNT;
        Instance.SetMask(true, onComplete:() =>
        {
            if (isAll) { LoadScene("StartScene"); }
            else { LoadSelf(); }
        });
    }

    /// <summary>
    /// 继续游戏
    /// </summary>
    public void OnContinueGame()
    {

    }

    /// <summary>
    /// 重开游戏
    /// </summary>
    public void OnAgainGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 退出游戏
    /// </summary>
    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public static void LoadSelf()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// 加载场景
    /// </summary>
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnDestroy()
    {
        Instance = null;
        m_Mask.DOKill();
    }

}