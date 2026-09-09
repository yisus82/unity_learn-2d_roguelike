using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject playerPrefab;
    
    private BoardManager _boardManager;
    private PlayerController _playerController;
    private int _turn;

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
        _boardManager.GenerateBoard();
        var player = Instantiate(playerPrefab);
        _playerController = player.GetComponent<PlayerController>();
        _boardManager.SpawnPlayer(_playerController);
        _turn = 0;
    }
    
    public void NextTurn() {
        _turn++;
        Debug.Log("Turn " + _turn);
    }
}
