using UnityEngine;

public class EnemyAnimationTriggers : EntityAnimationTriggers
{
    private Enemy enemy;
    private Enemy_VFX enemyVFX;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        enemyVFX = GetComponentInParent<Enemy_VFX>();
    }
    private void EnableCounterWindow()
    {
        enemyVFX.EnableAttackAlert(true);
        enemy.EnableCounterWindow(true);
    }
    private void DisableCounterWindow()
    {
        enemyVFX.EnableAttackAlert(false);
        enemy.EnableCounterWindow(false);
    }
}

