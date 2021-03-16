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

    public void HandleKnightMovement(int directionX, int directionY, ref bool[,] returnedValue, bool canCaptureAllies)
    {
        ChessPiece cp;
        if(directionX >= 0 && directionX < 8 && directionY >= 0 && directionY < 8)
        {
            cp = BoardManager.Instance.Pieces[directionX, directionY];
            if (cp == null)
                returnedValue[directionX, directionY] = true;
            else
            {
                if (cp.IsWhite != IsWhite && !canCaptureAllies)
                    returnedValue[directionX, directionY] = true;
                if (cp.IsWhite == IsWhite && canCaptureAllies)
                    returnedValue[directionX, directionY] = true;
            }
        }
    }
}
