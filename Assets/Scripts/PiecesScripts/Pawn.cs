using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    [SerializeField] Queen queenPrefab;

    public override bool[,] GetValidMoves(bool canCaptureAllies)
    {
        bool[,] returnedValue = new bool[8, 8];

        HandleDiagonalLeftMovement(ref returnedValue, canCaptureAllies);
        HandleDiagonalRightMovement(ref returnedValue, canCaptureAllies);
        HandleForwardMovement(ref returnedValue);
        HandleDoubleMovement(ref returnedValue);

        return returnedValue;
    }
    public bool[,] HandleEnPasse(bool[,] returnedValue)
    {
        ChessPiece c1;
        ChessPiece c2;

        if (this.PositionX == 0)
        {
            c1 = null;
            c2 = BoardManager.Instance.Pieces[PositionX + 1, PositionY];
        }
        else if (this.PositionX == 7)
        {
            c1 = BoardManager.Instance.Pieces[PositionX - 1, PositionY];
            c2 = null;
        }
        else
        {
            c1 = BoardManager.Instance.Pieces[PositionX - 1, PositionY];
            c2 = BoardManager.Instance.Pieces[PositionX + 1, PositionY];
        }

        if (c1 != null)
        {
            if(c1.IsWhite != this.IsWhite)
            {
                if (this.IsWhite)
                    returnedValue[c1.PositionX, c1.PositionY + 1] = true;
                else
                    returnedValue[c1.PositionX, c1.PositionY - 1] = true;
            }
        }
        else if(c2 != null)
        {
            if (c2.IsWhite != this.IsWhite)
            {
                if (this.IsWhite)
                    returnedValue[c2.PositionX, c2.PositionY + 1] = true;
                else
                    returnedValue[c2.PositionX, c2.PositionY - 1] = true;
            }
        }
        return returnedValue;
    }
    public bool CanAttack(int x, int y)
    {
        if(IsWhite)
        {
            if((x == PositionX - 1 || x == PositionX + 1) && y == PositionY + 1)
                return true;
            return false;
        }
        else
        {
            if ((x == PositionX - 1 || x == PositionX + 1) && y == PositionY - 1)
                return true;
            return false;
        }
    }
    public ChessPiece[,] PromoteToQueen(ChessPiece[,] pieces, List<ChessPiece> lista)
    {
        Queen queen = Instantiate<Queen>(queenPrefab, this.transform.position, this.transform.rotation);
        queen.PositionX = this.PositionX;
        queen.PositionY = this.PositionY;

        pieces[queen.PositionX, queen.PositionY] = queen;
        lista.Add(queen);
        gameObject.SetActive(false);
        return pieces;
    }

    void HandleDiagonalRightMovement(ref bool[,] returnedValue, bool canCaptureAllies)
    {
        if (IsWhite)
        {
            if (PositionX != 7 && PositionY != 7)
            {
                HandlePawnAttackMovement(1, 1, ref returnedValue, canCaptureAllies);
            }
        }

        if (!IsWhite)
        {
            if (PositionX != 0 && PositionY != 0)
            {
                HandlePawnAttackMovement(-1, -1, ref returnedValue, canCaptureAllies);
            }
        }
    }
    void HandleDiagonalLeftMovement(ref bool[,] returnedValue, bool canCaptureAllies)
    {
        if (IsWhite)
        {
            if (PositionY != 7)
            {
                HandlePawnAttackMovement(-1, 1, ref returnedValue, canCaptureAllies);
            }
        }

        if (!IsWhite)
        {
            if (PositionX != 7 && PositionY != 0)
            {
                HandlePawnAttackMovement(1, -1, ref returnedValue, canCaptureAllies);
            }
        }
    }
    void HandleForwardMovement(ref bool[,] returnedValue)
    {
        if (IsWhite)
        {
            if (PositionY != 7)
            {
                ChessPiece c1 = BoardManager.Instance.Pieces[PositionX, PositionY + 1];
                if (c1 == null)
                    returnedValue[PositionX, PositionY + 1] = true;
            }
        }

        if (!IsWhite)
        {
            if (PositionY != 0)
            {
                ChessPiece c1 = BoardManager.Instance.Pieces[PositionX, PositionY - 1];
                if (c1 == null)
                    returnedValue[PositionX, PositionY - 1] = true;
            }
        }
    }
    void HandleDoubleMovement(ref bool[,] returnedValue)
    {
        ChessPiece c1, c2;
        if (IsWhite)
        {
            if (PositionY == 1)
            {
                c1 = BoardManager.Instance.Pieces[PositionX, PositionY + 1];
                c2 = BoardManager.Instance.Pieces[PositionX, PositionY + 2];

                if (c1 == null && c2 == null)
                    returnedValue[PositionX, PositionY + 2] = true;
            }
        }

        if (!IsWhite)
        {
            if (PositionY == 6)
            {
                c1 = BoardManager.Instance.Pieces[PositionX, PositionY - 1];
                c2 = BoardManager.Instance.Pieces[PositionX, PositionY - 2];

                if (c1 == null && c2 == null)
                    returnedValue[PositionX, PositionY - 2] = true;
            }
        }
    }
    void HandlePawnAttackMovement(int x, int y, ref bool[,] returnedValue, bool canCaptureAllies)
    {
        if (PositionX + x >= 0 && PositionX + x < 8 && PositionY + y >= 0 && PositionY + y < 8)
        {
            ChessPiece c1 = BoardManager.Instance.Pieces[PositionX + x, PositionY + y];

            if (c1 != null)
            {
                if (c1.IsWhite != IsWhite && !canCaptureAllies)
                    returnedValue[PositionX + x, PositionY + y] = true;
                if (c1.IsWhite == IsWhite && canCaptureAllies)
                    returnedValue[PositionX + x, PositionY + y] = true;
            }

        }
    }
}
