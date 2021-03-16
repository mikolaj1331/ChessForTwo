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
        if (gui.gameIsPaused) return;
        RespondToPlayerInput();
    }

    void RespondToPlayerInput()
    {
        CheckIfValidClick();
    }

    void CheckIfValidClick()
    {
        RaycastHit[] rayHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 50f, LayerMask.GetMask("ChessBoardBlock"));
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
            //Debug.Log("You clicked " + boardBlock.name);
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
        if (!boardManager.CheckIfNotCheckmate(isWhiteTurn))
        {
            DisplayGameOverScreen();
        }
    }

    private void DisplayGameOverScreen()
    {
        string team, toDisplay;
        if (isWhiteTurn)
            team = "White player";
        else
            team = "Black player";

        toDisplay = "Game over!\n" + team + " has won!";

        gui.PauseGame();
        gui.OpenWindow(pauseMenuWindow);
        gui.ChangeText(textBox, toDisplay);
    }
}

//TODO: Weird behaviour while selecting a chess piece probably cause by raycasting or camera position (SOLUTION: Raycast was hitting a collider that was set around entire block, changed so that collider is only on top side of the cube)
