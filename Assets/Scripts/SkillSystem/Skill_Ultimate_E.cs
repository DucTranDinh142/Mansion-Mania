using System.Collections.Generic;
using UnityEngine;

public class Skill_Ultimate_E : Skill_Base
{
    [SerializeField] private GameObject ultimatePrefab;

    [Header("Slowdown Upgrade")]
    [SerializeField] private float slowDownPercent = 1;
    [SerializeField] private float slowDownDomainDuration = 4;

    [Header("Shard casting Upgrade")]
    [SerializeField] private int shardsToCast = 16;
    [SerializeField] private float shardCastingDomainSlowDown = 0.8f;
    [SerializeField] private float shardCastingDomainDuration = 6;
    private float spellcastTimer;
    private float spellsPerSecond;

    [Header("Time echo casting Upgrade")]
    [SerializeField] private int echosToCast = 10;
    [SerializeField] private float echoCastingDomainSlowDown = 0.8f;
    [SerializeField] private float echoCastingDomainDuration = 6;

    [Header("Ultimate details")]
    public float maxDomainSize = 10;
    public float expandSpeed = 3;

    private List<Enemy> trappedTargets = new List<Enemy>();
    private Transform currentTarget;

    public void DoSpellCasting()
    {
        spellcastTimer -= Time.deltaTime;

        if (currentTarget == null)
            currentTarget = FindTargetInDomain();

        if (currentTarget != null && spellcastTimer < 0)
        {
            CastSpell(currentTarget);
            spellcastTimer = 1 / spellsPerSecond;
            currentTarget = null;
        }
    }
    private void CastSpell(Transform target)
    {
        if (upgradeType == SkillUpgradeType.Ultimate_EchoSpam_E2)
        {
            Vector3 offset = Random.value < 0.5f ? new Vector2(1, 0) : new Vector2(-1, 0);
            skillManager.timeEcho.CreateTimeEcho(target.position + offset);
        }
        if (upgradeType == SkillUpgradeType.Ultimate_ShardSpam_E1)
        {
            skillManager.shard.CreateRawShard(target, true);
        }
    }
    private Transform FindTargetInDomain()
    {
        trappedTargets.RemoveAll(target => target == null || target.health.isDead);

        if(trappedTargets.Count ==0) return null;

        int randomIndex = Random.Range(0, trappedTargets.Count);
        return trappedTargets[randomIndex].transform;
    }
    public float GetDomainDuration()
    {
        if (upgradeType == SkillUpgradeType.Ultimate_E)
            return slowDownDomainDuration;
        else if (upgradeType == SkillUpgradeType.Ultimate_ShardSpam_E1)
            return shardCastingDomainDuration;
        else if (upgradeType == SkillUpgradeType.Ultimate_EchoSpam_E2)
            return echoCastingDomainDuration;
        return 0;
    }
    public float GetSlowPercent()
    {
        if (upgradeType == SkillUpgradeType.Ultimate_E)
            return slowDownPercent;
        else if (upgradeType == SkillUpgradeType.Ultimate_ShardSpam_E1)
            return shardCastingDomainSlowDown;
        else if (upgradeType == SkillUpgradeType.Ultimate_EchoSpam_E2)
            return echoCastingDomainSlowDown;
        return 0;
    }
    public int GetSpellsToCast()
    {
        if (upgradeType == SkillUpgradeType.Ultimate_ShardSpam_E1)
            return shardsToCast;
        else if (upgradeType == SkillUpgradeType.Ultimate_EchoSpam_E2)
            return echosToCast;
        return 0;
    }
    public bool InstantDomain()
    {
        return upgradeType != SkillUpgradeType.Ultimate_ShardSpam_E1
            && upgradeType != SkillUpgradeType.Ultimate_EchoSpam_E2;
    }
    public void CreateDomain()
    {
        spellsPerSecond = GetSpellsToCast() / GetDomainDuration();
        GameObject ultimate = Instantiate(ultimatePrefab, transform.position, Quaternion.identity);
        ultimate.GetComponent<SkillObject_Ultimate>().SetupDomain(this);
    }
    public void AddTarget(Enemy target)
    {
        trappedTargets.Add(target);
    }

    public void ClearTargets()
    {
        foreach (var enemy in trappedTargets)
            enemy.StopSlowDown();

        trappedTargets = new List<Enemy>();
    }
}