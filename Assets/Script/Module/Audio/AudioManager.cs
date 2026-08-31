using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    Queue<AudioTracker> queueAudioTrackerBGM = new Queue<AudioTracker>();
    Queue<AudioTracker> queueAudioTrackerSFX = new Queue<AudioTracker>();

    List<AudioTracker> listAudioTrackerBGMUnavailable = new List<AudioTracker>();
    List<AudioTracker> listAudioTrackerSFXUnavailable = new List<AudioTracker>();

    [SerializeField] AudioClip audioBGM;

    [Space]
    [SerializeField] bool boolBGMMute;
    [SerializeField] bool boolSFXMute;

    [Space]
    [SerializeField] float fltSeamlessSpeed = .3f;
    [Range(0, 1)]
    [SerializeField] float fltVolumeBGM = 1;
    [Range(0, 1)]
    [SerializeField] float fltVolumeSFX = 1;

    public bool BoolBGMMute => boolBGMMute;
    public bool BoolSFXMute => boolSFXMute;

    public float FltVolumeBGM => fltVolumeBGM;
    public float FltVolumeSFX => fltVolumeSFX;

    protected override void Awake()
    {
        base.Awake();
        
        Initialize();
    }

    private void Initialize()
    {
        if (queueAudioTrackerBGM.Count == 0)
            queueAudioTrackerBGM.Enqueue(SpawnAudioTracker(true));

        if (queueAudioTrackerSFX.Count == 0)
            queueAudioTrackerSFX.Enqueue(SpawnAudioTracker(false));

        if (Instance == this && audioBGM)
            SetBGM(audioBGM);
    }

    protected override void Destroy()
    {
        if(Instance && audioBGM)
            Instance.SetBGM(audioBGM, true);

        base.Destroy();
    }

    public void UnMuteBGM()
    {
        boolBGMMute = !boolBGMMute;

        foreach (AudioTracker _bgm in listAudioTrackerBGMUnavailable)
            if (_bgm)
                _bgm.AudioSource.mute = boolBGMMute;
    }

    public void UnMuteSFX()
    {
        boolSFXMute = !boolSFXMute;

        foreach (AudioTracker _sfx in listAudioTrackerSFXUnavailable)
            if (_sfx)
                _sfx.AudioSource.mute = boolSFXMute;
    }

    public void SetVolumeBGM(float _fltVolume)
    {
        fltVolumeBGM = _fltVolume;

        foreach (AudioTracker _bgm in listAudioTrackerBGMUnavailable)
            if (_bgm)
                _bgm.AudioSource.volume = fltVolumeBGM;
    }

    public void SetVolumeSFX(float _fltVolume)
    {
        fltVolumeSFX = _fltVolume;

        foreach (AudioTracker _sfx in listAudioTrackerSFXUnavailable)
            if(_sfx)
                _sfx.AudioSource.volume = fltVolumeBGM;
    }

    public AudioTracker SetBGM(AudioClip _audioClip, bool _boolSeamless = false, bool _boolStack = false)
    {
        AudioTracker audioTrackerTemp;

        if (_boolStack)
        {
            if (queueAudioTrackerBGM.Count == 0)
                audioTrackerTemp = SpawnAudioTracker(true);
            else
                audioTrackerTemp = queueAudioTrackerBGM.Dequeue();
        }
        else
        {
            audioTrackerTemp = listAudioTrackerBGMUnavailable.Count > 0 ? listAudioTrackerBGMUnavailable[0] : queueAudioTrackerBGM.Dequeue();

            if (audioTrackerTemp.AudioSource.clip == _audioClip)
                return audioTrackerTemp;
        }

        audioTrackerTemp.AudioSource.volume = fltVolumeBGM;
        audioTrackerTemp.AudioSource.mute = boolBGMMute;

        if (_boolSeamless)
        {
            StopAllCoroutines();
            StartCoroutine(CoroutineSeamless(audioTrackerTemp, _audioClip));
        }
        else
        {
            audioTrackerTemp.Play(_audioClip, null);
        }

        return audioTrackerTemp;
    }

    private IEnumerator CoroutineSeamless(AudioTracker _audioTracker, AudioClip _audioClip)
    {
        float fltVolume = _audioTracker.AudioSource.volume;

        while(_audioTracker.AudioSource.volume > 0)
        {
            _audioTracker.AudioSource.volume -= Time.deltaTime * fltSeamlessSpeed;

            yield return null;
        }

        _audioTracker.Play(_audioClip, null);

        while (_audioTracker.AudioSource.volume < fltVolume)
        {
            _audioTracker.AudioSource.volume += Time.deltaTime * fltSeamlessSpeed;

            yield return null;
        }
    }

    public AudioTracker SetSFX(AudioClip _audioClip, AudioTrigger _audioTrigger)
    {
        AudioTracker audioTrackerTemp;

        if (queueAudioTrackerSFX == null || queueAudioTrackerSFX.Count == 0)
            audioTrackerTemp = SpawnAudioTracker(false);
        else
            audioTrackerTemp = queueAudioTrackerSFX.Dequeue();

        audioTrackerTemp.AudioSource.volume = fltVolumeSFX;
        audioTrackerTemp.AudioSource.mute = boolSFXMute;
        audioTrackerTemp.Play(_audioClip, _audioTrigger);

        return audioTrackerTemp;
    }

    private AudioTracker SpawnAudioTracker(bool _boolBGM)
    {
        AudioTracker audioTracker = new GameObject().AddComponent<AudioTracker>();
        audioTracker.transform.SetParent(transform);
        audioTracker.Setup(_boolBGM);

        return audioTracker;
    }

    public void Available(bool _boolBGM, AudioTracker _audioTracker)
    {
        if (_boolBGM)
        {
            if (listAudioTrackerBGMUnavailable.Contains(_audioTracker))
            {
                listAudioTrackerBGMUnavailable.Remove(_audioTracker);
                queueAudioTrackerBGM.Enqueue(_audioTracker);
            }
        }
        else
        {
            if (listAudioTrackerSFXUnavailable.Contains(_audioTracker))
            {
                listAudioTrackerSFXUnavailable.Remove(_audioTracker);
                queueAudioTrackerSFX.Enqueue(_audioTracker);
            }
        }
    }

    public void Unavailable(bool _boolBGM, AudioTracker _audioTracker)
    {
        if (_boolBGM)
        {
            if (!listAudioTrackerBGMUnavailable.Contains(_audioTracker))
                listAudioTrackerBGMUnavailable.Add(_audioTracker);
        }
        else
        {
            if (!listAudioTrackerSFXUnavailable.Contains(_audioTracker))
                listAudioTrackerSFXUnavailable.Add(_audioTracker);
        }
    }
}
