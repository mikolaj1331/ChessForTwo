using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[SelectionBase]
public class ChessBlockEditor : MonoBehaviour
{
    [SerializeField] float gridSize = 3f;
    [SerializeField] float scaler = 1f;
    
    public GameObject occupiedByPiece;
    public bool isPlaceable = true;

    public float GridSize { get => gridSize; set => gridSize = value; }
    public float Scaler { get => scaler; set => scaler = value; }
    public GameObject OccupiedByPiece { get => occupiedByPiece; set => occupiedByPiece = value; }

    private void Start()
    {
        transform.localScale = new Vector3(scaler,scaler,scaler);    
    }

    void Update()
    {
        SnapToGrid();
        UpdateName();
    }

    private void SnapToGrid()
    {
        transform.position = new Vector3(GetGridPosition().x * gridSize, 0f, GetGridPosition().y * gridSize);
    }

    private void UpdateName()
    {
        gameObject.name = GetGridPosition().x + " , " + GetGridPosition().y;
    }
    Vector2Int GetGridPosition()
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x / gridSize), Mathf.RoundToInt(transform.position.z / gridSize));
    }
}
