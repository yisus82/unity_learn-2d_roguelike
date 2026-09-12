using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject playerPrefab;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI gameOverText;
    
    private BoardManager _boardManager;
    private PlayerController _playerController;
    private int _turn;
    private int _level;
    private int _foodAmount;
    private int _foodCount;
    private int _obstacleCount;
    public bool IsGameOver { get; private set; }

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
        _turn = 0;
        _level = 0;
        _foodAmount = 10;
        foodText.text = "Food: " + _foodAmount;
        _boardManager = GameObject.FindGameObjectWithTag("BoardManager").GetComponent<BoardManager>();
        var player = Instantiate(playerPrefab);
        _playerController = player.GetComponent<PlayerController>();
        GenerateNextLevel();
    }
    
    public void NextTurn() {
        _turn++;
        ChangeFoodAmount(-1);
    }

    public void GenerateNextLevel()
    {
        _level++;
        _foodCount = Random.Range(1, 5);
        _obstacleCount = Random.Range(5, 10);
        _boardManager.GenerateBoard(_playerController, _foodCount, _obstacleCount);
    }

    public void Restart()
    {
        gameOverText.gameObject.SetActive(false);
        IsGameOver = false;
        Start();
    }
    
    public void ChangeFoodAmount(int amount)
    {
        _foodAmount += amount;
        foodText.text = "Food: " + _foodAmount;
        if (_foodAmount <= 0)
        {
            GameOver();
        }
    }

    public void RemoveCellObject(Vector2Int cellPosition)
    {
        _boardManager.RemoveCellObject(cellPosition);
    }

    private void GameOver()
    {
        _playerController.GetComponent<Animator>().SetTrigger("Die");
        gameOverText.text = "Game Over!\n\nYou've died on day " + _level +
                            "\n\nPress Enter to play a new game\n\nPress Esc to exit game";
        gameOverText.gameObject.SetActive(true);
        IsGameOver = true;
    }
}
