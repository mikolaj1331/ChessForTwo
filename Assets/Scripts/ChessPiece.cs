using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessPiece : MonoBehaviour
{
    public int PositionX {set; get;}
    public int PositionY { set; get; }
    public bool IsWhite;


    public virtual bool[,] GetValidMoves()
    {
        return new bool[8,8];
    }
}
