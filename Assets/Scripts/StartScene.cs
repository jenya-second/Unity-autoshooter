using UnityEngine;

public class StartScene : MonoBehaviour
{
    public float lowVolume = 0.7f;
    public AudioClip hover;
    public AudioClip click;

    void Start()
    {
        Progress.instance.GetComponent<AudioSource>().volume = lowVolume;
    }

    public void OnClick()
    {
        GetComponent<AudioSource>().clip = click;
        GetComponent<AudioSource>().Play();
    }
}
