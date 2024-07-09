using UnityEngine;

public class Pellet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore();
            Destroy(gameObject);
        }
    }
}
