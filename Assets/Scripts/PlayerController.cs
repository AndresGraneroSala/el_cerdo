using System;
using UnityEngine;
using UnityEngine.InputSystem; 
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference  move;
    [SerializeField] private InputActionReference stabilizedMoveAction;
    [field: SerializeField] public Vector2 direction{get; private set;}
    [SerializeField] private Vector2 aerocrobatic;
    
    private PlayerMove _playerMove;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = move.action.ReadValue<Vector2>();

        if (stabilizedMoveAction.action.ReadValue<float>()>0)
        {
            _playerMove.Stablice();
        }
        
    }
}
