using Cinemachine;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private RaycastHit hit;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private LayerMask interactionLayer;

    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void Shoot(WeaponSO weapon)
    {
        muzzleFlash.Play();
        impulseSource.GenerateImpulse();

        if (Physics.Raycast
            (Camera.main.transform.position, Camera.main.transform.forward, out hit, weapon.gunRange, interactionLayer, QueryTriggerInteraction.Ignore))
        {
          Instantiate(weapon.hitVFXPrefab, hit.point, Quaternion.identity);

           EnemyHealth enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
           enemyHealth?.TakeDamage(weapon.damage);
        }
        
    }
}
