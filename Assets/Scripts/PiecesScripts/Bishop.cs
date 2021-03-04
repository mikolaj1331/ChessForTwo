using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override bool[,] GetValidMoves(bool canCaptureAllies)
    {
        bool[,] returnedValue = new bool[8, 8];

        //Top Left Movement
        HandleBishopMovement(-1, 1, ref returnedValue, canCaptureAllies);
        //Top Right Movement
        HandleBishopMovement(1, 1, ref returnedValue, canCaptureAllies);
        //Bottom Left Movement
        HandleBishopMovement(-1, -1, ref returnedValue, canCaptureAllies);
        //Bottom Right Movement
        HandleBishopMovement(1, -1, ref returnedValue, canCaptureAllies);

        return returnedValue;
    }

    void HandleBishopMovement(int x, int y, ref bool[,] returnedValue, bool canCaptureAllies)
    {
        int i = PositionX;
        int j = PositionY;

        while(true)
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
                if (cp.IsWhite != IsWhite && !canCaptureAllies)
                    returnedValue[i, j] = true;
                if (cp.IsWhite == IsWhite && canCaptureAllies)
                    returnedValue[i, j] = true;
                break;
            }
        }
    }
}
