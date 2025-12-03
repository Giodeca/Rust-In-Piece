using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ModuleUI : MonoBehaviour
{
    public ModuleState ModuleState;
    public ModuleType ModuleType;

    [SerializeField] private Image image;
    [SerializeField] private Color coloToSet;
    [SerializeField] private Color coloBase;

    [SerializeField] private Color colorDamaged;
    [SerializeField] private Color colorNormal;
    [SerializeField] private Color colorDestroyed;
    private bool isColorChanged;

    private void OnEnable()
    {
        EventManager.ModuleChangeState += OnStateChange;
        EventManager.ModuleUsed += OnModuleUsed;
        //EventManager.DestroyModule += OnModuleDestroy;
    }

    private void OnDisable()
    {
        EventManager.ModuleChangeState -= OnStateChange;
        EventManager.ModuleUsed -= OnModuleUsed;
        //EventManager.DestroyModule -= OnModuleDestroy;
    }

    private void Start()
    {
        ColorSetUp();
    }
    public void SetModuleUI()
    {
        if (ModuleState == ModuleState.Damaged)
        {
            image.color = colorDamaged;
        }
    }

    private void OnStateChange(ModuleType mT, ModuleState mS)
    {
        if (mT == ModuleType)
        {
            ModuleState = mS;
            ColorSetUp();
        }
    }

    internal void ColorSetUp()
    {
        switch (ModuleState)
        {
            case ModuleState.Damaged:
                image.color = colorDamaged;
                coloBase = image.color;
                break;
            case ModuleState.Normal:
                image.color = colorNormal;
                coloBase = image.color;
                break;
            case ModuleState.Destroyed:
                image.color = colorDestroyed;
                coloBase = image.color;
                break;

        }
    }

    private void OnModuleUsed(ModuleType module)
    {
        if (module == this.ModuleType && ModuleState == ModuleState.Normal && !isColorChanged)
        {
            isColorChanged = true;
            StartCoroutine(ColorAnim());
        }
    }

    private IEnumerator ColorAnim()
    {
        image.color = coloToSet;
        yield return new WaitForSeconds(1f);
        image.color = coloBase;
        isColorChanged = false;
    }
}
