using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override bool[,] GetValidMoves()
    {
        bool[,] returnedValue = new bool[8, 8];
        ChessPiece cp;

        HandleRookRightMovement(ref returnedValue);
        HandleRookLeftMovement(ref returnedValue);
        HandleRookForwardMovement(ref returnedValue);
        HandleRookBackwardsMovement(ref returnedValue);

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
}