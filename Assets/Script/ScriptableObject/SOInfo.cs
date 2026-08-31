using UnityEngine;

[CreateAssetMenu(fileName = "Info - ", menuName = "ScriptableObjects/Info")]
public class SOInfo : ScriptableObject
{
    public string strTitle;
    
    [TextArea(3, 10)]
    public string strDesc;
}
