using UnityEngine;

public class PlayerStoppedState : PlayerState
{
    public PlayerStoppedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //Debug.LogError("ENTER Stopped STATE");

        EventManager.OnCameraShake.Invoke();

        AudioManager.instance.PlaySFX(2, PlayerManager.instance.player.transform);
        if (!player.IsGroundDetected())
            player.Rb.gravityScale = player.originalGravityScale * player.descendCoefficient;

        player.InputManager.OnDisable();
        stateTimer = player.TimeStopDuration;

        Time.timeScale = 0;

        /*Debug.Log($"stateTimer: {stateTimer}");

        stateTimer -= Time.unscaledDeltaTime;
        
        Debug.Log($"stateTimer: {stateTimer}");*/
    }

    public override void Exit()
    {
        base.Exit();
        Time.timeScale = 1;

        player.Rb.gravityScale = player.originalGravityScale;
        //player.InputManager.OnEnable();
    }


    public override void Update()
    {
        base.Update();

        stateTimer -= Time.unscaledDeltaTime;

        Debug.Log(stateTimer);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.DamagedState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
