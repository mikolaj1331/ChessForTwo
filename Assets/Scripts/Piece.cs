using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType { King, Queen, Rook, Knight, Bishop, Pawn};
public class Piece : MonoBehaviour
{
    [SerializeField] PieceType type;

    bool isWhite;
    public bool IsWhite { get => isWhite; set => isWhite = value; }
    public PieceType Type { get => type; set => type = value; }
}
