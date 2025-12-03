using Unity.VisualScripting;
using UnityEngine;

public class PlayerRepairState : PlayerState, IModule, ISavable<ModuleData>
{
    public PlayerRepairState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }


    private ModuleState moduleState = ModuleState.Normal;
    public ModuleState ModuleState => moduleState;

    private ModuleType moduleType = ModuleType.Repair;
    public ModuleType ModuleType => moduleType;


    public override void Enter()
    {
        base.Enter();
        player.RepairCounter = player.repairTime;
        player.repairVFX.SetActive(true);

        Debug.LogWarning("Start Repair");
        AudioManager.instance.PlaySFX(21, PlayerManager.instance.player.transform);
    }

    public override void Exit()
    {
        base.Exit();
        player.repairVFX.SetActive(false);

    }

    public override void Update()
    {
        base.Update();

        if (player.InputManager.Repair().inProgress)
        {
            player.RepairCounter -= Time.deltaTime;
        }
        else
        {
            Debug.LogWarning("Repair Cancelled");
            stateMachine.ChangeState(player.IdleState);
            AudioManager.instance.StopSFX(21);
        }

        if (player.RepairCounter < 0)
        {
            player.Heal();
            WhenAbilityUsed();
            Debug.LogWarning("Repaired");
            stateMachine.ChangeState(player.MoveState);
        }

        /*if (player.MoveInput != Vector3.zero)
        {
            Debug.LogWarning("Repair Cancelled");
            stateMachine.ChangeState(player.MoveState);
        }*/
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void WhenDamaged()
    {
        player.damagedModuleCounter = Mathf.Clamp(player.damagedModuleCounter + 1, 0, player.playerModules.Count);

        moduleState = ModuleState.Damaged;
        EventManager.ModuleChangeState?.Invoke(moduleType, moduleState);
        player.SetPlayerRepairCost();
    }

    public void WhenDestroyed()
    {
        player.damagedModuleCounter = Mathf.Clamp(player.damagedModuleCounter + 1, 0, player.playerModules.Count);

        moduleState = ModuleState.Destroyed;
        EventManager.ModuleChangeState?.Invoke(moduleType, moduleState);
        player.SetPlayerRepairCost();
    }

    public void WhenRepaired()
    {
        player.damagedModuleCounter = Mathf.Clamp(player.damagedModuleCounter - 1, 0, player.playerModules.Count);

        moduleState = ModuleState.Normal;
        EventManager.ModuleChangeState?.Invoke(moduleType, moduleState);
        player.SetPlayerRepairCost();
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
