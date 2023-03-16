//帮助校验
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtlities //不继承，并且改为静态类，静态类不会被实例化
{
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
}
