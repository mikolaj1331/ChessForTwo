using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;


public enum MoveType { Move, EnPassant, QueenSideCastling, KingSideCastling, PawnPromotion}
public class MatchLogger : MonoBehaviour
{
    List<MoveLogger> matchLog;
    private void Start()
    {
        matchLog = new List<MoveLogger>();
    }
    public void LogMovement(MoveType moveType, ChessPiece chessPiece, ChessPiece capturedChessPiece, float x, float y, int turn, bool isCaptureMove, bool isCheck, bool isCheckmate)
    {
        int count = matchLog.Count + 1;
        matchLog.Add(new MoveLogger(count, moveType, chessPiece, capturedChessPiece, new Vector2(chessPiece.PositionX, chessPiece.PositionY+1), new Vector2(x, y+1), turn, isCaptureMove, isCheck, isCheckmate));
    }
    public void EditLog(MoveLogger move, MoveType moveType, ChessPiece capturedChessPiece, bool isCheck, bool isCheckmate)
    {
        if(matchLog.Contains(move))
        {
            MoveLogger log = matchLog.Find(x => x.MoveId == move.MoveId);
            log.MoveType = moveType;
            log.CapturedChessPiece = capturedChessPiece;
            log.IsCaptureMove = true;
            log.IsCheck = isCheck;
            log.IsCheckmate = isCheckmate;
        }
        return;
    }
    public void EditLog(MoveLogger move, MoveType moveType, bool isCheck, bool isCheckmate)
    {
        if (matchLog.Contains(move))
        {
            MoveLogger log = matchLog.Find(x => x.MoveId == move.MoveId);
            log.MoveType = moveType;
            log.IsCheck = isCheck;
            log.IsCheckmate = isCheckmate;
        }
        return;
    }
    public void PrintLogger(GameObject go)
    {
        TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
        string result = "Turn\tW.Pieces\t\tB.Pieces\n\n";
        bool firstMove = true;

        for (int i = 0; i < matchLog.Count; i++)
        {
            var log = matchLog[i];            
            string move;

            if (!log.IsCaptureMove)
                move = SwitchPieceNameToSymbol(log.ChessPiece) + SwitchPositionToLetter(log.StartingPos.x) + log.StartingPos.y + "-" + SwitchPositionToLetter(log.DestinationPos.x) + log.DestinationPos.y;
            else
                move = SwitchPieceNameToSymbol(log.ChessPiece) + SwitchPositionToLetter(log.StartingPos.x) + log.StartingPos.y + "x" + SwitchPositionToLetter(log.DestinationPos.x) + log.DestinationPos.y;

            if (log.MoveType == MoveType.KingSideCastling || log.MoveType == MoveType.QueenSideCastling)
                move = "";

            string check = "";
            if (log.IsCheck)
                check += "+";
            else if (log.IsCheckmate)
                check += "++";
            else
                check = "";


            if (firstMove)
                result += log.Turn + ".\t" + move + "" + HandleMoveTypes(log) + check + " \t\t";
            else
                result += move + "" + HandleMoveTypes(log) + check + "\n";

            firstMove = !firstMove;            
        }
        text.text = result;
    }
    public int GetMatchLogLength()
    {
        return matchLog.Count;
    }
    public MoveLogger GetLastMove()
    {
        return matchLog.Last();
    }

    string SwitchPositionToLetter(float x)
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
    string HandleMoveTypes(MoveLogger move)
    {
        return move.MoveType switch
        {
            MoveType.EnPassant => "ep",
            MoveType.PawnPromotion => "=Q",
            MoveType.KingSideCastling => "O-O",
            MoveType.QueenSideCastling => "O-O-O",
            MoveType.Move => "",
            _ => "ERORR MATCH TYPE",
        };
    }
    string SwitchPieceNameToSymbol(ChessPiece chessPiece)
    {
        string tag = chessPiece.tag;
        return tag switch
        {
            "Pawn" => "",
            "Rook" => "R",
            "Knight" => "N",
            "Bishop" => "B",
            "Queen" => "Q",
            "King" => "K",
            _ => "ERROR TAG",
        };
    }
}
//TODO: Change MatchLogger and Pawn Promotion logic to allow for promotion into other chess pieces than queen
public class MoveLogger
{
    int moveId;
    MoveType moveType;
    Vector2 startingPos;
    Vector2 destinationPos;
    ChessPiece chessPiece;
    ChessPiece capturedChessPiece;
    int turn;
    bool isCaptureMove;
    bool isCheck;
    bool isCheckmate;

    public int MoveId { get => moveId; set => moveId = value; }
    public MoveType MoveType { get => moveType; set => moveType = value; }
    public Vector2 StartingPos { get => startingPos; set => startingPos = value; }
    public Vector2 DestinationPos { get => destinationPos; set => destinationPos = value; }
    public ChessPiece ChessPiece { get => chessPiece; set => chessPiece = value; }
    public ChessPiece CapturedChessPiece { get => capturedChessPiece; set => capturedChessPiece = value; }
    public int Turn { get => turn; set => turn = value; }
    public bool IsCaptureMove { get => isCaptureMove; set => isCaptureMove = value; }
    public bool IsCheck { get => isCheck; set => isCheck = value; }
    public bool IsCheckmate { get => isCheckmate; set => isCheckmate = value; }

    public MoveLogger(int moveId, MoveType moveType, ChessPiece chessPiece, ChessPiece capturedChessPiece, 
        Vector2 startingPos, Vector2 destinationPos, int turn, bool isCaptureMove, bool isCheck, bool isCheckmate)
    {
        this.MoveId = moveId;
        this.MoveType = moveType;
        this.StartingPos = startingPos;
        this.DestinationPos = destinationPos;
        this.ChessPiece = chessPiece;
        this.capturedChessPiece = capturedChessPiece;
        this.Turn = turn;
        this.IsCaptureMove = isCaptureMove;
        this.IsCheck = isCheck;
        this.IsCheckmate = isCheckmate;
    }
}