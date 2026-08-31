using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    private AudioTracker audioTracker;

    [Space]
    [SerializeField] bool boolOnEnable;
    [SerializeField] float fltDelayOnEnable = .2f;

    private void Awake()
    {
        if (boolOnEnable)
            return;

        TryGetComponent(out Button btn);

        if(btn)
            btn.onClick.AddListener(PlayAudio);
    }

    public void PlayAudio()
    {
        if (audioTracker && audioTracker.AudioTrigger == this)
            audioTracker.Play(audioClip, this);
        else
            audioTracker = AudioManager.Instance.SetSFX(audioClip, this);
    }

    public void PlayAudio(AudioClip _audioClip)
    {
        audioClip = _audioClip;

        PlayAudio();
    }

    private IEnumerator CoroutineDelay()
    {
        yield return new WaitForSeconds(fltDelayOnEnable);

        PlayAudio();
    }

    private void OnEnable()
    {
        if(boolOnEnable)
            StartCoroutine(CoroutineDelay());
    }
}
