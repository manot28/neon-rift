using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GraphicsManager : MonoBehaviour
{
    public static GraphicsManager Instance;

    private Bloom bloom;
    private Vignette vignette;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded; // when scene is loaded - calls event OnSceneLoaded that finds volume settings and applies em
        ApplySettings();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindVolume();
        ApplySettings();
    }

    void FindVolume()
    {
        Volume volume = FindFirstObjectByType<Volume>();

        if (volume != null)
        {
            volume.profile.TryGet(out bloom);
            volume.profile.TryGet(out vignette);
        }
    }

    public void ApplySettings()
    {
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSync", 1);

        Resolution[] resolutions = Screen.resolutions;
        int index = PlayerPrefs.GetInt("Resolution", resolutions.Length - 1); //choose the last index from the list 
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode); //set resolition at index

        if (bloom != null)
            bloom.active = PlayerPrefs.GetInt("Bloom", 1) == 1; //if bloom == 1, then true, if bloom (0) == 1 false, or 1 if none 

        if (vignette != null)
            vignette.active = PlayerPrefs.GetInt("Vignette", 1) == 1;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}