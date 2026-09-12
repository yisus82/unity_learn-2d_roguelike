using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public class Cell
    {
        public bool IsPassable;
        public CellObject cellObject;
    }
    
    public int width;
    public int height;
    public ExitObject exitPrefab;
    public Tile[] groundTiles;
    public Tile[] wallTiles;
    public FoodObject[] foodPrefabs;
    public ObstacleObject[] obstaclePrefabs;

    private Tilemap _tilemap;
    private Grid _grid;
    private Cell[,] _cells;
    private List<Vector2Int> _emptyCells;
    private int _foodCount;
    private int _obstacleCount;
    private PlayerController _player;

    private void Awake()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
        _grid = GetComponentInChildren<Grid>();
    }

    public void GenerateBoard(PlayerController player, int foodCount, int obstacleCount) {
        _player = player;
        _foodCount = foodCount;
        _obstacleCount = obstacleCount;
        ClearBoard();
        for (var y = 0; y < height; ++y)
        {
            for(var x = 0; x < width; ++x)
            {
                Tile tile;
                _cells[x, y] = new Cell();
                
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    tile = wallTiles[Random.Range(0, wallTiles.Length)];
                    _cells[x, y].IsPassable = false;
                }
                else
                {
                    tile = groundTiles[Random.Range(0, groundTiles.Length)];
                    _cells[x, y].IsPassable = true;
                    _emptyCells.Add(new Vector2Int(x, y));
                }
                
                _tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
        SetExitCell();
        SpawnPlayer();
        GenerateFood();
        GenerateObstacles();
    }
    
    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return _grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public Cell GetCell(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= width || cellIndex.y < 0 || cellIndex.y >= height)
        {
            return null;
        }
        return _cells[cellIndex.x, cellIndex.y];
    }

    private void ClearBoard()
    {
        _tilemap.ClearAllTiles();
        var foodObjects = GameObject.FindGameObjectsWithTag("Food");
        foreach (var foodObject in foodObjects)
        {
            Destroy(foodObject);
        }
        var obstacleObjects = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var obstacleObject in obstacleObjects)
        {
            Destroy(obstacleObject);
        }
        var exitObjects = GameObject.FindGameObjectsWithTag("Exit");
        foreach (var exitObject in exitObjects)
        {
            Destroy(exitObject);
        }
        _cells = new Cell[width, height];
        _emptyCells = new List<Vector2Int>();
    }

    private void SetExitCell()
    {
        var exitCellPosition = new Vector2Int(width - 2, height - 2);
        AddCellObject(exitPrefab, exitCellPosition);
        _emptyCells.Remove(exitCellPosition);
    }
    
    private void SpawnPlayer()
    {
        var playerSpawnPosition = new Vector2Int(1, 1);
        _player.Spawn(this, playerSpawnPosition);
        _emptyCells.Remove(playerSpawnPosition);
    }
    
    private void GenerateFood()
    {
        for (var i = 0; i < _foodCount; i++)
        {
            var emptyCellIndex = Random.Range(0, _emptyCells.Count);
            var emptyCellPosition = _emptyCells[emptyCellIndex];
            var foodPrefab = foodPrefabs[Random.Range(0, foodPrefabs.Length)];
            AddCellObject(foodPrefab, emptyCellPosition);
            _emptyCells.RemoveAt(emptyCellIndex);
            if (_emptyCells.Count == 0)
            {
                break;
            }
        }
    }

    private void AddCellObject(CellObject cellObject, Vector2Int cellPosition)
    {
        var obj = Instantiate(cellObject);
        obj.transform.position = CellToWorld(cellPosition);
        obj.cellPosition = cellPosition;
        var cell = GetCell(cellPosition);
        cell.cellObject = obj;
    }

    public void RemoveCellObject(Vector2Int cellPosition)
    {
        var cell = GetCell(cellPosition);
        cell.cellObject =  null;
        _emptyCells.Add(cellPosition);
    }
    
    private void GenerateObstacles()
    {
        for (var i = 0; i < _obstacleCount; i++)
        {
            var emptyCellIndex = Random.Range(0, _emptyCells.Count);
            var emptyCellPosition = _emptyCells[emptyCellIndex];
            var obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            AddCellObject(obstaclePrefab, emptyCellPosition);
            _emptyCells.RemoveAt(emptyCellIndex);
            if (_emptyCells.Count == 0)
            {
                break;
            }
        }
    }
}