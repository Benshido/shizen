using UnityEngine;
using static Unity.VisualScripting.Member;

public class CustomAudioPlayer : MonoBehaviour
{
    [SerializeField] bool createSourceAsChild = true;
    [Header("Clips")]
    [SerializeField] ClipArray[] clips;
    [SerializeField] GameObject sourcePrefab;
    // [SerializeField] bool playOnAwake;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < clips.Length; i++)
        {
            var source = clips[i].source;
            if (source == null)
            {
                source = Instantiate(sourcePrefab, transform).GetComponent<AudioSource>();

                source.name += i;
                clips[i].source = source;
            }

            if (!createSourceAsChild)
            {
                source.transform.parent = null;
                clips[i].MyObject = gameObject;
            }

            if (clips[i].group != null) clips[i].source.outputAudioMixerGroup = clips[i].group;
        }
    }


    public void PlayAudio(int index)
    {
        clips[index].PlayAudio();
    }

    private void OnDestroy()
    {
        if (!createSourceAsChild)
            for (int i = clips.Length - 1; i >= 0; i--)
            {
                if (clips[i].source != null)
                {
                    var destroyer = clips[i].source.gameObject.AddComponent<AudioSourceDestroyer>();
                    destroyer.Destroyer();
                }
            }
    }
}
