using UnityEngine;
using UnityEngine.UI;

public class FillHandler : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private Transform endPosition;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void Start()
    {
        SetSliderValues();
    }

    private void SetSliderValues()
    {
        slider.minValue = PlayerManager.instance.player.transform.position.x;
        slider.maxValue = endPosition.position.x;
    }

    private void Update()
    {
        slider.value = PlayerManager.instance.player.transform.position.x;
    }
}
