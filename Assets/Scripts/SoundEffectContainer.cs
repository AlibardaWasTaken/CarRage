using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundEffectContainer", menuName = "CarRage/SoundEffectContainer", order = 51)]
public class SoundEffectContainer : ScriptableObject
{
    [SerializeField] List<SoundEffect> soundEffects;
    [SerializeField] AudioMixerGroup output;
    public List<SoundEffect> SoundEffects { get => soundEffects; set => soundEffects = value; }
    public AudioMixerGroup Output { get => output; }


    public AudioClip PlayRandom()
    {
        if (SoundEffects.Count == 0)
        {
            Debug.Log("No soundsFound");
            return null;
        }
        var TempObj = (GameObject)Instantiate(CommonSoundManager.Instance.SoundObject, Camera.main.transform.position, Quaternion.identity, Camera.main.transform);
        var comp = TempObj.GetComponent<AudioSource>();
        var randclip = SoundEffects[Random.Range(0, SoundEffects.Count)].AudioClip;
        comp.clip = randclip;
        comp.outputAudioMixerGroup = Output;
        comp.Play();
        Destroy(TempObj, comp.clip.length);
        return randclip;
        
    }


}
