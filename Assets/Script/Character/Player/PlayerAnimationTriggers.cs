using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    //[SerializeField] private ParticleSystem weaponTrails;
    private Player player => GetComponentInParent<Player>();

    /*private void PlayVFX()
    {
        weaponTrails.Play();
    }*/

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    /*private void Reload()
    {
        player.Reload();
    }*/

    /*private void ShootTrigger()
    {
        player.Shoot();
    }*/

   
}
