using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override bool[,] GetValidMoves(bool canCaptureAllies)
    {
        bool[,] returnedValue = new bool[8, 8];

        //Righ movement
        HandleRookMovement(1, 0, ref returnedValue, canCaptureAllies, false) ;
        //Down movement
        HandleRookMovement(0, -1, ref returnedValue, canCaptureAllies, false);
        //Left movement
        HandleRookMovement(-1, 0, ref returnedValue, canCaptureAllies, false);
        //Right movement
        HandleRookMovement(0, 1, ref returnedValue, canCaptureAllies, false);

        return returnedValue;
    }
    void HandleRookMovement(int x, int y, ref bool[,] returnedValue, bool canCaptureAllies, bool canPassThroughObjects)
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
                if(cp.IsWhite == IsWhite && canCaptureAllies)
                    returnedValue[i, j] = true;
                if(!canPassThroughObjects)
                {
                    break;
                }
            }
        }
    }
}