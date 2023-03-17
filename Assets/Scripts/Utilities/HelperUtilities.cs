//����У��
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtlities //���̳У����Ҹ�Ϊ��̬�࣬��̬�಻�ᱻʵ����
{
    public static Camera mainCamera;

   //��ȡ�����������
    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        Vector3 mouseScreenPosition = Input.mousePosition;
        //�����������Ļ��Χ���ƶ�
        mouseScreenPosition.x = Mathf.Clamp(mouseScreenPosition.x, 0f, Screen.width);
        mouseScreenPosition.y = Mathf.Clamp(mouseScreenPosition.y, 0f, Screen.height);
        //�����λ��ת��Ϊ��������
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        worldPosition.z = 0f;
        return worldPosition;
    }

    //��ȡ���������ĽǶ�
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;
        return degrees;
    }


    //ͨ����Һ����֮�䷽�������ĽǶ�������������
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


    //��֤���ַ���,�Ƿ���true
    public static bool ValidateCheckEmptyString(Object thisObject ,string fieldName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log("��" + thisObject.name.ToString()+"�У�"+ fieldName + "�ǿյ�,����Ҫ��һ��ֵ");
            return true;
        }
        return false;
    }

    //��֤�Ƿ�Ϊ��ֵ����ֵ����true
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName,UnityEngine.Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log(thisObject.name.ToString() + "�У�" + fieldName + "�ǿ�ֵ");
            return true;
        }
        else
        {
            return false;
        }
    }


    //��֤�б�Ϊ�ջ��б���nullֵ���Ƿ���true
    public static bool ValidateCheckEnumerableValues(Object thisObject,string fieldName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log("����" + thisObject.name.ToString() + "��" + fieldName + "��null");
            return true;
        }

        foreach(var item in enumerableObjectToCheck)//�����б�ÿһ��ֵ
        {
            if (item == null)//��nullֵ
            {
                Debug.Log(fieldName + "�п�ֵ���ڶ���" + thisObject.name.ToString()+"��");
                error = true;//����true
            }
            else count++;
        }
        if (count == 0)//�б�Ϊ��
        {
            Debug.Log(fieldName + "û��ֵ���ڶ���" + thisObject.name.ToString()+"��");
            error = true;
        }
        return error;
    }

    //�������,�����Ϸ���true
    public static bool ValidateCheckPositiveValue(Object thisObject,string fieldName,int valueToCheck,bool isZeroAllowed)
    {
        bool error = false;
        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(thisObject.name.ToString() + "�У�" + fieldName + "��������������0");
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(thisObject.name.ToString() + "�У�" + fieldName + "����������");
                error = true;
            }
        }
        return error;
    }

    //��ȡ������������spawn position
    public static Vector3 GetSpawnPositionNearestToPlayer(Vector3 playerPosition)
    {
        //��ǰ����
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        //��ǰ�������������
        Grid grid = currentRoom.instantiatedRoom.grid;
        //��ʼ���������
        Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0f);

        //ѭ��������ǰ���������spwan position����
        foreach (Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
        {
            // spwan position���������꣬������Ҫ����ת��Ϊ��������
            Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

            //�Ƚ��ҳ������spawn position
            if (Vector3.Distance(spawnPositionWorld, playerPosition) < Vector3.Distance(nearestSpawnPosition, playerPosition))
            {
                //���������spawn position
                nearestSpawnPosition = spawnPositionWorld;
            }
        }
        return nearestSpawnPosition;
    }

}
