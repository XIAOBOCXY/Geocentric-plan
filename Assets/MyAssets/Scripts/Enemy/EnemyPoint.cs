using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoint : MonoBehaviour
{
    [SerializeField] private bool m_IsBoss;
    [SerializeField] private Vector2Int m_EnemyCount = new Vector2Int(2, 3);
    [SerializeField] private Transform m_SpawnPoint;

    private List<Transform> m_PointList;
    private Door m_Door;

    public bool BossRoom
    {
        get { return m_IsBoss; }
    }

    private void Awake()
    {
        m_PointList = new List<Transform>();
        for (int i = 0; i < m_SpawnPoint.childCount; i++)
        {
            m_PointList.Add(m_SpawnPoint.GetChild(i));
        }
        m_SpawnPoint.gameObject.SetActive(false);
    }

    public void UnlockDoor()
    {
        var room = GetComponentInParent<InstantiatedRoom>();
        room.GetComponentInChildren<Door>().UnlockDoor();
    }

    /// <summary>
    /// 获取随机点
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetRandomPoints()
    {
        List<Vector3> points = new List<Vector3>();
        int num = Random.Range(m_EnemyCount.x, m_EnemyCount.y + 1);
        if (m_IsBoss) { num = 1; }

        num = Mathf.Clamp(num, 1, m_PointList.Count);
        for (int i = 0; i < num; i++)
        {
            Transform item = m_PointList[Random.Range(0, m_PointList.Count)];
            points.Add(item.position);
            m_PointList.Remove(item);
        }
        return points;
    }

}
