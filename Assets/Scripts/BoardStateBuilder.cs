using System.Collections.Generic;
using UnityEngine;
using Assets.Model.ChessboardMain.Pieces;

public static class BoardStateBuilder
{
    [System.Serializable]
    public class PieceData
    {
        public string type;
        public string color;
        public int x;
        public int y;
    }

    public static string GenerateBoardStateJson()
    {
        List<PieceData> pieces = new List<PieceData>();
        Piece[] allPieces = GameObject.FindObjectsOfType<Piece>();

        foreach (Piece piece in allPieces)
        {
            pieces.Add(new PieceData
            {
                type = piece.GetAbbreviation(),
                color = piece.GetColor().ToString().ToLower(),
                x = Mathf.RoundToInt(piece.transform.position.x),
                y = Mathf.RoundToInt(piece.transform.position.y)
            });
        }

        return JsonHelper.ToJson(pieces.ToArray(), true);
    }
}
