using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override bool[,] GetValidMoves(bool canCaptureAllies)
    {
        bool[,] returnedValue = new bool[8, 8];

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                HandleOneDirectionLoopMovement(i, j, ref returnedValue, canCaptureAllies, false);
            }
        }
        return returnedValue;
    }    
}
