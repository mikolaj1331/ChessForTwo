using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject boardPrefab;

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

    [SerializeField] Transform whitePiecesParent;
    [SerializeField] Transform blackPiecesParent;

    ChessBlockEditor[] blocks;

    private void Start()
    {
        Instantiate(boardPrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        blocks = FindObjectsOfType<ChessBlockEditor>();
        InstantiatePieces();
    }

    private void InstantiatePieces()
    {
        InstantiateWhitePieces();
        InstantiateBlackPieces();
    }

    private void InstantiateWhitePieces()
    {
        AddPiece(RookW_Prefab, new Vector2(0, 0));
        AddPiece(KnightW_Prefab, new Vector2(1, 0));
        AddPiece(BishopW_Prefab, new Vector2(2, 0));
        AddPiece(KingW_Prefab, new Vector2(3, 0));
        AddPiece(QueenW_Prefab, new Vector2(4, 0));
        AddPiece(BishopW_Prefab, new Vector2(5, 0));
        AddPiece(KnightW_Prefab, new Vector2(6, 0));
        AddPiece(RookW_Prefab, new Vector2(7, 0));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(PawnW_Prefab, new Vector2(i, 1));
        }
    }

    private void InstantiateBlackPieces()
    {
        AddPiece(RookB_Prefab, new Vector2(0, 7));
        AddPiece(KnightB_Prefab, new Vector2(1, 7));
        AddPiece(BishopB_Prefab, new Vector2(2, 7));
        AddPiece(KingB_Prefab, new Vector2(3, 7));
        AddPiece(QueenB_Prefab, new Vector2(4, 7));
        AddPiece(BishopB_Prefab, new Vector2(5, 7));
        AddPiece(KnightB_Prefab, new Vector2(6, 7));
        AddPiece(RookB_Prefab, new Vector2(7, 7));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(PawnB_Prefab, new Vector2(i, 6));
        }
    }

    void AddPiece(GameObject prefab, Vector2 position)
    {
        var pieceComponent = prefab.GetComponent<ChessPiece>();
        foreach (var block in blocks)
        {
            if (block.name == position.x + " , " + position.y)
            {
                if (pieceComponent.IsWhite)
                {
                    var tmp = Instantiate(prefab, block.transform.position, Quaternion.identity, whitePiecesParent);
                    block.occupiedByPiece = tmp;
                }
                else
                {
                    var tmp = Instantiate(prefab, block.transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f), blackPiecesParent);
                    block.occupiedByPiece = tmp;
                }
            }
        }
    }
}
