using BramaBadura.Core;
using BramaBadura.Movement;
using BramaBadura.Saving;
using BramaBadura.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BramaBadura.Combat
{
    public class Fighter : MonoBehaviour, IAction, IModifierProvider
    {
        [SerializeField] private float timeBetweenAttack = 1f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon weapon = null;

        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Weapon currentWeaponConfig = null;

        private Animator anim;

        private void Awake()
        {
            currentWeaponConfig = weapon;
        }

        private void Start()
        {
            anim = GetComponent<Animator>();
            if(currentWeaponConfig == null)
            {
                EquipWeapon(weapon);
            }
        }
        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead()) return;


            if (!GetisInRange(target.transform))
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }
        public void EquipWeapon(Weapon weapon)
        {
            if (rightHandTransform == null || leftHandTransform == null)
            {
                Debug.LogError("Hand transforms are not assigned in Fighter.");
                return;
            }
            currentWeaponConfig = weapon;
            weapon.Spawn(rightHandTransform,leftHandTransform, anim);
        }

        public Health Gettarget()
        {
            return target;
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timeBetweenAttack)
            {
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
           anim.ResetTrigger("stopAttack");
           anim.SetTrigger("attack");
        }

        //Animation Event
        private void Hit()
        {
            if (target == null) return;

            float damage = GetComponent<BaseStats>().GetHealth(Stat.Damage);
            if (currentWeaponConfig.HasProjectile())
            {
                currentWeaponConfig.LauchProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }else
            {
                target.TakeDamage(gameObject ,damage);
            }
        }

        private void Shoot()
        {
            Hit();
        }

        private bool GetisInRange(Transform targetTransform)
        {
            return Vector3.Distance(transform.position, targetTransform.position) < currentWeaponConfig.GetRange();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && !GetisInRange(combatTarget.transform)) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject CombatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = CombatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            anim.ResetTrigger("attack");
            anim.SetTrigger("stopAttack");
            target = null;
            GetComponent<Mover>().Cancel();
        }

        public IEnumerable<float> GetModifier(Stat stat)
        {
            if(stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetDamage();
            }
        }

        public IEnumerable<float> GetProcentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeaponConfig.GetProcentageBonus();
            }
        }
    }
}


