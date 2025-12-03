using UnityEngine;

public class PlayerMoveState : PlayerGroundedState, IModule, ISavable<ModuleData>
{
    private ModuleState moduleState = ModuleState.Normal;
    public ModuleState ModuleState { get => moduleState; }

    private ModuleType moduleType = ModuleType.Movement;
    public ModuleType ModuleType => moduleType;
    private float stepTimer = 0.35f; // Timer per controllare i passi

    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }


    public override void Enter()
    {
        base.Enter();
        player.walkVFX.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        player.walkVFX.SetActive(false);
    }

    public override void Update()
    {
        base.Update();

        //player.Look();
        //player.Move(player.DefaultSpeed * player.MoveInput.x, rb.velocity.y);
        //player.InstantVelocity(player.MoveInput.x * player.DefaultSpeed, rb.velocity.y);
        //WhenAbilityUsed();
        DetectSteps();

        if (player.MoveInput.x == 0 || player.isWallDetected())
            stateMachine.ChangeState(player.IdleState);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.Move(player.DefaultSpeed * player.MoveInput.x, rb.velocity.y);
    }

    public void WhenDamaged()
    {
        //Debug.LogError("MOVEMENT DANNEGGIATO");

        moduleState = ModuleState.Damaged;
        EventManager.ModuleChangeState?.Invoke(moduleType, moduleState);
        player.Die();
    }

    public void WhenDestroyed()
    {
        WhenDamaged();
        EventManager.ModuleChangeState?.Invoke(moduleType, moduleState);
    }

    public void WhenRepaired()
    {
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

    public void DetectSteps()
    {
        if (player.MoveInput.x != 0 && player.IsGroundDetected() && stepTimer <= 0f)
        {
            player.PlayStepSound(); // Riproduci il suono
            stepTimer = player.stepInterval; // Resetta il timer
        }

        // Riduci il timer
        if (stepTimer > 0f)
        {
            stepTimer -= Time.deltaTime;
        }
    }
}
