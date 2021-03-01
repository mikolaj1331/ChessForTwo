using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchLogger : MonoBehaviour
{
    List<MoveLogger> matchLog;
    private void Start()
    {
        matchLog = new List<MoveLogger>();
    }
    public void LogMovement(ChessPiece cp, float x, float y)
    {
        matchLog.Add(new MoveLogger(new Vector2(cp.PositionX, cp.PositionY), new Vector2(x, y), cp));
    }
    public void PrintLogger(GameObject go)
    {
        TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
        string result = "";
        foreach(var log in matchLog)
        {
            string name = log.Cp.name;
            name = name.Replace("(Clone)", "");
            string xStartValue = SwitchPositionToLetter(log.StartingPos.x);
            string xDestValue = SwitchPositionToLetter(log.DestinationPos.x);
            result += name + " \t" + xStartValue + log.StartingPos.y + " -> " + xDestValue + log.DestinationPos.y + "\n";
        }
        text.text = result;
    }
    public string SwitchPositionToLetter(float x)
    {
        switch (x)
        {
            case 0:
                return "a";
            case 1:
                return "b";
            case 2:
                return "c";
            case 3:
                return "d";
            case 4:
                return "e";
            case 5:
                return "f";
            case 6:
                return "g";
            case 7:
                return "h";
            default:
                return "error";
        }
    }
}

public class MoveLogger
{
    Vector2 startingPos;
    Vector2 destinationPos;
    ChessPiece cp;

    public Vector2 StartingPos { get => startingPos; set => startingPos = value; }
    public Vector2 DestinationPos { get => destinationPos; set => destinationPos = value; }
    public ChessPiece Cp { get => cp; set => cp = value; }

    public MoveLogger(Vector2 startingPos, Vector2 destinationPos, ChessPiece chessPiece)
    {
        this.StartingPos = startingPos;
        this.DestinationPos = destinationPos;
        this.Cp = chessPiece;
    }
}