using UnityEngine;
using UnityEngine.UI; // Wajib ada untuk mengakses komponen UI

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Membuatnya bisa diakses dari mana saja

    public Text scoreText; // Referensi ke komponen UI Text
    private int score = 0;

    void Awake()
    {
        // Pengaturan 'Singleton' agar hanya ada satu ScoreManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Mengatur teks skor di awal permainan
        UpdateScoreText();
    }

    // Fungsi ini akan dipanggil oleh buku saat diambil
    public void AddPoint()
    {
        score++; // Sama dengan score = score + 1;
        UpdateScoreText();
    }

    // Fungsi untuk memperbarui teks di layar
    void UpdateScoreText()
    {
        scoreText.text = "Buku Terkumpul: " + score.ToString();
    }
}