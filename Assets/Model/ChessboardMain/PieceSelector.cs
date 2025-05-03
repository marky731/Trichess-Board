using UnityEngine;

public class PieceSelector : MonoBehaviour
{
    public GameObject highlightPrefab; // Highlight için prefab (Inspector'dan atanacak)
    private GameObject currentHighlight; // Aktif highlight objesi
    private const string m_TagString = "Piecse"; // Tag string constant
    
    void Start()
    {
        // Check if highlight prefab is assigned
        Debug.Log("Highlight prefab reference: " + (highlightPrefab != null ? "ASSIGNED" : "MISSING"));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Sol tıklama kontrolü
        {
            Debug.Log("Mouse clicked at screen position: " + Input.mousePosition);
            
            // Check if camera is available
            if (Camera.main == null)
            {
                Debug.LogError("Main camera is null! Tag a camera as 'MainCamera'");
                return;
            }
            
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Converted to world position: " + mousePos);
            
            // Perform raycast and log result
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            Debug.Log("Raycast hit anything: " + (hit.collider != null));

            if (hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name + " with tag: " + hit.collider.tag);

                // Add debug for tag check
                Debug.Log("Tag check: Is Piece=" + hit.collider.CompareTag("Piece") + 
                          ", Is Piecse=" + hit.collider.CompareTag("Piecse"));

                if (hit.collider.CompareTag("Piece") || hit.collider.CompareTag("Piecse"))     
                {
                    Debug.Log("PIECE DETECTED - Creating highlight");
                    
                    // Eski highlight varsa sil
                    if (currentHighlight != null)
                    {
                        Debug.Log("Destroying previous highlight");
                        Destroy(currentHighlight);
                    }

                    // Check prefab before instantiation
                    if (highlightPrefab == null)
                    {
                        Debug.LogError("Highlight prefab is not assigned in inspector!");
                        return;
                    }

                    // Yeni highlight oluştur ve taşın konumuna yerleştir
                    currentHighlight = Instantiate(highlightPrefab, hit.collider.transform.position, Quaternion.identity);
                    Debug.Log("Highlight created at position: " + hit.collider.transform.position);

                    // Ensure the highlight is visible on top
                    SpriteRenderer highlightRenderer = currentHighlight.GetComponent<SpriteRenderer>();
                    if (highlightRenderer != null)
                    {
                        Debug.Log("Highlight has SpriteRenderer");
                        // Get the piece's renderer
                        SpriteRenderer pieceRenderer = hit.collider.GetComponent<SpriteRenderer>();
                        if (pieceRenderer != null)
                        {
                            // Set the highlight's sorting order above the piece
                            highlightRenderer.sortingOrder = pieceRenderer.sortingOrder + 1;
                            Debug.Log("Set highlight sorting order to: " + highlightRenderer.sortingOrder + 
                                      " (piece order: " + pieceRenderer.sortingOrder + ")");
                        }
                        else
                        {
                            Debug.LogWarning("Piece doesn't have a SpriteRenderer");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Highlight prefab doesn't have a SpriteRenderer");
                    }

                    // Highlight objesini seçilen taşın child'ı yap ki taşla birlikte hareket etsin
                    currentHighlight.transform.SetParent(hit.collider.transform);
                    Debug.Log("Highlight parented to piece");
                }
                else
                {
                    Debug.Log("Object clicked is not a chess piece");
                }
            }
        }
    }
}