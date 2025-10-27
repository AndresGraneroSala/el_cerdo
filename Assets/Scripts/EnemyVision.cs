using System;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Field of View Settings")]
    public float viewDistance = 20f;
    public float viewAngle = 45f;
    public float maxHeightDifference = 5f;

    [Header("Close Range Detection")]
    public float closeRangeDistance = 5f;  // rango del raycast corto

    [Header("Layers")]
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    private EnemyController _controller;

    private void Awake()
    {
        _controller = GetComponent<EnemyController>();
    }

    void Update()
    {
        DetectPlayer();
    }

    void DetectPlayer()
    {
        bool playerDetected = false;

        // --------------------------
        // 1️⃣ DETECCIÓN POR PIRÁMIDE
        // --------------------------
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 dirToTarget = target.transform.position - transform.position;
            float distanceToTarget = dirToTarget.magnitude;

            // Verificar altura
            float heightDifference = Mathf.Abs(dirToTarget.y);
            if (heightDifference > maxHeightDifference)
                continue;

            // Verificar ángulo horizontal
            Vector3 dirToTargetXZ = new Vector3(dirToTarget.x, 0, dirToTarget.z);
            float angleToTarget = Vector3.Angle(transform.forward, dirToTargetXZ);

            if (angleToTarget < viewAngle / 2)
            {
                // Chequear si hay obstáculos
                if (!Physics.Raycast(transform.position, dirToTarget.normalized, distanceToTarget, obstacleMask))
                {
                    playerDetected = true;
                    break;
                }
            }
        }

        // --------------------------
        // 2️⃣ DETECCIÓN POR RAY CORTO
        // --------------------------
        if (!playerDetected)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, closeRangeDistance, targetMask))
            {
                // Si hay contacto directo con el jugador
                playerDetected = true;
            }
        }

        _controller.SetIsInVision(playerDetected);
    }

    // --------------------------
    // 4️⃣ GIZMOS VISUALES
    // --------------------------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward * viewDistance;

        // Rotaciones para las esquinas
        Quaternion leftRot = Quaternion.AngleAxis(-viewAngle / 2, Vector3.up);
        Quaternion rightRot = Quaternion.AngleAxis(viewAngle / 2, Vector3.up);

        Vector3 leftDir = leftRot * forward;
        Vector3 rightDir = rightRot * forward;

        // Altura superior e inferior
        Vector3 upOffset = Vector3.up * maxHeightDifference;
        Vector3 downOffset = Vector3.down * maxHeightDifference;

        // Esquinas de la base cuadrada
        Vector3 topLeft = origin + leftDir + upOffset;
        Vector3 topRight = origin + rightDir + upOffset;
        Vector3 bottomLeft = origin + leftDir + downOffset;
        Vector3 bottomRight = origin + rightDir + downOffset;

        // Líneas desde el origen
        Gizmos.DrawLine(origin, topLeft);
        Gizmos.DrawLine(origin, topRight);
        Gizmos.DrawLine(origin, bottomLeft);
        Gizmos.DrawLine(origin, bottomRight);

        // Base
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(topLeft, bottomLeft);
        Gizmos.DrawLine(topRight, bottomRight);

        // Raycast corto (detección cercana)
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin, closeRangeDistance);
    }
}
