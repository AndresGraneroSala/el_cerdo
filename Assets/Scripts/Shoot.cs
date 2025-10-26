using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform spawnBullet;
    [SerializeField] private float timeBetweenBullets = 0.1f;
    private float _timer;
    private Pool _pool;
    private bool _isShooting;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pool = PoolManager.instance.GetPool("P_Bullets");
    }

    public void SetIsShooting(bool isShooting)
    {
        _isShooting = isShooting;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!_isShooting)
        {
            _timer = timeBetweenBullets;
            return; 
        }
        
        _timer += Time.deltaTime;

        if (_timer >= timeBetweenBullets)
        {
            GameObject bullet = _pool.nextObject();
            
            bullet.transform.position = spawnBullet.position;
            bullet.transform.rotation = spawnBullet.rotation;
            bullet.SetActive(true);
            
            _timer = 0;
        }
    }
}
