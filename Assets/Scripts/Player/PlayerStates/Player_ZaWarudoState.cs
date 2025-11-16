using UnityEngine;

public class Player_ZaWarudoState : PlayerState
{
    private Vector2 originalPosition;
    private float originalGravity;
    private float finalRiseDistance;

    private bool isLevitating;
    private bool createdDomain;
    public Player_ZaWarudoState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }

    public override void Enter()
    {
        base.Enter();

        originalPosition = player.transform.position;
        originalGravity = rigidbody.gravityScale;
        finalRiseDistance = GetAvalableRiseDistance();

        player.SetVelocity(0, player.riseSpeed);
        player.health.SetCanTakeDamage(false);
    }

    public override void Update()
    {
        base.Update();

        if(Vector2.Distance(originalPosition, player.transform.position) >= finalRiseDistance && isLevitating == false)
            Levitate();
        if (isLevitating)
        {
            skillManager.ultimate.DoSpellCasting();

            if (stateTimer < 0)
            {
                isLevitating = false;
                createdDomain = false;
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
        rigidbody.gravityScale = originalGravity;
        player.health.SetCanTakeDamage(true);
    }
    private void Levitate()
    {
        isLevitating = true;
        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.gravityScale = 0;

        stateTimer = skillManager.ultimate.GetDomainDuration();

        if (createdDomain == false)
        {
            createdDomain = true;
            skillManager.ultimate.CreateDomain();
        }
    }

    private float GetAvalableRiseDistance()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector2.up, player.riseMaxDistance, player.whatIsGround);
        return hit.collider != null? hit.distance - 1 : player.riseMaxDistance;
    }
}
