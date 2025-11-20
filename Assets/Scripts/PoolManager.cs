using System;
using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    [SerializeField] private List<Pool> pools;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    public Pool GetPool(String poolName, int poolSize = 50, GameObject poolPrefab = null)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (poolName == pools[i].poolName)
            {
                return pools[i];
            }
        }

        if (poolPrefab == null)
        {
            Debug.LogError($"[PoolManager] No se puede crear pool '{poolName}': prefab es null");
            return null;
        }


        Pool p = new Pool(poolName, poolSize, poolPrefab);
        AddPool(p);
        return p;
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
        
        pool.Init(parent, children);
        
    }
    
    private void OnValidate()
    {
        // Verifica nombres duplicados de pools en el inspector
        HashSet<string> seenNames = new HashSet<string>();

        foreach (var pool in pools)
        {
            if (pool == null) continue;

            if (string.IsNullOrWhiteSpace(pool.poolName))
            {
                Debug.LogWarning($"[PoolManager] Hay un pool sin nombre asignado.");
                continue;
            }

            if (!seenNames.Add(pool.poolName))
            {
                Debug.LogError($"[PoolManager] Nombre de pool duplicado detectado: '{pool.poolName}'");
            }
        }
    }
}

[Serializable]
public class Pool
{
    public string poolName;
    public int poolSize;
    public GameObject prefab;
    private List<GameObject> _objects;
    private int _index=0;
    private Transform _parent;

    public Pool(string poolName, int poolSize, GameObject prefab)
    {
        this.poolName = poolName;
        this.poolSize = poolSize;
        this.prefab = prefab;
    }
    
    public void Init(Transform parent, List<GameObject> objects)
    {
        this._parent = parent;
        this._objects = objects;
    }

    public GameObject NextObject()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            GameObject obj = _objects[_index];
            _index = (_index + 1) % _objects.Count;

            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        
        Debug.LogWarning($"[Pool '{poolName}'] No hay objetos disponibles en el pool.");
        return null;
    }

    public void PullObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(_parent);
    }
}