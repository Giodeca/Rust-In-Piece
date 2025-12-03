
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Rb.gravityScale = player.originalGravityScale * player.descendCoefficient;
    }

    public override void Exit()
    {
        base.Exit();

        //AudioManager.instance.PlaySFX(36, null);

        if (player.IsGroundDetected())
            player.Rb.gravityScale = player.originalGravityScale;


        if (player.MoveInput == Vector3.zero)
            player.Rb.velocity = new Vector2(0, rb.velocity.y);

    }


    public override void Update()
    {
        base.Update();

        player.coyoteTimeCounter -= Time.deltaTime;

        /*if (player.isWallDetected())
            stateMachine.ChangeState(player.wallSlideState);*/

        if (player.CanShoot() && player.InputManager.Shoot())
        {
            stateMachine.ChangeState(player.ShootState);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.MoveState);
        }

        if (player.InputManager.LongJump() && player.coyoteTimeCounter > 0)
        {
            //player.JumpCancelled = false; 
            stateMachine.ChangeState(player.JumpState);
        }
        else if (player.JumpState.moduleState == ModuleState.Normal && player.hasDoubleJump && player.InputManager.LongJump())
        {
            player.hasDoubleJump = false;
            stateMachine.ChangeState(player.JumpState);
        }

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        //player.SetVelocity(rb.velocity.x, rb.velocity.y * player.descendRate);

        if (player.MoveInput.x != 0)
        {
            player.Move(player.DefaultSpeed * player.MoveInput.x, rb.velocity.y);
            /*if (player.useLerpedMovement)
                player.LerpedVelocity(player.DefaultSpeed * player.MoveInput.x);
            else
                player.InstantVelocity(player.DefaultSpeed * player.MoveInput.x, rb.velocity.y);*/
        }
        else
            player.Move(0, rb.velocity.y);
        //player.InstantVelocity(0, rb.velocity.y);
    }
}
