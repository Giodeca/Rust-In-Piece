using UnityEngine;

public enum ModuleState
{
    Normal,
    Damaged,
    Destroyed
}
public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.Anim.SetBool(animBoolName, true);
        //player.weaponAnim.SetBool(animBoolName, true);
        rb = player.Rb;
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        player.MoveInput = new Vector3(player.InputManager.Movement().normalized.x, 0, rb.velocity.y);
        player.Anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void FixedUpdate() { }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
        //player.weaponAnim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
