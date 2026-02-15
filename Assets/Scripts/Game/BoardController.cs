using UnityEngine;

public class BoardController : MonoBehaviour
{
    private Tile[,] tiles = new Tile[8, 8];

    public static BoardController Instance;

    private void Awake()
    {
        Instance = this;
        CacheTiles();
    }

    private void CacheTiles()
    {
        Tile[] children = GetComponentsInChildren<Tile>();

        foreach (Tile tile in children)
        {
            tiles[tile.x, tile.y] = tile;
        }
    }

    public Tile GetTile(int x, int y)
    {
        return tiles[x, y];
    }
}
