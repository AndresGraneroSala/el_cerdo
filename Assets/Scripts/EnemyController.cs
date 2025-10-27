using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        Patrol,
        Follow,
        Attack
    }
    
    [SerializeField] private GameObject [] patrolTargets;

    private EnemyState _enemyState;
    private Transform target;
    private Shoot _shoot;
    private PlayerMove _move;
    private bool _isInVision;

    public void SetIsInVision( bool isInVision )
    {
        _isInVision = isInVision;
    }
    

    private void Awake()
    {
        _shoot = GetComponent<Shoot>();
        _move = GetComponent<PlayerMove>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = patrolTargets[0].transform;
    }

    // Update is called once per frame
    void Update()
    {       
        Vector3 directionToTarget = target.position - transform.position;
        _move.SetDirection(directionToTarget);
    }
}
