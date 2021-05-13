using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuWindow;
    [SerializeField] TextMeshProUGUI textBox;

    GUIController gui;
    BoardManager boardManager;
    public bool isWhiteTurn = true;

    bool whiteTurnFinished = false;
    bool blackTurnFinished = false;

    void Start()
    {
        gui = FindObjectOfType<GUIController>();
        boardManager = FindObjectOfType<BoardManager>();
    }
    void Update()
    {
        if (gui.isPaused) return;
        RespondToPlayerInput();
    }
    void RespondToPlayerInput()
    {
        Ray point = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask("ChessBoardBlock");
        RaycastHit[] rayHits = Physics.RaycastAll(point, 50f, layerMask);
        foreach (RaycastHit rayHit in rayHits)
        {
            ChessBlockEditor boardBlock = rayHit.transform.GetComponent<ChessBlockEditor>();
            if (boardBlock == null || boardBlock.isBorder) continue;
            HandlePlayerInput(boardBlock);
        }
    }

    void HandlePlayerInput(ChessBlockEditor boardBlock)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (boardManager.selectedChessPiece == null)
            {
                boardManager.SelectChessPiece((int)boardBlock.transform.position.x, (int)boardBlock.transform.position.z, isWhiteTurn);
            }
            else
            {
                bool validMove = boardManager.MoveChessPiece((int)boardBlock.transform.position.x, (int)boardBlock.transform.position.z, isWhiteTurn);

                if (validMove)
                {
                    HandleCheckmate();

                    if (isWhiteTurn)
                        whiteTurnFinished = true;
                    else
                        blackTurnFinished = true;

                    if(whiteTurnFinished && blackTurnFinished)
                    {
                        boardManager.turn++;
                        whiteTurnFinished = false;
                        blackTurnFinished = false;
                    }

                    isWhiteTurn = !isWhiteTurn;
                }
                else
                {
                    boardManager.SelectChessPiece((int)boardBlock.transform.position.x, (int)boardBlock.transform.position.z, isWhiteTurn);
                }
            }
        }
    }
    void HandleCheckmate()
    {
        GameResult result = boardManager.CheckIfNotCheckmate(isWhiteTurn);
        
        switch(result)
        {
            case GameResult.NotCheckmate:
                break;
            case GameResult.Checkmate:
                DisplayGameOverScreen(false);
                break;
            case GameResult.Draw:
                DisplayGameOverScreen(true);
                break;
        }
    }
    void DisplayGameOverScreen(bool isDraw)
    {
        string toDisplay = "Game over!\n";
        if (!isDraw)
        {
            string team; 
            if (isWhiteTurn)
                team = "White pieces";
            else
                team = "Black pieces";

            toDisplay += team + " won!";
        }
        else
        {
            toDisplay += "It's a stalemate!";
        }
        

        gui.PauseGame();
        gui.OpenWindow(pauseMenuWindow);
        gui.ChangeText(textBox, toDisplay);
    }
}