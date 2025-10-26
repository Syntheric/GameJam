using Unity.VisualScripting;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f;
    private bool movingRight = true;

    private void Update()
    {
        // Gerakkan musuh terus ke arah saat ini
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kalau nabrak tembok (tag "Wall"), balik arah
        if (collision.collider.CompareTag("Wall"))
        {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         if (collision.gameObject.CompareTag("Wall"))
        {
            Flip();
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;

        // Balik arah sprite
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Balik arah gerak
        speed *= -1;
    }
}
