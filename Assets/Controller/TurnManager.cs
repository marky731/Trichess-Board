using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Controller
{
    public class TurnManager : MonoBehaviour
    {
        // Event that is triggered when the turn changes
        public event Action<int> OnTurnChanged;
        
        private int _currentPlayer = 1; // 1 → 2 → 3 şeklinde dönecek
        
        // Property with notification when changed
        public int currentPlayer 
        { 
            get { return _currentPlayer; }
            private set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                    // Trigger the event
                    OnTurnChanged?.Invoke(_currentPlayer);
                }
            }
        }

        void Start()
        {
            // Announce the initial player
            Debug.Log("Game started. Current player: Player " + currentPlayer);
            // Trigger the event for the initial player
            OnTurnChanged?.Invoke(currentPlayer);
        }

        public void NextTurn()
        {
            // Calculate the next player (1 → 2 → 3 → 1)
            int nextPlayer = (currentPlayer % 3) + 1;
            
            // Set the current player to the next player
            currentPlayer = nextPlayer;
            
            Debug.Log("Turn changed. Current player: Player " + currentPlayer);
        }
        
        // Set the current player directly (useful for testing or resetting)
        public void SetCurrentPlayer(int playerId)
        {
            if (playerId < 1 || playerId > 3)
            {
                Debug.LogError("Invalid player ID: " + playerId + ". Must be between 1 and 3.");
                return;
            }
            
            currentPlayer = playerId;
            Debug.Log("Current player set to: Player " + currentPlayer);
        }
    }
}
