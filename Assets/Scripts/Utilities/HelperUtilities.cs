//����У��
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtlities //���̳У����Ҹ�Ϊ��̬�࣬��̬�಻�ᱻʵ����
{
    //��֤���ַ���,�Ƿ���true
    public static bool ValidateCheckEmptyString(Object thisObject ,string fileName,string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log("��" + thisObject.name.ToString()+"�У�"+fileName+ "�ǿյ�,����Ҫ��һ��ֵ");
            return true;
        }
        return false;
    }
    //��֤�б�Ϊ�ջ��б���nullֵ���Ƿ���true
    public static bool ValidateCheckEnumerableValues(Object thisObject,string fileName,IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log("����" + thisObject.name.ToString() + "��" + fileName + "��null");
            return true;
        }

        foreach(var item in enumerableObjectToCheck)//�����б�ÿһ��ֵ
        {
            if (item == null)//��nullֵ
            {
                Debug.Log(fileName + "�п�ֵ���ڶ���" + thisObject.name.ToString()+"��");
                error = true;//����true
            }
            else count++;
        }
        if (count == 0)//�б�Ϊ��
        {
            Debug.Log(fileName + "û��ֵ���ڶ���" + thisObject.name.ToString()+"��");
            error = true;
        }
        return error;
    }

}
