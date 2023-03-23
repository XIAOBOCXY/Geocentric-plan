using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> m_EnemyPres;
    private EnemyPoint[] m_EnemyPoints;

    private bool m_Init;
    private List<Enemy> m_EnemyList;

    private bool m_IsOpenedBossDoor;//�Ƿ��Boss��
    private int m_EnemyCount;

    private void Awake()
    {
        m_EnemyList = new List<Enemy>();
    }

    private void Update()
    {
        InitEnemy();
        if (!m_IsOpenedBossDoor)
        {
            if (UpdateEnemies())
            {
                GetBossPoint().UnlockDoor();
                m_IsOpenedBossDoor = true;
            }
        }
        Global.Instance.UpdateMonster(m_EnemyCount);
    }

    /// <summary> ��ʼ������ </summary>
    private void InitEnemy()
    {
        if (m_Init) { return; }
        
        m_EnemyPoints = FindObjectsOfType<EnemyPoint>();
        if (m_EnemyPoints != null && m_EnemyPoints.Length > 0)
        {
            for (int i = 0; i < m_EnemyPoints.Length; i++)
            {
                GeneratorEnemies(m_EnemyPoints[i]);
            }
            m_Init = true;
        }
    }

    /// <summary> �������� </summary>
    public void GeneratorEnemies(EnemyPoint enemyPoint)
    {
        var points = enemyPoint.GetRandomPoints();
        for (int i = 0; i < points.Count; i++)
        {
            Enemy enemy = Instantiate(GetEnemy(enemyPoint.BossRoom), transform);
            m_EnemyList.Add(enemy);
            enemy.Init(points[i]);
        }
    }

    /// <summary> ��ȡ���� </summary>
    private Enemy GetEnemy(bool isBoss)
    {
        return m_EnemyPres[System.Convert.ToInt32(isBoss)];
    }

    private EnemyPoint GetBossPoint()
    {
        for (int i = 0; i < m_EnemyPoints.Length; i++)
        {
            if (m_EnemyPoints[i].BossRoom)
            {
                return m_EnemyPoints[i];
            }
        }
        return null;
    }

    /// <summary> ���¼���Ƿ���Boss�� </summary>
    private bool UpdateEnemies()
    {
        m_EnemyCount = 0;
        if (m_EnemyList.Count <= 0) { return false; }

        bool isTrue = true;
        for (int i = 0; i < m_EnemyList.Count; i++)
        {
            if (!m_EnemyList[i].IsBoss && !m_EnemyList[i].IsDeath)
            {
                m_EnemyCount++;
                isTrue = false;
            }
        }
        return isTrue;
    }

}
