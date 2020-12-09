﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
   public override bool[,] GetValidMoves()
    {
        bool[,] returnedValue = new bool[8, 8];

        HandleDiagonalLeftMovement(ref returnedValue);
        HandleDiagonalRightMovement(ref returnedValue);
        HandleForwardMovement(ref returnedValue);
        HandleSpecialMovement(ref returnedValue);

        return returnedValue;
    }

    void HandleDiagonalRightMovement(ref bool[,] returnedValue)
    {
        
        if (IsWhite)
        {
            if (PositionX != 7 && PositionY != 7)
            {
                HandlePawnAttackMovement(1, 1, ref returnedValue);
            }
        }
        else
        {
            if (PositionX != 0 && PositionY != 0)
            {
                HandlePawnAttackMovement(-1, -1, ref returnedValue);
            }
        }
    }
    void HandleDiagonalLeftMovement(ref bool[,] returnedValue)
    {
        if (IsWhite)
        {
            if (PositionY != 7)
            {
                HandlePawnAttackMovement(-1, 1, ref returnedValue);
            }
        }
        else
        {
            if (PositionX != 7 && PositionY != 0)
            {
                HandlePawnAttackMovement(1, -1, ref returnedValue);
            }
        }
    }
    void HandleForwardMovement(ref bool[,] returnedValue)
    {
        if (IsWhite)
        {
            if (PositionY != 7)
            {
                ChessPiece c1 = BoardManager.Instance.Pieces[PositionX, PositionY + 1];
                if (c1 == null)
                    returnedValue[PositionX, PositionY + 1] = true;
            }
        }
        else
        {
            if (PositionY != 0)
            {
                ChessPiece c1 = BoardManager.Instance.Pieces[PositionX, PositionY - 1];
                if (c1 == null)
                    returnedValue[PositionX, PositionY - 1] = true;
            }
        }
    }
    void HandleSpecialMovement(ref bool[,] returnedValue)
    {
        ChessPiece c1;
        ChessPiece c2;
        if (IsWhite)
        {
            if (PositionY == 1)
            {
                c1 = BoardManager.Instance.Pieces[PositionX, PositionY + 1];
                c2 = BoardManager.Instance.Pieces[PositionX, PositionY + 2];

                if (c1 == null && c2 == null)
                    returnedValue[PositionX, PositionY + 2] = true;
            }
        }
        else
        {
            if (PositionY == 6)
            {
                c1 = BoardManager.Instance.Pieces[PositionX, PositionY - 1];
                c2 = BoardManager.Instance.Pieces[PositionX, PositionY - 2];

                if (c1 == null && c2 == null)
                    returnedValue[PositionX, PositionY - 2] = true;
            }
        }
    }
    void HandlePawnAttackMovement(int x, int y, ref bool[,] returnedValue)
    {
        ChessPiece c1 = BoardManager.Instance.Pieces[PositionX + x, PositionY + y];
        
        if (c1 != null && c1.IsWhite != IsWhite)
            returnedValue[PositionX + x, PositionY + y] = true;
    }
}