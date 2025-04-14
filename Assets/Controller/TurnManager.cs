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
        public int currentPlayer { get; private set; } = 1; // 1 → 2 → 3 şeklinde dönecek

        public void NextTurn()
        {
            currentPlayer = (currentPlayer % 3) + 1; // 1 → 2 → 3 → 1 şeklinde ilerler
            Debug.Log("Şu anki oyuncu: Player " + currentPlayer);
        }
    }
}
