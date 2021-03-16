using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    public List<ChessPiece> activeChessPieces;
    public ChessPiece[,] Pieces { set; get; }
    public ChessPiece selectedChessPiece;
    public int turn;

    bool[,] ValidMoves { set; get; }
    ChessBlockEditor[] blocks;
    MatchLogger logger;

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
    public bool CheckIfNotCheckmate(bool isWhiteTurn)
    {
        bool returnedValue = true;
        var kings = FindObjectsOfType<King>();

        foreach (var k in kings)
        {
            if (k.IsWhite == isWhiteTurn) continue;

            returnedValue = CheckIfAnyPossibleMovesValid(k);
            if (returnedValue) return returnedValue;

            if (k.IsChecked)
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
        if (returnedValue == false)
        {
            MoveLogger lastMove = logger.GetLastMove();
            logger.EditLog(lastMove, lastMove.MoveType, false, true);
        }

        return returnedValue;
    }
    public bool MoveChessPiece(int x, int y, bool isWhiteTurn)
    {
        if (selectedChessPiece.IsWhite != isWhiteTurn) return false;
        if (ValidMoves[x, y])
        {            
            if (selectedChessPiece.CapturedPiece(Pieces[x, y]))
            {
                ChessPiece target = Pieces[x, y];
                logger.LogMovement(MoveType.Move, selectedChessPiece, target, x, y, turn, true, false, false);
                King king = target.GetAlliedKing(!target.IsWhite);
                if (king.ListOfDanger.Contains(target)) king.ListOfDanger.Remove(target);
            }
            else
            {
                logger.LogMovement(MoveType.Move, selectedChessPiece, null, x, y, turn, false, false, false);
            }

            ProcessMovement(x, y, selectedChessPiece);
            GameLogicChecks();
            ResetVisuals();

            selectedChessPiece = null;

            return true;
        }
        else
            return false;
    }

    void Start()
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
        ChessPiece[,] pieces = new ChessPiece[8, 8];
        var ChessPieces = FindObjectsOfType<ChessPiece>();
        for (int i = 0; i < ChessPieces.Length; i++)
        {
            var tmp = ChessPieces[i];
            tmp.PositionX = (int)tmp.transform.position.x;
            tmp.PositionY = (int)tmp.transform.position.z;
            pieces[tmp.PositionX, tmp.PositionY] = tmp;
            activeChessPieces.Add(tmp);
        }
        return pieces;
    }
    bool[,] GetActualValidMoves(ChessPiece piece)
    {
        ValidMoves = piece.GetValidMoves(false);
        ValidMoves = piece.FindInvalidMoves(ValidMoves);
        ValidMoves = IsAlliedKingChecked(piece, ValidMoves);

        if (piece.CompareTag("Pawn"))
        {
            if (logger.GetMatchLogLength() != 0)
            {
                MoveLogger lastMove = logger.GetLastMove();
                bool neighbour = (lastMove.ChessPiece.PositionX == piece.PositionX - 1 || lastMove.ChessPiece.PositionX == piece.PositionX + 1);
                if (lastMove.ChessPiece.CompareTag("Pawn") && Mathf.Abs(lastMove.DestinationPos.y - lastMove.StartingPos.y) == 2 && neighbour)
                {
                    ValidMoves = piece.GetComponent<Pawn>().HandleEnPasse(ValidMoves);
                }
            }
        }
        return ValidMoves;
    }
    bool[,] IsAlliedKingChecked(ChessPiece piece, bool[,] validMoves)
    {
        King king = piece.GetAlliedKing(piece.IsWhite);
        List<ChessPiece> list = king.ListOfDanger;
        if (list.Count > 0)
            validMoves = piece.HandleKingCheckedMoves(list.Count, validMoves);
        else
            king.IsChecked = false;

        return validMoves;
    }
    void GameLogicChecks()
    {
        CheckForPromotion();
        CheckIfCanCaptureEnemyKing();
        CheckForEnPassant();
        CheckForCastling();
    }
    void CheckForCastling()
    {
        if(selectedChessPiece.CompareTag("King"))
        {
            MoveLogger lastMove = logger.GetLastMove();
            float direction = lastMove.DestinationPos.x - lastMove.StartingPos.x;
            float absoluteDirection = Mathf.Abs(direction);

            if (absoluteDirection == 2)
            {
                ChessPiece rook;
                if (lastMove.DestinationPos.x == 2)
                {
                    rook = Pieces[0, selectedChessPiece.PositionY];
                    logger.EditLog(lastMove, MoveType.QueenSideCastling, lastMove.IsCheck, lastMove.IsCheckmate);
                }
                else if (lastMove.DestinationPos.x == 6)
                {
                    rook = Pieces[7, selectedChessPiece.PositionY];
                    logger.EditLog(lastMove, MoveType.KingSideCastling, lastMove.IsCheck, lastMove.IsCheckmate);
                }
                else
                    return;

                UpdateLoggerIfCheckMove(lastMove, rook);

                direction /= absoluteDirection;
                ProcessMovement((int)selectedChessPiece.PositionX - (int)direction, selectedChessPiece.PositionY, rook);
            }
        }
    }    
    void CheckForEnPassant()
    {
        if (selectedChessPiece.CompareTag("Pawn"))
        {
            MoveLogger lastMove = logger.GetLastMove();

            if (lastMove.DestinationPos.x != lastMove.StartingPos.x && lastMove.CapturedChessPiece == null)
            {
                ChessPiece target;
                if (lastMove.ChessPiece.IsWhite)
                {
                    target = Pieces[lastMove.ChessPiece.PositionX, lastMove.ChessPiece.PositionY - 1];
                    lastMove.ChessPiece.CapturedPiece(target);
                }
                else
                {
                    target = Pieces[lastMove.ChessPiece.PositionX, lastMove.ChessPiece.PositionY + 1];
                    lastMove.ChessPiece.CapturedPiece(target);
                }

                logger.EditLog(lastMove, MoveType.EnPassant, target, lastMove.IsCheck, lastMove.IsCheckmate);
                UpdateLoggerIfCheckMove(lastMove,selectedChessPiece);               
            }
        }
    }
    bool CheckIfAnyPossibleMovesValid(ChessPiece chessPiece)
    {
        var moves = GetActualValidMoves(chessPiece);
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 8; j++)
                if (moves[i, j]) return true;
        return false;
    }
    void CheckIfCanCaptureEnemyKing()
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

                        MoveLogger lastMove = logger.GetLastMove();
                        logger.EditLog(lastMove, lastMove.MoveType,true,lastMove.IsCheckmate);

                        return;
                    }
                }
                if (k.ListOfDanger.Contains(ch))
                    k.ListOfDanger.Remove(ch);
            }
        }
    }
    void CheckForPromotion()
    {
        if(selectedChessPiece.CompareTag("Pawn"))
        {
            if ((selectedChessPiece.IsWhite && selectedChessPiece.PositionY == 7) || (!selectedChessPiece.IsWhite && selectedChessPiece.PositionY == 0))
            {
                selectedChessPiece.GetComponent<Pawn>().PromotePawn();

                MoveLogger lastMove = logger.GetLastMove();
                logger.EditLog(lastMove, MoveType.PawnPromotion, lastMove.IsCheck, lastMove.IsCheckmate);
                UpdateLoggerIfCheckMove(lastMove,selectedChessPiece);
            }
        }
        return;
    }
    void UpdateLoggerIfCheckMove(MoveLogger lastMove, ChessPiece chessPieceThatChecks)
    {
        var moves = chessPieceThatChecks.GetValidMoves(false);
        King enemyKing = chessPieceThatChecks.GetAlliedKing(!chessPieceThatChecks.IsWhite);
        if (moves[enemyKing.PositionX, enemyKing.PositionY])
            logger.EditLog(lastMove, lastMove.MoveType, true, lastMove.IsCheckmate);
    }
    void ProcessMovement(int destinationX, int destinationY, ChessPiece selectedChessPiece)
    {
        Pieces[selectedChessPiece.PositionX, selectedChessPiece.PositionY] = null;
        selectedChessPiece.transform.position = new Vector3(destinationX, selectedChessPiece.transform.position.y, destinationY);
        selectedChessPiece.PositionX = destinationX;
        selectedChessPiece.PositionY = destinationY;
        Pieces[destinationX, destinationY] = selectedChessPiece;
        selectedChessPiece.hasMoved = true;
    }
    void ProcessVisuals(bool turn, ChessPiece piece)
    {
        if (turn == piece.IsWhite)
        {
            HighlightValidMoves(true);
            piece.GetComponent<Outline>().OutlineColor = Color.green;
        }
        else
        {
            HighlightValidMoves(false);
            piece.GetComponent<Outline>().OutlineColor = Color.red;
        }
    }
    void HighlightValidMoves(bool isYourTurn)
    {
        foreach (var block in blocks)
        {
            if (block.isBorder) continue;
            if (ValidMoves[(int)block.transform.position.x, (int)block.transform.position.z])
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
        UnhighlightValidMoves();
        DeOutlineObject();
    }    
    void UnhighlightValidMoves()
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