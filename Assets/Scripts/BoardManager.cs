using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class Cell
    {
        public bool IsPassable;
        public CellObject cellObject;
    }
    
    public int width;
    public int height;
    public Vector2Int playerSpawnPosition;
    public Tile[] groundTiles;
    public Tile[] wallTiles;
    public FoodObject[] foodPrefabs;

    private Tilemap _tilemap;
    private Grid _grid;
    private Cell[,] _cells;
    private List<Vector2Int> _emptyCells;
    private int _foodCount;
    private PlayerController _player;
    
    public void GenerateBoard(PlayerController player, int foodCount) {
        _player = player;
        _foodCount = foodCount;
        _tilemap = GetComponentInChildren<Tilemap>();
        _grid = GetComponentInChildren<Grid>();
        _cells = new Cell[width, height];
        _emptyCells = new List<Vector2Int>();
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
        SpawnPlayer();
        GenerateFood();
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
    
    private void SpawnPlayer()
    {
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
            var food = Instantiate(foodPrefab);
            food.transform.position = CellToWorld(emptyCellPosition);
            var cell = GetCell(emptyCellPosition);
            cell.cellObject = food;
            _emptyCells.RemoveAt(emptyCellIndex);
            if (_emptyCells.Count == 0)
            {
                break;
            }
        }
    }
}