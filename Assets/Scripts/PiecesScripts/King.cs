using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override bool[,] GetValidMoves()
    {
        bool[,] returnedValue = new bool[8,8];
        //Front
        HandleKingMovement(-1, 1, ref returnedValue);
        HandleKingMovement(0, 1, ref returnedValue);
        HandleKingMovement(1, 1, ref returnedValue);
        //Sides
        HandleKingMovement(-1, 0, ref returnedValue);
        HandleKingMovement(1, 0, ref returnedValue);
        //Back
        HandleKingMovement(-1, -1, ref returnedValue);
        HandleKingMovement(0, -1, ref returnedValue);
        HandleKingMovement(1, -1, ref returnedValue);

        return returnedValue;
    }

    void HandleKingMovement(int x, int y, ref bool[,] returnedValue)
    {
        if(PositionX + x < 8 && PositionX + x >= 0 && PositionY + y < 8 && PositionY + y >= 0)
        {
            ChessPiece cp = BoardManager.Instance.Pieces[PositionX + x, PositionY + y];
            if (cp != null)
            {
                if (cp.IsWhite != IsWhite)
                {
                    returnedValue[PositionX + x, PositionY + y] = true;
                }
            }
            else
            {
                returnedValue[PositionX + x, PositionY + y] = true;
            }
        }
    }
}
