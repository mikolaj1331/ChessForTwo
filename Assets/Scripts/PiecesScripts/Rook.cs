using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override bool[,] GetValidMoves(bool canCaptureAllies)
    {
        bool[,] returnedValue = new bool[8, 8];

        //Righ movement
        HandleOneDirectionLoopMovement(1, 0, ref returnedValue, canCaptureAllies, false) ;
        //Down movement
        HandleOneDirectionLoopMovement(0, -1, ref returnedValue, canCaptureAllies, false);
        //Left movement
        HandleOneDirectionLoopMovement(-1, 0, ref returnedValue, canCaptureAllies, false);
        //Right movement
        HandleOneDirectionLoopMovement(0, 1, ref returnedValue, canCaptureAllies, false);

        return returnedValue;
    } 
}