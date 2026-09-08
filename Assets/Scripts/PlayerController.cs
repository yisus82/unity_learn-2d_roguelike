using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _boardManager;
    private Vector2Int _currentPosition;
    private InputAction _moveAction;

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        if (_moveAction.WasPressedThisFrame())
        {
            var move = _moveAction.ReadValue<Vector2>();
            var newPosition = _currentPosition + new Vector2Int((int)move.x, (int)move.y);
            Move(newPosition);
        }
    }

    public void Spawn(BoardManager boardManager, Vector2Int position)
    {
        _boardManager = boardManager;
        Move( position);
    }

    private void Move(Vector2Int position)
    {
        if (!_boardManager.GetCell(position).IsPassable)
        {
            return;
        }
        
        _currentPosition = position;
        transform.position = _boardManager.CellToWorld(position);
    }
}
