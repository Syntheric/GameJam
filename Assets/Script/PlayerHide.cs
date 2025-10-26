using UnityEngine;

public class PlayerHide : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    [Header("Visual")]
    [Range(0f, 1f)] public float hiddenAlpha = 0.8f; // alpha saat sembunyi

    [HideInInspector] public bool isHidden = false; // dicek PlayerHealth

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hide"))
        {
            isHidden = true;

            if (spriteRenderer != null)
            {
                Color faded = originalColor;
                faded.a = hiddenAlpha;
                spriteRenderer.color = faded;
            }

            // Debug.Log("Player masuk hide zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hide"))
        {
            isHidden = false;

            if (spriteRenderer != null)
                spriteRenderer.color = originalColor;

            // Debug.Log("Player keluar hide zone");
        }
    }
}
