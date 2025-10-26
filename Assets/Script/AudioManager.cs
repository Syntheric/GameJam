using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ada untuk manajemen scene
using System.Collections.Generic; // Wajib ada untuk menggunakan List

// Kita buat sebuah class kecil di sini untuk memasangkan nama scene dengan lagunya.
// [System.Serializable] membuatnya bisa terlihat di Inspector Unity.
[System.Serializable]
public class SceneMusic
{
    public string sceneName;
    public AudioClip musicClip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton

    // Kita ganti satu AudioClip dengan sebuah List (daftar)
    public List<SceneMusic> musicPlaylist;
    
    private AudioSource audioSource;

    void Awake()
    {
        // Setup Singleton dengan DontDestroyOnLoad
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // OnEnable dipanggil saat objek diaktifkan
    void OnEnable()
    {
        // 'Mendaftarkan' fungsi OnSceneLoaded agar dipanggil setiap kali scene berubah
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // OnDisable dipanggil saat objek dinonaktifkan
    void OnDisable()
    {
        // 'Membatalkan pendaftaran' agar tidak terjadi error jika objek ini hancur
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Ini adalah fungsi yang akan dijalankan secara otomatis saat scene baru selesai dimuat
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Cari lagu yang cocok dengan nama scene yang baru dimuat
        AudioClip clipToPlay = FindMusicForScene(scene.name);

        if (clipToPlay != null)
        {
            // Cek apakah lagu yang mau diputar berbeda dengan yang sedang diputar
            if (audioSource.clip != clipToPlay)
            {
                audioSource.clip = clipToPlay;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            // Jika tidak ada lagu khusus untuk scene ini, matikan musik
            audioSource.Stop();
        }
    }

    // Fungsi bantuan untuk mencari klip di dalam playlist kita
    AudioClip FindMusicForScene(string sceneName)
    {
        foreach (SceneMusic sm in musicPlaylist)
        {
            // Jika nama scene di playlist sama dengan nama scene sekarang
            if (sm.sceneName == sceneName)
            {
                return sm.musicClip; // Kembalikan lagunya
            }
        }
        return null; // Jika tidak ketemu, kembalikan null
    }
}