using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform spawnBullet;
    [SerializeField] private float timeBetweenBullets = 0.1f;
    private PlayerController _playerController;
    private float _timer;
    private Pool _pool;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _pool = PoolManager.instance.GetPool("P_Bullets");
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerController.isShooting)
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
