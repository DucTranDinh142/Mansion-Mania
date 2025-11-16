using UnityEngine;

public class Player_SwordThrowState : PlayerState
{
    private Camera mainCamera;
    public Player_SwordThrowState(StateMachine stateMachine, string animatorBoolName, Player player) : base(stateMachine, animatorBoolName, player)
    {
    }
    public override void Enter()
    {
        base.Enter();
            if(mainCamera == null)
            mainCamera = Camera.main;
        skillManager.swordThrow.EnableDots(true);
    }
    public override void Update()
    {
        base.Update();

        Vector2 directionToMouse = DirectionToMouse();

        player.SetVelocity(0, rigidbody.linearVelocity.y);
        player.HandleFlip(directionToMouse.x);
        skillManager.swordThrow.PredictTrajectory(directionToMouse);

        if (input.Player.Attack.WasPressedThisFrame())
        {
            animator.SetBool("SwordThrowPerformed", true);

            skillManager.swordThrow.EnableDots(false);
            skillManager.swordThrow.ConfirmTrajectory(directionToMouse);
        }
        if (input.Player.RangeAttack.WasReleasedThisFrame() || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        animator.SetBool("SwordThrowPerformed", false);
        skillManager.swordThrow.EnableDots(false);
    }
    public Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(player.mousePosition);

        Vector2 direction = worldMousePosition - playerPosition;
        return direction.normalized;
    }
}
