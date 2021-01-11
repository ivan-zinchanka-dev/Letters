using UnityEngine;
using UnityEngine.UI;

public class DiscJockey : MonoBehaviour
{

    //[SerializeField] private float StartVolume = 0.5f;
    public static float Volume { get; private set; } = 0.5f;
    private AudioSource audioSource = null;

    public void SetSoundsVolume(Slider s)
    {
        Volume = s.value;
    }


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = Volume;
        audioSource.Play();
    }

    private void Update()
    {
        audioSource.volume = Volume;
    }


}
