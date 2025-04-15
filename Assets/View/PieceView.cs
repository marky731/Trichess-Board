//Bu PieceView sınıfı, oyun tahtasındaki taşları görsel olarak temsil eden nesneleri yaratmak ve konumlandırmak için kullanılır.
//Unity'nin MonoBehaviour sınıfından türediği için Unity sahnesinde çalışabilir.
using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Model;

public class PieceView : MonoBehaviour
{
    public GameObject kingPrefab;
    public GameObject queenPrefab;
    public GameObject rookPrefab;
    public GameObject bishopPrefab;
    public GameObject knightPrefab;
    public GameObject pawnPrefab;

    private Dictionary<PieceType, GameObject> piecePrefabs;
    private BoardPositions board;

    void Awake()
    {
        board = FindFirstObjectByType<BoardPositions>();
        if (board == null)
        {
            Debug.LogError("PieceView: Board bulunamadı!");
        }

        piecePrefabs = new Dictionary<PieceType, GameObject>
        {
            { PieceType.King, kingPrefab },
            { PieceType.Queen, queenPrefab },
            { PieceType.Rook, rookPrefab },
            { PieceType.Bishop, bishopPrefab },
            { PieceType.Knight, knightPrefab },
            { PieceType.Pawn, pawnPrefab }
        };
    }

    public void SpawnPiece(PieceType type, string startPosition, PieceColor color)
    {
        if (!piecePrefabs.ContainsKey(type))
        {
            Debug.LogError("PieceView: Hatalı taş tipi!");
            return;
        }

        Vector2? pos = GetPositionFromNotation(startPosition);
        if (pos == null)
        {
            Debug.LogError($"PieceView: '{startPosition}' konumu geçersiz!");
            return;
        }

        GameObject pieceObj = Instantiate(piecePrefabs[type]);
        pieceObj.transform.position = new Vector3(pos.Value.x, pos.Value.y, 0);
        SetPieceColor(pieceObj, color);

        GameObject parent = GameObject.Find("Pieces");
        if (parent == null)
        {
            parent = new GameObject("Pieces");
        }
        pieceObj.transform.SetParent(parent.transform);
    }

    private Vector2? GetPositionFromNotation(string notation)
    {
        if (board == null)
        {
            Debug.LogError("PieceView: Board bulunamadı!");
            return null;
        }
        
        var position = board.GetPosition(notation);
        if (position.HasValue)
        {
            return new Vector2((float)position.Value.x, (float)position.Value.y);
        }
        
        return null;
    }

    private void SetPieceColor(GameObject pieceObj, PieceColor color)
    {
        SpriteRenderer renderer = pieceObj.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.LogError("PieceView: SpriteRenderer bulunamadı!");
            return;
        }

        switch (color)
        {
            case PieceColor.White:
                renderer.color = Color.white;
                break;
            case PieceColor.Black:
                renderer.color = Color.black;
                break;
            case PieceColor.Gray:
                renderer.color = Color.gray;
                break;
        }
    }
}
