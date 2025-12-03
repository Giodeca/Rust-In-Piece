using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallaxEffect; //Indica un offset tra il centro della telecamera e il centro del background. È serialize field, cioè da impostare dall'inspector
    [SerializeField] private float yOffset;
    private float xPosition;
    private float lenght;

    void Start()
    {
        cam = GameObject.Find("Main Camera");

        lenght = GetComponent<SpriteRenderer>().bounds.size.x; //lunghezza del background.
        xPosition = transform.position.x; //la sua posizione appena viene avviato il programma
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(xPosition + distanceToMove, cam.transform.position.y + yOffset);

        if (distanceMoved > xPosition + lenght)
            xPosition = xPosition + lenght;
        else if (distanceMoved < xPosition - lenght)
            xPosition = xPosition - lenght;
    }
}
