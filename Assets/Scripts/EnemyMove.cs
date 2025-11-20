using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float separationForce = 2f;
    [SerializeField] private float separationRadius = 3f;
    
    private Vector3 _direction;
    private float _currentSpeed;

    public void SetSpeed(float speed)
    {
        _currentSpeed = speed;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction.normalized;
    }

    void Update()
    {
        Vector3 separation = CalculateSeparation();
        Vector3 finalDirection = (_direction + separation).normalized;

        if (finalDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(finalDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        transform.position += transform.forward * _currentSpeed * Time.deltaTime;
    }

    private Vector3 CalculateSeparation()
    {
        Vector3 separation = Vector3.zero;
        int count = 0;

        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, separationRadius);
        
        foreach (Collider collider in nearbyEnemies)
        {
            if (collider.CompareTag("Enemy") && collider.gameObject != this.gameObject)
            {
                Vector3 directionAway = transform.position - collider.transform.position;
                float distance = directionAway.magnitude;
                
                if (distance > 0)
                {
                    separation += directionAway.normalized / distance;
                    count++;
                }
            }
        }

        if (count > 0)
        {
            separation /= count;
            separation *= separationForce;
        }

        return separation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}