using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PoolManager : SingletonMonobehaviour<PoolManager>
{

    [SerializeField] private Pool[] poolArray = null;
    private Transform objectPoolTransform;
    private Dictionary<int, Queue<Component>> poolDictionary = new Dictionary<int, Queue<Component>>();

    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
        public string componentType;
    }

    private void Start()
    {
        objectPoolTransform = this.gameObject.transform;

        //创建对象池数组
        for (int i = 0; i < poolArray.Length; i++)
        {
            //创建对象池
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize, poolArray[i].componentType);
        }
    }

    //创建对象池
    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        //实例id
        int poolKey = prefab.GetInstanceID();
        //实例名称
        string prefabName = prefab.name;
        //创建父游戏对象
        //通过设置父对象，将一个的对象池中的物体设置到同一个父对象下，同时将所有对象池中的父对象都设置为同一个父对象表示所有对象池的集合。达到场景上面更加直观的目的。
        GameObject parentGameObject = new GameObject(prefabName + "Anchor");
        parentGameObject.transform.SetParent(objectPoolTransform);
        //查找字典中是否已存在id
        if (!poolDictionary.ContainsKey(poolKey))
        {
            //加入字典
            poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i < poolSize; i++)
            {
                //实例化对象
                GameObject newObject = Instantiate(prefab, parentGameObject.transform) as GameObject;
                //不激活
                newObject.SetActive(false);
                //加入队列
                poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType)));

            }
        }

    }
    
    //重用对象
    public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        //实例id
        int poolKey = prefab.GetInstanceID();
        //字典中查找
        if (poolDictionary.ContainsKey(poolKey))
        {
            //从队列中取出
            Component componentToReuse = GetComponentFromPool(poolKey);
            //重置对象
            ResetObject(position, rotation, componentToReuse, prefab);

            return componentToReuse;
        }
        else
        {
            Debug.Log("没有" + prefab);
            return null;
        }
    }

    //通过poolkey从队列中取出
    private Component GetComponentFromPool(int poolKey)
    {
        //出队
        Component componentToReuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(componentToReuse);

        if (componentToReuse.gameObject.activeSelf == true)
        {
            componentToReuse.gameObject.SetActive(false);//不激活
        }
        return componentToReuse;
    }

    //重置对象
    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
    {
        componentToReuse.transform.position = position;
        componentToReuse.transform.rotation = rotation;
        componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtlities.ValidateCheckEnumerableValues(this, nameof(poolArray), poolArray);
    }
#endif
    #endregion

}
