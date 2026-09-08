using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private class Cell
    {
        public bool IsPassable;
    }
    
    public int width;
    public int height;
    public Tile[] groundTiles;
    public Tile[] wallTiles;

    private Tilemap _tilemap;
    private Cell[,] _cells;

    private void Start()
    {
        _tilemap = GetComponentInChildren<Tilemap>();
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

}