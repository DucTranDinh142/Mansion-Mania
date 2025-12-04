using UnityEngine;

public class KING_SpellCastState : EnemyState
{
    private KING king;
    public KING_SpellCastState(Enemy enemy, StateMachine stateMachine, string animatorBoolName) : base(enemy, stateMachine, animatorBoolName)
    {
        king = enemy as KING;
    }
    public override void Enter()
    {
        base.Enter();
        king.SetVelocity(0, 0);
        king.SetSpellCastPerformed(false);
    }
    public override void Update()
    {
        base.Update();

        if (king.spellCastPerformed)
            animator.SetBool("SpellCast_Performed", true);

        if (triggerCalled)
            stateMachine.ChangeState(king.kingBattleState);
    }
    public override void Exit()
    {
        base.Exit();
        king.SetSpellCastOnCooldown();
        animator.SetBool("SpellCast_Performed", false);
    }

}
