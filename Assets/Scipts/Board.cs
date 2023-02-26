using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    //T?o Tilemap và 2 tile tr?ng v?i có s?
    public Tilemap tilemap { get; private set; }
    public Tile blankBlue;
    public Tile blankGreen;
    public Tile blankRed;
    public Tile blankYellow;
    public Tile number;

    private GameObject stat;
    public GameStats gameStats;
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        stat = GameObject.Find("Game Stats");
        gameStats = stat.GetComponent<GameStats>();
    }

    //Hi?n th? các ô trong tilemap
    public void draw(Cell[,] state, int n)
    {
        int width=state.GetLength(0);
        int height=state.GetLength(1);

        for(int x=0; x<width; x++)
        {
            for(int y=0; y<height; y++)
            {
                Cell cell = state[x,y];
                tilemap.SetTile(cell.position, GetTile(cell, n)); //Gán v? trí cho t?ng Cell
            }
        }
    }

    //Hàm ?? xác ??nh ô ?ó là tr?ng hay có s?
    private Tile GetTile(Cell cell, int n)
    {
        if (cell.number != 0)
            return number;
        else
        {
            if(n==1)
            {
                return blankBlue;
            }
            else if(n==2)
            {
                return blankGreen;
            }
            else if(n==3)
            {
                return blankRed;
            }
            else return blankYellow;
        }
    }

    //Hàm ?? l?y v? trí c?a ô so v?i t?a ?? th? gi?i
    public Vector3 getTilePosition(int x, int y)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(x-4,y-4,0));
    }

    public Vector3 getTilePositionForComputer(int x, int y)
    {
        return tilemap.GetCellCenterWorld(new Vector3Int(x -8 , y-8 , 0));
    }
}
