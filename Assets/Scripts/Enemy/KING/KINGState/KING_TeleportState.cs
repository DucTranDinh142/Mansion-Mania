using UnityEngine;

public class KING_TeleportState : EnemyState
{
    private KING king;
    public KING_TeleportState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
        king = enemy as KING;
    }

    public override void Update()
    {
        base.Update();

        if (king.teleportTrigger)
        {
            king.transform.position = king.FindTeleportPoint();
            king.SetTeleportTrigger(false);
        }
        if (triggerCalled)
        {
            if (king.CanDoSpellCast())
                stateMachine.ChangeState(king.kingSpellCastState);
            else
                stateMachine.ChangeState(king.kingBattleState);
        }
    }

}
