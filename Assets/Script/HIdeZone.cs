using UnityEngine;
using System.Collections;

public class HideZoneTransparency : MonoBehaviour
{
    [Header("Alpha Settings")]
    [Range(0f,1f)] public float visibleAlpha = 0.5f;  // transparansi saat player masuk
    [Range(0f,1f)] public float normalAlpha = 1f;     // alpha normal saat kosong
    public float fadeSpeed = 2f;

    [Header("Dark Factor")]
    [Range(0f,1f)] public float darkFactor = 0.3f;    // 0 = hitam pekat, 1 = normal

    private SpriteRenderer rend;
    private Color originalColor;
    private Coroutine currentCoroutine;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        originalColor = rend.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(currentCoroutine != null) StopCoroutine(currentCoroutine);

            // Warna gelap tapi tetap transparan
            Color targetColor = new Color(
                originalColor.r * darkFactor,
                originalColor.g * darkFactor,
                originalColor.b * darkFactor,
                visibleAlpha
            );

            currentCoroutine = StartCoroutine(FadeToColor(targetColor));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            if(currentCoroutine != null) StopCoroutine(currentCoroutine);

            // Kembali ke warna asli
            Color targetColor = originalColor;
            targetColor.a = normalAlpha;

            currentCoroutine = StartCoroutine(FadeToColor(targetColor));
        }
    }

    private IEnumerator FadeToColor(Color targetColor)
    {
        Color startColor = rend.color;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * fadeSpeed;
            rend.color = Color.Lerp(startColor, targetColor, elapsed);
            yield return null;
        }

        rend.color = targetColor;
    }
}
