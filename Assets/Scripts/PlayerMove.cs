using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 5,speedAero=2;
    [SerializeField] private float rotSpeed = 5,rotSpeedAero=10;
    [SerializeField] private float stabSpeed = 0.5f;
    [SerializeField] [Range(-360.0f, 360.0f)] private float minPitch = 60f;
    [SerializeField] [Range(-360.0f, 360.0f)] private float maxPitch =  300f;
    [SerializeField] private GameObject model;

    private Vector2 _direction;
    private bool _isAero=false;
    private float _currentXRotation = 0f;
    private float _currentYRotation = 0f;
    [SerializeField] private bool _notBlockWas=false;
    private const float AngleEpsilon = 1;
    
    private float GetSpeed()
    {
        return _isAero? speedAero:speed;
    }

    private float GetSpeedRotation()
    {
        return _isAero? rotSpeedAero:rotSpeed;
    }
    
    public void SetAero(bool isAero)
    {
        _isAero = isAero;

        if (isAero)
        {
            _notBlockWas = true;
        }
    }
    
    public void SetDirection(Vector2 dir)
    {
        _direction = dir;
    }

    public void SetSpeed(float speed)
    {
        if (speed < 0f)
        {
            return;
        }
        this.speed = speed;
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
        Vector3 currentEuler = transform.eulerAngles;
        _currentXRotation = currentEuler.x;
        _currentYRotation = currentEuler.y;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //constant move
        transform.position += transform.forward * GetSpeed();

        //rotation quads
        Vector2 dir = _direction;
    
        _currentXRotation += GetSpeedRotation() * dir.y;
        _currentYRotation += GetSpeedRotation() * dir.x;

        if (!_isAero)
        {
            float x = ClampAngle360(_currentXRotation, minPitch, maxPitch);

            if (_notBlockWas)
            {
                if (Mathf.Abs(x - _currentXRotation) < AngleEpsilon)
                {
                    _notBlockWas = false;
                }
            }

            if (!_notBlockWas)
            {
                _currentXRotation = x;
            }
            
            
        }

        Quaternion newRotation = Quaternion.Euler(_currentXRotation, _currentYRotation, 0f);
    
        transform.rotation = newRotation;

        TmpAnim();
    }
    
    float ClampAngle360(float angle, float min, float max)
{ 
    float normalizedAngle = Mathf.DeltaAngle(0, angle);
    float normalizedMin = Mathf.DeltaAngle(0, min);
    float normalizedMax = Mathf.DeltaAngle(0, max);
    
    if (normalizedMin < normalizedMax)
    {
        if (normalizedAngle < normalizedMin) return min;
        if (normalizedAngle > normalizedMax) return max;
        return angle;
    }
    else
    {
        if (normalizedAngle > normalizedMax && normalizedAngle < normalizedMin)
        {
            float distToMin = Mathf.Abs(Mathf.DeltaAngle(normalizedAngle, normalizedMin));
            float distToMax = Mathf.Abs(Mathf.DeltaAngle(normalizedAngle, normalizedMax));
            return distToMin < distToMax ? min : max;
        }
        return angle;
    }
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
        float newX = Mathf.LerpAngle(_currentXRotation, 0f, Time.fixedDeltaTime * stabSpeed);
        _currentXRotation = newX;
        transform.rotation = Quaternion.Euler(_currentXRotation, _currentYRotation, 0f);
    }
}