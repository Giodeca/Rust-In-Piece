using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private float ambMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private AudioSource[] amb;
    public bool playBgm;
    [SerializeField] private int bgmIndex;

    private bool canPlaySFX;
    private bool canPlayAMB;


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;

        Invoke("AllowSFX", 1f);
        PlayBGM(bgmIndex);

    }

    private void Update()
    {

        //try
        //{
        //    if (!playBgm)
        //        StopAllBGM();
        //    else
        //    {
        //        if (!bgm[bgmIndex].isPlaying)
        //            PlayBGM(bgmIndex);
        //    }
        //}
        //catch (Exception e)
        //{
        //    Debug.LogError(e);
        //}


        //TEST SOUND//
        if (Input.GetKeyDown(KeyCode.M))
            PlaySFX(0, PlayerManager.instance.player.transform);
    }

    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        //if (sfx[_sfxIndex].isPlaying)
        //    return;

        if (!canPlaySFX)
            return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)
            return;

        if (_sfxIndex < sfx.Length)
        {
            //sfx[_sfxIndex].pitch = Random.Range(.85f, 1.1f);
            sfx[_sfxIndex].Play();
        }

    }

    public void PlayAMB(int _ambIndex, Transform _source)
    {
        //if (sfx[_sfxIndex].isPlaying)
        //    return;

        if (!canPlayAMB)
            return;

        if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > ambMinimumDistance)
            return;

        if (_ambIndex < amb.Length)
        {
            //amb[_ambIndex].pitch = Random.Range(.85f, 1.1f);
            amb[_ambIndex].Play();
        }

    }

    public void PlayRandomBGM()
    {
        bgmIndex = UnityEngine.Random.Range(9, bgm.Length);
        PlayBGM(bgmIndex);
        //PlayBGM(0);
    }

    public void StopSFX(int _index) => sfx[_index].Stop();
    public void StopAMB(int _index) => amb[_index].Stop();

    public void StopSFXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));
    public void StopAMBWithTime(int _index) => StartCoroutine(DecreaseVolume(amb[_index]));

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;

            yield return new WaitForSeconds(.25f);

            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;


        StopAllBGM();

        bgm[bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX()
    {
        canPlaySFX = true;
    }

    private void AllowAMB()
    {
        canPlaySFX = true;
    }

}
