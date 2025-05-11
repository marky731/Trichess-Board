using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Assets.Model.ChessboardMain.Pieces; // Piece sınıfı için

public class TriChessAIRequester : MonoBehaviour
{
    public string currentTurn = "gray"; // Şimdilik sadece gray oyuncusu oynuyor

    void Start()
    {
        Debug.Log("Start() fonksiyonu tetiklendi.");
        SendBoardStateToAI();
    }

    public void SendBoardStateToAI()
    {
        Debug.Log("✅ SendBoardStateToAI() çağrildi.");
        StartCoroutine(SendRequestCoroutine());
    }

    IEnumerator SendRequestCoroutine()
    {
        Debug.Log("📤 SendRequestCoroutine() başladi.");

        string url = "http://localhost:5000/get-move";

        // 🧠 Otomatik JSON üret
        string boardStateJson = BoardStateBuilder.GenerateBoardStateJson();

        // JSON string oluşturuluyor
        string json = "{\"board_state\": " + boardStateJson + ", \"current_turn\": \"" + currentTurn + "\"}";
        Debug.Log("📦 Gönderilen JSON: " + json);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("✅ AI'dan gelen yanit: " + responseText);

            AIResponse aiResponse = JsonUtility.FromJson<AIResponse>(responseText);
            Debug.Log("🎯 AI hamlesi: " + aiResponse.move);

            ApplyMove(aiResponse.move);
        }
        else
        {
            Debug.LogError("❌ Hata oluştu: " + request.error);
            Debug.LogError("🌐 HTTP Kodu: " + request.responseCode);
        }
    }

    void ApplyMove(string move)
    {
        string[] parts = move.Split('-');
        if (parts.Length != 2)
        {
            Debug.LogError("❌ Hatali hamle formati: " + move);
            return;
        }

        string from = parts[0];
        string to = parts[1];

        Vector2Int fromPos = ChessNotationToPosition(from);
        Vector2Int toPos = ChessNotationToPosition(to);

        // Sahnedeki tüm Piece bileşenlerini bul
        Piece[] allPieces = GameObject.FindObjectsOfType<Piece>();
        foreach (Piece piece in allPieces)
        {
            Vector2 piecePos = new Vector2(
                Mathf.RoundToInt(piece.transform.position.x),
                Mathf.RoundToInt(piece.transform.position.y)
            );

            if ((int)piecePos.x == fromPos.x &&
                (int)piecePos.y == fromPos.y &&
                piece.GetColor().ToString().ToLower() == "gray")
            {
                piece.transform.position = new Vector3(toPos.x, toPos.y, piece.transform.position.z);
                Debug.Log($"🚚 Taş taşındı: {from} -> {to}");
                return;
            }
        }

        Debug.LogWarning("🔍 Taş bulunamadı: " + from);
    }

    Vector2Int ChessNotationToPosition(string notation)
    {
        if (notation.Length < 2) return Vector2Int.zero;
        char file = notation[0];
        int rank = int.Parse(notation.Substring(1));

        int x = file - 'A';
        int y = rank - 1;

        return new Vector2Int(x, y);
    }

    [System.Serializable]
    public class AIResponse
    {
        public string move;
    }
}
