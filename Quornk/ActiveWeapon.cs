using Cinemachine;
using StarterAssets;
using TMPro;
using UnityEngine;

public class ActiveWeapon : MonoBehaviour
{
    [SerializeField] private WeaponSO startintWeapon;
    private WeaponSO weaponSO;
    [SerializeField] private CinemachineVirtualCamera playerFollowCamera;
    [SerializeField] private Camera weaponCamera;
    [SerializeField] private GameObject zoomImage;
    [SerializeField] private TextMeshProUGUI ammoText;

    private StarterAssetsInputs starterAssetsInputs;
    private FirstPersonController firstPersonController;

    [SerializeField] private Animator anim;

    private Weapon currentWeapon;

    const string SHOOT_STRING = "Shoot";

    private float lastTimeShoot = 0f;
    private float defaultFOV;
    private float defaultRotationSpeed;
    private int currentAmmo;
    private void Awake()
    {
        starterAssetsInputs = GetComponentInParent<StarterAssetsInputs>();
        firstPersonController = GetComponentInParent<FirstPersonController>();

        anim = GetComponent<Animator>();

        defaultFOV = playerFollowCamera.m_Lens.FieldOfView;
        defaultRotationSpeed = firstPersonController.RotationSpeed;
    }

    private void Start()
    {
        SwitchWeapon(startintWeapon);
        AdjustAmmo(weaponSO.magazineCapacity);
    }

    private void Update()
    {
        HandleShoot();
        HandleZoom();
    }

    public void AdjustAmmo(int amount)
    {
        currentAmmo += amount;

        if (currentAmmo > weaponSO.magazineCapacity)
        {
            currentAmmo = weaponSO.magazineCapacity;
        }

        ammoText.text = currentAmmo.ToString("D2"); 
    }

    public void SwitchWeapon(WeaponSO weaponSO)
    {
        if (currentWeapon)
        {
            Destroy(currentWeapon.gameObject);
        }

        Weapon newWeapon = Instantiate(weaponSO.weaponPrefab, transform).GetComponent<Weapon>();
        currentWeapon = newWeapon;
        this.weaponSO = weaponSO;
        AdjustAmmo(weaponSO.magazineCapacity);
    }
    private void HandleShoot()
    {
        lastTimeShoot += Time.deltaTime;

        if (!starterAssetsInputs.shoot) return;

        if (lastTimeShoot >= weaponSO.fireRate && currentAmmo > 0)
        {
            currentWeapon.Shoot(weaponSO);
            anim.Play(SHOOT_STRING, 0, 0f);
            lastTimeShoot = 0f;
            AdjustAmmo(-1);
        }

        if (!weaponSO.isAutomatic)
        {
            starterAssetsInputs.ShootInput(false);
        }

    }

    private void HandleZoom()
    {
        if(!weaponSO.canZoom) return;

        if (starterAssetsInputs.zoom)
        {
            playerFollowCamera.m_Lens.FieldOfView = weaponSO.zoomAmount;
            weaponCamera.fieldOfView = weaponSO.zoomAmount;
            zoomImage.SetActive(true);
            firstPersonController.ChangeRotationSpeed(weaponSO.zoomRotationSpeed);
        }
        else
        {
            playerFollowCamera.m_Lens.FieldOfView = defaultFOV;
            weaponCamera.fieldOfView = defaultFOV;
            zoomImage.SetActive(false);
            firstPersonController.ChangeRotationSpeed(defaultRotationSpeed);
        }
    }
}
