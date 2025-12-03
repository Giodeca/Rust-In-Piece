using UnityEngine;
using UnityEngine.InputSystem;

public class Aiming : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private GameObject player;

    [SerializeField] private float smoothSpeed = 10f; // Velocità di smussatura
    [SerializeField] private float joystickDeadZone = 0.2f; // Zona morta del joystick
    private Vector3 aimDirection;
    private Vector3 mousePos;
    private Vector3 lastDirection;

    private void Start()
    {
    }
    private void Update()
    {
        GetInput(); // Ottieni input da mouse o joystick
        SmoothRotateToDirection(); // Ruota in modo fluido verso la direzione indicata
        transform.position = player.transform.position; // Mantieni il puntatore sulla posizione del giocatore
    }

    private void GetInput()
    {
        float stickHorizontal = Input.GetAxis("RightStickHorizontal");
        float stickVertical = Input.GetAxis("RightStickVertical");

        if (Gamepad.current != null)
        {
            if (Mathf.Abs(stickHorizontal) > joystickDeadZone || Mathf.Abs(stickVertical) > joystickDeadZone)
                aimDirection = new Vector3(stickHorizontal, stickVertical, 0).normalized;
        }
        else
        {
            mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            aimDirection = (mousePos - transform.position).normalized;
        }
    }

    private void SmoothRotateToDirection()
    {
        if (aimDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);
        }
    }
}

