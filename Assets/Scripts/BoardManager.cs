using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public class Cell
    {
        public bool IsPassable;
    }
    
    public int width;
    public int height;
    public Vector2Int playerSpawnPosition;
    public Tile[] groundTiles;
    public Tile[] wallTiles;

    private Tilemap _tilemap;
    private Grid _grid;
    private Cell[,] _cells;
    
    public void GenerateBoard() {
        _tilemap = GetComponentInChildren<Tilemap>();
        _grid = GetComponentInChildren<Grid>();
        _cells = new Cell[width, height];
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
                }
                
                _tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
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

    public void SpawnPlayer(PlayerController player)
    {
        player.Spawn(this, playerSpawnPosition);
    }
}