using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
   public override bool[,] GetValidMoves()
    {
        bool[,] returnedValue = new bool[8, 8];

        ChessPiece c1, c2;

        if(IsWhite)
        {
            // Diagonal left
            if(PositionX != 0 && PositionY != 7)
            {
                c1 = BoardManager.Instance.Pieces[PositionX - 1, PositionY + 1];
                if (c1 != null && !c1.IsWhite)
                    returnedValue[PositionX - 1, PositionY + 1] = true;
            }
            // Diagonal right
            if (PositionX != 7 && PositionY != 7)
            {
                c1 = BoardManager.Instance.Pieces[PositionX + 1, PositionY + 1];
                if (c1 != null && !c1.IsWhite)
                    returnedValue[PositionX + 1, PositionY + 1] = true;
            }
            // Middle
            if(PositionY != 7)
            {
                c1 = BoardManager.Instance.Pieces[PositionX, PositionY + 1];
                if(c1 == null)
                {
                    returnedValue[PositionX, PositionY + 1] = true;
                }
            }
            // Middle on first move (special move)
            if(PositionY == 1)
            {
                //c1 = BoardManager.Instance.Pieces[PositionX, PositionY + 1];
                c2 = BoardManager.Instance.Pieces[PositionX, PositionY + 2];

                if (c2 == null)
                    returnedValue[PositionX, PositionY + 2] = true;
            }
        }
        else
        {
            // Diagonal left movement
            if (PositionX != 0 && PositionY != 0)
            {
                c1 = BoardManager.Instance.Pieces[PositionX - 1, PositionY - 1];
                if (c1 != null && c1.IsWhite)
                    returnedValue[PositionX - 1, PositionY - 1] = true;
            }
            // Diagonal right movement
            if (PositionX != 7 && PositionY != 0)
            {
                c1 = BoardManager.Instance.Pieces[PositionX + 1, PositionY - 1];
                if (c1 != null && c1.IsWhite)
                    returnedValue[PositionX + 1, PositionY - 1] = true;
            }
            // Middle forward movement
            if (PositionY != 0)
            {
                c1 = BoardManager.Instance.Pieces[PositionX, PositionY - 1];
                if (c1 == null)
                {
                    returnedValue[PositionX, PositionY - 1] = true;
                }
            }
            // Middle on first move (special movement)
            if (PositionY == 6)
            {
                c1 = BoardManager.Instance.Pieces[PositionX, PositionY - 1];
                c2 = BoardManager.Instance.Pieces[PositionX, PositionY - 2];

                if (c1 == null && c2 == null)
                    returnedValue[PositionX, PositionY - 2] = true;
            }
        }

        return returnedValue;
    }
}
