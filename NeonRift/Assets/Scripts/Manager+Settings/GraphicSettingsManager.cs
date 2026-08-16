using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.SceneManagement;
public class GraphicSettingsManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text resolutionText;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Toggle bloomToggle;
    [SerializeField] private Toggle vignetteToggle;

    [Header("Volume")]
    [SerializeField] private Volume globalVolume;

    private Bloom bloom;
    private Vignette vignette;
    private Resolution[] resolutions;
    private int currentResolution;
    void Start()
    {
        resolutions = Screen.resolutions;

        // get bloom & vignette from volume profile
       globalVolume.profile.TryGet(out bloom);
       globalVolume.profile.TryGet(out vignette);

        LoadSettings();
    }

    #region RESOLUTION

    public void NextResolution()
    {
        currentResolution++;
        if (currentResolution >= resolutions.Length)
            currentResolution = 0;
        ApplyResolution();
    }

    public void PreviousResolution()
    {
        currentResolution--;
        if (currentResolution < 0)
            currentResolution = resolutions.Length - 1;
        ApplyResolution();
    }

    void ApplyResolution()
    {
        Resolution res = resolutions[currentResolution];

        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);

        PlayerPrefs.SetInt("Resolution", currentResolution);
        GraphicsManager.Instance.ApplySettings();

        UpdateResolutionText();
    }

    void UpdateResolutionText()
    {
        Resolution res = resolutions[currentResolution];
        resolutionText.text = $"{res.width} x {res.height}";
    }

    #endregion

    #region TOGGLES
    public void SetBloom(bool enabled)
    {
        PlayerPrefs.SetInt("Bloom", enabled ? 1 : 0);
        GraphicsManager.Instance.ApplySettings();
    }
    public void SetVignette(bool enabled)
    {
        PlayerPrefs.SetInt("Vignette", enabled ? 1 : 0);
        GraphicsManager.Instance.ApplySettings();
    }

    public void SetVSync(bool enabled)
    {
        PlayerPrefs.SetInt("VSync", enabled ? 1 : 0);
        GraphicsManager.Instance.ApplySettings();
    }
    #endregion

    void LoadSettings()
    {
        // resolution
        currentResolution = PlayerPrefs.GetInt("Resolution", resolutions.Length - 1);
        ApplyResolution();

        // vsync
        bool vsync = PlayerPrefs.GetInt("VSync", 1) == 1;
        QualitySettings.vSyncCount = vsync ? 1 : 0;
        vsyncToggle.SetIsOnWithoutNotify(vsync);

        // vignette
        bool vignetteEnabled = PlayerPrefs.GetInt("Vignette", 1) == 1;

        if (vignette != null)
            vignette.active = vignetteEnabled;

        vignetteToggle.SetIsOnWithoutNotify(vignetteEnabled);

        // bloom
        bool bloomEnabled = PlayerPrefs.GetInt("Bloom", 1) == 1;

        if (bloom != null)
            bloom.active = bloomEnabled;

        bloomToggle.SetIsOnWithoutNotify(bloomEnabled);

    }
   
}