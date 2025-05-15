using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public static GameBoard Instance;

    [Header("UI Controller")]
    [SerializeField] private GameUIController gameUI;

    [Header("Player Presets")]
    [SerializeField] private PlayerData xPlayerData;  
    [SerializeField] private PlayerData oPlayerData;

    [Header("Cell")]
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellParent;          
    private int boardSize = 3;             
    
    private PlayerData player1Data;
    private PlayerData player2Data;

    private GameMode currentMode;
    private PlayerData aiPlayer;

    [SerializeField] private PlayerData currentPlayer;
    private List<Cell> cellList = new List<Cell>();

    private bool gameActive;

    public bool GameActive => gameActive;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }


    public void StartGame()
    {
        currentMode = GameManager.Instance.CurrentMode;

        ClearBoard();               
        GenerateBoard();            
        ResetBoard();             

        player1Data = xPlayerData;
        player2Data = oPlayerData;
        currentPlayer = player1Data;
        gameActive = true;

        if (currentMode == GameMode.SinglePlayer)
            aiPlayer = player2Data;

        gameUI.SetTurnStatus($"Turn: {currentPlayer.symbol}");
    }

    public void Start()
    {
        StartGame();
    }

    public void MakeMove(int index)
    {
        if (index < 0 || index >= cellList.Count) return;
        if (!gameActive || !cellList[index].IsEmpty()) return;

        cellList[index].SetState(currentPlayer);
        if (CheckWinner())
        {
            gameActive = false;
            gameUI.ShowResult($"Player {currentPlayer.symbol} Wins!");
            gameUI.RegisterResultButtons(StartGame, () => SceneLoader.Instance.LoadScene(SceneType.Home));
        }
        else if (IsBoardFull())
        {
            gameActive = false;
            gameUI.ShowResult("It's a Draw!");
            gameUI.RegisterResultButtons(StartGame, () => SceneLoader.Instance.LoadScene(SceneType.Home));
        }
        else
        {
            SwitchTurn();
        }
    }

    private void MakeAIMove()
    {
        if (!gameActive) return;

        var available = new List<int>();

        for (int i = 0; i < cellList.Count; i++)
        {
            if (cellList[i].IsEmpty())
                available.Add(i);
        }

        if (available.Count > 0)
        {
            int randomIndex = available[Random.Range(0, available.Count)];
            MakeMove(randomIndex);
        }
    }

    private void SwitchTurn()
    {
        currentPlayer = (currentPlayer == player1Data) ? player2Data : player1Data;
        gameUI.SetTurnStatus($"Turn: {currentPlayer.symbol}");

        if (currentMode == GameMode.SinglePlayer && currentPlayer == aiPlayer)
        {
            Invoke(nameof(MakeAIMove), 0.5f); // AI delay
        }
    }

    private bool CheckWinner()
    {
        int[,] winPatterns = new int[,]
        {
        {0,1,2}, {3,4,5}, {6,7,8}, // Rows
        {0,3,6}, {1,4,7}, {2,5,8}, // Columns
        {0,4,8}, {2,4,6}           // Diagonals
        };

        for (int i = 0; i < winPatterns.GetLength(0); i++)
        {
            int a = winPatterns[i, 0];
            int b = winPatterns[i, 1];
            int c = winPatterns[i, 2];

            if (IsSameOwner(a, b, c))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsSameOwner(int a, int b, int c)
    {
        if (a >= cellList.Count || b >= cellList.Count || c >= cellList.Count)
            return false;

        var ownerA = cellList[a].GetOwner();
        var ownerB = cellList[b].GetOwner();
        var ownerC = cellList[c].GetOwner();

        return ownerA != null && ownerA == ownerB && ownerB == ownerC;
    }

    private bool IsBoardFull()
    {
        foreach (var cell in cellList)
        {
            if (cell.IsEmpty()) return false;
        }
        return true;
    }

    private void GenerateBoard()
    {
        int totalCells = boardSize * boardSize;
        for (int i = 0; i < totalCells; i++)
        {
            GameObject cellGO = Instantiate(cellPrefab, cellParent);
            Cell cell = cellGO.GetComponent<Cell>();
            cell.index = i;
            cellList.Add(cell);
        }
    }

    private void ClearBoard()
    {
        foreach (Transform child in cellParent)
        {
            Destroy(child.gameObject);
        }
        cellList.Clear();
    }

    public void ResetBoard()
    {
        foreach (var cell in cellList)
        {
            cell.ResetCell();
        }
    }
}
