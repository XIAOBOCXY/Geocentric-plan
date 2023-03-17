//帮助校验
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtlities //不继承，并且改为静态类，静态类不会被实例化
{
    public static Camera mainCamera;

   //获取鼠标世界坐标
    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        Vector3 mouseScreenPosition = Input.mousePosition;
        //限制鼠标在屏幕范围内移动
        mouseScreenPosition.x = Mathf.Clamp(mouseScreenPosition.x, 0f, Screen.width);
        mouseScreenPosition.y = Mathf.Clamp(mouseScreenPosition.y, 0f, Screen.height);
        //将鼠标位置转换为世界坐标
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        worldPosition.z = 0f;
        return worldPosition;
    }

    //获取方向向量的角度
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;
        return degrees;
    }


    //通过玩家和鼠标之间方向向量的角度来获得射击方向
    public static AimDirection GetAimDirection(float angleDegrees)
    {
        AimDirection aimDirection;
        //Up Right
        if (angleDegrees >= 22f && angleDegrees <= 67f)
            aimDirection = AimDirection.UpRight;
        // Up
        else if (angleDegrees > 67f && angleDegrees <= 112f)
            aimDirection = AimDirection.Up;
        // Up Left
        else if (angleDegrees > 112f && angleDegrees <= 158f)
            aimDirection = AimDirection.UpLeft;
        // Left
        else if ((angleDegrees <= 180f && angleDegrees > 158f) || (angleDegrees > -180 && angleDegrees <= -135f))
            aimDirection = AimDirection.Left;
        // Down
        else if ((angleDegrees > -135f && angleDegrees <= -45f))
            aimDirection = AimDirection.Down;
        // Right
        else if ((angleDegrees > -45f && angleDegrees <= 0f) || (angleDegrees > 0 && angleDegrees < 22f))
            aimDirection = AimDirection.Right;
        else
            aimDirection = AimDirection.Right;
        return aimDirection;
    }


    //验证空字符串,非法则true
    public static bool ValidateCheckEmptyString(Object thisObject ,string fieldName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log("在" + thisObject.name.ToString()+"中，"+ fieldName + "是空的,必须要放一个值");
            return true;
        }
        return false;
    }

    //验证是否为空值，空值返回true
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName,UnityEngine.Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log(thisObject.name.ToString() + "中，" + fieldName + "是空值");
            return true;
        }
        else
        {
            return false;
        }
    }


    //验证列表为空或列表有null值，非法则true
    public static bool ValidateCheckEnumerableValues(Object thisObject,string fieldName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log("对象" + thisObject.name.ToString() + "中" + fieldName + "是null");
            return true;
        }

        foreach(var item in enumerableObjectToCheck)//遍历列表每一个值
        {
            if (item == null)//有null值
            {
                Debug.Log(fieldName + "有空值，在对象" + thisObject.name.ToString()+"中");
                error = true;//返回true
            }
            else count++;
        }
        if (count == 0)//列表为空
        {
            Debug.Log(fieldName + "没有值，在对象" + thisObject.name.ToString()+"中");
            error = true;
        }
        return error;
    }

    //检查正数,不符合返回true
    public static bool ValidateCheckPositiveValue(Object thisObject,string fieldName,int valueToCheck,bool isZeroAllowed)
    {
        bool error = false;
        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(thisObject.name.ToString() + "中，" + fieldName + "必须是正数或者0");
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(thisObject.name.ToString() + "中，" + fieldName + "必须是正数");
                error = true;
            }
        }
        return error;
    }

    //获取距离玩家最近的spawn position
    public static Vector3 GetSpawnPositionNearestToPlayer(Vector3 playerPosition)
    {
        //当前房间
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        //当前房间的网格坐标
        Grid grid = currentRoom.instantiatedRoom.grid;
        //初始化最近距离
        Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0f);

        //循环遍历当前房间的所有spwan position数组
        foreach (Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
        {
            // spwan position是网格坐标，我们需要将它转换为世界坐标
            Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

            //比较找出最近的spawn position
            if (Vector3.Distance(spawnPositionWorld, playerPosition) < Vector3.Distance(nearestSpawnPosition, playerPosition))
            {
                //更新最近的spawn position
                nearestSpawnPosition = spawnPositionWorld;
            }
        }
        return nearestSpawnPosition;
    }

}
