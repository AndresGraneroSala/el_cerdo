using System;
using UnityEditor.Search;
using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;
    [SerializeField] private List<Pool> pools;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Pool pool in pools)
        {
            InitializePool(pool);
        }
        
    }

    public Pool GetPool(String poolName)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (poolName == pools[i].poolName)
            {
                return pools[i];
            }
        }
        
        return null;
    }

    public void AddPool(Pool pool)
    {
        pools.Add(pool);
        InitializePool(pool);
        
    }

    public void RemovePool(Pool pool)
    {
        pools.Remove(pool);
    }

    private void InitializePool(Pool pool)
    {
        Transform parent = new GameObject(pool.poolName).transform;
        parent.SetParent(transform);

        List<GameObject> children = new List<GameObject>();
        
        for (int i = 0; i < pool.poolSize; i++)
        {
            GameObject obj = Instantiate(pool.prefab, parent);
            obj.SetActive(false);
            obj.transform.SetParent(parent);
            children.Add(obj);
        }
        
        pool.init(parent, children);
        
    }
}

[Serializable]
public class Pool
{
    public string poolName;
    public int poolSize;
    public GameObject prefab;
    private List<GameObject> objects;
    private int index=0;
    private Transform parent;

    Pool(string poolName, int poolSize)
    {
        this.poolName = poolName;
        this.poolSize = poolSize;
    }
    
    public void init(Transform parent, List<GameObject> objects)
    {
        this.parent = parent;
        this.objects = objects;
    }

    public GameObject nextObject()
    {
        // Busca el siguiente objeto inactivo
        for (int i = 0; i < objects.Count; i++)
        {
            GameObject obj = objects[index];
            index = (index + 1) % objects.Count;

            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // Si todos están activos, puedes:
        // 1️⃣ Devolver null (no hay disponibles)
        // 2️⃣ O crear uno nuevo dinámicamente
        // Aquí optamos por null:
        Debug.LogWarning($"[Pool '{poolName}'] No hay objetos disponibles en el pool.");
        return null;
    }

    public void pullObject(GameObject obj)
    {
        //objects.Add(obj);
        obj.SetActive(false);
        obj.transform.SetParent(parent);
    }
}