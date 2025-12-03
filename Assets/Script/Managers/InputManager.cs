using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonA<InputManager>
{
    private PlayerControls playerControls;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();

    }

    public Vector2 Movement()
    {
        return playerControls.Player.Movement.ReadValue<Vector2>();
    }

    public bool Shoot()
    {
        return playerControls.Player.Shoot.WasReleasedThisFrame();
    }

    public bool ChargeShot()
    {
        return playerControls.Player.Shoot.inProgress;
    }

    public InputAction Repair()
    {
        return playerControls.Player.Repair;
    }


    /*public bool Aim()
    {
        
    }*/

    public bool LongJump()
    {

        return playerControls.Player.Jump.triggered;
    }

    public bool ShortJump()
    {

        return playerControls.Player.Jump.WasReleasedThisFrame();
    }

    public bool Dash()
    {
        return playerControls.Player.Dash.triggered;
    }

    public bool MeleeAttack()
    {
        return playerControls.Player.MeleeAttack.triggered;
    }


    public void OnEnable()
    {
        playerControls.Enable();
    }

    public void OnDisable()
    {
        playerControls.Disable();
    }
}
