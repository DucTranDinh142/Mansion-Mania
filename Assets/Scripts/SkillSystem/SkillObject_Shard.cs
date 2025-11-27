using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SkillObject_Shard : SkillObject_Base
{
    public event Action OnExplode;
    private Skill_Shard_C shardManager;

    private Transform target;
    private float speed;

    private void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed*Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float speed, Transform newTarget = null)
    {
        target = newTarget == null ? ClosestTarget() : newTarget;
        this.speed = speed;
    }
    public void SetupShard(Skill_Shard_C shardManager)
    {
        this.shardManager = shardManager;

        playerStats = shardManager.player.stats;
        scaleData = shardManager.scaleData;

        float detonationTime = shardManager.GetDetonateTime();

        Invoke(nameof(Explode), detonationTime);
    }
    public void SetupShard(Skill_Shard_C shardManager, float detonationTime,bool canMove, float shardSpeed, Transform target = null)
    {

        this.shardManager = shardManager;

        playerStats = shardManager.player.stats;
        scaleData = shardManager.scaleData;

        Invoke(nameof(Explode), detonationTime);

        if (canMove)
            MoveTowardsClosestTarget(shardSpeed, target);
    }
 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null) return;

        Explode();
    }

    public void Explode()
    {
        //Megumi method
        DamageEnemiesInRadius(transform, checkRadius, shardManager);

        OnExplode?.Invoke();
        Destroy(gameObject);
    }
}