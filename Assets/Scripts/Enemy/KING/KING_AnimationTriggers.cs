using UnityEngine;

public class KING_AnimationTriggers : EnemyAnimationTriggers
{
   private KING king;

    protected override void Awake()
    {
        base.Awake();
        king = GetComponentInParent<KING>();
    }
    private void TeleportTrigger()
    {
        king.SetTeleportTrigger(true);
    }
}
