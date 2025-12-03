using UnityEngine;
using UnityEngine.Rendering;

public class CRTVolume : MonoBehaviour
{
    [SerializeField] private string parameter;
    [SerializeField] private Volume volume;

    private int isActive;
    // Start is called before the first frame update
    void Start()
    {
        SetCameraVolume();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnOffCameraVolume(bool _isActive)
    {
        if (_isActive)
            isActive = 1;
        else
            isActive = 0;

        volume.enabled = _isActive;

        PlayerPrefs.SetInt(parameter, isActive);
    }

    private void SetCameraVolume()
    {
        try
        {
            isActive = PlayerPrefs.GetInt(parameter);

            if (isActive == 1)
                volume.enabled = true;
            else
                volume.enabled = false;
        }
        catch
        {
            volume.enabled = true;
        }
    }

    public bool GetActiveValue()
    {
        bool active;
        try
        {
            isActive = PlayerPrefs.GetInt(parameter);

            if (isActive == 1)
                active = true;
            else
                active = false;
        }
        catch
        {
            active = true;
        }

        return active;
    }

    private void OnEnable()
    {
        EventManager.OnCameraVolumeChanged += OnOffCameraVolume;
        EventManager.OnReturnCameraVolumeValue += GetActiveValue;
    }

    private void OnDisable()
    {
        EventManager.OnCameraVolumeChanged -= OnOffCameraVolume;
        EventManager.OnReturnCameraVolumeValue += GetActiveValue;

    }
}
