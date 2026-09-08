using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private BoardManager _boardManager;
    private PlayerController _playerController;
    private int turn;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void Start()
    {
        _boardManager = GameObject.FindGameObjectWithTag("BoardManager").GetComponent<BoardManager>();
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _boardManager.GenerateBoard();
        _playerController.Spawn(_boardManager, _boardManager.playerSpawnPosition);
        turn = 0;
    }
    
    public void NextTurn() {
        turn++;
    }
}
