using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    public ChessPiece[,] Pieces { set; get; }
    public ChessPiece selectedChessPiece;
    public int turn;

    bool[,] ValidMoves { set; get; }
    List<ChessPiece> activeChessPieces;
    ChessBlockEditor[] blocks;
    MatchLogger logger;

    private void Start()
    {
        turn = 1;
        Instance = this;
        blocks = FindObjectsOfType<ChessBlockEditor>();
        logger = GetComponent<MatchLogger>();
        Pieces = InitialSetup();
    }
    private ChessPiece[,] InitialSetup()
    {
        activeChessPieces = new List<ChessPiece>();
        ChessPiece[,] pieces = new ChessPiece[8,8];
        var ChessPieces = FindObjectsOfType<ChessPiece>();
        for(int i = 0; i < ChessPieces.Length; i++)
        {
            var tmp = ChessPieces[i];
            tmp.PositionX = (int)tmp.transform.position.x;
            tmp.PositionY = (int)tmp.transform.position.z;
            pieces[tmp.PositionX, tmp.PositionY] = tmp;
            activeChessPieces.Add(tmp);
        }
        return pieces;
    }    
    public void SelectChessPiece(int x, int y, bool isWhiteTurn)
    {
        ResetVisuals();

        var piece = Pieces[x, y];
        if (piece == null)
        {
            selectedChessPiece = null;
            return;
        }       
        ValidMoves = piece.GetValidMoves(false);

        if(piece.CompareTag("King"))
        {
            ValidMoves = piece.GetComponent<King>().FindInvalidMoves(ValidMoves, activeChessPieces);
        }

        ProcessVisuals(isWhiteTurn, piece);

        selectedChessPiece = piece;
    }
    public bool MoveChessPiece(int x, int y, bool isWhiteTurn)
    {
        if (selectedChessPiece.IsWhite != isWhiteTurn) return false;
        if (ValidMoves[x, y])
        {
            if (Pieces[x, y] != null)
            {
                ChessPiece target = Pieces[x, y];
                if (target.GetType() == typeof(King))
                {
                    // Win the game
                }
                Pieces[x, y].gameObject.SetActive(false);
                activeChessPieces.Remove(Pieces[x, y]);
            }
            logger.LogMovement(selectedChessPiece, x, y, turn);
            ProcessMovement(x, y);
            CheckForPromotion(selectedChessPiece);
            ResetVisuals();
            selectedChessPiece = null;
            return true;
        }
        else
            return false;
    }

    private void CheckForPromotion(ChessPiece selectedChessPiece)
    {
        if(selectedChessPiece.CompareTag("Pawn"))
        {
            if ((selectedChessPiece.IsWhite && selectedChessPiece.PositionY == 7) || (!selectedChessPiece.IsWhite && selectedChessPiece.PositionY == 0))
            {
                Pieces = selectedChessPiece.GetComponent<Pawn>().PromoteToQueen(Pieces,activeChessPieces);
                
            }
        }
        return;
    }

    void ProcessMovement(int x, int y)
    {
        Pieces[selectedChessPiece.PositionX, selectedChessPiece.PositionY] = null;
        selectedChessPiece.transform.position = new Vector3(x, selectedChessPiece.transform.position.y, y);
        selectedChessPiece.PositionX = x;
        selectedChessPiece.PositionY = y;
        Pieces[x, y] = selectedChessPiece;        
    }
    void ProcessVisuals(bool turn, ChessPiece piece)
    {
        if (turn == piece.IsWhite)
        {
            HighlightValidMoves(ValidMoves, blocks, true);
            piece.GetComponent<Outline>().OutlineColor = Color.green;
        }
        else
        {
            HighlightValidMoves(ValidMoves, blocks, false);
            piece.GetComponent<Outline>().OutlineColor = Color.red;
        }
    }
    void HighlightValidMoves(bool[,] validMoves, ChessBlockEditor[] blocks, bool isYourTurn)
    {
        foreach (var block in blocks)
        {
            if (validMoves[(int)block.transform.position.x, (int)block.transform.position.z])
            {
                GameObject highlightObject = block.transform.GetChild(6).gameObject;
                highlightObject.gameObject.SetActive(true);
                if (isYourTurn)
                    highlightObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                else
                    highlightObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
        }
    }
    void ResetVisuals()
    {
        UnhighlightValidMoves(blocks);
        DeOutlineObject();
    }    
    void UnhighlightValidMoves(ChessBlockEditor[] blocks)
    {
        foreach(var block in blocks)
        {
            GameObject highlightObject = block.transform.GetChild(6).gameObject;
            highlightObject.gameObject.SetActive(false);
        }
    }        
    void DeOutlineObject()
    {
        foreach(var p in activeChessPieces)
        {
            p.GetComponent<Outline>().OutlineColor = Color.black;
        }
    }    
}