using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class ChessBlockEditor : MonoBehaviour
{
    [SerializeField] string displayText;
    [SerializeField] TextMeshPro textComponent;

    public bool isBorder;
    void Update()
    {
        UpdateName();
        DisplayText();
    }

    void DisplayText()
    {
        if(isBorder)
            textComponent.text = displayText;
    }

    private void UpdateName()
    {
        gameObject.name = GetGridPosition().x + " , " + GetGridPosition().y;
    }
    Vector2Int GetGridPosition()
    {
        return new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
    }
}
