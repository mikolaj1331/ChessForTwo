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
        RaycastHit[] rayHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition),50f,LayerMask.GetMask("ChessBoardBlock"));
        foreach (RaycastHit rayHit in rayHits)
        {
            ChessBlockEditor boardBlock = rayHit.transform.GetComponent<ChessBlockEditor>();
            if (boardBlock == null) continue;
            if(Input.GetMouseButtonDown(0))
            {
                Camera cam = (Camera)FindObjectOfType(typeof(Camera));
                Debug.DrawLine(cam.transform.position, rayHit.point, Color.red, 2f);
                Debug.Log("Nacisnales " + boardBlock.name);
                if(boardManager.selectedChessPiece == null)
                {
                    boardManager.SelectChessPiece((int)boardBlock.transform.position.x, (int)boardBlock.transform.position.z,isWhiteTurn);
                }
                else
                {
                    bool validMove = boardManager.MoveChessPiece((int)boardBlock.transform.position.x, (int)boardBlock.transform.position.z);
                    
                    if(validMove)
                        isWhiteTurn = !isWhiteTurn;
                    else
                        boardManager.SelectChessPiece((int)boardBlock.transform.position.x, (int)boardBlock.transform.position.z, isWhiteTurn);
                }
            }
        }
    }
}

//TODO: Weird behaviour while selecting a chess piece probably cause by raycasting or camera position