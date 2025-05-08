using UnityEngine;

public class BoardClickHandlerAttacher : MonoBehaviour
{
    void Start()
    {
        // Find the board GameObject by looking for the Board component
        Board boardComponent = FindFirstObjectByType<Board>();
        
        if (boardComponent == null)
        {
            Debug.LogError("Board component not found in the scene!");
            return;
        }
        
        GameObject boardObject = boardComponent.gameObject;
        
        // Check if the board already has a BoardClickHandler component
        if (boardObject.GetComponent<BoardClickHandler>() == null)
        {
            // Add the BoardClickHandler component to the board
            boardObject.AddComponent<BoardClickHandler>();
            Debug.Log("BoardClickHandler component added to the board GameObject.");
            
            // Make sure the board has a collider for click detection
            if (boardObject.GetComponent<Collider2D>() == null)
            {
                BoxCollider2D collider = boardObject.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(10, 10); // Adjust size as needed
                Debug.Log("Added BoxCollider2D to the board for click detection.");
            }
        }
        else
        {
            Debug.Log("BoardClickHandler component already exists on the board GameObject.");
        }
    }
}
