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

    void HandleDiagonalRightMovement(ref bool[,] returnedValue, bool canCaptureAllies)
    {
        
        if (IsWhite)
        {
            if (PositionX != 7 && PositionY != 7)
            {
                HandlePawnAttackMovement(1, 1, ref returnedValue, canCaptureAllies);
            }
        }
        
        if(!IsWhite)
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
        
        if(!IsWhite)
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

        if(!IsWhite)
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

        if(!IsWhite)
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
        if(PositionX + x >= 0 && PositionX + x < 8 && PositionY + y >= 0 && PositionY + y < 8)
        {
            ChessPiece c1 = BoardManager.Instance.Pieces[PositionX + x, PositionY + y];

            if (c1 != null)
            {
                if(c1.IsWhite != IsWhite && !canCaptureAllies)
                    returnedValue[PositionX + x, PositionY + y] = true;
                if(c1.IsWhite == IsWhite && canCaptureAllies)
                    returnedValue[PositionX + x, PositionY + y] = true;
            }
                
        }        
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
        queen.IsWhite = this.IsWhite;
        pieces[queen.PositionX, queen.PositionY] = queen;
        lista.Add(queen);
        gameObject.SetActive(false);
        return pieces;
    }
}
