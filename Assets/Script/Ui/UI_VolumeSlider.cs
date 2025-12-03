using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;
    public float valueSave;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    public AudioMixer AudioMixer { get { return audioMixer; } }
    public float Multiplier { get { return multiplier; } }

    private void Awake()
    {

    }

    private void Start()
    {

    }
    public void SliderValue(float _value)
    {
        if (_value <= 0)
            _value = 0.001f;

        var value = Mathf.Log10(_value);

        audioMixer.SetFloat(parameter, value * multiplier);
        PlayerPrefs.SetFloat(parameter, slider.value);
        //valueSave = slider.value;
    }

    public void SaveDataSound(ref SerializableDictionary<string, DataSoundSlider> valueDictionary)
    {
        DataSoundSlider dataS = new DataSoundSlider();
        //if (!valueDictionary.TryGetValue(parameter, out DataSoundSlider dataS))
        //{
        //    valueDictionary[parameter] = new DataSoundSlider();
        //}
        //else
        valueDictionary[parameter] = SaveDataSoundStruct(ref dataS);
    }
    public DataSoundSlider SaveDataSoundStruct(ref DataSoundSlider data)
    {
        data.valueSave = valueSave;
        data.valueSlider = slider.value;
        return data;
    }

    public void LoadSlider(float value)
    {
        Debug.Log(slider.value);

        if (value >= 0.001f)
        {
            slider.value = value;
            Debug.Log(slider.value);
        }

    }
}
[System.Serializable]
public struct DataSoundSlider
{
    public float valueSave;
    public float valueSlider;
}