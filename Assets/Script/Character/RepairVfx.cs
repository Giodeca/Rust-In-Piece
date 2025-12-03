using UnityEngine;

public class RepairVfx : MonoBehaviour
{
    [SerializeField] private GameObject reapairVfx;
    public void ActivateAnimRepair()
    {
        reapairVfx.SetActive(true);
    }
    public void DeactivateAnimRepair()
    {
        reapairVfx.SetActive(false);
    }
}
