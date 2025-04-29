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

    private void OnMouseDown()
    {
        Debug.Log("Taş tıklandı: " + gameObject.name);

        // Sıra bu oyuncudaysa seçilebilir
        if (GameManager.Instance.CurrentPlayerId == playerId)
        {
            Debug.Log("Taş seçildi: " + gameObject.name);
            SelectThisPiece();
        }
        else
        {
            Debug.Log("Bu taş sana ait değil.");
        }
    }

    private void SelectThisPiece()
    {
        // Daha önce seçilmiş taş varsa, onun ışığını kapat
        if (currentlySelectedPiece != null && currentlySelectedPiece != gameObject)
        {
            Light previousLight = currentlySelectedPiece.GetComponentInChildren<Light>();
            if (previousLight != null)
            {
                previousLight.enabled = false;
            }
        }

        // Bu taşın ışığını aç
        if (selectionLight != null)
        {
            selectionLight.enabled = true;
        }

        // Bu taşı seçili olarak kaydet
        currentlySelectedPiece = gameObject;
    }
}