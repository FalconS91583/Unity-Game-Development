using UnityEngine;

public enum WeaponType { Pistol, Revolver, AutoRifle, Shotgun, Sniper }
public enum ShootType { Single, Auto }

[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;

    [Header("Shooting")]
    public ShootType shootType;
    private float defaultFireRate;
    public float fireRate = 1;
    public int bulletsPerShot {  get; private set; }
    private float lastShootTime;

    [Header("Magazine")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Header("Spread")]
    private float baseSpread;
    private float currentSpread = 2;
    private float maxSpread = 3;
    private float spreadIncreaseRate = 0.15f;

    private float lastSpreadUpdateTime;
    private float spreadCooldown = 1;

    [Header("Recoil Settings")]
    public float recoilX = -2f; 
    public float recoilY = 2f;  
    public float recoilZ = 0.35f; 

    public float snappiness = 6f; 
    public float returnSpeed = 2f; 

    [Header("Accuracy & Modifiers")]
    public float movementSpreadPenalty = 1.5f; 
    public float aimSpreadMultiplier = 0.2f;   

    [Header("Ballistics & Range")]
    public float bulletSpeed = 50f; 
    public float effectiveRange = 150f; 

    [Header("Mobility & Handling")]
    [Range(0.5f, 1.1f)]
    public float mobilityMultiplier = 1f; 

    [Header("ADS Settings")]
    public float adsSpeed = 10f; 
    public float zoomRatio = 40f; 

    [Header("Burst")]
    private bool burstAvailable;
    public bool burstActive;
    private int burstBulletsPerShot;
    private float burstFireRate;
    public float burstFireDelay {  get; private set; }

    [Header("Anim Speed")]
    public float reloadSpeed {  get; private set; }
    public float equipSpeed { get; private set; }

    public Weapon(WeaponData weaponData)
    {
        weaponType = weaponData.weaponType;
        shootType = weaponData.shootType;

        fireRate = weaponData.fireRate;
        bulletsPerShot = weaponData.bulletsPerShot;

        bulletsInMagazine = weaponData.bulletsInMagazine;
        magazineCapacity = weaponData.magazineCapacity;
        totalReserveAmmo = weaponData.totalReserveAmmo;

        baseSpread = weaponData.baseSpread;
        maxSpread = weaponData.maxSpread;
        spreadIncreaseRate = weaponData.spreadIncreaseRate;

        recoilX = weaponData.recoilX;
        recoilY = weaponData.recoilY;
        recoilZ = weaponData.recoilZ;
        snappiness = weaponData.snappiness;
        returnSpeed = weaponData.returnSpeed;

        movementSpreadPenalty = weaponData.movementSpreadPenalty;
        aimSpreadMultiplier = weaponData.aimSpreadMultiplier;

        bulletSpeed = weaponData.bulletSpeed;
        effectiveRange = weaponData.effectiveRange;

        mobilityMultiplier = weaponData.mobilityMultiplier;

        adsSpeed = weaponData.adsSpeed;
        zoomRatio = weaponData.zoomRatio;

        burstAvailable = weaponData.burstAvailable;
        burstActive = weaponData.burstActive;
        burstBulletsPerShot = weaponData.burstBulletsPerShot;
        burstFireRate = weaponData.burstFireRate;
        burstFireDelay = weaponData.burstFireDelay;

        reloadSpeed = weaponData.reloadSpeed;
        equipSpeed = weaponData.equipSpeed;

        defaultFireRate = fireRate;

    }

    public bool CanShoot() => HaveEnoughBullets() && ReadyToFire();


    public Vector3 ApplySpread(Vector3 originalDirection, float currentVelocity, bool isAiming)
    {
        UpdateSpread();

        float finalSpread = currentSpread;

       
        if (currentVelocity > 0.1f)
        {
            finalSpread *= movementSpreadPenalty;
        }

        if (isAiming)
        {
            finalSpread *= aimSpreadMultiplier;
        }

        
        float randomizedValue = Random.Range(-finalSpread, finalSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();

        lastSpreadUpdateTime = Time.time;
    }

    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maxSpread);
    }


    public Vector3 GetRecoilPattern()
    {
        float randomY = Random.Range(-recoilY, recoilY);
        
        return new Vector3(recoilX, randomY, recoilZ);
    }
    private bool ReadyToFire()
    {
        if (Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }
        return false;
    }

    private bool HaveEnoughBullets() => bulletsInMagazine > 0;

    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity) return false;
        return totalReserveAmmo > 0;
    }

    public void ReloadBullets()
    {
        int bulletsNeeded = magazineCapacity - bulletsInMagazine;
        int bulletsToReload = Mathf.Min(bulletsNeeded, totalReserveAmmo);

        totalReserveAmmo -= bulletsToReload;
        bulletsInMagazine += bulletsToReload;

        if (totalReserveAmmo < 0) totalReserveAmmo = 0;
    }

    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }
        return burstActive;
    }

    public void ToggleBurst()
    {
        if (!burstAvailable) return;

        burstActive = !burstActive;
        if (burstActive)
        {
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }
}