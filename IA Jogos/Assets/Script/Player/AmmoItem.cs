using UnityEngine;

public class AmmoItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Shooting shooting = other.GetComponent<Shooting>();
            if (shooting != null)
            {
                shooting.ReloadAmmo(); // Chama o método ReloadAmmo no script Shooting
            }
            Destroy(gameObject);
        }
    }
}
