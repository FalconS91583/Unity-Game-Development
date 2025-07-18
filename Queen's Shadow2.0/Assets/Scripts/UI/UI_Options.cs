using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Options : MonoBehaviour
{
    private Player player;

    [SerializeField] private Toggle healthBarToggle;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float miexerMultiplier = 25;

    [Header("BGM Volume Settings")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private string bgmParameter;

    [Header("SFX Volume Settings")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private string sfxParameter;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        healthBarToggle.onValueChanged.AddListener(OnHealthBarToggleChanged);
    }

    public void BGMSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * miexerMultiplier;
        audioMixer.SetFloat(bgmParameter, newValue);
    }
    public void SFXSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * miexerMultiplier;
        audioMixer.SetFloat(sfxParameter, newValue);
    }

    private void OnHealthBarToggleChanged(bool isOn)
    {
        player.health.EnableHealthBar(isOn);
    }

    public void GoMainMenuButton() => GameManager.instance.ChangeScene("MainMenu", RespawnType.None);

    private void OnEnable()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, .6f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.6f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value);
    }

    public void LoadUpVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, .6f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.6f);
    }
}
