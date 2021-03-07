using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : ChessPiece
{
    public override bool[,] GetValidMoves(bool canCaptureAllies)
    {
        bool[,] returnedValue = new bool[8, 8];

        //Top Left Movement
        HandleOneDirectionLoopMovement(-1, 1, ref returnedValue, canCaptureAllies, false);
        //Top Right Movement
        HandleOneDirectionLoopMovement(1, 1, ref returnedValue, canCaptureAllies, false);
        //Bottom Left Movement
        HandleOneDirectionLoopMovement(-1, -1, ref returnedValue, canCaptureAllies, false);
        //Bottom Right Movement
        HandleOneDirectionLoopMovement(1, -1, ref returnedValue, canCaptureAllies, false);

        return returnedValue;
    }
}
