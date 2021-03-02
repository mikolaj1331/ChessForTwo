﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardManager : MonoBehaviour
{   
    [Header("Models Prefabs")]
    [SerializeField] GameObject boardPrefab;
    [Header("White Chess Pieces Prefabs")]
    [SerializeField] WhiteChessPiecesPrefabs prefabsW;
    [Header("Black Chess Pieces Prefabs")]
    [SerializeField] BlackChessPiecesPrefabs prefabsB;
    [Header("Parent Transforms")]
    [SerializeField] Transform whitePiecesParent;
    [SerializeField] Transform blackPiecesParent;

    //Public variables needed for game logic
    public static BoardManager Instance { set; get; }
    public ChessPiece[,] Pieces { set; get; }
    public ChessPiece selectedChessPiece;
    public int turn;

    //Private variables needed for BoardManager's logic
    bool[,] validMoves { set; get; }
    List<GameObject> activeChessPieces;    
    ChessBlockEditor[] blocks;
    MatchLogger logger;

    private void Start()
    {
        turn = 1;
        Instance = this;
        Instantiate(boardPrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        blocks = FindObjectsOfType<ChessBlockEditor>();
        InstantiatePieces();
        logger = GetComponent<MatchLogger>();
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
        AddPiece(prefabsW.RookW_Prefab, new Vector2Int(0, 0));
        AddPiece(prefabsW.KnightW_Prefab, new Vector2Int(1, 0));
        AddPiece(prefabsW.BishopW_Prefab, new Vector2Int(2, 0));
        AddPiece(prefabsW.KingW_Prefab, new Vector2Int(3, 0));
        AddPiece(prefabsW.QueenW_Prefab, new Vector2Int(4, 0));
        AddPiece(prefabsW.BishopW_Prefab, new Vector2Int(5, 0));
        AddPiece(prefabsW.KnightW_Prefab, new Vector2Int(6, 0));
        AddPiece(prefabsW.RookW_Prefab, new Vector2Int(7, 0));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(prefabsW.PawnW_Prefab, new Vector2Int(i, 1));
        }
    }
    private void InstantiateBlackPieces()
    {
        AddPiece(prefabsB.RookB_Prefab, new Vector2Int(0, 7));
        AddPiece(prefabsB.KnightB_Prefab, new Vector2Int(1, 7));
        AddPiece(prefabsB.BishopB_Prefab, new Vector2Int(2, 7));
        AddPiece(prefabsB.KingB_Prefab, new Vector2Int(3, 7));
        AddPiece(prefabsB.QueenB_Prefab, new Vector2Int(4, 7));
        AddPiece(prefabsB.BishopB_Prefab, new Vector2Int(5, 7));
        AddPiece(prefabsB.KnightB_Prefab, new Vector2Int(6, 7));
        AddPiece(prefabsB.RookB_Prefab, new Vector2Int(7, 7));

        for (int i = 0; i < 8; i++)
        {
            AddPiece(prefabsB.PawnB_Prefab, new Vector2Int(i, 6));
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
        ResetVisuals();

        var piece = Pieces[x, y];
        if (piece == null)
        {
            selectedChessPiece = null;
            return;
        }       
        validMoves = piece.GetValidMoves();

        if(piece.CompareTag("King"))
        {
            validMoves = piece.GetComponent<King>().FindInvalidMoves(validMoves);
        }

        ProcessVisuals(isWhiteTurn, piece);

        selectedChessPiece = piece;
    }
    public bool MoveChessPiece(int x, int y, bool isWhiteTurn)
    {
        if (selectedChessPiece.IsWhite != isWhiteTurn) return false;
        if (validMoves[x, y])
        {
            if (Pieces[x, y] != null)
            {
                ChessPiece target = Pieces[x, y];
                if (target.GetType() == typeof(King))
                {
                    // Win the game
                }
                activeChessPieces.Remove(Pieces[x, y].gameObject);
                Pieces[x, y].gameObject.SetActive(false);
            }
            logger.LogMovement(selectedChessPiece, x, y, turn);
            ProcessMovement(x, y);
            ResetVisuals();
            return true;
        }
        else
            return false;
    }
    void ProcessMovement(int x, int y)
    {
        Pieces[selectedChessPiece.PositionX, selectedChessPiece.PositionY] = null;
        selectedChessPiece.transform.position = new Vector3(x, selectedChessPiece.transform.position.y, y);
        selectedChessPiece.PositionX = x;
        selectedChessPiece.PositionY = y;
        Pieces[x, y] = selectedChessPiece;
        selectedChessPiece = null;
    }
    void ProcessVisuals(bool turn, ChessPiece piece)
    {
        if (turn == piece.IsWhite)
        {
            HighlightValidMoves(validMoves, blocks, true);
            piece.GetComponent<Outline>().OutlineColor = Color.green;
        }
        else
        {
            HighlightValidMoves(validMoves, blocks, false);
            piece.GetComponent<Outline>().OutlineColor = Color.red;
        }
    }
    void HighlightValidMoves(bool[,] validMoves, ChessBlockEditor[] blocks, bool isYourTurn)
    {
        foreach (var block in blocks)
        {
            if (validMoves[(int)block.transform.position.x, (int)block.transform.position.z])
            {
                GameObject highlightObject = block.transform.GetChild(6).gameObject;
                highlightObject.gameObject.SetActive(true);
                if (isYourTurn)
                    highlightObject.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                else
                    highlightObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
        }
    }
    void ResetVisuals()
    {
        UnhighlightValidMoves(blocks);
        DeOutlineObject();
    }    
    void UnhighlightValidMoves(ChessBlockEditor[] blocks)
    {
        foreach(var block in blocks)
        {
            GameObject highlightObject = block.transform.GetChild(6).gameObject;
            highlightObject.gameObject.SetActive(false);
        }
    }        
    void DeOutlineObject()
    {
        foreach(var p in activeChessPieces)
        {
            p.GetComponent<Outline>().OutlineColor = Color.black;
        }
    }
    
}

[System.Serializable]
public class WhiteChessPiecesPrefabs
{
    public GameObject KingW_Prefab;
    public GameObject QueenW_Prefab;
    public GameObject RookW_Prefab;
    public GameObject KnightW_Prefab;
    public GameObject BishopW_Prefab;
    public GameObject PawnW_Prefab;
}

[System.Serializable]
public class BlackChessPiecesPrefabs
{
    public GameObject KingB_Prefab;
    public GameObject QueenB_Prefab;
    public GameObject RookB_Prefab;
    public GameObject KnightB_Prefab;
    public GameObject BishopB_Prefab;
    public GameObject PawnB_Prefab;
}