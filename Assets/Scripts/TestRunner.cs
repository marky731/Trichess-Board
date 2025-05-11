using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRunner : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Starting Bishop Movement Tests...");
        
        // Add the BishopMovementTest component to this GameObject
        gameObject.AddComponent<BishopMovementTest>();
        
        Debug.Log("Bishop Movement Tests initialized.");
    }
}
