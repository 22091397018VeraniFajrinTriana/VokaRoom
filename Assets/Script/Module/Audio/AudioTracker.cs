using System.Collections;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioTracker : MonoBehaviour
{
    private AudioSource audioSource;

    private AudioClip audioClip;

    private AudioTrigger audioTrigger;

    private bool boolBGM;

    [Header("Reference")]
    private AudioManager audioManager;

    public AudioSource AudioSource => audioSource;

    public AudioTrigger AudioTrigger => audioTrigger;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip _audioClip, AudioTrigger _audioTrigger)
    {
        audioClip = _audioClip;
        audioTrigger = _audioTrigger;

        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();

        StopAllCoroutines();
        StartCoroutine(CoroutinePool(audioClip.length));
    }

    private IEnumerator CoroutinePool(float _fltAudioLength)
    {
        audioManager.Unavailable(boolBGM, this);

        yield return new WaitForSeconds(_fltAudioLength);

        audioManager.Available(boolBGM, this);
        audioTrigger = null;
    }


    public void Setup(bool _boolBGM)
    {
        gameObject.name = "Audio Tracker " + (_boolBGM ? "BGM" : "SFX");
        boolBGM = _boolBGM;

        if(!audioSource)
            audioSource = GetComponent<AudioSource>();

        if (boolBGM)
        {
            audioSource.playOnAwake = true;
            audioSource.loop = true;
        }
        else
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }
}