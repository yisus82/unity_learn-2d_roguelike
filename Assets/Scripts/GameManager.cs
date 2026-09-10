using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject playerPrefab;
    public TextMeshProUGUI foodText;
    
    private BoardManager _boardManager;
    private PlayerController _playerController;
    private int _turn;
    private int _foodAmount;
    private int _foodCount;

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
        _foodCount = Random.Range(1, 5);
        _foodAmount = 100;
        foodText.text = "Food: " + _foodAmount;
        _boardManager = GameObject.FindGameObjectWithTag("BoardManager").GetComponent<BoardManager>();
        var player = Instantiate(playerPrefab);
        _playerController = player.GetComponent<PlayerController>();
        _boardManager.GenerateBoard(_playerController, _foodCount);
    }
    
    public void NextTurn() {
        _turn++;
        ChangeFoodAmount(-1);
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

    private void GameOver()
    {
        Destroy(_playerController.gameObject);
    }
}
