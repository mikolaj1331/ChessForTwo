using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;


public enum MoveType { Move, Capture, EnPassant, QueenSideCastling, KingSideCastling, Check, Checkmate, PawnPromotion }
public class MatchLogger : MonoBehaviour
{
    List<MoveLogger> matchLog;
    private void Start()
    {
        matchLog = new List<MoveLogger>();
    }
    public void LogMovement(MoveType moveType, ChessPiece chessPiece, ChessPiece capturedChessPiece, float x, float y, int turn)
    {
        int count = matchLog.Count + 1;
        matchLog.Add(new MoveLogger(count, moveType, chessPiece, capturedChessPiece, new Vector2(chessPiece.PositionX, chessPiece.PositionY+1), new Vector2(x, y+1), turn));
    }
    public void EditLog(MoveLogger move, MoveType moveType, ChessPiece capturedChessPiece)
    {
        if(matchLog.Contains(move))
        {
            MoveLogger log = matchLog.Find(x => x.MoveId == move.MoveId);
            log.MoveType = moveType;
            log.CapturedChessPiece = capturedChessPiece;
        }
        return;
    }
    public void EditLog(MoveLogger move, MoveType moveType)
    {
        if (matchLog.Contains(move))
        {
            MoveLogger log = matchLog.Find(x => x.MoveId == move.MoveId);
            log.MoveType = moveType;
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
    int moveId;
    MoveType moveType;
    Vector2 startingPos;
    Vector2 destinationPos;
    ChessPiece chessPiece;
    ChessPiece capturedChessPiece;
    int turn;

    public int MoveId { get => moveId; set => moveId = value; }
    public MoveType MoveType { get => moveType; set => moveType = value; }
    public Vector2 StartingPos { get => startingPos; set => startingPos = value; }
    public Vector2 DestinationPos { get => destinationPos; set => destinationPos = value; }
    public ChessPiece ChessPiece { get => chessPiece; set => chessPiece = value; }
    public ChessPiece CapturedChessPiece { get => capturedChessPiece; set => capturedChessPiece = value; }
    public int Turn { get => turn; set => turn = value; }
    

    public MoveLogger(int moveId, MoveType moveType, ChessPiece chessPiece, ChessPiece capturedChessPiece, Vector2 startingPos, Vector2 destinationPos, int turn)
    {
        this.MoveId = moveId;
        this.MoveType = moveType;
        this.StartingPos = startingPos;
        this.DestinationPos = destinationPos;
        this.ChessPiece = chessPiece;
        this.CapturedChessPiece = capturedChessPiece;
        this.Turn = turn;
    }
}