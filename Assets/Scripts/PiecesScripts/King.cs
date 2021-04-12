using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    List<ChessPiece> listOfDanger;
    bool isChecked = false;

    public List<ChessPiece> ListOfDanger { get => listOfDanger; set => listOfDanger = value; }
    public bool IsChecked { get => isChecked; set => isChecked = value; }

    private void Start()
    {
        ListOfDanger = new List<ChessPiece>();
    }
    public override bool[,] GetValidMoves(bool canCaptureAllies)
    {
        bool[,] returnedValue = new bool[8,8];

        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                HandleKingMovement(i, j, ref returnedValue, canCaptureAllies);
            }
        }

        HandleCastlingMovement(ref returnedValue, -1);
        HandleCastlingMovement(ref returnedValue, 1);

        return returnedValue;
    }

    bool[,] HandleCastlingMovement(ref bool[,] returnedValue, int direction)
    {
        if (hasMoved) return returnedValue;

        int boundary;
        if (direction == -1) boundary = 0;
        else if (direction == 1) boundary = 7;
        else return returnedValue;

        bool[,] moves = new bool[8, 8];
        HandleDirectionalLoopMovement(direction, 0, ref moves, false, false);
        if (moves[boundary, this.PositionY] != false || moves[boundary - direction, this.PositionY] != true) return returnedValue;

        ChessPiece rook = BoardManager.Instance.Pieces[boundary - direction, this.PositionY];
        if (rook != null) return returnedValue;

        rook = BoardManager.Instance.Pieces[boundary, this.PositionY];
        if (rook.hasMoved) return returnedValue;

        foreach (var piece in BoardManager.Instance.activeChessPieces)
        {
            if (piece.IsWhite == this.IsWhite || piece.CompareTag("King")) continue;

            var pieceMoves = piece.GetValidMoves(false);
            if (pieceMoves[this.PositionX + (direction * 2), this.PositionY] || pieceMoves[this.PositionX + direction, this.PositionY]) return returnedValue;
        }

        returnedValue[PositionX + (direction * 2), PositionY] = true;

        return returnedValue;
    }

    void HandleKingMovement(int directionX, int directionY, ref bool[,] returnedValue, bool canCaptureAllies)
    {
        if(PositionX + directionX < 8 && PositionX + directionX >= 0 && PositionY + directionY < 8 && PositionY + directionY >= 0)
        {
            ChessPiece cp = BoardManager.Instance.Pieces[PositionX + directionX, PositionY + directionY];
            if (cp != null)
            {
                if (cp.IsWhite != IsWhite)
                    returnedValue[PositionX + directionX, PositionY + directionY] = true;
                if (cp.IsWhite == IsWhite && canCaptureAllies)
                    returnedValue[PositionX + directionX, PositionY + directionY] = true;
            }
            else
            {
                returnedValue[PositionX + directionX, PositionY + directionY] = true;
            }
        }
    }

    public override bool[,] FindInvalidMoves(bool[,] returnedValue)
    {
        foreach (var pi in BoardManager.Instance.activeChessPieces)
        {
            if (pi.IsWhite == this.IsWhite) continue;
            var moves = pi.GetValidMoves(true);
            
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(!pi.CompareTag("Pawn"))
                    {
                        if (returnedValue[i, j] && moves[i, j])
                        {
                            returnedValue[i, j] = false;
                        }
                    }
                    else
                    {
                        if(returnedValue[i,j] && pi.GetComponent<Pawn>().CanAttack(i, j))
                        {
                            returnedValue[i, j] = false;
                        }
                    }
                }
            }
        }
        return returnedValue;
    }

    public override bool[,] HandleKingCheckedMoves(int count, bool[,] returnedValue)
    {
        if (count == 0) return returnedValue;

        var moves = returnedValue;
        for (int k = 0; k < listOfDanger.Count; k++)
        {
            ChessPiece attacker = this.ListOfDanger[k];                
            var attackerMoves = attacker.GetShortestPath(attacker, this, true);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (attackerMoves[i, j] && returnedValue[i, j])
                        moves[i, j] = false;
                }
            }
        }
        return returnedValue;
    }
}
