using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform reticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, reticle.transform.rotation.eulerAngles.z * PlayerManager.instance.player.facingDir);

        if(PlayerManager.instance.player.facingDir > 0)
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z);
        else if(PlayerManager.instance.player.facingDir < 0)
            transform.rotation = Quaternion.Euler(180, 0, transform.rotation.eulerAngles.z);

    }
}
