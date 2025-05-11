using UnityEngine;

public class PieceClickHandler : MonoBehaviour
{
    public int playerId; // Bu taş hangi oyuncuya ait (0, 1, 2)

    private Light selectionLight; // Taşın altındaki ışık
    private static GameObject currentlySelectedPiece = null; // Şu anda seçilen taş

    private void Start()
    {
        // Bu taşın çocuk objeleri arasında Light bileşeni bul
        selectionLight = GetComponentInChildren<Light>();
        if (selectionLight != null)
        {
            selectionLight.enabled = false; // Başta kapalı olacak
        }
    }

}