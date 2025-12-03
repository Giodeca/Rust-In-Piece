

public class PlayerDamagedState : PlayerState
{
    public PlayerDamagedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(2, PlayerManager.instance.player.transform);
        if (!player.IsGroundDetected())
        player.Rb.gravityScale = player.originalGravityScale * player.descendCoefficient;

        player.InputManager.OnDisable();
        stateTimer = player.KnockbackDuration;

        UnityEngine.Debug.LogError("ENTER DAMAGED STATE");
    }

    public override void Exit()
    {
        base.Exit();

        player.Rb.gravityScale = player.originalGravityScale;
        player.InputManager.OnEnable();

        player.SetupZeroKnockbackPower();
    }


    public override void Update()
    {
        base.Update();

        player.HitKnockback();

        if (stateTimer < 0 && !player.IsGroundDetected())
            stateMachine.ChangeState(player.AirState);
        else if (stateTimer < 0)
            stateMachine.ChangeState(player.MoveState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
