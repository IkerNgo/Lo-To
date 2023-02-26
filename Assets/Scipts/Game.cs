using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Game : MonoBehaviour
{
    private Board board;                        //Bàn game loto
    private Cell[,] state;                      //Mot mang nhung ô thuoc bàn game
    private GameObject canvas;
    private GameManager gameManager;
    private int[] amountNumberInEachCol;        //Mang de xác dinh có bao nhiêu so xuat hien trong mot cot
    private int[,] numberArray;                 //Mang tat ca so có trong bàn game
    private int[] full5Number;                  //Mang nhung hàng dã du 5 so
    private List<int> numberList = new List<int>();//List nhung hàng dã du 5 so
    private bool help7, help6, help5, help4;    //Bool nhung hàng can uu tiên
    private int rowNeedHelp6, rowNeedHelp7, rowNeedHelp5, rowNeedHelp4;//Nhung hàng can uu tiên
    private List<int> rowHadStar = new List<int>();
    [SerializeField] private TextMeshProUGUI myTextElement;//So hien thi trên mat ô
    [SerializeField] private GameObject hadCell;
    [SerializeField] private GameObject frame;
    [SerializeField] private GameObject spinStar;
    private void Awake()
    {
        board = GetComponentInChildren<Board>();
        canvas = GameObject.Find("Canvas");
        gameManager=FindObjectOfType<GameManager>();
        Instantiate(frame,board.getTilePosition(8,8),Quaternion.identity);
    }
    private void Start()
    {
        newGame();
    }

    private void Update()
    {
        if(!gameManager.won && !gameManager.lose && !gameManager.pause)
        {
            if (Input.GetMouseButtonDown(0))
            {
                getHadCell();
            }
            checkWin();
            check4Number();
        }
    }
    private void newGame()
    {
        state = new Cell[9, 9];                 //Tao mang 0 9x9 Cell
        initialVariables();                     //Khoi tao các thông so ban dau
        generateAmountNumberInEachColumn();     //Tao so con so xuat hien trong 1 cot
        generateNumberCell();                   //Gán so vào các ô
        generateCell(state);                    //Gán các thông so cua tung Cell vào mang
        Camera.main.transform.position = new Vector3(9 / 2, 9 / 2 * 1.5f, -10);//Giúp camera focus vào trung tâm bàn game
        board.draw(state, board.gameStats.color);                      //Hien thi bàn game
    }
    private void initialVariables()
    {
        amountNumberInEachCol = new int[9];     //Tao mang 9 so 0 tuong trung cho các cot chua có gì
        numberArray = new int[9, 9];            //Tao mang 0 9x9 tuong trung cho ca bàn game deu là ô trong
        full5Number = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };//Tao mang -1 vì se có hàng so 0
        numberList.Clear();                     //Xóa list
    }
    private void generateCell(Cell[,] state)
    {
        for(int i=0; i < 9; i++)
        {
            for(int j =0; j < 9; j++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(j, 8-i, 0);//Do Tilemap chon góc duoi trái làm diem dau nên phai lon nguoc lên
                if (numberArray[i, j]==0)
                {
                    cell.type = Cell.Type.Blank;//Neu ô dó có giá tri bang 0 se là ô trong
                }
                if(numberArray[i, j]!=0)        //Neu khác 0 se tien hành gán so và vi trí vào Cell
                {
                    cell.type = Cell.Type.Number;
                    cell.number = numberArray[i,j];
                    cell.had = false;
                    Vector3 position = board.getTilePosition(j,8-i);//Lay vi trí cua ô theo toa do the gioi
                    myTextElement.text = cell.number.ToString();
                    //Tao mot Text so tai vi trí ô tuong ung
                    TextMeshProUGUI number = Instantiate(myTextElement, position, Quaternion.identity,canvas.transform);
                }
                state[i,j] = cell;
            }
        }
    }

    //Hàm de lay so trong khoang tu 0 den 8 mà không trùng voi các so dã có
    private int getColumn(int[] col)
    {
        int column = Random.Range(0, 9);
        for (int j = 0; j < col.Length; j++)
        {
            if(column == col[j])
            {
                column = getColumn(col);
            }
        }
        return column;
    }

    //Chay ngau nhiên so con so xuat hien trong 1 cot
    private int getAmountNumberInColumn(int n)
    {
        if (n == 1)
        {
            int amount = Random.Range(4, 6);
            return amount;
        }
        if (n == 2)
        {
            int amount = Random.Range(5, 6);
            return amount;
        }
        if (n == 3)
        {
            int amount = Random.Range(4, 5);
            return amount;
        }
        if (n == 4)
        {
            return 4;
        }
        else return 5;
    }

    //Hàm tao so con so có trong mot hàng
    private void generateAmountNumberInEachColumn()
    {
        int[] col = new int[9] {-1,-1,-1,-1,-1,-1,-1,-1,-1};//Mang de xác dinh cot nào dã duoc tao so roi
        int count4 = 0;                     //So cot có 4 con so
        int count5 = 2;                     //So cot có 5 con so trong dó cot 0 và 8 mac dinh bang 5
        int count6 = 0;                     //So cot có 6 con so
        for (int i = 0; i < 9; i++)
        {
            col[i] = getColumn(col);        //Chon ngau nhiên mot cot de tao so con so trong cot
            if (col[i] == 0 || col[i] == 8) 
            {
                amountNumberInEachCol[col[i]] = 5;//Neu cot bang 0 hoac 8 se mac dinh có 5 con so
            }
            else                            //Các truong hop de tao thành 3 cot 4, 3 cot 5 và 3 cot 6
            {
                if (count4 < 3 && count6 < 3 && count5 < 3)
                {
                    int n = getAmountNumberInColumn(1);
                    if (n == 4)
                        count4++;
                    else if (n == 6)
                        count6++;
                    else count5++;
                    amountNumberInEachCol[col[i]] = n;
                }
                else if (count4 == 3 && count6 < 3 && count5 < 3)
                {
                    int n = getAmountNumberInColumn(2);
                    if (n == 6)
                        count6++;
                    else count5++;
                    amountNumberInEachCol[col[i]] = n;
                }
                else if (count6 == 3 && count4 < 3 && count5 < 3)
                {
                    int n = getAmountNumberInColumn(3);
                    if (n == 4)
                        count4++;
                    else count5++;
                    amountNumberInEachCol[col[i]] = n;
                }
                else if (count5 == 3 && count4 < 3 && count6 < 3)
                {
                    int n = getAmountNumberInColumn(4);
                    if (n == 4)
                        count4++;
                    else count6++;
                    amountNumberInEachCol[col[i]] = n;
                }
                else if (count4 == 3 && count5 == 3)
                {
                    amountNumberInEachCol[col[i]] = 6;
                }
                else if (count4 == 3 && count6 == 3)
                {
                    amountNumberInEachCol[col[i]] = 5;
                }
                else if (count6 == 3 && count5 == 3)
                {
                    amountNumberInEachCol[col[i]] = 4;
                }
            }
        }
    }

    //Tao so o tung vi trí ô
    private void generateNumberCell()
    {
        for (int i = 0; i < 9; i++)                 //Tao theo cot tu 0 den 8
        {
            List<int> numberPosition = new List<int>();
            for (int j = 0; j < amountNumberInEachCol[i]; j++)//So con so duoc tao tùy thuoc vào so con so tung cot duoc tao o trên
            {
                if (i == 0)                         //Neu là hàng dau tiên se tao so tu 1-9
                {
                    checkRow();
                    numberPosition.Add(generate5NumberEachRow(numberPosition));
                    numberArray[numberPosition[j], i] = getRandomNumber(1, 9, numberArray);
                }
                else if (i == 8)                    //Neu là hàng cuoi cùng thì tao so tu 80-90
                {
                    checkRow();
                    numberPosition.Add(generate5NumberEachRow(numberPosition));
                    numberArray[numberPosition[j], i] = getRandomNumber(80, 90, numberArray);
                }
                else if (i == 4)                    //Neu là hàng 4 thì se kiem tra dieu kien de uu tiên hàng
                {
                    help4 = false;
                    checkRow();
                    if (help4)
                    {
                        if (numberPosition.Contains(rowNeedHelp4))
                            numberPosition.Add(generate5NumberEachRow(numberPosition));
                        else numberPosition.Add(rowNeedHelp4);
                        help4 = false;
                    }
                    else numberPosition.Add(generate5NumberEachRow(numberPosition));

                    numberArray[numberPosition[j], i] = getRandomNumber(i * 10, i * 10 + 9, numberArray);
                }
                else if (i == 5)                    //Neu là hàng 5 thì se kiem tra dieu kien de uu tiên hàng
                {
                    help5 = false;
                    checkRow();
                    if (help5)
                    {
                        if (numberPosition.Contains(rowNeedHelp5))
                            numberPosition.Add(generate5NumberEachRow(numberPosition));
                        else numberPosition.Add(rowNeedHelp5);
                        help5 = false;
                    }
                    else numberPosition.Add(generate5NumberEachRow(numberPosition));

                    numberArray[numberPosition[j], i] = getRandomNumber(i * 10, i * 10 + 9, numberArray);
                }
                else if (i == 6)                    //Neu là hàng 6 thì so kiem tra dieu kien de uu tiên hàng
                {
                    help6 = false;
                    checkRow();
                    if (help6)
                    {
                        if (numberPosition.Contains(rowNeedHelp6))
                            numberPosition.Add(generate5NumberEachRow(numberPosition));
                        else numberPosition.Add(rowNeedHelp6);
                        help6 = false;
                    }
                    else numberPosition.Add(generate5NumberEachRow(numberPosition));

                    numberArray[numberPosition[j], i] = getRandomNumber(i * 10, i * 10 + 9, numberArray);
                }
                else if(i==7)                       //Neu là hàng 7 thì so kiem tra dieu kien de uu tiên hàng
                {
                    help7 = false;
                    checkRow();
                    if (help7)
                    {
                        if(numberPosition.Contains(rowNeedHelp7))
                            numberPosition.Add(generate5NumberEachRow(numberPosition));
                        else numberPosition.Add(rowNeedHelp7);
                        help7 = false;
                    }   
                    else numberPosition.Add(generate5NumberEachRow(numberPosition));

                    numberArray[numberPosition[j], i] = getRandomNumber(i * 10, i * 10 + 9, numberArray);
                }
                else                                 //Các hàng còn lai khoi tao bình thuong
                {
                    checkRow();
                    numberPosition.Add(generate5NumberEachRow(numberPosition));
                    numberArray[numberPosition[j], i] = getRandomNumber(i * 10, i * 10 + 9, numberArray);
                }
            }

        }
    }

    //Hàm tao mot con so thuoc mot khoang và không trùng voi so dã có truoc dó
    private int getRandomNumber(int a, int b, int[,] array)
    {
        int n = Random.Range(a, b+1);
        for (int i = 0; i < 9; i++)
        {
            for(int j=0; j < 9; j++)
            {
                if (n == array[i,j])
                {
                    n = getRandomNumber(a, b, array);
                }
            }
        }
        return n;
    }

    //Hàm kiem tra de uu tiên tung hàng
    private void checkRow()
    {
        for (int h = 0; h < 9; h++)
        {
            int count = 0;                  //Count dùng de xác dinh các hàng nào dã du 5 so
            for (int k = 0; k < 9; k++)
            {
                if (numberArray[h, k] != 0)
                {
                    count++;
                }
                if (count == 5)
                {
                    full5Number[h] = h;     //VD: Neu hàng 3 du 5 so thì ô 3 cua mang se bang 3
                }
                if(k>3 && count<=0)         //Dieu kien de uu tiên hàng 4
                {
                    help4 = true;
                    rowNeedHelp4 = h;
                }
                if (k > 4 && count <= 1)    //Dieu kien de uu tiên hàng 5
                {
                    help5 = true;
                    rowNeedHelp5 = h;
                }
                if (k > 5 && count <= 2)    //Dieu kien de uu tiên hàng 6
                {
                    help6 = true;
                    rowNeedHelp6 = h;
                }
                if (k>6 && count<=3)        //Dieu kien de uu tiên hàng 7
                {
                    help7 = true;
                    rowNeedHelp7 = h;
                }
            }
            if (full5Number[h]!=-1)         //Neu hàng nào du 5 so se thêm hàng dó vào list
            {
                numberList.Add(h);
            }
        }
        
    }

    //Hàm chon vi trí không trùng voi vi trí dã có trong cot hoac dính vào hàng dã du 5 so
    private int generate5NumberEachRow(List<int> list)
    {
        int n = Random.Range(0, 9);

        while(numberList.Contains(n) || list.Contains(n))
        {
            n = Random.Range(0, 9);
        }
        return n;
    }

    private void getHadCell()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);

        //Debug.Log(state[8-cellPosition.y,cellPosition.x].number);
        if(cellPosition.x<=8 && cellPosition.x>=0 && cellPosition.y<=8 && cellPosition.y>=0)
        {
            if (state[8-cellPosition.y, cellPosition.x].type == Cell.Type.Number && !state[8-cellPosition.y,cellPosition.x].had)
            {
                if (gameManager.numberCalledList.Contains(state[8 - cellPosition.y, cellPosition.x].number))
                {
                    Instantiate(hadCell, board.getTilePosition(cellPosition.x+4,cellPosition.y+4), Quaternion.identity);
                    state[8-cellPosition.y, cellPosition.x].had = true;
                }
            }
        }
    }

    private void checkWin()
    {
        for(int j=0; j<=8;j++)         
        {
            int count = 0;
            for(int i=0; i<=8;i++)
            {
                if (state[j,i].type==Cell.Type.Number && state[j,i].had)
                {
                    count++;
                }
            }

            if(count == 5)
            {
                gameManager.won = true;
                gameManager.lose = false;
            }
        }
    }

    private void check4Number()
    {
        for (int j = 0; j <= 8; j++)
        {
            int count = 0;
            if (rowHadStar.Contains(j))
            {
                continue;
            }
            for (int i = 0; i <= 8; i++)
            {
                if (state[j, i].type == Cell.Type.Number && state[j, i].had)
                {
                    count++;
                }
            }

            if (count == 4)
            {
                Instantiate(spinStar, board.getTilePosition(3, 12 - j), Quaternion.identity);
                rowHadStar.Add(j);
                gameManager.playNoticeSound();
                continue;
            }

        }
    }
}
