using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    List<ChessPiece> listOfDanger;
    bool isChecked = false;

    public List<ChessPiece> ListOfDanger { get => listOfDanger; set => listOfDanger = value; }
    public bool IsChecked { get => isChecked; set => isChecked = value; }

    private void Start()
    {
        ListOfDanger = new List<ChessPiece>();
    }
    public override bool[,] GetValidMoves(bool canCaptureAllies)
    {
        bool[,] returnedValue = new bool[8,8];

        for(int i = -1; i < 2; i++)
        {
            for(int j = -1; j < 2; j++)
            {
                HandleKingMovement(i, j, ref returnedValue, canCaptureAllies);
            }
        }
        return returnedValue;
    }

    void HandleKingMovement(int x, int y, ref bool[,] returnedValue, bool canCaptureAllies)
    {
        if(PositionX + x < 8 && PositionX + x >= 0 && PositionY + y < 8 && PositionY + y >= 0)
        {
            ChessPiece cp = BoardManager.Instance.Pieces[PositionX + x, PositionY + y];
            if (cp != null)
            {
                if (cp.IsWhite != IsWhite)
                    returnedValue[PositionX + x, PositionY + y] = true;
                if (cp.IsWhite == IsWhite && canCaptureAllies)
                    returnedValue[PositionX + x, PositionY + y] = true;
            }
            else
            {
                returnedValue[PositionX + x, PositionY + y] = true;
            }
        }
    }

    public override bool[,] FindInvalidMoves(bool[,] returnedValue, List<ChessPiece> chessPieces)
    {
        foreach (var pi in chessPieces)
        {
            if (pi.IsWhite == this.IsWhite) continue;
            var moves = pi.GetValidMoves(true);
            
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(!pi.CompareTag("Pawn"))
                    {
                        if (returnedValue[i, j] && moves[i, j])
                        {
                            returnedValue[i, j] = false;
                        }
                    }
                    else
                    {
                        if(returnedValue[i,j] && pi.GetComponent<Pawn>().CanAttack(i, j))
                        {
                            returnedValue[i, j] = false;
                        }
                    }
                }
            }
        }
        return returnedValue;
    }


    public override bool[,] HandleKingChecked(int count, bool[,] returnedValue)
    {
        if (count == 0) return returnedValue;

        var moves = returnedValue;
        for (int k = 0; k < listOfDanger.Count; k++)
        {
            ChessPiece attacker = this.ListOfDanger[k];                
            var attackerMoves = attacker.GetShortestPath(attacker, this, true);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (attackerMoves[i, j] && returnedValue[i, j])
                        moves[i, j] = false;
                }
            }
        }
        return returnedValue;
    }
}
