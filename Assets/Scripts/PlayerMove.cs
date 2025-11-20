using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 5, intervalSpeed = 0.1f;
    [SerializeField] private float speedAero = 2;
    [SerializeField] private float rotSpeed = 5, rotSpeedAero = 10;
    [SerializeField] private float stabSpeed = 0.5f;

    [SerializeField] [Range(-360.0f, 360.0f)]
    private float minPitch = 60f;

    [SerializeField] [Range(-360.0f, 360.0f)]
    private float maxPitch = 300f;

    [SerializeField] private GameObject model;

    [SerializeField] private RectTransform imageSpeed, imageAero;
    [SerializeField] private GameObject speedUI, aeroUI;

    private Vector2 _direction;
    private bool _isAero = false;
    private float _currentXRotation = 0f;
    private float _currentYRotation = 0f;
    private bool _notBlockWas = false;
    private const float AngleEpsilon = 1;
    private float _speed;

    private bool _isInFloor = false;

    private float GetSpeed()
    {
        return _isAero ? speedAero : _speed;
    }

    private float GetSpeedRotation()
    {
        return _isAero ? rotSpeedAero : rotSpeed;
    }

    public void ChangeSpeed(bool up)
    {
        if (up)
        {
            IncreaseSpeed();
        }
        else
        {
            DecreaseSpeed();
        }

        _speed = Mathf.Clamp(_speed, 0, maxSpeed);

    }

    private void IncreaseSpeed()
    {
        _speed += intervalSpeed;
    }

    private void DecreaseSpeed()
    {
        _speed -= intervalSpeed;
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

    

    private void Start()
    {
        minPitch = VerifyAngle(minPitch);
        maxPitch = VerifyAngle(maxPitch);
        Vector3 currentEuler = transform.eulerAngles;
        _currentXRotation = currentEuler.x;
        _currentYRotation = currentEuler.y;

        _speed = maxSpeed;
    }

    private void LateUpdate()
    {
        SetUISpeed();
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * GetSpeed();
        Vector2 dir = _direction;

        _currentXRotation += GetSpeedRotation() * dir.y;
        _currentYRotation += GetSpeedRotation() * dir.x;


        if (_isInFloor)
        {
            _currentXRotation = Mathf.Lerp(_currentYRotation, 0f, 5f);
            transform.position += new Vector3(0, 0.2f, 0);

        }

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

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            _isInFloor = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            _isInFloor = false;
        }
    }


    private void SetUISpeed()
    {
        if (speedUI==null)
        {
            return;
        }
        
        aeroUI.SetActive(_isAero);
        speedUI.SetActive(!_isAero);


        RectTransform image = _isAero ? imageAero : model.GetComponent<RectTransform>();


        if (image == null)
        {
            return;
        }

        float scaleX = GetSpeed() / maxSpeed;


        Vector3 scale = image.localScale;
        scale.x = scaleX;
        image.localScale = scale;

    }
}