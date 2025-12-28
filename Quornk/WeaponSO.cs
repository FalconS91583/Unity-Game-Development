using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]

public class WeaponSO : ScriptableObject
{
    public GameObject weaponPrefab;
    public int damage = 1;
    public float fireRate = 0.5f;
    public float gunRange = 20f;
    public GameObject hitVFXPrefab;
    public bool isAutomatic = false;
    public bool canZoom = false;
    public float zoomAmount = 20f;
    public float zoomRotationSpeed = 0.4f;
    public int magazineCapacity = 12;

}
