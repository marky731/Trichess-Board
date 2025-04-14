//Bu sınıf, oyunun bitip bitmediğini kontrol eder.
//Eğer 2 oyuncu mat olmuşsa, kazanan oyuncuyu belirler ve bunu ekrana yazdırır.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Controller
{
    public class GameOverManager : MonoBehaviour
    {
        // Başlangıçta üç oyuncu var
        private int activePlayers = 3;
        // Tahta objesini tutan değişken
        private Board board;

        // Başlangıçta board objesini bulur
        void Start()
        {
            board = FindFirstObjectByType<Board>();
        }

        // Oyun bitme durumunu kontrol eder
        public void CheckGameOver()
        {
            // Elenen oyuncuları saymak için bir sayaç
            int eliminatedPlayers = 0;

            // Tüm oyuncuları kontrol et (1, 2, 3 numaralı oyuncular)
            for (int i = 1; i <= 3; i++)
            {
                // Eğer oyuncu mat olmuşsa, elenir
                if (board.IsCheckmate(i, board))
                {
                    eliminatedPlayers++; // Elenen oyuncuları say
                }
            }

            // Eğer 2 oyuncu elenmişse, oyun biter ve kazananı yazdırır
            if (eliminatedPlayers == 2)
            {
                Debug.Log("Oyun bitti! Kazanan: Player " + (3 - eliminatedPlayers));
            }
        }
    }
}
