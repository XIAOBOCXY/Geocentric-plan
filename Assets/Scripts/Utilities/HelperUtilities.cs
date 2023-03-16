//����У��
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtlities //���̳У����Ҹ�Ϊ��̬�࣬��̬�಻�ᱻʵ����
{
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
}
