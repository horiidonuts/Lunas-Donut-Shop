using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private bool isMusicSlider;
    [SerializeField] private bool isSfxSlider;
    
    
    private Slider _slider;

    private float _musicVolume;
    private float _sfxVolume;

    void Start()
    {
        musicSource = musicSource.GetComponent<AudioSource>();
        sfxSource = sfxSource.GetComponent<AudioSource>();
        _slider = GetComponent<Slider>();
        _slider.value = musicSource.volume;
        _slider.onValueChanged.AddListener(AdjustMusic);
    }
    
    public void AdjustMusic(float value)
    {
        if (isMusicSlider)
        {
            _musicVolume = value;
            musicSource.volume = _musicVolume;
            PlayerPrefs.SetFloat("MusicVolume", value);
            PlayerPrefs.Save();
        }

        if (isSfxSlider)
        {
            sfxSource.volume = value;
            sfxSource.volume = _sfxVolume;
            PlayerPrefs.SetFloat("SfxVolume", value);
            PlayerPrefs.Save();
        }
    }
}