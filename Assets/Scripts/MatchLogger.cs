using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class MatchLogger : MonoBehaviour
{
    List<MoveLogger> matchLog;
    private void Start()
    {
        matchLog = new List<MoveLogger>();
    }
    public void LogMovement(ChessPiece chessPiece, ChessPiece capturedChessPiece, float x, float y, int turn)
    {
        matchLog.Add(new MoveLogger(chessPiece, capturedChessPiece, new Vector2(chessPiece.PositionX, chessPiece.PositionY+1), new Vector2(x, y+1), turn));
    }
    public void EditLog(MoveLogger move, ChessPiece chessPiece, ChessPiece capturedChessPiece, Vector2 startingPosition, Vector2 destinationPosition, int turn)
    {
        if(matchLog.Contains(move))
        {
            MoveLogger log = matchLog.Find(x => x.Turn == turn);
            log.ChessPiece = chessPiece;
            log.CapturedChessPiece = capturedChessPiece;
            log.StartingPos = startingPosition;
            log.DestinationPos = destinationPosition;
        }
        return;
    }
    public void PrintLogger(GameObject go)
    {
        TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
        string result = "";
        foreach(var log in matchLog)
        {
            string name = log.ChessPiece.name;
            string capturedName;
            if (log.CapturedChessPiece != null)
                capturedName = " x " + log.CapturedChessPiece.name;
            else
                capturedName = "\t\t";
            name = name.Replace("(Clone)", "");
            string xStartValue = SwitchPositionToLetter(log.StartingPos.x);
            string xDestValue = SwitchPositionToLetter(log.DestinationPos.x);
            result += log.Turn + ". " + name + capturedName + "\t"+ xStartValue + log.StartingPos.y + " -> " + xDestValue + log.DestinationPos.y + "\n";
        }
        text.text = result;
    }
    public string SwitchPositionToLetter(float x)
    {
        return x switch
        {
            0 => "a",
            1 => "b",
            2 => "c",
            3 => "d",
            4 => "e",
            5 => "f",
            6 => "g",
            7 => "h",
            _ => "error",
        };
    }
    
    public int GetMatchLogLength()
    {
        return matchLog.Count;
    }
    public MoveLogger GetLastMove()
    {
        return matchLog.Last();
    }
}

public class MoveLogger
{
    Vector2 startingPos;
    Vector2 destinationPos;
    ChessPiece chessPiece;
    ChessPiece capturedChessPiece;
    int turn;

    enum MoveType { Standard, Capture, EnPassant, QueenSideCastling, KingSideCastling, Check, Checkmate}

    public Vector2 StartingPos { get => startingPos; set => startingPos = value; }
    public Vector2 DestinationPos { get => destinationPos; set => destinationPos = value; }
    public ChessPiece ChessPiece { get => chessPiece; set => chessPiece = value; }
    public ChessPiece CapturedChessPiece { get => capturedChessPiece; set => capturedChessPiece = value; }
    public int Turn { get => turn; set => turn = value; }

    public MoveLogger(ChessPiece chessPiece, ChessPiece capturedChessPiece, Vector2 startingPos, Vector2 destinationPos, int turn)
    {
        this.StartingPos = startingPos;
        this.DestinationPos = destinationPos;
        this.ChessPiece = chessPiece;
        this.CapturedChessPiece = capturedChessPiece;
        this.Turn = turn;
    }
}