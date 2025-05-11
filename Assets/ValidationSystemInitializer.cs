using UnityEngine;

/// <summary>
/// This script initializes the name-based validation system when the game starts.
/// It adds the NameBasedValidationInitializer component to a new GameObject.
/// </summary>
public class ValidationSystemInitializer : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("ValidationSystemInitializer Awake called.");
        
        // Check if the NameBasedValidationInitializer already exists
        NameBasedValidationInitializer initializer = FindFirstObjectByType<NameBasedValidationInitializer>();
        if (initializer == null)
        {
            // Create a new GameObject for the initializer
            GameObject initializerObj = new GameObject("NameBasedValidationInitializer");
            initializer = initializerObj.AddComponent<NameBasedValidationInitializer>();
            Debug.Log("Created NameBasedValidationInitializer");
        }
        else
        {
            Debug.Log("NameBasedValidationInitializer already exists");
        }
    }
}
