using UnityEngine;

public class PlayerDashState : PlayerState, IModule, ISavable<ModuleData>
{
    /*float inputMagnitude;
    Quaternion rotation;*/

    private LayerMask playerExludedMasks;
    public ModuleState moduleState = ModuleState.Normal;
    public ModuleState ModuleState { get => moduleState; }

    private ModuleType moduleType = ModuleType.Dash;
    public ModuleType ModuleType => moduleType;
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(3, PlayerManager.instance.player.transform);
        if (stateTimer > 0)
            return;

        //rotation = Quaternion.LookRotation(player.moveInput, Vector3.up);

        player.dashUses--;
        //HUDManager.Instance.SetCooldownOf(HUDManager.Instance.dashImages[player.dashUses]);
        stateTimer = player.dashDuration;

        //player.GetComponent<CapsuleCollider>().excludeLayers = player.maskToExclude;

        //player.Rb.velocity = new Vector2(player.Rb.velocity.x, 0);

        player.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();
        //player.GetComponent<CapsuleCollider>().excludeLayers = player.defaultExclude;
        player.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        player.InstantVelocity(player.dashSpeed * player.dashDir, 0);
        WhenAbilityUsed();

        if (stateTimer < 0 && !player.IsGroundDetected())
            stateMachine.ChangeState(player.AirState);
        else if (stateTimer < 0)
            stateMachine.ChangeState(player.IdleState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //player.Move(player.dashSpeed * player.dashDir, 0);

        //player.InstantVelocity(player.dashSpeed * player.dashDir, 0);
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
