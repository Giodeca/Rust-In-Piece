using System.Collections;
using UnityEngine;


public class LaserScript : MonoBehaviour
{
    [SerializeField] private Transform transformScale;
    [SerializeField] private float scalingFactor;
    [SerializeField] private float actualScale;
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    [SerializeField] private float fireRate;
    [SerializeField] private float timerToActiovation;
    private float elapseTime;
    [SerializeField] private bool isFireFull;
    [SerializeField] private float duration;

    [SerializeField] private SpriteRenderer laserOnSprite;
    //private float spriteHeight = 4;
    //private float spriteMinHeight = 0;

    private Coroutine coroutine;


    private void Start()
    {
        actualScale = minScale;
        transform.localScale = new Vector3(transformScale.localScale.x, actualScale, transformScale.localScale.z);
        ResetTrap();

    }

    private void Update()
    {
        FireLogic();
    }

    private void FireLogic()
    {
        if (timerToActiovation < fireRate && !isFireFull)
        {
            timerToActiovation += Time.deltaTime;
        }
        else if (timerToActiovation >= fireRate && !isFireFull)
        {

            StartCoroutine(AnimationScale());
            timerToActiovation = 0;
            isFireFull = true;
        }

        if (isFireFull && timerToActiovation < fireRate)
        {

            timerToActiovation += Time.deltaTime;

            if (timerToActiovation >= fireRate)
            {

                StartCoroutine(AnimationScaleDecrease());
                ResetTrap();
            }


        }
    }

    private void ResetTrap()
    {

        timerToActiovation = 0;
        isFireFull = false;
    }

    private IEnumerator AnimationScale()
    {
        elapseTime = 0;

        AudioManager.instance.PlaySFX(27, transform);

        while (elapseTime < duration)
        {
            elapseTime += Time.deltaTime;
            float t = elapseTime / duration;
            laserOnSprite.size = new Vector2(laserOnSprite.size.x, Mathf.Lerp(0, 4, t));
            actualScale = Mathf.Lerp(minScale, maxScale, t);
            transformScale.localScale = new Vector3(transformScale.localScale.x, actualScale, transform.localScale.z);

            yield return null;
        }
    }

    private IEnumerator AnimationScaleDecrease()
    {
        elapseTime = 0;
        while (elapseTime < duration)
        {
            elapseTime += Time.deltaTime;
            float t = elapseTime / duration;
            laserOnSprite.size = new Vector2(laserOnSprite.size.x, Mathf.Lerp(4, 0, t));
            actualScale = Mathf.Lerp(maxScale, minScale, t);
            transformScale.localScale = new Vector3(transformScale.localScale.x, actualScale, transform.localScale.z);

            yield return null;
        }
    }
}
