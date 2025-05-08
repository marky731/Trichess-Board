using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class GameInitializerEditor
{
    static GameInitializerEditor()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    static void OnEditorUpdate()
    {
        // Only run once
        EditorApplication.update -= OnEditorUpdate;

        // Check if we're in play mode
        if (EditorApplication.isPlaying)
            return;

        // Check if GameInitializer already exists in the scene
        GameInitializer[] initializers = Object.FindObjectsOfType<GameInitializer>();
        if (initializers.Length > 0)
        {
            Debug.Log("GameInitializer already exists in the scene.");
            return;
        }

        // Ask the user if they want to add a GameInitializer to the scene
        if (EditorUtility.DisplayDialog("Add GameInitializer",
            "Would you like to add a GameInitializer to the current scene? This will ensure all required components for piece movement are created.",
            "Yes", "No"))
        {
            // Create a new GameObject with GameInitializer component
            GameObject gameInitializerObj = new GameObject("GameInitializer");
            gameInitializerObj.AddComponent<GameInitializer>();
            
            // Mark the scene as dirty
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            
            Debug.Log("GameInitializer added to the scene.");
        }
    }

    [MenuItem("Trichess/Add GameInitializer to Scene")]
    static void AddGameInitializerToScene()
    {
        // Check if GameInitializer already exists in the scene
        GameInitializer[] initializers = Object.FindObjectsOfType<GameInitializer>();
        if (initializers.Length > 0)
        {
            Debug.Log("GameInitializer already exists in the scene.");
            return;
        }

        // Create a new GameObject with GameInitializer component
        GameObject gameInitializerObj = new GameObject("GameInitializer");
        gameInitializerObj.AddComponent<GameInitializer>();
        
        // Mark the scene as dirty
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        
        Debug.Log("GameInitializer added to the scene.");
    }
}
