using BramaBadura.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Projectile : MonoBehaviour
{
    private Health target = null;
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool isHoming = true;
    [SerializeField] private GameObject hitEffect = null;
    [SerializeField] private float maxLifeTime = 10f;
    [SerializeField] private GameObject[] destroyOnHit = null;
    [SerializeField] private float lifeAfterImapct = 2f;

    [SerializeField] private UnityEvent onHit;

    private float damage = 0;

    private GameObject instigator = null;
    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    void Update()
    {
        if (target == null) return;

        if (isHoming && !target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Health newTarget,GameObject newInstigator ,float newDamage)
    {
        target = newTarget;
        damage = newDamage;
        instigator = newInstigator;

        Destroy(this.gameObject, maxLifeTime);
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if(targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) return;
        if (target.IsDead()) return;
        target.TakeDamage(instigator, damage);

        speed = 0;

        onHit.Invoke();

        if(hitEffect != null)
        {
            Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        }
        foreach(GameObject toDestroy in destroyOnHit)
        {
            Destroy(toDestroy);
        }

        Destroy(this.gameObject, lifeAfterImapct);
    }
}
