using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeDestroy;
    [SerializeField] private float damage;
    [SerializeField] private string poolName="P_Bullets";
    private float _timer;
    private Pool _pool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _pool = PoolManager.instance.GetPool(poolName);
    }

    // Update is called once per frame
    void Update()
    {
        _timer  += Time.deltaTime;
        if (_timer >= timeDestroy)
        {
            _pool.pullObject(gameObject);
            _timer = 0;
        }
        
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime*speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
