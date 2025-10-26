using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference stabilizedMoveAction;
    [SerializeField] private InputActionReference shootAction;
    private Vector2 aerocrobatic;

    private PlayerMove _playerMove;
    private Shoot _shoot;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _shoot = GetComponent<Shoot>();
    }

    // Update is called once per frame
    void Update()
    {
        _playerMove.SetDirection(move.action.ReadValue<Vector2>());

        if (stabilizedMoveAction.action.ReadValue<float>() > 0)
        {
            _playerMove.Stablice();
        }

        _shoot.SetIsShooting(shootAction.action.ReadValue<float>() > 0);

    }
}
