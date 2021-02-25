using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override bool[,] GetValidMoves()
    {
        bool[,] returnedValue = new bool[8,8];

        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                HandleKingMovement(i, j, ref returnedValue);
            }
        }

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
