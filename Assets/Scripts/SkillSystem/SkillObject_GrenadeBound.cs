using System.Collections.Generic;
using UnityEngine;

public class SkillObject_GrenadeBound : SkillObject_Sword
{
    [SerializeField] private float bounceSpeed = 16;
    private int bounceCount = 4;

    private Collider2D[] enemyTargets;
    private Transform nextTarget;
    private List<Transform> selectedBefore = new List<Transform>();

    public override void SetUpSword(Skill_SwordThrow_D swordManager, Vector2 direction)
    {
        animator.SetTrigger("Bounce");
        base.SetUpSword(swordManager, direction);

        bounceSpeed = swordManager.bounceSpeed;
        bounceCount = swordManager.bounceCount;
    }
    protected override void Update()
    {
        HandleComeback();
        HandleBounce();
    }
    private void HandleBounce()
    {
        if (nextTarget == null)
            return;

        transform.position = Vector2.MoveTowards(transform.position, nextTarget.position, bounceSpeed*Time.deltaTime);

        if(Vector2.Distance(transform.position, nextTarget.position) < .75f)
        {
            DamageEnemiesInRadius(transform,checkRadius,swordManager);
            BounceToNextTarget();

            if(bounceCount == 0 || nextTarget == null)
            {
                nextTarget = null;
                GetSwordBackToPlayer();
            }
        }
    }
    private void BounceToNextTarget()
    {
        nextTarget = GetNextTarget();
        bounceCount--;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyTargets == null)
        {
            enemyTargets = GetEnemiesAround(transform, 8);
            skillObjectRigidbody.simulated = false;
        }

        DamageEnemiesInRadius(transform, checkRadius, swordManager);
        if (enemyTargets.Length <= 1 || bounceCount == 0)
            GetSwordBackToPlayer();
        else 
            nextTarget = GetNextTarget();
    }

    private Transform GetNextTarget()
    {
        List<Transform> validTarget = GetValidTargets();

        int randomIndex = Random.Range(0, validTarget.Count);

        Transform nextTarget = validTarget[randomIndex];
        selectedBefore.Add(nextTarget);
        return nextTarget;
    }
    private List<Transform> GetValidTargets()
    {
        List<Transform> validTargets = new List<Transform>();

        foreach (var enemy in GetAliveTargets())
        {
            if(enemy != null && selectedBefore.Contains(enemy.transform) == false)
                validTargets.Add(enemy.transform);
        }
        if(validTargets.Count > 0)
            return validTargets;
        else
        {
            selectedBefore.Clear();
            return GetAliveTargets();
        }
    }
    private List<Transform> GetAliveTargets()
    {
        List<Transform> aliveTargets = new List<Transform>();
        
        foreach(var enemy in enemyTargets)
        {
            if(enemy != null)
                aliveTargets.Add(enemy.transform);
        }
        return aliveTargets;
    }

}