using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField] SOInfo soInfo;
    
    [Header("Reference")]
    [SerializeField] PanelInfo panelInfo;
    
    public void OnFound()
    {
        if(panelInfo.BoolIsOpen()) return;
        
        panelInfo.Setup(soInfo);
    }

    public void OnLost()
    {
        
    }
}
