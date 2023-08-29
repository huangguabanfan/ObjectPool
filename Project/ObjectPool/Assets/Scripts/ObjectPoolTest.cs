using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ObjectPoolTest : MonoBehaviour
{
    public int Num = 50;
    public GameObject CubePrefab;
    private ObjectPoolBase m_CubePool;
    
    public GameObject SpherePrefab;
    private ObjectPoolBase m_SpherePool;

    private Coroutine SpawnCoroutine;

    public Button SpawnButton;
    public Button ClearButton;

    private void OnEnable()
    {
        //Spawn();
    }

    private void OnDisable()
    {
        //Clear();
    }

    private void Start()
    {
        SpawnButton.gameObject.SetActive(true);
        ClearButton.gameObject.SetActive(false);
        
        SpawnButton.onClick.AddListener(() =>
        {
            SpawnButton.gameObject.SetActive(false);
            ClearButton.gameObject.SetActive(true);
            Spawn();
        });
        
        ClearButton.onClick.AddListener(() =>
        {
            SpawnButton.gameObject.SetActive(true);
            ClearButton.gameObject.SetActive(false);
            Clear();
        });
    }

    public void Spawn()
    {
        m_CubePool = ObjectPoolManager.Instance.CreatePool(ObjectPoolNameConfig.TestCubePool, CubePrefab, 10);
        m_SpherePool = ObjectPoolManager.Instance.CreatePool(ObjectPoolNameConfig.TestSpherePool, SpherePrefab, 10);
        if (SpawnCoroutine != null)
        {
            StopCoroutine(SpawnCoroutine);
        }
        
        SpawnCoroutine = StartCoroutine(AutoGenerate());
    }

    public void Clear()
    {
        m_CubePool = null;
        ObjectPoolManager.Instance.ClearPool(ObjectPoolNameConfig.TestCubePool);
        m_SpherePool = null;
        ObjectPoolManager.Instance.ClearPool(ObjectPoolNameConfig.TestSpherePool);
        
        StopCoroutine(SpawnCoroutine);
        SpawnCoroutine = null;
    }

    private IEnumerator AutoGenerate()
    {
        WaitForSeconds wait = new WaitForSeconds(5);
        while (true)
        {
            GenerateCubes();
            GenerateSpheres();
            yield return wait;
        }
    }
    
    private void GenerateCubes()
    {
        for (int i = 0; i < Num; i++)
        {
            var obj = m_CubePool.Get();
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(transform);
            obj.transform.position = UnityEngine.Random.insideUnitSphere * 10;
            StartCoroutine(AutoCubePut(obj));
        }
    }
    
    private void GenerateSpheres()
    {
        for (int i = 0; i < Num; i++)
        {
            var obj = m_SpherePool.Get();
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(transform);
            obj.transform.position = UnityEngine.Random.insideUnitSphere * 10;
            StartCoroutine(AutoSpherePut(obj));
        }
    }

    private IEnumerator AutoCubePut(PoolObject poolObject)
    {
        WaitForSeconds wait = new WaitForSeconds(UnityEngine.Random.Range(1,8));
        yield return wait;

        //由于该协程没有stop掉,所以做个兼容
        if (!poolObject.IsDestroyed())
        {
            m_CubePool?.PutBack(poolObject);
        }
    }
    
    private IEnumerator AutoSpherePut(PoolObject poolObject)
    {
        WaitForSeconds wait = new WaitForSeconds(UnityEngine.Random.Range(1,8));
        yield return wait;

        if (!poolObject.IsDestroyed())
        {
            m_SpherePool?.PutBack(poolObject);
        }
    }
}