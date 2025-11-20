using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference aerocrobaticAction;
    [SerializeField] private InputActionReference stabilizedMoveAction;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference changeSpeedAction, pauseAction;

    private PlayerMove _playerMove;
    private Shoot _shoot;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _shoot = GetComponent<Shoot>();
        aerocrobaticAction.action.Enable();

    }

    private void OnEnable()
    {
        move.action.Enable();
        aerocrobaticAction.action.Enable();
        stabilizedMoveAction.action.Enable();
        shootAction.action.Enable();
        changeSpeedAction.action.Enable();
        changeSpeedAction.action.performed += OnChangeSpeed;
        pauseAction.action.Enable();
        pauseAction.action.performed += OnPausePerformed;
    }

    private void OnDisable()
    {
        move.action.Disable();
        aerocrobaticAction.action.Disable();
        stabilizedMoveAction.action.Disable();
        shootAction.action.Disable();
        changeSpeedAction.action.Disable();
        changeSpeedAction.action.performed -= OnChangeSpeed;
        pauseAction.action.Disable();
        pauseAction.action.performed -= OnPausePerformed;

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

        _playerMove.SetAero(aerocrobaticAction.action.ReadValue<float>() > 0);
    }

    private void OnChangeSpeed(InputAction.CallbackContext ctx)
    {
        float value = ctx.ReadValue<float>();
        bool up = value > 0;
        _playerMove.ChangeSpeed(up);
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        GameManager.Instance.SwitchPause();
    }
}