using BramaBadura.Saving;
using BramaBadura.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace BramaBadura.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        private LazyValue<float> health;

        bool isDead = false;

        [SerializeField] private float regenrationProcentage = 70;
        [SerializeField] private TakeDamageEvent takeDamge;
        [SerializeField] private UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float> 
        {

        }

        private void Awake()
        {
            health = new LazyValue<float>(GetInitialHealth);
            anim = GetComponent<Animator>();
        }
        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetHealth(Stat.Health);
        }

        private void Start()
        {
            GetComponent<BaseStats>().onLevelUp += Regenratehealth;
            health.ForceInit();
        }
        private void Regenratehealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetHealth(Stat.Health) * (regenrationProcentage / 100);
            health.value = Mathf.Max(health.value, regenHealthPoints);
        }

        public bool IsDead()
        {
            return isDead;
        }

        private Animator anim;
        public void TakeDamage(GameObject instigator ,float amout)
        {
            health.value = Mathf.Max(health.value - amout, 0);

            takeDamge.Invoke(amout);
            if (health.value <= 0)
            {
                onDie.Invoke();
                Die();
                AwardXP(instigator);
            }
        }

        public float GetHealthPoints()
        {
            return health.value;
        }

        public float GetMaxhealthPoints()
        {
            return GetComponent<BaseStats>().GetHealth(Stat.Health);
        }

        void AwardXP(GameObject instigator)
        {
            if (instigator == null) return;

            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainXP(GetComponent<BaseStats>().GetHealth(Stat.ExperienceReward));
        }

        public float GetProcentage()
        {
            return 100 * GetFraction();
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            GetComponent<Rigidbody>().isKinematic = true;

            if (anim == null)
            {
                anim = GetComponent<Animator>();
            }

            if (anim != null)
            {
                anim.SetTrigger("die");
            }
            else
            {
                Debug.LogError("Animator is NULL in Health.Die()");
            }

            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public float GetFraction()
        {
            return health.value / GetComponent<BaseStats>().GetHealth(Stat.Health);
        }
        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            if (anim == null) anim = GetComponent<Animator>();

            health.value = (float)state;
            if (health.value <= 0)
            {
                Die();
            }
        }

        public void Heal(float healToRestore)
        {
            health.value = Mathf.Min(health.value + healToRestore, GetMaxhealthPoints());
        }

    }

}

