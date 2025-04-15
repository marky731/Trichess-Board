using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateChessPiecePrefabs : EditorWindow
{
    private Color kingColor = Color.yellow;
    private Color queenColor = Color.magenta;
    private Color rookColor = Color.red;
    private Color bishopColor = Color.blue;
    private Color knightColor = Color.green;
    private Color pawnColor = Color.white;

    [MenuItem("Tools/Create Chess Piece Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<CreateChessPiecePrefabs>("Create Chess Piece Prefabs");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create Simple Chess Piece Prefabs", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("Piece Colors (for easy identification):");
        kingColor = EditorGUILayout.ColorField("King Color", kingColor);
        queenColor = EditorGUILayout.ColorField("Queen Color", queenColor);
        rookColor = EditorGUILayout.ColorField("Rook Color", rookColor);
        bishopColor = EditorGUILayout.ColorField("Bishop Color", bishopColor);
        knightColor = EditorGUILayout.ColorField("Knight Color", knightColor);
        pawnColor = EditorGUILayout.ColorField("Pawn Color", pawnColor);

        GUILayout.Space(20);

        if (GUILayout.Button("Create All Prefabs"))
        {
            CreatePrefab("King", kingColor);
            CreatePrefab("Queen", queenColor);
            CreatePrefab("Rook", rookColor);
            CreatePrefab("Bishop", bishopColor);
            CreatePrefab("Knight", knightColor);
            CreatePrefab("Pawn", pawnColor);
            
            EditorUtility.DisplayDialog("Prefabs Created", 
                "Chess piece prefabs have been created in Assets/Prefabs folder.\n\n" +
                "Remember to assign these prefabs to the PieceView component in your scene.", 
                "OK");
        }
    }

    private void CreatePrefab(string pieceName, Color color)
    {
        // Create a new GameObject
        GameObject pieceObj = new GameObject(pieceName + "Prefab");
        
        // Add a SpriteRenderer component
        SpriteRenderer renderer = pieceObj.AddComponent<SpriteRenderer>();
        
        // Create a simple square sprite
        Texture2D texture = new Texture2D(64, 64);
        Color[] colors = new Color[64 * 64];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }
        texture.SetPixels(colors);
        texture.Apply();
        
        // Create a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
        renderer.sprite = sprite;
        
        // Make sure the Prefabs directory exists
        if (!Directory.Exists("Assets/Prefabs"))
        {
            Directory.CreateDirectory("Assets/Prefabs");
        }
        
        // Create the prefab
        string prefabPath = "Assets/Prefabs/" + pieceName + "Prefab.prefab";
        PrefabUtility.SaveAsPrefabAsset(pieceObj, prefabPath);
        
        // Destroy the temporary GameObject
        DestroyImmediate(pieceObj);
        
        Debug.Log(pieceName + " prefab created at " + prefabPath);
    }
}
