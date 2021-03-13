using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GUIController gui;
    BoardManager boardManager;
    public bool isWhiteTurn = true;

    private void Start()
    {
        gui = FindObjectOfType<GUIController>();
        boardManager = FindObjectOfType<BoardManager>();
    }
    private void Update()
    {
        if (gui.gameIsPaused) return;
        RespondToPlayerInput();
    }

    private void RespondToPlayerInput()
    {
        CheckIfValidClick();
    }

    private void CheckIfValidClick()
    {
        RaycastHit[] rayHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 50f, LayerMask.GetMask("ChessBoardBlock"));
        foreach (RaycastHit rayHit in rayHits)
        {
            ChessBlockEditor boardBlock = rayHit.transform.GetComponent<ChessBlockEditor>();
            if (boardBlock == null) continue;
            HandlePlayerInput(boardBlock);
        }
    }

    private void HandlePlayerInput(ChessBlockEditor boardBlock)
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
                    // TODO: FIX THE CHECKMATE ALGORITHM, IF THE KING IS NOT CHECKED BUT CANT MAKE ANYMOVES IT SHOULDNT COUNT AS CHECKMATE
                    if (!boardManager.CheckIfNotCheckmate(isWhiteTurn))
                    {
                        string team;
                        if (isWhiteTurn)
                            team = "white player";
                        else
                            team = "black player";

                        Debug.Log("Checkmate! " + team + " won!");
                    }
                    isWhiteTurn = !isWhiteTurn;
                    boardManager.turn++;
                }
                else
                {
                    boardManager.SelectChessPiece((int)boardBlock.transform.position.x, (int)boardBlock.transform.position.z, isWhiteTurn);
                }
            }
        }
    }
}

//TODO: Weird behaviour while selecting a chess piece probably cause by raycasting or camera position (SOLUTION: Raycast was hitting a collider that was set around entire block, changed so that collider is only on top side of the cube)
