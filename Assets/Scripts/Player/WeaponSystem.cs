using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] float bulletDamage;
    [SerializeField] int magazineSize;
    [SerializeField] float fireRate;
    public FireMode fireMode;
    [SerializeField] float reloadTime;
    private bool isReloading;
    private int currentAmmo;
    private bool canShoot;
    [SerializeField] float range;

    public delegate void AmmoChanged(int currentAmmo);
    public static event AmmoChanged OnAmmoChanged;




    void Start()
    {
        currentAmmo = magazineSize;
        canShoot = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(ReloadCoroutine());
        }


        if (fireMode == FireMode.FullAuto)
        {
            if (Input.GetMouseButton(0) && canShoot && !isReloading && currentAmmo > 0)
            {
                Shoot();
            }
        }
        else if (fireMode == FireMode.SemiAuto)
        {
            if (Input.GetMouseButtonDown(0) && canShoot && !isReloading && currentAmmo > 0)
            {
                Shoot();
            }
        }

    }


    public enum FireMode
    {
        SemiAuto,
        FullAuto
    }

    IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = magazineSize;
        OnAmmoChanged?.Invoke(currentAmmo);
        isReloading = false;
        canShoot = true;
    }

    IEnumerator ShootCooldownCoroutine()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            EnemyBase enemy = hit.collider.gameObject.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(bulletDamage);
            }
        }
        currentAmmo--;
        OnAmmoChanged?.Invoke(currentAmmo);
        StartCoroutine(ShootCooldownCoroutine());

    }

    public void IncreaseDamage(float amount)
    {
        bulletDamage += amount;
    }

    public void IncreaseFireRate(float amount)
    {
        fireRate -= amount; // lower fireRate = faster shooting
    }

    public void IncreaseAmmoCapacity(int amount)
    {
        magazineSize += amount;
        currentAmmo += amount;
    }
}
