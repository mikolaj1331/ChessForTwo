using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[SelectionBase]
public class ChessBlockEditor : MonoBehaviour
{
    float gridSize = 3.25f;
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
