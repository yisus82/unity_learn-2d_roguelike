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
        if (!_boardManager || _moveAction == null || !_moveAction.WasPressedThisFrame())
        {
            return;
        }
        
        var move = _moveAction.ReadValue<Vector2>();
        var newPosition = _currentPosition + new Vector2Int((int)move.x, (int)move.y);
        if (Move(newPosition))
        {
            GameManager.Instance.NextTurn();
        }
    }

    public void Spawn(BoardManager boardManager, Vector2Int position)
    {
        _boardManager = boardManager;
        Move( position);
    }

    private bool Move(Vector2Int position)
    {
        if (!_boardManager.GetCell(position).IsPassable)
        {
            return false;
        }
        
        _currentPosition = position;
        transform.position = _boardManager.CellToWorld(position);
        return true;
    }
}
