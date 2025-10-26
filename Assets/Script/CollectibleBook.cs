using UnityEngine;

public class CollectibleBook : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Cek jika yang menyentuh adalah Player
        if (other.CompareTag("Player"))
        {
            // 1. Panggil ScoreManager untuk menambah skor
            ScoreManager.instance.AddPoint();

            // 2. Hancurkan objek buku ini
            Destroy(gameObject);
        }
    }
}