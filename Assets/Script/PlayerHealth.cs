using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private bool isDead = false;
    private PlayerHide playerHide;

    private void Start()
    {
        playerHide = GetComponent<PlayerHide>();
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.CompareTag("Enemy") && !isDead)
        {
            if(playerHide != null && playerHide.isHidden)
            {
                // Debug.Log("Player tersembunyi, musuh tidak bisa membunuh");
                return;
            }

            isDead = true;
            Die();
        }
    }
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if(collision.collider.CompareTag("Enemy") && !isDead)
    //     {
    //         if(playerHide != null && playerHide.isHidden)
    //         {
    //             Debug.Log("Player tersembunyi, musuh tidak bisa membunuh");
    //             return;
    //         }

    //         isDead = true;
    //         Die();
    //     }
    // }

    private void Die()
    {
        // bisa diganti animasi
        Invoke(nameof(RestartLevel), 0.5f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
