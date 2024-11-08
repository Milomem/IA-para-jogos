using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    public int maxAmmo = 1; // Quantidade máxima de munição definida aqui
    private int currentAmmo;
    public float reloadTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && currentAmmo > 0) // Adicionado currentAmmo > 0
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    IEnumerator Reload()
    {
        // yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        yield return null; // Adicionado para garantir que todos os caminhos de código retornem um valor
    }

    void Shoot()
    {
        if (currentAmmo > 0) // Adicionado para garantir que currentAmmo seja maior que 0
        {
            currentAmmo--;
            Debug.Log("Current Ammo: " + currentAmmo); // Adicionado para verificar o valor de currentAmmo
            Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            ReloadAmmo();
            Destroy(other.gameObject);
            currentAmmo = maxAmmo;
            // Debug.Log("Ammo reloaded by item.");
        }
    }

    public void ReloadAmmo() // Tornado público
    {
        currentAmmo = maxAmmo;
        // Debug.Log("Ammo reloaded by item.");
    }
}