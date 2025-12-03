using UnityEngine;
public class PlayerShootState : PlayerState, IModule
{
    public PlayerShootState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }


    private ModuleState moduleState = ModuleState.Normal;

    public ModuleState ModuleState => moduleState;

    private ModuleType moduleType = ModuleType.Shoot;
    public ModuleType ModuleType => moduleType;

    public override void Enter()
    {
        base.Enter();


        if (player.chargedShotTimeCounter >= player.ChargedShotTime)
        {
            stateMachine.ChangeState(player.ChargedShotState);
        }
        else
        {
            player.NormalShot();
        }

        //player.ResetChargeShotSounds();


        //stateTimer = 0.5f;
    }

    public override void Exit()
    {
        base.Exit();
        player.chargedShotTimeCounter = 0;
        player.SetShootCooldownTimer();

    }

    public override void Update()
    {
        base.Update();



        /*if (stateTimer < 0 && !player.IsGroundDetected())
            stateMachine.ChangeState(player.AirState);
        else if (stateTimer < 0)
            stateMachine.ChangeState(player.MoveState);*/
        WhenAbilityUsed();

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.AirState);
        else
            stateMachine.ChangeState(player.MoveState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.Move(player.DefaultSpeed * player.MoveInput.x, rb.velocity.y);
        //player.InstantVelocity(player.MoveInput.x * player.DefaultSpeed, rb.velocity.y);
    }

    public void WhenDamaged()
    {
        player.damagedModuleCounter = Mathf.Clamp(player.damagedModuleCounter + 1, 0, player.playerModules.Count);

        moduleState = ModuleState.Damaged;
        EventManager.ModuleChangeState?.Invoke(moduleType, moduleState);
    }

    public void WhenDestroyed()
    {
        player.damagedModuleCounter = Mathf.Clamp(player.damagedModuleCounter + 1, 0, player.playerModules.Count);

        moduleState = ModuleState.Destroyed;
        EventManager.ModuleChangeState?.Invoke(moduleType, moduleState);
    }

    public void WhenRepaired()
    {
        player.damagedModuleCounter = Mathf.Clamp(player.damagedModuleCounter - 1, 0, player.playerModules.Count);

        moduleState = ModuleState.Normal;
        EventManager.ModuleChangeState?.Invoke(moduleType, moduleState);
    }

    public void Save(ref ModuleData data)
    {
        data.ModuleState = moduleState;
        data.ModuleType = moduleType;
    }

    public void Load(ModuleData data)
    {
        moduleState = data.ModuleState;
        moduleType = data.ModuleType;

        switch (moduleState)
        {
            case ModuleState.Normal:
                WhenRepaired();
                break;
            case ModuleState.Damaged:
                WhenDamaged();
                break;
            case ModuleState.Destroyed:
                WhenDestroyed();
                break;
            default:
                break;
        }
    }
    public void WhenAbilityUsed()
    {
        EventManager.ModuleUsed?.Invoke(moduleType);
    }
}
