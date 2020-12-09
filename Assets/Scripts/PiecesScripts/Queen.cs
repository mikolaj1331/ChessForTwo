using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : ChessPiece
{
    public override bool[,] GetValidMoves()
    {
        bool[,] returnedValue = new bool[8, 8];

        HandleRookRightMovement(ref returnedValue);
        HandleRookLeftMovement(ref returnedValue);
        HandleRookForwardMovement(ref returnedValue);
        HandleRookBackwardsMovement(ref returnedValue);

        //Top Left Movement
        HandleBishopMovement(-1, 1, ref returnedValue);
        //Top Right Movement
        HandleBishopMovement(1, 1, ref returnedValue);
        //Bottom Left Movement
        HandleBishopMovement(-1, -1, ref returnedValue);
        //Bottom Right Movement
        HandleBishopMovement(1, -1, ref returnedValue);

        return returnedValue;
    }
    void HandleRookRightMovement(ref bool[,] returnedValue)
    {
        for (int i = PositionX; i < 8; i++)
        {
            ChessPiece cp = BoardManager.Instance.Pieces[i, PositionY];
            if (cp == null)
                returnedValue[i, PositionY] = true;
            else
            {
                if (cp.IsWhite != IsWhite)
                    returnedValue[i, PositionY] = true;
            }
        }
    }
    void HandleRookLeftMovement(ref bool[,] returnedValue)
    {
        for (int i = PositionX; i >= 0; i--)
        {
            ChessPiece cp = BoardManager.Instance.Pieces[i, PositionY];
            if (cp == null)
                returnedValue[i, PositionY] = true;
            else
            {
                if (cp.IsWhite != IsWhite)
                    returnedValue[i, PositionY] = true;
            }
        }
    }
    void HandleRookForwardMovement(ref bool[,] returnedValue)
    {
        for (int i = PositionY; i < 8; i++)
        {
            ChessPiece cp = BoardManager.Instance.Pieces[PositionX, i];
            if (cp == null)
                returnedValue[PositionX, i] = true;
            else
            {
                if (cp.IsWhite != IsWhite)
                    returnedValue[PositionX, i] = true;
            }
        }
    }
    void HandleRookBackwardsMovement(ref bool[,] returnedValue)
    {
        for (int i = PositionY; i >= 0; i--)
        {
            ChessPiece cp = BoardManager.Instance.Pieces[PositionX, i];
            if (cp == null)
                returnedValue[PositionX, i] = true;
            else
            {
                if (cp.IsWhite != IsWhite)
                    returnedValue[PositionX, i] = true;
            }
        }
    }
    void HandleBishopMovement(int x, int y, ref bool[,] returnedValue)
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
