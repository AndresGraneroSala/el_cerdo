using System;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private PlayerController _playerController;

    [SerializeField] private float _speed = 5;
    [SerializeField] private float _rotSpeed = 5;

    [SerializeField] private GameObject model;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += _speed * transform.forward;

        /* dir -> rot
         * X -> Y
         * Y -> X
         */
        //Vector2 _playerController.direction
        transform.Rotate(
            _rotSpeed * _playerController.direction.y,
            _rotSpeed * _playerController.direction.x,
            0
        );

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);


        TmpAnim();
    }

    public void TmpAnim()
    {
        model.transform.localRotation = Quaternion.Euler(-30 * _playerController.direction.x, -90, 0);
    }

    public void Stablice()
    {
        Vector3 currentRotation = transform.eulerAngles;
        float newX = Mathf.LerpAngle(currentRotation.x, 0f, Time.fixedDeltaTime * _rotSpeed);
        transform.eulerAngles = new Vector3(newX, currentRotation.y, currentRotation.z);
    }
}