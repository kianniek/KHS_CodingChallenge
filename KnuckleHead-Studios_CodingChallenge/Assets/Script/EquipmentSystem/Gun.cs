using System.Collections;
using EquipmentSystem;
using UnityEngine;
using UnityEngine.Events;

public class Gun : Item
{
    [Header("Gun Settings")] public bool isAutoMode = false;
    private bool isShooting = false;
    private int ammoCount = 0;
    [SerializeField] private int maxAmmoCount = 30;
    [SerializeField] private float fireRate = 0.1f;

    [Header("Bullet Settings")] [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private UnityEvent onShoot;
    [SerializeField] private UnityEvent onEmptyShoot;
    [SerializeField] private UnityEvent onModeSwitchToSingle;
    [SerializeField] private UnityEvent onModeSwitchToAuto;
    [SerializeField] private UnityEvent<int> onAmmoUpdate;

    public int Reload(int ammoToAdd)
    {
        // Calculate how much ammoToAdd we can add to the clip
        var ammoNeeded = maxAmmoCount - ammoCount;

        // If we have more ammoToAdd than the clip can take, only fill it up to maxAmmoCount
        if (ammoToAdd >= ammoNeeded)
        {
            ammoCount = maxAmmoCount;

            // Update the ammo count
            UpdateAmmoCount();

            // Return the leftover ammoToAdd
            return ammoToAdd - ammoNeeded;
        }
        else
        {
            // Otherwise, add all the ammoToAdd we have to the clip
            ammoCount += ammoToAdd;

            // Update the ammo count
            UpdateAmmoCount();

            // Return 0 since there's no leftover ammoToAdd
            return 0;
        }
    }

    public override void Equip()
    {
        base.Equip();

        if (isAutoMode)
        {
            onModeSwitchToAuto.Invoke();
        }
        else
        {
            onModeSwitchToSingle.Invoke();
        }

        Debug.Log("Gun equipped.");
    }

    public override void Unequip()
    {
        base.Unequip();
        Debug.Log("Gun unequipped.");
        UpdateAmmoCount();
    }

    public override void Use()
    {
        if (isAutoMode)
        {
            // Automatic fire
            Debug.Log("Firing automatic shots!");
            isShooting = true;
            StartCoroutine(ShootAuto());
        }
        else
        {
            isShooting = true;
            Shoot();
            isShooting = false;
        }

        UpdateAmmoCount();
    }

    public override void StoppedUse()
    {
        isShooting = false;
    }

    public override void SecondaryUse()
    {
        ToggleAutoMode();
    }

    public void ToggleAutoMode()
    {
        isAutoMode = !isAutoMode;

        if (isAutoMode)
        {
            onModeSwitchToAuto.Invoke();
        }
        else
        {
            onModeSwitchToSingle.Invoke();
        }

        Debug.Log(isAutoMode ? "Automatic mode enabled." : "Automatic mode disabled.");
    }

    public void SetAmmoCount(int ammo)
    {
        ammoCount = ammo;
        UpdateAmmoCount();
    }

    public void Shoot()
    {
        if (ammoCount > 0)
        {
            // Single shot fire
            Debug.Log("Firing shot!");
            SpawnBullet();
            onShoot.Invoke();
            ammoCount--;
        }
        else
        {
            Debug.Log("Out of ammo!");
            onEmptyShoot.Invoke();
        }

        UpdateAmmoCount();
    }

    IEnumerator ShootAuto()
    {
        while (ammoCount > 0 && isShooting)
        {
            Shoot();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void SpawnBullet()
    {
        if (!bulletPrefab || !bulletSpawnPoint)
        {
            Debug.LogWarning("Bullet prefab or bullet spawn point not set.");
            return;
        }

        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody>();
        bulletRigidbody.AddForce(bulletSpawnPoint.forward * bulletSpeed, ForceMode.Impulse);
    }

    private void UpdateAmmoCount()
    {
        onAmmoUpdate.Invoke(ammoCount);
    }
}