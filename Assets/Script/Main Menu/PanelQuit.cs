using UnityEngine;

public class PanelQuit : PanelSimple
{
    public void BtnYes()
    {
        Application.Quit();
    }

    public void BtnNo()
    {
        Close();
    }
}
