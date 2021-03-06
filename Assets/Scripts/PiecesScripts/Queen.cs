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
    void HandleOneDirectionLoopMovement(int x, int y, ref bool[,] returnedValue, bool canCaptureAllies, bool canPassThroughObjects)
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
                if (cp.IsWhite != IsWhite && !canCaptureAllies)
                    returnedValue[i, j] = true;
                if (cp.IsWhite == IsWhite && canCaptureAllies && cp != this)
                    returnedValue[i, j] = true;
                if(!canPassThroughObjects)
                {
                    break;
                }
                
            }
        }
    }
}
