using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override bool[,] GetValidMoves()
    {
        bool[,] returnedValue = new bool[8, 8];

        // LeftDown
        HandleKnightMovement(PositionX - 2, PositionY - 1, ref returnedValue);
        // LeftUp
        HandleKnightMovement(PositionX - 2, PositionY + 1, ref returnedValue);
        // DownLeft
        HandleKnightMovement(PositionX - 1, PositionY - 2, ref returnedValue);
        // UpLeft
        HandleKnightMovement(PositionX - 1, PositionY + 2, ref returnedValue);
        // DownRight
        HandleKnightMovement(PositionX + 1, PositionY - 2, ref returnedValue);
        // UpRight
        HandleKnightMovement(PositionX + 1, PositionY + 2, ref returnedValue);
        // RightDown
        HandleKnightMovement(PositionX + 2, PositionY - 1, ref returnedValue);
        // RightUp
        HandleKnightMovement(PositionX + 2, PositionY + 1, ref returnedValue);   

        return returnedValue;
    }

    public void HandleKnightMovement(int x, int y, ref bool[,] returnedValue)
    {
        ChessPiece cp;
        if(x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            cp = BoardManager.Instance.Pieces[x, y];
            if (cp == null)
                returnedValue[x, y] = true;
            else if (cp.IsWhite != IsWhite)
                returnedValue[x, y] = true;
        }
    }
}
