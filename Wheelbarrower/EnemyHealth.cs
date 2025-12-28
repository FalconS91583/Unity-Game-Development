using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 5;

    [SerializeField] private int currentHitPoint = 0;

    [SerializeField] private int difficultyRamp = 1;

    private Enemy enemy;
    private void OnEnable()
    {
        currentHitPoint = maxHitPoints;
    }

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    private void ProcessHit()
    {
        currentHitPoint--;
        if(currentHitPoint <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }
}
