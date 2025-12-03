using UnityEngine;

public class WalkAnim : MonoBehaviour
{
    [SerializeField] private GameObject walkVfx;
    public void ActivateAnimRepair()
    {
        walkVfx.SetActive(true);
    }
    public void DeactivateAnimRepair()
    {
        walkVfx.SetActive(false);
    }
}
