using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { set; get; }
    private bool [,] validMoves { set; get; }

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

    List<GameObject> activeChessPieces;
    public ChessPiece[,] Pieces { set; get; }
    public ChessPiece selectedChessPiece;
    ChessBlockEditor[] blocks;

    private void Start()
    {
        Instance = this;
        Instantiate(boardPrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        blocks = FindObjectsOfType<ChessBlockEditor>();
        InstantiatePieces();
    }

    private void InstantiatePieces()
    {
        activeChessPieces = new List<GameObject>();
        Pieces = new ChessPiece[8,8];
        InstantiateWhitePieces();
        InstantiateBlackPieces();
    }

    private void InstantiateWhitePieces()
    {
        AddPiece(RookW_Prefab, new Vector2Int(0, 0));
        AddPiece(KnightW_Prefab, new Vector2Int(1, 0));
        AddPiece(BishopW_Prefab, new Vector2Int(2, 0));
        AddPiece(KingW_Prefab, new Vector2Int(3, 0));
        AddPiece(QueenW_Prefab, new Vector2Int(4, 0));
        AddPiece(BishopW_Prefab, new Vector2Int(5, 0));
        AddPiece(KnightW_Prefab, new Vector2Int(6, 0));
        AddPiece(RookW_Prefab, new Vector2Int(7, 0));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(PawnW_Prefab, new Vector2Int(i, 1));
        }
    }

    private void InstantiateBlackPieces()
    {
        AddPiece(RookB_Prefab, new Vector2Int(0, 7));
        AddPiece(KnightB_Prefab, new Vector2Int(1, 7));
        AddPiece(BishopB_Prefab, new Vector2Int(2, 7));
        AddPiece(KingB_Prefab, new Vector2Int(3, 7));
        AddPiece(QueenB_Prefab, new Vector2Int(4, 7));
        AddPiece(BishopB_Prefab, new Vector2Int(5, 7));
        AddPiece(KnightB_Prefab, new Vector2Int(6, 7));
        AddPiece(RookB_Prefab, new Vector2Int(7, 7));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(PawnB_Prefab, new Vector2Int(i, 6));
        }
    }

    void AddPiece(GameObject prefab, Vector2Int position)
    {
        var pieceComponent = prefab.GetComponent<ChessPiece>();
        foreach (var block in blocks)
        {
            if (block.name == position.x + " , " + position.y)
            {
                if (pieceComponent.IsWhite)
                {
                    var tmp = Instantiate(prefab, block.transform.position, Quaternion.identity, whitePiecesParent);
                    Pieces[position.x, position.y] = tmp.GetComponent<ChessPiece>();
                    Pieces[position.x, position.y].PositionX = position.x;
                    Pieces[position.x, position.y].PositionY = position.y;
                    activeChessPieces.Add(tmp);
                }
                else
                {
                    var tmp = Instantiate(prefab, block.transform.position, transform.rotation * Quaternion.Euler(0f, 180f, 0f), blackPiecesParent);
                    Pieces[position.x, position.y] = tmp.GetComponent<ChessPiece>();
                    Pieces[position.x, position.y].PositionX = position.x;
                    Pieces[position.x, position.y].PositionY = position.y;
                    activeChessPieces.Add(tmp);
                }
            }
        }
    }

    public void SelectChessPiece(int x, int y, bool isWhiteTurn)
    {
        UnhighlightValidMoves(blocks);
        var piece = Pieces[x, y];
        if (piece == null) return;
        if (piece.IsWhite != isWhiteTurn) return;

        validMoves = piece.GetValidMoves();
        HighlightValidMoves(validMoves,blocks);

        selectedChessPiece = piece;
    }

    private void HighlightValidMoves(bool[,] validMoves, ChessBlockEditor[] blocks)
    {
        foreach(var block in blocks)
        {
            if(validMoves[(int)block.transform.position.x, (int)block.transform.position.z])
            {
                GameObject highlightObject = block.transform.GetChild(6).gameObject;
                highlightObject.gameObject.SetActive(true);
            }
        }
    }

    void UnhighlightValidMoves(ChessBlockEditor[] blocks)
    {
        foreach(var block in blocks)
        {
            GameObject highlightObject = block.transform.GetChild(6).gameObject;
            highlightObject.gameObject.SetActive(false);
        }
    }

    public bool MoveChessPiece(int x, int y)
    {
        if (validMoves[x, y])
        {
            if (Pieces[x, y] != null)
            {
                ChessPiece target = Pieces[x, y];
                if (target.GetType() == typeof(King))
                {
                    // Win the game

                }
                Destroy(Pieces[x, y].gameObject);
            }
            Pieces[selectedChessPiece.PositionX, selectedChessPiece.PositionY] = null;
            selectedChessPiece.transform.position = new Vector3(x, selectedChessPiece.transform.position.y, y);
            selectedChessPiece.PositionX = x;
            selectedChessPiece.PositionY = y;
            Pieces[x, y] = selectedChessPiece;
            selectedChessPiece = null;
            UnhighlightValidMoves(blocks);
            return true;
        }
        else
            return false;
    }
}


/*
 * TO CHECK THE ARRAY
 for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                if (Pieces[i, j] == null)
                    continue;
                Debug.Log(Pieces[i, j].name + "  " + i + " " + j);
            }
        }
 */
