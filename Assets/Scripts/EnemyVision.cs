using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private EnemyController _controller;

    private void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _controller.SetIsInVision(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _controller.SetIsInVision(false);
        }
    }

    private void OnDisable()
    {
        _controller.SetIsInVision(false);
    }
}
