using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float rotSpeed = 5;
    [SerializeField] private float stabSpeed = 0.5f;
    [SerializeField] [Range(-360.0f, 360.0f)] private float minPitch = 60f;
    [SerializeField] [Range(-360.0f, 360.0f)] private float maxPitch =  300f;
    [SerializeField] private GameObject model;

    private Vector2 _direction;

    public void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }
    
    public void SetDirection(Vector3 worldDirection)
    {
        worldDirection = worldDirection.normalized;
        
        // 2. Convertir a espacio local
        Vector3 localDir = transform.InverseTransformDirection(worldDirection);
        
        // 3. Calcular ángulos de rotación necesarios
        float targetYaw = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;
        float targetPitch = -Mathf.Asin(localDir.y) * Mathf.Rad2Deg;
        
        // 4. Convertir a valores normalizados (-1 a 1)
        Vector2 normalizedInput = new Vector2(
            Mathf.Clamp(targetYaw / 90f, -1f, 1f),
            Mathf.Clamp(targetPitch / 90f, -1f, 1f)
        );
        
        _direction = normalizedInput;
    }
    
    private void Start()
    {
        minPitch = VerifyAngle(minPitch);
        maxPitch = VerifyAngle(maxPitch);
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //constant move
        transform.position += transform.forward * speed;

        //rotation
        Vector2 dir = _direction;
        transform.Rotate(rotSpeed * dir.y, rotSpeed * dir.x, 0f, Space.Self);

        var euler = transform.eulerAngles;
        euler.z = 0f;

        euler.x = ClampAngle360(euler.x, minPitch, maxPitch);
        
        transform.eulerAngles = euler;


        TmpAnim();
    }
    
    float ClampAngle360(float angle, float min, float max)
    {
        if (min < max)
        {
            if (angle < min) angle = min;
            if (angle > max) angle = max;
        }
        else
        {
            if (angle > max && angle < min)
            {
                float distToMin = Mathf.DeltaAngle(angle, min);
                float distToMax = Mathf.DeltaAngle(angle, max);
                angle = Mathf.Abs(distToMin) < Mathf.Abs(distToMax) ? min : max;
            }
        }
        return angle;
    }

    float VerifyAngle(float angle)
    {
        angle %= 360f;               
        if (angle < 0f) angle += 360f;
        return angle;
    }


    public void TmpAnim()
    {
        model.transform.localRotation = Quaternion.Euler(-30 * _direction.x, -90, 0);
    }

    public void Stablice()
    {
        Vector3 currentRotation = transform.eulerAngles;
        float newX = Mathf.LerpAngle(currentRotation.x, 0f, Time.fixedDeltaTime * stabSpeed);
        transform.eulerAngles = new Vector3(newX, currentRotation.y, currentRotation.z);
    }
}