using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private enum EnemyState
    {
        Patrol,
        Follow,
        Attack
    }

    [SerializeField] private float moveSpeed = 0.5f, patrolSpeed = 0.1f;
    [SerializeField] private Transform[] patrolTargets;
    [SerializeField] private float proximityRange = 1f;
    [SerializeField] private float distanceForget = 20;
    [SerializeField] private Vector3 detectionBoxSize = new Vector3(10, 5, 15);
    [SerializeField] private float attackRange = 5f;

    private int _indexPatrol = 0;
    private EnemyState _enemyState;
    private Transform _target;
    private Shoot _shoot;
    private EnemyMove _enemyMove;
    private float _distanceToTarget;

    private void Awake()
    {
        _shoot = GetComponent<Shoot>();
        _enemyMove = GetComponent<EnemyMove>();
    }

    void Start()
    {
        _target = patrolTargets[_indexPatrol];
        _enemyMove.SetSpeed(patrolSpeed);
    }

    void Update()
    {
        if (_target != null)
            _distanceToTarget = Vector3.Distance(transform.position, _target.position);

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
        if (GameManager.Instance.Player != null)
        {
            if (IsPlayerInDetectionBox())
            {
                _enemyState = EnemyState.Follow;
                _enemyMove.SetSpeed(moveSpeed);
                _target = GameManager.Instance.Player.transform;
                return;
            }
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
            _enemyMove.SetSpeed(patrolSpeed);
            _enemyState = EnemyState.Patrol;
            _target = patrolTargets[_indexPatrol];
            return;
        }

        if (_distanceToTarget <= attackRange)
        {
            _enemyState = EnemyState.Attack;
        }
    }

    private void StateAttack()
    {
        _shoot.SetIsShooting(true);

        if (_distanceToTarget > attackRange)
        {
            _enemyState = EnemyState.Follow;
        }
    }

    private void FollowTarget()
    {
        if (_target == null) return;
        Vector3 directionToTarget = (_target.position - transform.position).normalized;
        _enemyMove.SetDirection(directionToTarget);
    }

    private bool IsPlayerInDetectionBox()
    {
        if (GameManager.Instance.Player == null) return false;

        Vector3 playerPosition = GameManager.Instance.Player.transform.position;
        Vector3 localPlayerPos = transform.InverseTransformPoint(playerPosition);

        Vector3 boxCenter = new Vector3(0, 0, detectionBoxSize.z / 2f);

        return Mathf.Abs(localPlayerPos.x - boxCenter.x) <= detectionBoxSize.x / 2f &&
               Mathf.Abs(localPlayerPos.y - boxCenter.y) <= detectionBoxSize.y / 2f &&
               Mathf.Abs(localPlayerPos.z - boxCenter.z) <= detectionBoxSize.z / 2f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Vector3 boxCenter = new Vector3(0, 0, detectionBoxSize.z / 2f);
        Gizmos.DrawWireCube(boxCenter, detectionBoxSize);

        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanceForget);

        if (_target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _target.position);
        }

        if (patrolTargets != null && patrolTargets.Length > 0)
        {
            Gizmos.color = Color.white;
            for (int i = 0; i < patrolTargets.Length; i++)
            {
                if (patrolTargets[i] != null)
                {
                    Gizmos.DrawSphere(patrolTargets[i].position, 0.3f);
                    if (i < patrolTargets.Length - 1 && patrolTargets[i + 1] != null)
                    {
                        Gizmos.DrawLine(patrolTargets[i].position, patrolTargets[i + 1].position);
                    }
                }
            }
        }
    }
}