using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{

    [SerializeField] private Transform wepaon;
    [SerializeField] private ParticleSystem projectileParticles;
    [SerializeField] private Transform target;
    [SerializeField] private float towerRange = 15f; 
    void Update()
    {
        FindClosestTarget();
        Aim();
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach(Enemy enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if(targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }

    private void Aim()
    {
        float targetDistance = Vector3.Distance(transform.position, target.position);


        wepaon.LookAt(target);

        if(targetDistance < towerRange)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    private void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
