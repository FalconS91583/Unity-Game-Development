using BramaBadura.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BramaBadura.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order =0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private AnimatorOverrideController weaponOverride = null;
        [SerializeField] private WeaponID weaponPrefab = null;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float weaponProcentageBonus = 0;
        [SerializeField] private float weaponRange = 1.5f;
        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private Projectile projectile = null;

        private const string weaponName = "Weapon";

        public void Spawn(Transform righthand, Transform lefthand, Animator anim)
        {
            DestroyOldWeapon(righthand, lefthand);
            if (weaponPrefab == null)
            {
                Debug.LogError("weaponPrefab is null in Weapon.Spawn");
                return;
            }
            if (anim == null)
            {
                Debug.LogError("Animator is null in Weapon.Spawn");
                return;
            }
            if (weaponPrefab != null)
            {
                Transform handTransform = GetTransform(righthand, lefthand);
                WeaponID weapon =  Instantiate(weaponPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            if (weaponOverride != null)
            {
                anim.runtimeAnimatorController = weaponOverride;
            }
            else
            {
                var overrideController = anim.runtimeAnimatorController as AnimatorOverrideController;
                if(overrideController != null)
                {
                    anim.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetTransform(Transform righthand, Transform lefthand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = righthand;
            else handTransform = lefthand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LauchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calcculatedDamage)
        {
            Projectile projectileInstance = 
                Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator,calcculatedDamage);
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetRange()
        {
            return weaponRange;
        }
        public float GetProcentageBonus()
        {
            return weaponProcentageBonus;
        }
    }

}
