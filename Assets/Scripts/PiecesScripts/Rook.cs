﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override bool[,] GetValidMoves()
    {
        bool[,] returnedValue = new bool[8, 8];

        //Righ movement
        HandleRookMovement(1, 0, ref returnedValue);
        //Down movement
        HandleRookMovement(0, -1, ref returnedValue);
        //Left movement
        HandleRookMovement(-1, 0, ref returnedValue);
        //Right movement
        HandleRookMovement(0, 1, ref returnedValue);

        return returnedValue;
    }

    void HandleRookMovement(int x, int y, ref bool[,] returnedValue)
    {
        int i = PositionX;
        int j = PositionY;

        while (true)
        {
            i += x;
            j += y;

            if (i < 0 || i >= 8 || j < 0 || j >= 8)
                break;
            ChessPiece cp = BoardManager.Instance.Pieces[i, j];
            if (cp == null)
                returnedValue[i, j] = true;
            else
            {
                if (cp.IsWhite != IsWhite)
                    returnedValue[i, j] = true;
                break;
            }
        }
    }
}