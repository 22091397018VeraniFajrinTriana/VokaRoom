using TMPro;
using UnityEngine;

public class PanelInfo : PanelSimple
{
    [Space]
    [SerializeField] TMP_Text textTitle;
    [SerializeField] TMP_Text textDesc;
    
    private SOInfo soInfo;
    
    public void Setup(SOInfo _soInfo)
    {
        soInfo = _soInfo;

        textTitle.text = soInfo.strTitle;
        textDesc.text = soInfo.strDesc;
        
        Open();
    }
}
