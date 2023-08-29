using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此脚本为ObjectPool的基类,用于管理一个池子内的同种物体
/// 主要提供了Get, PutBack以及Clear函数分别用于取,还,删除操作
/// </summary>

public class ObjectPoolBase
{
    //存储全部的列表
    private Dictionary<PoolObject, bool> m_TotalObjects;
    //当前池子里的objects
    private Stack<PoolObject> m_Objects;
    //使用的原型
    private PoolObject m_Prototype;
    //使用的预设
    private GameObject m_Prefab;
    //挂在的父节点
    private Transform m_Parent;
    
    public ObjectPoolBase(GameObject prefab, int size = 0, Transform parent = null)
    {
        m_Prefab = prefab;
        m_Parent = parent;
        m_Prototype = prefab.GetComponent<PoolObject>();

        if (parent == null)
        {
            Debug.LogError("ObjectPool ctor wrong, Miss parent node!");
        }
        
        if (m_Prototype == null)
        {
            m_Prototype = Object.Instantiate(prefab).AddComponent<PoolObject>();
            
            m_Prototype.transform.SetParent(parent);
            m_Prototype.gameObject.SetActive(false);
        }
        
        m_TotalObjects = new Dictionary<PoolObject, bool>();
        m_Objects = new Stack<PoolObject>();
        
        InitPoolObjects(size);
    }

    private void InitPoolObjects(int size)
    {
        for (int i = 0; i < size; i++)
        {
            GenerateObject();
        }
    }

    private void GenerateObject()
    {
        var obj = Object.Instantiate(m_Prototype);
        obj.transform.SetParent(m_Parent);
        obj.transform.position = Vector3.zero;
        obj.gameObject.SetActive(false);
        
        obj.Init();
        
        m_Objects.Push(obj);
        m_TotalObjects[obj] = true;
    }

    private int Size()
    {
        return m_Objects.Count;
    }

    //取出物体
    public PoolObject Get()
    {
        if (Size() <= 0)
        {
            GenerateObject();
        }

        var obj = m_Objects.Pop();
        obj.Show();
        return obj;
    }

    //放回物体
    public void PutBack(PoolObject poolObject)
    {
        if (poolObject.IsDestroyed())
        {
            Debug.LogError("poolObject is destroyed, but you still try to put it back, please check your code");
            return;
        }
        
        poolObject.Hide();
        var transform = poolObject.transform;
        transform.SetParent(m_Parent);
        transform.position = Vector3.zero;
        poolObject.gameObject.SetActive(false);
        m_Objects.Push(poolObject);
    }

    //清空pool,使用该函数后,这个pool就无法使用了,需要重新创建
    public void Clear()
    {
        foreach (var data in m_TotalObjects)
        {
            if (data.Key != null)
            {
                data.Key.Destroy();
            }
        }
        
        Object.Destroy(m_Prototype.gameObject);

        m_Prefab = null;
        m_Prototype = null;
        m_TotalObjects.Clear();
        m_Objects.Clear();
    }
    
}