public class PlayerGroundedState : PlayerState
{
    float scrollValue;

    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.hasDoubleJump = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.coyoteTimeCounter = player.coyoteTime;

        if (player.CanShoot() && player.InputManager.Shoot())
        {
            stateMachine.ChangeState(player.ShootState);
        }

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.AirState);

        if (player.InputManager.Repair().triggered && player.CanHeal())
        {
            //AudioManager.instance.PlaySFX(21, PlayerManager.instance.player.transform);
            stateMachine.ChangeState(player.RepairState);
        }
            

        /*if (player.jumpBufferCounter > 0 && player.coyoteTimeCounter > 0)
        {
            stateMachine.ChangeState(player.JumpState);
        }*/
        if (player.InputManager.LongJump() && player.coyoteTime > 0)
        {
            stateMachine.ChangeState(player.JumpState);
        }


        if (player.CanAttack() && player.InputManager.MeleeAttack())
        {
            //stateMachine.ChangeState(player.MeleeAttackState);
        }
    }
}
