using UnityEngine;

public class AmmoPickUp : PickUp
{

    [SerializeField] private int ammoAmout = 100;
    protected override void OnPickUp(ActiveWeapon activeWeapon)
    {
        activeWeapon.AdjustAmmo(ammoAmout);
    }
}
