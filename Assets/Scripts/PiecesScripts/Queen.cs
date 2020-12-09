using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override bool[,] GetValidMoves()
    {
        bool[,] returnedValue = new bool[8, 8];

        //Forward Left Movement
        HandleOneDirectionLoopMovement(-1, 1, ref returnedValue);
        //Forward Right Movement
        HandleOneDirectionLoopMovement(1, 1, ref returnedValue);
        //Backwards Left Movement
        HandleOneDirectionLoopMovement(-1, -1, ref returnedValue);
        //Backwards Right Movement
        HandleOneDirectionLoopMovement(1, -1, ref returnedValue);
        //Right Movement
        HandleOneDirectionLoopMovement(1, 0, ref returnedValue);
        //Backwards Movement
        HandleOneDirectionLoopMovement(0, -1, ref returnedValue);
        //Left Movement
        HandleOneDirectionLoopMovement(-1, 0, ref returnedValue);
        //Forward Movement
        HandleOneDirectionLoopMovement(0, 1, ref returnedValue);

        return returnedValue;
    }
    void HandleOneDirectionLoopMovement(int x, int y, ref bool[,] returnedValue)
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
