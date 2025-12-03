using System.Collections;
using TMPro;
using UnityEngine;

public class HitPointUi : MonoBehaviour
{
    private float elapseTime;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] private float timeToDisappear;
    [SerializeField] private TMP_Text text;
    [SerializeField]
    private float speed;


    public void AssignText(float damage)
    {
        text.text = damage + "00";
        StartCoroutine(FadeText());
    }
    private void Update()
    {
        rb.velocity = Vector2.up * speed;
    }
    IEnumerator FadeText()
    {
        while (elapseTime < timeToDisappear)
        {
            elapseTime += Time.deltaTime;
            float t = elapseTime / timeToDisappear;

            text.alpha = Mathf.Lerp(text.alpha, 0, t);

            if (elapseTime >= timeToDisappear)
            { Destroy(gameObject); }
            yield return null;
        }


    }
}
