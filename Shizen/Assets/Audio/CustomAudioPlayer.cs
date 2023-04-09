using UnityEngine;

public class CustomAudioPlayer : MonoBehaviour
{
    [Header("Clips")]
    [SerializeField] ClipArray[] clips;
    [SerializeField] GameObject sourcePrefab;
   // [SerializeField] bool playOnAwake;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].source == null)
            {
                var source = Instantiate(sourcePrefab, transform).GetComponent<AudioSource>();
                source.name += i;
                clips[i].source = source;
            }
            if (clips[i].group != null) clips[i].source.outputAudioMixerGroup = clips[i].group;
        }
    }


    public void PlayAudio(int index)
    {
        clips[index].PlayAudio();
    }
}
