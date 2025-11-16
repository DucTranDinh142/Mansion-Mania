using System;
using System.Collections;
using UnityEngine;

public class Skill_Shard_C : Skill_Base
{
    private SkillObject_Shard currentShard;
    private Entity_Health playerHealth;

    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private GameObject SAVEPrefab;
    [SerializeField] private float detonateTime = 1.5f;
    GameObject preferPrefab;

    [Header("Moving Shard Upgrade")]
    [SerializeField] private float shardSpeed = 6.1f;

    [Header("Multicast Shard Upgrade")]
    [SerializeField] private int maxCharge = 3;
    [SerializeField] private int currentCharge;
    [SerializeField] private bool isCharging;
    [Header("Teleport Shard Upgrade")]
    [SerializeField] private float shardExistDuration = 4;
    [Header("HP Rewind Shard Upgrade")]
    [SerializeField] private float saveHealPercent; 

    protected override void Awake()
    {
        base.Awake();
        currentCharge = maxCharge;
        playerHealth = GetComponentInParent<Entity_Health>();
    }
    private void Update()
    {
        if (Unlock(SkillUpgradeType.Shard_Teleport_C2) || Unlock(SkillUpgradeType.Shard_TeleportHPRewind_C21))
            preferPrefab = SAVEPrefab;
        else preferPrefab = shardPrefab;
       // preferPrefab = Unlock(SkillUpgradeType.Shard_Teleport_C2 ) ? SAVEPrefab : shardPrefab;
    }
    public override void TryUseSkill()
    {
        if (CanUseSkill() == false) return;

        if (Unlock(SkillUpgradeType.Shard_C))
            HandleShardRegular();
        if(Unlock(SkillUpgradeType.Shard_MoveToEnemy_C1))
            HandleShardMoving();
        if (Unlock(SkillUpgradeType.Shard_MulticastAndMoveToEnemy_C11))
            HandleShardMulticast();
        if(Unlock(SkillUpgradeType.Shard_Teleport_C2))
            HandleShardTeleport();
        if(Unlock(SkillUpgradeType.Shard_TeleportHPRewind_C21))
            HandleShardHealthRewind();
    }
    private void HandleShardHealthRewind()
    {
        if (currentShard == null)
        {
            CreateShard(preferPrefab);
            saveHealPercent = playerHealth.GetHPPercent();
            Debug.Log(saveHealPercent);
        }
        else
        {
            SwapPlayerAndShard();
            playerHealth.SetHPtoPercent(saveHealPercent);
            SetSkillOnCoolDown();
        }
    }
    private void HandleShardTeleport()
    {
        if (currentShard == null)
        {
            CreateShard(preferPrefab);
        }
        else
        {
            SwapPlayerAndShard();
            SetSkillOnCoolDown();
        }
    }
    private void SwapPlayerAndShard()
    {
        Vector3 shardPosition = currentShard.transform.position;
        Vector3 playerPosition = player.transform.position;
        currentShard.transform.position = playerPosition;
        currentShard.Explode();
        player.TeleportPlayer(shardPosition);
    }
    private void HandleShardMulticast()
    {
        if(currentCharge <= 0) return;

        CreateShard(preferPrefab);
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        currentCharge--;

        if(isCharging == false)
            StartCoroutine(ShardRechargeCoroutine());
    }
    private IEnumerator ShardRechargeCoroutine()
    {
        isCharging = true;

        while(currentCharge < maxCharge)
        {
            yield return new WaitForSeconds(cooldown);
            currentCharge++;
        }

        isCharging = false;
    }
    private void HandleShardMoving()
    {
        CreateShard(preferPrefab);
        currentShard.MoveTowardsClosestTarget(shardSpeed);
        SetSkillOnCoolDown();
    }
    private void HandleShardRegular()
    {
        CreateShard(preferPrefab);
        SetSkillOnCoolDown();
    }

    public void CreateShard(GameObject prefab)
    {
        GameObject shard = Instantiate(prefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<SkillObject_Shard>();
        currentShard.SetupShard(this);
        if (Unlock(SkillUpgradeType.Shard_Teleport_C2) || Unlock(SkillUpgradeType.Shard_TeleportHPRewind_C21))
            currentShard.OnExplode += ForceCooldown;
    }

    public void CreateRawShard(Transform target = null, bool shardsCanMove = false)
    {
        bool canMove = shardsCanMove == false? 
            Unlock(SkillUpgradeType.Shard_MoveToEnemy_C1) || Unlock(SkillUpgradeType.Shard_MulticastAndMoveToEnemy_C11)
            : shardsCanMove;

        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        shard.GetComponent<SkillObject_Shard>().SetupShard(this, detonateTime, canMove, shardSpeed, target);
    }

    public float GetDetonateTime()
    {
        if (Unlock(SkillUpgradeType.Shard_Teleport_C2)|| Unlock(SkillUpgradeType.Shard_TeleportHPRewind_C21))
            return shardExistDuration;

        return detonateTime;
    }
    private void ForceCooldown()
    {
        if (OnCooldown() == false)
        {
            SetSkillOnCoolDown();
            currentShard.OnExplode -= ForceCooldown;
        }
    }
}
