using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public int PositionX {set; get;}
    public int PositionY { set; get; }
    public bool IsWhite;
    public abstract bool[,] GetValidMoves(bool canCaptureAllies);
    public virtual bool[,] FindInvalidMoves(bool[,] returnedValue, List<ChessPiece> chessPieces)
    {
        //foreach(ChessPiece cp in chessPieces)
        //{
        //    if (cp.IsWhite == this.IsWhite || cp.CompareTag("Pawn") || cp.CompareTag("Knight") || cp.CompareTag("Knight")) continue;
        //    var moves = cp.GetValidMoves(false);
        //    if(moves[PositionX,PositionY] == true)
        //    {

        //    }
        //}
        return new bool[8, 8];
    }
    public King GetAlliedKing()
    {
        var kings = FindObjectsOfType<King>();
        foreach (var k in kings)
        {
            if (k.IsWhite == IsWhite)
                return k;
        }
        return null;
    }
}
