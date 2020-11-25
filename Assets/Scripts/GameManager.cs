using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ChessBlockEditor[] chessBlocks; 
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

    private void Start()
    {
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
        Instantiate(RookW_Prefab, board.whitePiecesSpots[0].transform);
        Instantiate(KnightW_Prefab, board.whitePiecesSpots[1].transform);
        Instantiate(BishopW_Prefab, board.whitePiecesSpots[2].transform);
        Instantiate(KingW_Prefab, board.whitePiecesSpots[3].transform);
        Instantiate(QueenW_Prefab, board.whitePiecesSpots[4].transform);
        Instantiate(BishopW_Prefab, board.whitePiecesSpots[5].transform);
        Instantiate(KnightW_Prefab, board.whitePiecesSpots[6].transform);
        Instantiate(RookW_Prefab, board.whitePiecesSpots[7].transform);
        for (int i = 8; i < 16; i++)
        {
            Instantiate(PawnW_Prefab, board.whitePiecesSpots[i].transform);
        }
    }

    private void InstantiateBlackPieces(Board board)
    {
        Instantiate(RookB_Prefab, board.blackPiecesSpots[15].transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        Instantiate(KnightB_Prefab, board.blackPiecesSpots[14].transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        Instantiate(BishopB_Prefab, board.blackPiecesSpots[13].transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        Instantiate(QueenB_Prefab, board.blackPiecesSpots[12].transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        Instantiate(KingB_Prefab, board.blackPiecesSpots[11].transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        Instantiate(BishopB_Prefab, board.blackPiecesSpots[10].transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        Instantiate(KnightB_Prefab, board.blackPiecesSpots[9].transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        Instantiate(RookB_Prefab, board.blackPiecesSpots[8].transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        for (int i = 7; i >= 0; i--)
        {
            Instantiate(PawnB_Prefab, board.blackPiecesSpots[i].transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f));
        }
    }
}
