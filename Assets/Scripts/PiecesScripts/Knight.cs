using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override bool[,] GetValidMoves(bool canCaptureAllies)
    {
        bool[,] returnedValue = new bool[8, 8];

        // LeftDown
        HandleKnightMovement(PositionX - 2, PositionY - 1, ref returnedValue, canCaptureAllies);
        // LeftUp
        HandleKnightMovement(PositionX - 2, PositionY + 1, ref returnedValue, canCaptureAllies);
        // DownLeft
        HandleKnightMovement(PositionX - 1, PositionY - 2, ref returnedValue, canCaptureAllies);
        // UpLeft
        HandleKnightMovement(PositionX - 1, PositionY + 2, ref returnedValue, canCaptureAllies);
        // DownRight
        HandleKnightMovement(PositionX + 1, PositionY - 2, ref returnedValue, canCaptureAllies);
        // UpRight
        HandleKnightMovement(PositionX + 1, PositionY + 2, ref returnedValue, canCaptureAllies);
        // RightDown
        HandleKnightMovement(PositionX + 2, PositionY - 1, ref returnedValue, canCaptureAllies);
        // RightUp
        HandleKnightMovement(PositionX + 2, PositionY + 1, ref returnedValue, canCaptureAllies);   

        return returnedValue;
    }

    public void HandleKnightMovement(int x, int y, ref bool[,] returnedValue, bool canCaptureAllies)
    {
        ChessPiece cp;
        if(x >= 0 && x < 8 && y >= 0 && y < 8)
        {
            cp = BoardManager.Instance.Pieces[x, y];
            if (cp == null)
                returnedValue[x, y] = true;
            else
            {
                if (cp.IsWhite != IsWhite && !canCaptureAllies)
                    returnedValue[x, y] = true;
                if (cp.IsWhite == IsWhite && canCaptureAllies)
                    returnedValue[x, y] = true;
            }
        }
    }
}
