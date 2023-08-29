using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 该脚本用于全局管理所有的ObjectPool,设计的意图是希望用于通过该单例进行创建以及销毁操作
/// 用户需要针对一个池子在ObjectPoolNameConfig里面注册ID,这样用户只需要凭借ID就可以轻松管理
/// 而不需要自己手动管理一个pool,用户只需要Creat,Clear配合使用,以及释放掉pool的引用即可,如果出现问题,多半是ObjectPool自己管理的问题
/// </summary>

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<int, ObjectPoolBase> m_Pools;
    private bool isInit = false;
    private Transform m_Root;

    private Dictionary<int, bool> m_PoolNames;

    private void Init()
    {
        m_Pools = new Dictionary<int, ObjectPoolBase>();
        m_PoolNames = new Dictionary<int, bool>();
        //通过反射获取ObjectPoolNameConfig里面的所有string成员
        var config = new ObjectPoolNameConfig();
        foreach (var item in config.GetType().GetFields())
        {
            if (item.FieldType == typeof(int))
            {
                Debug.Log($"add config name: {item.Name} value: {item.GetValue(config)}");
                m_PoolNames.Add((int)item.GetValue(config), true);
            }
        }
        
        m_Root = new GameObject("ObjectPool").transform;
        Object.DontDestroyOnLoad(m_Root);
        isInit = true;
        
    }

    public ObjectPoolBase CreatePool(int poolID, GameObject prefab, int size)
    {
        if (!isInit)
        {
            Init();
        }

        if (!m_PoolNames.ContainsKey(poolID))
        {
            Debug.LogError($"poolID {poolID} is not registered in ObjectPoolNameConfig class, please register!");
            return null;
        }
        
        Debug.Log($"CreateObjectPool poolID {poolID} prefab {prefab.name} size {size}");
        if (m_Pools.ContainsKey(poolID))
        {
            Debug.LogError("this pool poolID is already used, please change a poolID");
            return null;
        }
        
        var objectPool = new ObjectPoolBase(prefab, size, m_Root);
        m_Pools.Add(poolID, objectPool);
        return objectPool;
    }

    public void ClearPool(int poolID)
    {
        if (!isInit)
        {
            Debug.LogError("You should create pool at first");
            return;
        }
        
        if (!m_PoolNames.ContainsKey(poolID))
        {
            Debug.LogError($"poolID {poolID} is not registered in ObjectPoolNameConfig class, please register!");
            return;
        }
        
        Debug.Log($"ClearPool {poolID}");
        if (!m_Pools.ContainsKey(poolID))
        {
            Debug.LogError($"Clear pool wrong, {poolID} pool is not in pools, please check poolID");
            return;
        }

        if (m_Pools.TryGetValue(poolID, out ObjectPoolBase pool))
        {
           pool.Clear();
           m_Pools.Remove(poolID);
        }
    }
    
}