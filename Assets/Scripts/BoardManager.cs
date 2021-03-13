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
    ChessPiece[,] InitialSetup()
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

        ValidMoves = GetActualValidMoves(piece);
        ProcessVisuals(isWhiteTurn, piece);
        selectedChessPiece = piece;
    }
    bool[,] GetActualValidMoves(ChessPiece piece)
    {
        ValidMoves = piece.GetValidMoves(false);
        ValidMoves = piece.FindInvalidMoves(ValidMoves, activeChessPieces);
        ValidMoves = IsAlliedKingChecked(piece, ValidMoves);

        return ValidMoves;
    }
    bool[,] IsAlliedKingChecked(ChessPiece piece, bool[,] validMoves)
    {
        King king = piece.GetAlliedKing(piece.IsWhite);
        List<ChessPiece> list = king.ListOfDanger;
        if (list.Count > 0)
            validMoves = piece.HandleKingChecked(list.Count, validMoves);
        else
            king.IsChecked = false;

        return validMoves;
    }
    public bool MoveChessPiece(int x, int y, bool isWhiteTurn)
    {
        if (selectedChessPiece.IsWhite != isWhiteTurn) return false;
        if (ValidMoves[x, y])
        {
            if (Pieces[x, y] != null)
            {
                ChessPiece target = Pieces[x, y];
                King king = target.GetAlliedKing(!target.IsWhite);
                if (king.ListOfDanger.Contains(target)) king.ListOfDanger.Remove(target);
                target.gameObject.SetActive(false);
                activeChessPieces.Remove(target);
            }
            logger.LogMovement(selectedChessPiece, x, y, turn);
            ProcessMovement(x, y);
            CheckForPromotion(selectedChessPiece);
            CheckIfCanCaptureEnemyKing(activeChessPieces, selectedChessPiece);
            ResetVisuals();
            selectedChessPiece = null;
            return true;
        }
        else
            return false;
    }
    public bool CheckIfNotCheckmate(bool isWhiteTurn)
    {
        bool returnedValue = true;
        var kings = FindObjectsOfType<King>();

        foreach(var k in kings)
        {
            if (k.IsWhite == isWhiteTurn) continue;

            returnedValue = CheckIfAnyPossibleMovesValid(k);
            if (returnedValue) return returnedValue;

            if(k.IsChecked)
            {
                foreach (var ch in activeChessPieces)
                {
                    if (ch.IsWhite == isWhiteTurn || ch.CompareTag("King")) continue;
                    returnedValue = CheckIfAnyPossibleMovesValid(ch);
                    if (returnedValue) return returnedValue;
                }
            }   
            else
            {
                returnedValue = true;
            }
        }
        return returnedValue;
    }
    bool CheckIfAnyPossibleMovesValid(ChessPiece chessPiece)
    {
        var moves = GetActualValidMoves(chessPiece);
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (moves[i, j]) return true;
        return false;
    }
    void CheckIfCanCaptureEnemyKing(List<ChessPiece> activeChessPieces, ChessPiece selectedChessPiece)
    {
        var kings = FindObjectsOfType<King>();

       foreach(var k in kings)
        {
            foreach (var ch in activeChessPieces)
            {
                if (ch.IsWhite == selectedChessPiece.IsWhite)
                {
                    var moves = ch.GetValidMoves(false);
                    if (moves[k.PositionX, k.PositionY])
                    {
                        k.ListOfDanger.Add(ch);
                        k.IsChecked = true;
                        return;
                    }
                }
                if (k.ListOfDanger.Contains(ch))
                    k.ListOfDanger.Remove(ch);
            }
        }
    }
    void CheckForPromotion(ChessPiece selectedChessPiece)
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
            if (block.isBorder) continue;
            if (validMoves[(int)block.transform.position.x, (int)block.transform.position.z])
            {
                GameObject highlightObject = block.transform.GetChild(6).gameObject;
                highlightObject.gameObject.SetActive(true);
                if (isYourTurn)
                    highlightObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                else
                    highlightObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                if(Pieces[(int)block.transform.position.x, (int)block.transform.position.z])
                {
                    highlightObject.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
                    highlightObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                }
                else
                    highlightObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
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
            if (block.isBorder) continue;
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