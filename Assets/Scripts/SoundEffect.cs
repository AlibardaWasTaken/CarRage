using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffect", menuName = "CarRage/SoundEffect", order = 51)]
public class SoundEffect : ScriptableObject
{
    [SerializeField] string iD;
    [SerializeField] AudioClip audioClip;

    public string ID { get => iD; }
    public AudioClip AudioClip { get => audioClip; }
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (iD == "")
        {
            iD = GUID.Generate().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
