using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter Attack Details")]
    [SerializeField] private float counterRecovery = .1f;
    [SerializeField] private LayerMask whatIsCounterable;

    public bool CounterAttackPerformed()
    {
        bool hasPerformedCounter = false;  

        foreach (var target in GetDetectedColliders(whatIsCounterable))
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if (counterable == null)
                continue;

            if (counterable.CanBeCountered)
            {
                counterable.HandleCouter();
                hasPerformedCounter = true;
            }
        }

        return hasPerformedCounter;
    }

    public float GetCounterRecoveryDuration() => counterRecovery;
}
