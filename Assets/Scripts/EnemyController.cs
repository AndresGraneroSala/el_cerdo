using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        Patrol,
        Follow,
        Attack
    }
    
    [SerializeField] private float moveSpeed = 0.5f, patrolSpeed = 0.1f;
    [SerializeField] private Transform [] patrolTargets;
    [SerializeField] private float proximityRange=1f;
    [SerializeField] private float distanceForget = 20;
    [SerializeField] private float distanceToAttack = 10;
    [FormerlySerializedAs("radiusDetection")] [SerializeField] private float distanceDetection = 50;
    private int _indexPatrol=0;
    private EnemyState _enemyState;
    private Transform _target;
    private Shoot _shoot;
    private PlayerMove _move;
    private bool _isInVision;
    private float _distanceToTarget;
    
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
        _target = patrolTargets[_indexPatrol];
        _move.SetSpeed(patrolSpeed);
    }

    private void LateUpdate()
    {
        _distanceToTarget = Vector3.Distance(transform.position, _target.position);
    }

    // Update is called once per frame
    void Update()
    {
        switch (_enemyState)
        {
            case EnemyState.Patrol:
                StatePatrol();
                break;
            case EnemyState.Follow:
                StateFollow();
                break;
            case EnemyState.Attack:
                StateAttack();
                break;
        }
        
        FollowTarget();
    }

    private void StatePatrol()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _target.position);
        
        
        if (_isInVision || distanceToPlayer <= distanceDetection)
        {
            _enemyState = EnemyState.Follow;
            _move.SetSpeed(moveSpeed);
            _target = GameManager.Instance.player.transform;
            return;
        }
        
        if (_distanceToTarget < proximityRange)
        {
            _indexPatrol = (_indexPatrol + 1) % patrolTargets.Length;
            _target = patrolTargets[_indexPatrol];
        }
    }
    
    private void StateFollow()
    {
        if (_distanceToTarget >= distanceForget)
        {
            _move.SetSpeed(patrolSpeed);
            _enemyState = EnemyState.Patrol;
            _target = patrolTargets[_indexPatrol];
        }

        if (_distanceToTarget<=distanceToAttack)
        {
            StateAttack();
            _move.SetAero(true);
            _enemyState = EnemyState.Attack;
        }
    }

   

    private void StateAttack()
    {
        _shoot.SetIsShooting(_isInVision);
        
        if (_distanceToTarget > distanceToAttack)
        {
            _enemyState = EnemyState.Follow;
            _move.SetAero(false);
        }
    }

    private void FollowTarget()
    {
        Vector3 directionToTarget = _target.position - transform.position;
        _move.SetDirection(directionToTarget);
    }
}
