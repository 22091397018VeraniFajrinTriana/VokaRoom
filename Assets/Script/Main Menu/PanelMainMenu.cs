using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelMainMenu : MonoBehaviour
{
    [Header("Reference")]
    private PanelGuidebook panelGuidebook;
    private PanelQuit panelQuit;

    private void Awake()
    {
        panelGuidebook = GetComponent<PanelGuidebook>();
        panelQuit = GetComponent<PanelQuit>();
    }

    public void BtnPlay()
    {
        SceneManager.LoadScene("AR");
    }

    public void BtnGuidebook()
    {
        panelGuidebook.Open();
    }

    public void BtnQuit()
    {
        panelQuit.Open();
    }
}
