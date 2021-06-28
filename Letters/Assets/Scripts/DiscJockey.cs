using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class DiscJockey : MonoBehaviour
{ 
    [SerializeField] private AudioSource _audioSource = null;

    public float Volume { 
        get {          
            return _audioSource.volume;  
        } 
        private set {
            
            _audioSource.volume = value; 
        } 
    }

    public void SetSoundsVolume(Slider s)
    {
        Volume = s.value;
    }

    private void Start()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();        
        _audioSource.volume = Volume;
        _audioSource.Play();
    }

    private void Update()
    {
        _audioSource.volume = Volume;
    }

}
