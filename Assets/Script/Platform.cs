using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{

    [Header("SET ONLY IF THE PLATFORM NEEDS TO MOVE")]
    [SerializeField] private Transform[] points;
    [SerializeField] private bool moving;
    [SerializeField] private float lerpDuration;

    private int index;
    private RigidbodyInterpolation2D interpolation2D;
    private Rigidbody2D rb2D;
    // Start is called before the first frame update
    void Start()
    {
        if (moving)
        {
            index = 0;
            StartCoroutine(Move());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Move()
    {

        while (true)
        {
            yield return MoveEachPoint(points[index]);

            if (index < points.Length - 1)
                index++;
            else
                index = 0;
        }
    }

    private IEnumerator MoveEachPoint(Transform point)
    {
        Vector2 startPos = transform.position;
        float t = 0;

        float percentage = 0;

        while (percentage <= 1)
        {
            yield return null;
            t += Time.deltaTime;
            percentage = t / lerpDuration;
            transform.position = Vector3.Lerp(startPos, point.position, percentage);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            rb2D = collision.gameObject.GetComponent<Rigidbody2D>();
            if (collision.transform.position.y > transform.position.y)
            {
                interpolation2D = rb2D.interpolation;
                rb2D.interpolation = RigidbodyInterpolation2D.None;
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            rb2D.interpolation = interpolation2D;
            collision.transform.SetParent(null);
        }
    }
}
