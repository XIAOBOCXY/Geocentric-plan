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

        //�������������
        for (int i = 0; i < poolArray.Length; i++)
        {
            //���������
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize, poolArray[i].componentType);
        }
    }

    //���������
    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        //ʵ��id
        int poolKey = prefab.GetInstanceID();
        //ʵ������
        string prefabName = prefab.name;
        //��������Ϸ����
        //ͨ�����ø����󣬽�һ���Ķ�����е��������õ�ͬһ���������£�ͬʱ�����ж�����еĸ���������Ϊͬһ���������ʾ���ж���صļ��ϡ��ﵽ�����������ֱ�۵�Ŀ�ġ�
        GameObject parentGameObject = new GameObject(prefabName + "Anchor");
        parentGameObject.transform.SetParent(objectPoolTransform);
        //�����ֵ����Ƿ��Ѵ���id
        if (!poolDictionary.ContainsKey(poolKey))
        {
            //�����ֵ�
            poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i < poolSize; i++)
            {
                //ʵ��������
                GameObject newObject = Instantiate(prefab, parentGameObject.transform) as GameObject;
                //������
                newObject.SetActive(false);
                //�������
                poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType)));

            }
        }

    }
    
    //���ö���
    public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        //ʵ��id
        int poolKey = prefab.GetInstanceID();
        //�ֵ��в���
        if (poolDictionary.ContainsKey(poolKey))
        {
            //�Ӷ�����ȡ��
            Component componentToReuse = GetComponentFromPool(poolKey);
            //���ö���
            ResetObject(position, rotation, componentToReuse, prefab);

            return componentToReuse;
        }
        else
        {
            Debug.Log("û��" + prefab);
            return null;
        }
    }

    //ͨ��poolkey�Ӷ�����ȡ��
    private Component GetComponentFromPool(int poolKey)
    {
        //����
        Component componentToReuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(componentToReuse);

        if (componentToReuse.gameObject.activeSelf == true)
        {
            componentToReuse.gameObject.SetActive(false);//������
        }
        return componentToReuse;
    }

    //���ö���
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
