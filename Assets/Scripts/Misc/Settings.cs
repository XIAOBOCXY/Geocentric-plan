using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings//���static,�����Ϊ��̬������ɾ��MonoBehaviour�ű�����
{
    #region DUNGEON BUILD SETTINGS
    public const int maxDungeonRebuildAttemptsForRoomGraph = 1000;
    public const int maxDungeonBuildAttempts = 10;
    #endregion 


    #region ROOM SETTINGS
    public const int maxChildCorridors = 3;//һ���������������ӵĺ�����������
    #endregion
}
