using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Game : MonoBehaviour
{
    //Reference from Unity IDE
    public GameObject chesspiece;

    //Matrices needed, positions of each of the GameObjects
    //Also separate arrays for the players in order to easily keep track of them all
    //Keep in mind that the same objects are going to be in "positions" and "playerBlack"/"playerWhite"
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    //current turn
    private string currentPlayer = "white";

    //Game Ending
    private bool gameOver = false;

    //Unity calls this right when the game starts, there are a few built in functions
    //that Unity can call for you
    public void Start()
    {
        string[] whites = {"white_rook","white_knight", "white_bishop", "white_queen", "white_king", "white_bishop", "white_knight", "white_rook", "white_pawn", "white_pawn", "white_pawn", "white_pawn", "white_pawn", "white_pawn", "white_pawn", "white_pawn"};
        string[] blacks = {"black_rook","black_knight", "black_bishop", "black_queen", "black_king", "black_bishop", "black_knight", "black_rook", "black_pawn", "black_pawn", "black_pawn", "black_pawn", "black_pawn", "black_pawn", "black_pawn", "black_pawn"};
         
        // Randomly shuffle the arrays
        ShuffleArray(whites);
        ShuffleArray(blacks);

        //Create each chess piece from the array
        playerWhite = new GameObject[] { Create(whites[0], 0, 0), Create(whites[1], 1, 0),
            Create(whites[2], 2, 0), Create(whites[3], 3, 0), Create(whites[4], 4, 0),
            Create(whites[5], 5, 0), Create(whites[6], 6, 0), Create(whites[7], 7, 0),
            Create(whites[8], 0, 1), Create(whites[9], 1, 1), Create(whites[10], 2, 1),
            Create(whites[11], 3, 1), Create(whites[12], 4, 1), Create(whites[13], 5, 1),
            Create(whites[14], 6, 1), Create(whites[15], 7, 1) };
        playerBlack = new GameObject[] { Create(blacks[0], 0, 7), Create(blacks[1],1,7),
            Create(blacks[2],2,7), Create(blacks[3],3,7), Create(blacks[4],4,7),
            Create(blacks[5],5,7), Create(blacks[6],6,7), Create(blacks[7],7,7),
            Create(blacks[8], 0, 6), Create(blacks[9], 1, 6), Create(blacks[10], 2, 6),
            Create(blacks[11], 3, 6), Create(blacks[12], 4, 6), Create(blacks[13], 5, 6),
            Create(blacks[14], 6, 6), Create(blacks[15], 7, 6) };

        //Set all piece positions on the positions board
        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    //Shuffle chess piece array to set position of the chess pieces
    static void ShuffleArray<T>(T[] array)
    {
        System.Random rand = new System.Random();

        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            // Swap elements
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
    

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>(); //We have access to the GameObject, we need the script
        cm.name = name; //This is a built in variable that Unity has, so we did not have to declare it before
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate(); //It has everything set up so it can now Activate()
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();

        //Overwrites either empty space or whatever was there
        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            //Using UnityEngine.SceneManagement is needed here
            SceneManager.LoadScene("Game"); //Restarts the game by loading the scene over again
        }
    }
    
    public void Winner(string playerWinner)
    {
        gameOver = true;

        //Using UnityEngine.UI is needed here
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " Wins!!";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }
}
