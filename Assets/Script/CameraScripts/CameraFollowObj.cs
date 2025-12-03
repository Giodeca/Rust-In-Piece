using System.Collections;
using UnityEngine;

public class CameraFollowObj : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float flipYRoatationTime = 0.5f;

    private Coroutine turnCoroutine;
    private Player player;

    private bool isFacingRight;

    private void Awake()
    {

    }
    private void Start()
    {
        player = PlayerManager.instance.player;
    }
    private void Update()
    {
        transform.position = player.transform.position;
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipLerp());
    }

    private IEnumerator FlipLerp()
    {
        float startDirection = transform.eulerAngles.y;
        float endRoatation = DetermineEndRotation();
        float yRotation = 0f;

        float elapseTime = 0f;
        while (elapseTime < flipYRoatationTime)
        {
            elapseTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startDirection, endRoatation, (elapseTime / flipYRoatationTime));
            transform.rotation = Quaternion.Euler(0, yRotation, 0);

            yield return null;

        }
    }

    private float DetermineEndRotation()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
            return 180f;
        else
            return 0f;
    }
}
