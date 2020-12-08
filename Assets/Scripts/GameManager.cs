using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    BoardManager boardManager;
    public bool isWhiteTurn = true;

    private void Start()
    {
        boardManager = FindObjectOfType<BoardManager>();
    }
    private void Update()
    {
        RespondToPlayerInput();
    }

    private void RespondToPlayerInput()
    {
        RaycastHit[] rayHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition),50f,LayerMask.GetMask("ChessBoardBlock"));
        foreach (RaycastHit rayHit in rayHits)
        {
            //Debug.Log("Hit");
            ChessBlockEditor boardBlock = rayHit.transform.GetComponent<ChessBlockEditor>();
            if (boardBlock == null) continue;
            if(Input.GetMouseButtonDown(0))
            {
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
                }
            }
        }
    }
}
