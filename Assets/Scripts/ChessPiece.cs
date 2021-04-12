using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ChessPiece : MonoBehaviour
{
    public int PositionX {set; get;}
    public int PositionY { set; get; }
    public bool IsWhite;
    public bool hasMoved = false;
    
    public abstract bool[,] GetValidMoves(bool canCaptureAllies);
    public virtual bool[,] FindInvalidMoves(bool[,] returnedValue)
    {
        King myKing = GetAlliedKing(this.IsWhite);
        foreach (ChessPiece cp in BoardManager.Instance.activeChessPieces)
        {
            if (cp.IsWhite == this.IsWhite || cp.CompareTag("Pawn") || cp.CompareTag("Knight") || cp.CompareTag("King")) continue;
            var moves = cp.GetValidMoves(false);
            var myMoves = this.GetValidMoves(false);
            if (moves[PositionX, PositionY])
            {
                bool[,] shortestPath = GetShortestPath(cp, this, true);

                if (shortestPath[myKing.PositionX, myKing.PositionY])
                {
                    List<ChessPiece> listOfPiecesOnThePath = new List<ChessPiece>();
                    List<Vector2Int> listOfPositions = new List<Vector2Int>();

                    FindTheChessPiecesOnThePath(cp, listOfPiecesOnThePath, listOfPositions);

                    if (listOfPiecesOnThePath[0] == this && listOfPiecesOnThePath[1] == myKing)
                    {
                        Array.Clear(returnedValue, 0, 64);
                        if (myMoves[cp.PositionX, cp.PositionY])
                            returnedValue[cp.PositionX, cp.PositionY] = true;
                        foreach (var vector in listOfPositions)
                        {
                            if (shortestPath[vector.x, vector.y] && myMoves[vector.x, vector.y])
                                returnedValue[vector.x, vector.y] = true;
                        }
                    }
                }
            }
        }
        return returnedValue;
    }  
    public virtual bool[,] HandleKingCheckedMoves(int count, bool[,] returnedValue)
    {
        King myKing = GetAlliedKing(this.IsWhite);

        if (count == 0) return returnedValue;        
        if (count == 1)
        {
            ChessPiece attacker = myKing.ListOfDanger[0];
            var moves = new bool[8, 8];
            var attackerMoves = attacker.GetShortestPath(attacker, myKing, false);

            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if (attackerMoves[i, j] && returnedValue[i, j])
                        moves[i, j] = true;
                    else if (returnedValue[attacker.PositionX, attacker.PositionY])
                        moves[attacker.PositionX, attacker.PositionY] = true;
                    else
                        moves[i, j] = false;
                }
            }          
            return moves;
        }
        else if(count > 1)
        {
            Array.Clear(returnedValue, 0, 64);
        }
        return returnedValue;
    }
    public King GetAlliedKing(bool isWhite)
    {
        var kings = FindObjectsOfType<King>();
        foreach (var k in kings)
        {
            if (k.IsWhite == isWhite)
                return k;
        }
        return null;
    }
    public bool CapturedPiece(ChessPiece chessPiece)
    {
        if (chessPiece != null)
        {
            BoardManager.Instance.activeChessPieces.Remove(chessPiece);
            chessPiece.gameObject.SetActive(false);
            return true;
        }
        return false;
    }
    protected void HandleDirectionalLoopMovement(int directionX, int directionY, ref bool[,] returnedValue, bool canCaptureAllies, bool canPassThroughObjects)
    {
        int i = PositionX;
        int j = PositionY;

        while (true)
        {
            i += directionX;
            j += directionY;

            if (i < 0 || i >= 8 || j < 0 || j >= 8)
                break;
            ChessPiece cp = BoardManager.Instance.Pieces[i, j];
            if (cp == null)
                returnedValue[i, j] = true;
            else
            {
                if (cp.IsWhite != IsWhite && !canCaptureAllies)
                    returnedValue[i, j] = true;
                if (cp.IsWhite == IsWhite && canCaptureAllies && cp != this)
                    returnedValue[i, j] = true;
                if (!canPassThroughObjects)
                {
                    break;
                }
            }
        }
    }
    public bool[,] GetShortestPath(ChessPiece attacker, ChessPiece defender, bool canPassThroughObjects)
    {
        if (attacker.CompareTag("Pawn") || attacker.CompareTag("Knight") || attacker.CompareTag("King")) return new bool[8, 8];
        Vector2Int direction = CalcualteDirection(attacker, defender);
        bool[,] shortestPath = new bool[8, 8];
        attacker.HandleDirectionalLoopMovement(direction.x, direction.y, ref shortestPath, false, canPassThroughObjects);
        return shortestPath;
    }
    
    Vector2Int CalcualteDirection(ChessPiece attacker, ChessPiece defender)
    {
        int directionX = (defender.PositionX - attacker.PositionX);
        int directionY = (defender.PositionY - attacker.PositionY);

        if (directionX != 0)
            directionX /= Mathf.Abs((defender.PositionX - attacker.PositionX));
        if (directionY != 0)
            directionY /= Mathf.Abs((defender.PositionY - attacker.PositionY));

        return new Vector2Int(directionX, directionY);
    }
    void FindTheChessPiecesOnThePath(ChessPiece attacker, List<ChessPiece> listOfPiecesOnThePath, List<Vector2Int> listOfPositions)
    {
        Vector2Int direction = CalcualteDirection(attacker, this);

        int k = attacker.PositionX + direction.x, l = attacker.PositionY + direction.y;
        do
        {
            if (BoardManager.Instance.Pieces[k, l] != null)
            {
                listOfPiecesOnThePath.Add(BoardManager.Instance.Pieces[k, l]);
            }
            listOfPositions.Add(new Vector2Int(k, l));
            k += direction.x;
            l += direction.y;
        } while ((k >= 0 && k < 8) && (l >= 0 && l < 8));
    }
}
