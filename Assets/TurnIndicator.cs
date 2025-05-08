using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Controller;

public class TurnIndicator : MonoBehaviour
{
    // Public properties with private backing fields
    [SerializeField] private TextMeshProUGUI _turnText;
    public TextMeshProUGUI turnText
    {
        get { return _turnText; }
        set { _turnText = value; }
    }
    
    [SerializeField] private Image _backgroundImage;
    public Image backgroundImage
    {
        get { return _backgroundImage; }
        set { _backgroundImage = value; }
    }
    
    private TurnManager turnManager;
    
    // Player colors
    private Color player1Color = new Color(1f, 1f, 1f); // White
    private Color player2Color = new Color(0.5f, 0.5f, 0.5f); // Gray
    private Color player3Color = new Color(0.2f, 0.2f, 0.2f); // Black
    
    void Start()
    {
        // Find the TurnManager
        turnManager = FindFirstObjectByType<TurnManager>();
        if (turnManager == null)
        {
            Debug.LogError("TurnManager not found!");
            return;
        }
        
        // Subscribe to the OnTurnChanged event
        turnManager.OnTurnChanged += OnTurnChanged;
        
        // If turnText is not assigned, try to find it
        if (turnText == null)
        {
            turnText = GetComponentInChildren<TextMeshProUGUI>();
            if (turnText == null)
            {
                Debug.LogError("TextMeshProUGUI component not found!");
            }
        }
        
        // If backgroundImage is not assigned, try to find it
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
            if (backgroundImage == null)
            {
                Debug.LogError("Image component not found!");
            }
        }
        
        // Update the UI initially
        UpdateTurnIndicator();
    }
    
    void OnDestroy()
    {
        // Unsubscribe from the event when this object is destroyed
        if (turnManager != null)
        {
            turnManager.OnTurnChanged -= OnTurnChanged;
        }
    }
    
    // Event handler for when the turn changes
    void OnTurnChanged(int newPlayer)
    {
        UpdateTurnIndicator();
    }
    
    void UpdateTurnIndicator()
    {
        if (turnManager == null)
        {
            Debug.LogWarning("TurnManager is null in UpdateTurnIndicator!");
            return;
        }
        
        int currentPlayer = turnManager.currentPlayer;
        
        // Update the text
        if (_turnText != null)
        {
            _turnText.text = $"Player {currentPlayer}'s Turn";
        }
        else
        {
            Debug.LogWarning("TurnText is null in UpdateTurnIndicator!");
        }
        
        // Update the background color
        if (_backgroundImage != null)
        {
            switch (currentPlayer)
            {
                case 1:
                    _backgroundImage.color = player1Color;
                    if (_turnText != null) _turnText.color = Color.black; // Black text on white background
                    break;
                case 2:
                    _backgroundImage.color = player2Color;
                    if (_turnText != null) _turnText.color = Color.white; // White text on gray background
                    break;
                case 3:
                    _backgroundImage.color = player3Color;
                    if (_turnText != null) _turnText.color = Color.white; // White text on black background
                    break;
            }
        }
        else
        {
            Debug.LogWarning("BackgroundImage is null in UpdateTurnIndicator!");
        }
    }
    
    // This method can be called from other scripts to force an update
    public void ForceUpdate()
    {
        Debug.Log("Forcing update of TurnIndicator");
        UpdateTurnIndicator();
    }
}
