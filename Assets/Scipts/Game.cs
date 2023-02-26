using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class Game : MonoBehaviour
{
    private Board board;                        //B�n game loto
    private Cell[,] state;                      //Mot mang nhung � thuoc b�n game
    private GameObject canvas;
    private GameManager gameManager;
    private int[] amountNumberInEachCol;        //Mang de x�c dinh c� bao nhi�u so xuat hien trong mot cot
    private int[,] numberArray;                 //Mang tat ca so c� trong b�n game
    private int[] full5Number;                  //Mang nhung h�ng d� du 5 so
    private List<int> numberList = new List<int>();//List nhung h�ng d� du 5 so
    private bool help7, help6, help5, help4;    //Bool nhung h�ng can uu ti�n
    private int rowNeedHelp6, rowNeedHelp7, rowNeedHelp5, rowNeedHelp4;//Nhung h�ng can uu ti�n
    private List<int> rowHadStar = new List<int>();
    [SerializeField] private TextMeshProUGUI myTextElement;//So hien thi tr�n mat �
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
        initialVariables();                     //Khoi tao c�c th�ng so ban dau
        generateAmountNumberInEachColumn();     //Tao so con so xuat hien trong 1 cot
        generateNumberCell();                   //G�n so v�o c�c �
        generateCell(state);                    //G�n c�c th�ng so cua tung Cell v�o mang
        Camera.main.transform.position = new Vector3(9 / 2, 9 / 2 * 1.5f, -10);//Gi�p camera focus v�o trung t�m b�n game
        board.draw(state, board.gameStats.color);                      //Hien thi b�n game
    }
    private void initialVariables()
    {
        amountNumberInEachCol = new int[9];     //Tao mang 9 so 0 tuong trung cho c�c cot chua c� g�
        numberArray = new int[9, 9];            //Tao mang 0 9x9 tuong trung cho ca b�n game deu l� � trong
        full5Number = new int[9] { -1, -1, -1, -1, -1, -1, -1, -1, -1 };//Tao mang -1 v� se c� h�ng so 0
        numberList.Clear();                     //X�a list
    }
    private void generateCell(Cell[,] state)
    {
        for(int i=0; i < 9; i++)
        {
            for(int j =0; j < 9; j++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(j, 8-i, 0);//Do Tilemap chon g�c duoi tr�i l�m diem dau n�n phai lon nguoc l�n
                if (numberArray[i, j]==0)
                {
                    cell.type = Cell.Type.Blank;//Neu � d� c� gi� tri bang 0 se l� � trong
                }
                if(numberArray[i, j]!=0)        //Neu kh�c 0 se tien h�nh g�n so v� vi tr� v�o Cell
                {
                    cell.type = Cell.Type.Number;
                    cell.number = numberArray[i,j];
                    cell.had = false;
                    Vector3 position = board.getTilePosition(j,8-i);//Lay vi tr� cua � theo toa do the gioi
                    myTextElement.text = cell.number.ToString();
                    //Tao mot Text so tai vi tr� � tuong ung
                    TextMeshProUGUI number = Instantiate(myTextElement, position, Quaternion.identity,canvas.transform);
                }
                state[i,j] = cell;
            }
        }
    }

    //H�m de lay so trong khoang tu 0 den 8 m� kh�ng tr�ng voi c�c so d� c�
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

    //Chay ngau nhi�n so con so xuat hien trong 1 cot
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

    //H�m tao so con so c� trong mot h�ng
    private void generateAmountNumberInEachColumn()
    {
        int[] col = new int[9] {-1,-1,-1,-1,-1,-1,-1,-1,-1};//Mang de x�c dinh cot n�o d� duoc tao so roi
        int count4 = 0;                     //So cot c� 4 con so
        int count5 = 2;                     //So cot c� 5 con so trong d� cot 0 v� 8 mac dinh bang 5
        int count6 = 0;                     //So cot c� 6 con so
        for (int i = 0; i < 9; i++)
        {
            col[i] = getColumn(col);        //Chon ngau nhi�n mot cot de tao so con so trong cot
            if (col[i] == 0 || col[i] == 8) 
            {
                amountNumberInEachCol[col[i]] = 5;//Neu cot bang 0 hoac 8 se mac dinh c� 5 con so
            }
            else                            //C�c truong hop de tao th�nh 3 cot 4, 3 cot 5 v� 3 cot 6
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

    //Tao so o tung vi tr� �
    private void generateNumberCell()
    {
        for (int i = 0; i < 9; i++)                 //Tao theo cot tu 0 den 8
        {
            List<int> numberPosition = new List<int>();
            for (int j = 0; j < amountNumberInEachCol[i]; j++)//So con so duoc tao t�y thuoc v�o so con so tung cot duoc tao o tr�n
            {
                if (i == 0)                         //Neu l� h�ng dau ti�n se tao so tu 1-9
                {
                    checkRow();
                    numberPosition.Add(generate5NumberEachRow(numberPosition));
                    numberArray[numberPosition[j], i] = getRandomNumber(1, 9, numberArray);
                }
                else if (i == 8)                    //Neu l� h�ng cuoi c�ng th� tao so tu 80-90
                {
                    checkRow();
                    numberPosition.Add(generate5NumberEachRow(numberPosition));
                    numberArray[numberPosition[j], i] = getRandomNumber(80, 90, numberArray);
                }
                else if (i == 4)                    //Neu l� h�ng 4 th� se kiem tra dieu kien de uu ti�n h�ng
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
                else if (i == 5)                    //Neu l� h�ng 5 th� se kiem tra dieu kien de uu ti�n h�ng
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
                else if (i == 6)                    //Neu l� h�ng 6 th� so kiem tra dieu kien de uu ti�n h�ng
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
                else if(i==7)                       //Neu l� h�ng 7 th� so kiem tra dieu kien de uu ti�n h�ng
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
                else                                 //C�c h�ng c�n lai khoi tao b�nh thuong
                {
                    checkRow();
                    numberPosition.Add(generate5NumberEachRow(numberPosition));
                    numberArray[numberPosition[j], i] = getRandomNumber(i * 10, i * 10 + 9, numberArray);
                }
            }

        }
    }

    //H�m tao mot con so thuoc mot khoang v� kh�ng tr�ng voi so d� c� truoc d�
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

    //H�m kiem tra de uu ti�n tung h�ng
    private void checkRow()
    {
        for (int h = 0; h < 9; h++)
        {
            int count = 0;                  //Count d�ng de x�c dinh c�c h�ng n�o d� du 5 so
            for (int k = 0; k < 9; k++)
            {
                if (numberArray[h, k] != 0)
                {
                    count++;
                }
                if (count == 5)
                {
                    full5Number[h] = h;     //VD: Neu h�ng 3 du 5 so th� � 3 cua mang se bang 3
                }
                if(k>3 && count<=0)         //Dieu kien de uu ti�n h�ng 4
                {
                    help4 = true;
                    rowNeedHelp4 = h;
                }
                if (k > 4 && count <= 1)    //Dieu kien de uu ti�n h�ng 5
                {
                    help5 = true;
                    rowNeedHelp5 = h;
                }
                if (k > 5 && count <= 2)    //Dieu kien de uu ti�n h�ng 6
                {
                    help6 = true;
                    rowNeedHelp6 = h;
                }
                if (k>6 && count<=3)        //Dieu kien de uu ti�n h�ng 7
                {
                    help7 = true;
                    rowNeedHelp7 = h;
                }
            }
            if (full5Number[h]!=-1)         //Neu h�ng n�o du 5 so se th�m h�ng d� v�o list
            {
                numberList.Add(h);
            }
        }
        
    }

    //H�m chon vi tr� kh�ng tr�ng voi vi tr� d� c� trong cot hoac d�nh v�o h�ng d� du 5 so
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
