using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class TurnIndicatorCreator : Editor
{
    [MenuItem("Trichess/Create Turn Indicator")]
    static void CreateTurnIndicator()
    {
        // Check if Canvas exists, if not create one
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            // Create a new Canvas
            GameObject canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            // Add Canvas Scaler
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // Add Graphic Raycaster
            canvasObject.AddComponent<GraphicRaycaster>();
            
            Debug.Log("Created new Canvas");
        }
        
        // Create Turn Indicator GameObject
        GameObject turnIndicatorObject = new GameObject("TurnIndicator");
        turnIndicatorObject.transform.SetParent(canvas.transform, false);
        
        // Add RectTransform component
        RectTransform rectTransform = turnIndicatorObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1);
        rectTransform.anchoredPosition = new Vector2(-20, -20);
        rectTransform.sizeDelta = new Vector2(200, 50);
        
        // Add Image component
        Image backgroundImage = turnIndicatorObject.AddComponent<Image>();
        backgroundImage.color = Color.white;
        
        // Create Text GameObject
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(turnIndicatorObject.transform, false);
        
        // Add RectTransform component to Text
        RectTransform textRectTransform = textObject.AddComponent<RectTransform>();
        textRectTransform.anchorMin = new Vector2(0, 0);
        textRectTransform.anchorMax = new Vector2(1, 1);
        textRectTransform.pivot = new Vector2(0.5f, 0.5f);
        textRectTransform.anchoredPosition = Vector2.zero;
        textRectTransform.sizeDelta = Vector2.zero;
        
        // Add TextMeshProUGUI component
        TextMeshProUGUI turnText = textObject.AddComponent<TextMeshProUGUI>();
        turnText.text = "Player 1's Turn";
        turnText.color = Color.black;
        turnText.fontSize = 24;
        turnText.alignment = TextAlignmentOptions.Center;
        
        // Add TurnIndicator component
        TurnIndicator turnIndicator = turnIndicatorObject.AddComponent<TurnIndicator>();
        turnIndicator.turnText = turnText;
        turnIndicator.backgroundImage = backgroundImage;
        
        Debug.Log("Created Turn Indicator");
        
        // Select the created object
        Selection.activeGameObject = turnIndicatorObject;
    }
}
