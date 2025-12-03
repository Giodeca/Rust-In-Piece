using UnityEngine;

public class PlayerJumpState : PlayerState, IModule, ISavable<ModuleData>
{
    public ModuleState moduleState = ModuleState.Normal;
    private ModuleType moduleType = ModuleType.Jump;
    public ModuleType ModuleType => moduleType;
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public ModuleState ModuleState { get => moduleState; }

    public override void Enter()
    {
        base.Enter();

        player.Rb.gravityScale = player.originalGravityScale * player.descendCoefficient;


        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
        player.coyoteTimeCounter = -1;

        if (player.hasDoubleJump)
            AudioManager.instance.PlaySFX(5, PlayerManager.instance.player.transform);
        else
            AudioManager.instance.PlaySFX(4, PlayerManager.instance.player.transform);

        WhenAbilityUsed();

        //player.jumpBufferCounter = 0;

        //AudioManager.instance.PlaySFX(15, null);
    }

    public override void Exit()
    {
        base.Exit();
        /*AudioManager.instance.StopSFX(5);
        AudioManager.instance.StopSFX(4);*/

    }

    public override void Update()
    {
        base.Update();

        player.coyoteTimeCounter -= Time.deltaTime;

        if (player.CanShoot() && player.InputManager.Shoot())
        {
            stateMachine.ChangeState(player.ShootState);
        }

        /*if (player.JumpCancelled)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y/2);
            player.coyoteTimeCounter = 0;
            player.JumpCancelled = false;
        }*/

        if (player.InputManager.ShortJump())
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y / 2);

        }


        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.AirState);

        if (moduleState == ModuleState.Normal && player.hasDoubleJump && player.InputManager.LongJump())
        {
            //Debug.Log("DioBestia");
            player.hasDoubleJump = false;
            WhenAbilityUsed();
            stateMachine.ChangeState(player.JumpState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (player.MoveInput.x != 0)
            player.Move(player.DefaultSpeed * player.MoveInput.x, rb.velocity.y);
        //player.InstantVelocity(player.DefaultSpeed * player.MoveInput.x, rb.velocity.y);
        else
            player.Move(0, rb.velocity.y);
        //player.InstantVelocity(0, rb.velocity.y);
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

        try
        {
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
        catch
        {
            Debug.LogError("Invalid Parameter passef or Invalid values inside GameManager");
        }
    }

    public void WhenAbilityUsed()
    {

        EventManager.ModuleUsed?.Invoke(moduleType);
    }
}

