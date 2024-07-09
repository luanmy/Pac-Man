using UnityEngine;

public class PelletLarge
    : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore();
            GameManager.Instance.SuperPower();
            Destroy(gameObject);
        }
    }
}
