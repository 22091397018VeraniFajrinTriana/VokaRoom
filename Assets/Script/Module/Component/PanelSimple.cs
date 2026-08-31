using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSimple : MonoBehaviour
{
    [SerializeField] protected GameObject goPanel;

    [Space]
    [SerializeField] protected PanelSimple panelPrev;

    [Space]
    [SerializeField] string strTweenOpen = "Panel - Open";
    [SerializeField] string strTweenClose = "Panel - Close";

    [Header("Reference")]
    [SerializeField] DOTweenAnimation doTweenPanel;

    public bool BoolIsOpen() => goPanel.activeSelf;

    public virtual void Open()
    {
        goPanel.SetActive(true);

        if (doTweenPanel)
            doTweenPanel.DORestartById(strTweenOpen);
    }

    public virtual void Close()
    {
        StopAllCoroutines();
        StartCoroutine(CoroutineClose());
    }

    public void BtnClose()
    {
        if (panelPrev)
        {
            if(panelPrev.BoolIsOpen()) return;
            
            panelPrev.Open();
        }

        Close();
    }

    private IEnumerator CoroutineClose()
    {
        if (doTweenPanel)
        {
            doTweenPanel.DORestartById(strTweenClose);

            yield return new WaitForSeconds(doTweenPanel.duration + doTweenPanel.delay);
        }

        goPanel.SetActive(false);
    }
}
