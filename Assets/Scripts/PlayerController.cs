using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _boardManager;
    private Vector2Int _currentPosition;
    private InputAction _moveAction;
    private InputAction _restartAction;
    private InputAction _exitAction;
    private Animator _animator;

    private void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _restartAction = InputSystem.actions.FindAction("Restart");
        _exitAction = InputSystem.actions.FindAction("Exit");
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_exitAction.WasPressedThisFrame())
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        if (GameManager.Instance.IsGameOver)
        {
            if (!_restartAction.WasPressedThisFrame())
            {
                return;
            }
            Destroy(gameObject);
            GameManager.Instance.Restart();
        }

        if (!_moveAction.WasPressedThisFrame())
        {
            return;
        }
        
        var move = _moveAction.ReadValue<Vector2>();
        var newPosition = _currentPosition + new Vector2Int((int)move.x, (int)move.y);
        TryToMove(newPosition);
    }

    public void Spawn(BoardManager boardManager, Vector2Int position)
    {
        _boardManager = boardManager;
        if (!_boardManager.GetCell(position).IsPassable)
        {
            return;
        }
        Move( position);
    }

    public void Attack()
    {
        _animator.SetTrigger("Attack");
    }

    private void TryToMove(Vector2Int position)
    {
        if (!_boardManager.GetCell(position).IsPassable)
        {
            return;
        }
        
        GameManager.Instance.NextTurn();
        var cell = _boardManager.GetCell(position);
        var containedObject = cell.cellObject;
        
        if (containedObject)
        {
            containedObject.OnPlayerEntered();
            if (containedObject is ObstacleObject || containedObject is ExitObject)
            {
                return;
            }
        }
        Move(position);
    }

    private void Move(Vector2Int position)
    {
        _currentPosition = position;
        transform.position = _boardManager.CellToWorld(_currentPosition);
    }
}
