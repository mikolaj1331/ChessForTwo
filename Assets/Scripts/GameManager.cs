using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject KingW_Prefab;
    [SerializeField] public GameObject QueenW_Prefab;
    [SerializeField] public GameObject RookW_Prefab;
    [SerializeField] public GameObject KnightW_Prefab;
    [SerializeField] public GameObject BishopW_Prefab;
    [SerializeField] public GameObject PawnW_Prefab;

    [SerializeField] public GameObject KingB_Prefab;
    [SerializeField] public GameObject QueenB_Prefab;
    [SerializeField] public GameObject RookB_Prefab;
    [SerializeField] public GameObject KnightB_Prefab;
    [SerializeField] public GameObject BishopB_Prefab;
    [SerializeField] public GameObject PawnB_Prefab;

    [SerializeField] Transform blackPieces;
    [SerializeField] Transform whitePieces;

    ChessBlockEditor[] blocks;

    private void Start()
    {
        blocks = FindObjectsOfType<ChessBlockEditor>();
        InstantiatePieces();
    }

    private void InstantiatePieces()
    {
        Board board = FindObjectOfType<Board>();
        InstantiateWhitePieces(board);
        InstantiateBlackPieces(board);
    }

    private void InstantiateWhitePieces(Board board)
    {
        AddPiece(RookW_Prefab, new Vector2(0, 0), true, PieceType.Rook);
        AddPiece(KnightW_Prefab, new Vector2(1, 0), true, PieceType.Knight);
        AddPiece(BishopW_Prefab, new Vector2(2, 0), true, PieceType.Bishop);
        AddPiece(KingW_Prefab, new Vector2(3, 0), true, PieceType.King);
        AddPiece(QueenW_Prefab, new Vector2(4, 0), true, PieceType.Queen);
        AddPiece(BishopW_Prefab, new Vector2(5, 0), true, PieceType.Bishop);
        AddPiece(KnightW_Prefab, new Vector2(6, 0), true, PieceType.Knight);
        AddPiece(RookW_Prefab, new Vector2(7, 0), true, PieceType.Rook);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(PawnW_Prefab, new Vector2(i, 1), true, PieceType.Pawn);
        }
    }

    private void InstantiateBlackPieces(Board board)
    {
        AddPiece(RookB_Prefab, new Vector2(0, 7), false, PieceType.Rook);
        AddPiece(KnightB_Prefab, new Vector2(1, 7), false, PieceType.Knight);
        AddPiece(BishopB_Prefab, new Vector2(2, 7), false, PieceType.Bishop);
        AddPiece(KingB_Prefab, new Vector2(3, 7), false, PieceType.King);
        AddPiece(QueenB_Prefab, new Vector2(4, 7), false, PieceType.Queen);
        AddPiece(BishopB_Prefab, new Vector2(5, 7), false, PieceType.Bishop);
        AddPiece(KnightB_Prefab, new Vector2(6, 7), false, PieceType.Knight);
        AddPiece(RookB_Prefab, new Vector2(7, 7), false, PieceType.Rook);

        for (int i = 0; i < 8; i++)
        {
            AddPiece(PawnB_Prefab, new Vector2(i, 6), false, PieceType.Pawn);
        }
    }

    void AddPiece(GameObject prefab, Vector2 position, bool isWhite, PieceType type)
    {
        foreach(var block in blocks)
        {
            if(block.name == position.x + " , " + position.y)
            {
                if(isWhite)
                {
                    var tmp = Instantiate(prefab, block.transform.position, Quaternion.identity, whitePieces);
                    block.occupiedByPiece = tmp;
                    tmp.AddComponent<Piece>();
                    tmp.GetComponent<Piece>().IsWhite = true;
                    tmp.GetComponent<Piece>().Type = type;
                }
                else
                {
                    var tmp = Instantiate(prefab, block.transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f), blackPieces);
                    block.occupiedByPiece = tmp;
                    tmp.AddComponent<Piece>();
                    tmp.GetComponent<Piece>().IsWhite = false;
                    tmp.GetComponent<Piece>().Type = type;
                }
            }
        }
    }
}
