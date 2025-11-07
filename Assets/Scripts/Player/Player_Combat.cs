using UnityEngine;

public class Player_Combat : Entity_Combat
{
    [Header("Counter Attack Details")]
    [SerializeField] private float counterRecovery;
    public bool CounterAttackPerformed()
    {
        bool hasCounteredSomething = false;
        foreach (var target in GetDetectedColliders())
        {
            ICounterable counterable = target.GetComponent<ICounterable>();
            if (counterable == null)
                continue;

            if (counterable.CanBeCountered)
            {
                counterable.HandleCounter();
                hasCounteredSomething = true;
            }
        }
        return hasCounteredSomething;
    }
    public float GetCounterRecoveryDuration() => counterRecovery;
}
