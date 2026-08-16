using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public float Volume { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            AudioListener.volume = Volume;
        }
        else
            Destroy(gameObject);
    }

    public void SetVolume(float volume)
    {
        Volume = volume;

        AudioListener.volume = volume;

        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }
}