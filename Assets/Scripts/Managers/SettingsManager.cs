using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    #region Instance

    private static SettingsManager _instance;
    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SettingsManager>();
            }
            return _instance;
        }
    }

    #endregion

    [SerializeField] private Volume postProcessingVolume;

    private LiftGammaGain liftGammaGain;

    [Header("REFERENCED")]
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Toggle vSyncToggle;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown daltonismDropdown;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private KeySettingsController[] baseSettingsController;

    [Header("INFORMATION")]
    [SerializeField] private bool fullScreen;
    [SerializeField] private bool vsync;
    [SerializeField] private float brightness;
    [SerializeField] private int resolution;
    [SerializeField] private int daltonism;
    [SerializeField] private float master;
    [SerializeField] private float music;
    [SerializeField] private float fx;
    [SerializeField] private int language;

    Dictionary<string, KeySettingsController> keysSettingsController = new Dictionary<string, KeySettingsController>();

    private void Awake()
    {
        foreach (KeySettingsController keySetting in baseSettingsController)
        {
            keySetting.gameObject.SetActive(false);
        }

        if (postProcessingVolume.profile.TryGet<LiftGammaGain>(out LiftGammaGain lgg))
        {
            liftGammaGain = lgg;
        }

        resolutionDropdown.ClearOptions();
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        options.Clear();

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = Screen.resolutions[i].width.ToString() + "x" + Screen.resolutions[i].height.ToString();
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);

        LoadPrefs();

        keysSettingsController.Clear();
        
        int counter = 0;
        foreach (KeyValuePair<string, KeyCode> key in InputManager.Instance.keys)
        {
            baseSettingsController[counter].gameObject.SetActive(true);
            keysSettingsController.Add(key.Key, baseSettingsController[counter]);
            counter++;
        }

        UpdateKeysSettingController();
    }

    public void SavePrefs()
    {
        //SCREEN
        int k = (fullScreen == true) ? 1 : 0;
        PlayerPrefs.SetInt("fullScreen", k);
        int j = (vsync == true) ? 1 : 0;
        PlayerPrefs.SetInt("vsync", j);
        PlayerPrefs.SetString("resolution", Screen.resolutions[resolution].width+"x"+Screen.resolutions[resolution].height);
        PlayerPrefs.SetInt("daltonism", daltonism);
        PlayerPrefs.SetFloat("brightness", brightness);

        //VOLUME
        PlayerPrefs.SetFloat("master", master);
        PlayerPrefs.SetFloat("music", music);
        PlayerPrefs.SetFloat("effects", fx);

        //KEYS
        PlayerPrefs.SetString("keys", ConcatenateKeys());

        //LANGUAGE
        PlayerPrefs.SetInt("language", language);

        PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {
        //SCREEN
        if (PlayerPrefs.HasKey("fullScreen"))
        {
            bool k = (PlayerPrefs.GetInt("fullScreen") == 1 ? true : false);
            SetFullScreen(k);
        }
        else
        {
            SetFullScreen(true);
        }
        if (PlayerPrefs.HasKey("vsync"))
        {
            bool k = (PlayerPrefs.GetInt("vsync") == 1 ? true : false);
            SetVSync(k);
        }
        else
        {
            SetVSync(false);
        }
        if (PlayerPrefs.HasKey("brightness"))
        {
            SetBrightness(PlayerPrefs.GetFloat("brightness"));
        }
        else
        {
            SetBrightness(0);
        }
        if (PlayerPrefs.HasKey("resolution"))
        {
            SetResolution(SearchResolution(PlayerPrefs.GetString("resolution")));
        }
        else
        {
            SetResolution(Screen.resolutions.Length - 1);
        }
        if (PlayerPrefs.HasKey("daltonism"))
        {
            SetDaltonism(PlayerPrefs.GetInt("daltonism"));
        }
        else
        {
            SetDaltonism(0);
        }

        //VOLUME
        if (PlayerPrefs.HasKey("master"))
        {
            SetMasterVolume(PlayerPrefs.GetFloat("master"));
        }
        else
        {
            SetMasterVolume(1);
        }
        if (PlayerPrefs.HasKey("music"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("music"));
        }
        else
        {
            SetMusicVolume(1);
        }
        if (PlayerPrefs.HasKey("effects"))
        {
            SetFxVolume(PlayerPrefs.GetFloat("effects"));
        }
        else
        {
            SetFxVolume(1);
        }

        //KEYS
        if (PlayerPrefs.HasKey("keys"))
        {
            SetKeys(PlayerPrefs.GetString("keys"));
        }
        else
        {
            SetDefaultKeys();
        }

        //LANGUAGE
        if (PlayerPrefs.HasKey("language"))
        {
            SetIdiom(PlayerPrefs.GetInt("language"));
        }
        else
        {
            SetIdiom(2);
        }
    }

    public void ChangeKey(string keyName, KeySettingsController keySettingsController)
    {
        if (InputManager.Instance.keys.ContainsKey(keyName))
        {
            StartCoroutine(WaitingForNewKey(keyName, keySettingsController));
        }
    }

    IEnumerator WaitingForNewKey(string keyName, KeySettingsController keySettingsController)
    {
        bool changing = true;
        keySettingsController.ChangeKey("...");

        while (changing)
        {
            if (Input.anyKey)
            {
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(key))
                    {
                        bool having = false;
                        string havingKeyName = "";

                        foreach(KeyValuePair<string, KeyCode> previousKey in InputManager.Instance.keys)
                        {
                            if (previousKey.Value == key)
                            {
                                having = true;
                                havingKeyName = previousKey.Key;
                            }
                        }

                        if (having)
                        {
                            InputManager.Instance.keys[havingKeyName] = KeyCode.None;
                        }

                        InputManager.Instance.ChangeKey(keyName, key);
                        changing = false;
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }

        UpdateKeysSettingController();
    }

    public void ChangeFullScreen(bool toggleBool)
    {
        fullScreen = toggleBool;
        Screen.fullScreen = fullScreen;
    }


    public void ChangeVSync(bool toggleBool)
    {
        vsync = toggleBool;
        int j = (toggleBool == true) ? 1 : 0;
        QualitySettings.vSyncCount = j;
    }

    public void ChangeResolution (int dropdownInt)
    {
        resolution = dropdownInt;
        Screen.SetResolution(Screen.resolutions[resolution].width, Screen.resolutions[resolution].height, fullScreen);
    }

    public void ChangeDaltonism (int dropdownInt)
    {
        daltonism = dropdownInt;
        GameManager.Instance.cameraController.fullScreenShadersRenderer.material.SetInt("_TipoDaltonismo", dropdownInt);
    }

    public void ChangeBrightness(float sliderValue)
    {
        brightness = sliderValue;
        liftGammaGain.gamma.SetValue(new Vector4Parameter(new Vector4(1, 1, 1, brightness), true));
    }

    public void ChangeMasterVolume(float sliderValue)
    {
        master = sliderValue;
        mixer.SetFloat("MasterVolume", GetMixerVolume(master));
    }

    public void ChangeMusicVolume(float sliderValue)
    {
        music = sliderValue;
        mixer.SetFloat("MusicVolume", GetMixerVolume(music));
    }

    public void ChangeFxVolume(float sliderValue)
    {
        fx = sliderValue;
        mixer.SetFloat("EffectsVolume", GetMixerVolume(fx));
    }

    public void SetResolution(int resolution)
    {
        ChangeResolution(resolution);
        resolutionDropdown.value = resolution;
    }

    public void SetFullScreen(bool full)
    {
        ChangeFullScreen(full);
        fullScreenToggle.isOn = fullScreen;
    }

    public void SetDaltonism(int daltonism)
    {
        ChangeDaltonism(daltonism);
        daltonismDropdown.value = daltonism;
    }

    public void SetVSync(bool vsync)
    {
        ChangeVSync(vsync);
        vSyncToggle.isOn = vsync;
    }

    public void SetBrightness(float b)
    {
        ChangeBrightness(b);
        brightnessSlider.value = b;
    }

    public void SetMasterVolume(float masterVolume)
    {
        ChangeMasterVolume(masterVolume);
        masterSlider.value = master;
    }

    public void SetMusicVolume(float musicVolume)
    {
        ChangeMusicVolume(musicVolume);
        musicSlider.value = music;
    }

    public void SetFxVolume(float fxVolume)
    {
        ChangeFxVolume(fxVolume);
        fxSlider.value = fx;
    }

    public void SetKeys(string concatenatedKeys)
    {
        InputManager.Instance.keys.Clear();

        string word = "";
        
        string key = "";
        int keyCode;

        bool readingKey = true;

        for (int i = 0; i < concatenatedKeys.Length; i++)
        {
            char letter = concatenatedKeys[i];

            if (readingKey)
            {
                if (letter != '&')
                    word += letter;
                else
                {
                    key = word;
                    word = "";
                    readingKey = false;
                }
            }
            else
            {
                if (letter != '|')
                    word += letter;
                else
                {
                    keyCode = int.Parse(word);
                    InputManager.Instance.keys.Add(key, (KeyCode)keyCode);
                    key = "";
                    word = "";
                    readingKey = true;
                }
            }
        }
    }

    public void SetDefaultVideo()
    {
        SetFullScreen(true);
        SetVSync(false);
        SetBrightness(0);
        SetResolution(Screen.resolutions.Length - 1);
    }

    public void SetDefaultVolume()
    {
        SetMasterVolume(1);
        SetMusicVolume(1);
        SetFxVolume(1);
    }

    public void SetDefaultKeys()
    {
        InputManager.Instance.keys.Clear();

        InputManager.Instance.keys.Add("Forward Movement", KeyCode.W);
        InputManager.Instance.keys.Add("Backward Movement", KeyCode.S);
        InputManager.Instance.keys.Add("Rightward Movement", KeyCode.D);
        InputManager.Instance.keys.Add("Leftward Movement", KeyCode.A);
        InputManager.Instance.keys.Add("Sprinting", KeyCode.LeftShift);
        InputManager.Instance.keys.Add("Interact", KeyCode.F);
        InputManager.Instance.keys.Add("Inventory", KeyCode.I);
        InputManager.Instance.keys.Add("Flashlight", KeyCode.T);
        InputManager.Instance.keys.Add("Aim", KeyCode.Mouse1);
        InputManager.Instance.keys.Add("Reload", KeyCode.R);
        InputManager.Instance.keys.Add("Basic Attack", KeyCode.Mouse0);
        InputManager.Instance.keys.Add("Cast Hability 1", KeyCode.Q);
        InputManager.Instance.keys.Add("Cast Hability 2", KeyCode.E);
        InputManager.Instance.keys.Add("Cast Hability 3", KeyCode.X);
        InputManager.Instance.keys.Add("Unfusion", KeyCode.C);
        InputManager.Instance.keys.Add("Up Arrow", KeyCode.UpArrow);
        InputManager.Instance.keys.Add("Down Arrow", KeyCode.DownArrow);
        InputManager.Instance.keys.Add("Enter", KeyCode.Return);
		InputManager.Instance.keys.Add("Options", KeyCode.Escape);
	}

    public void SetIdiom(int lang)
    {
        language = lang;
        //TextManager.Instance.ChangeIdiom(lang);
    }

    public float GetMixerVolume(float num)
    {
        if (num != 0)
        {
            return Mathf.Log10(num) * 20;
        }
        else
        {
            return -80;
        }
    }

    public string ConcatenateKeys()
    {
        string concatenedKeys = "";
        foreach (KeyValuePair<string, KeyCode> key in InputManager.Instance.keys)
        {
            concatenedKeys += key.Key + "&" + (int)key.Value + "|";
        }
        return concatenedKeys;
    }

    public int SearchResolution(string resolution)
    {
        int width = 0;
        int height = 0;
        string word = "";

        for (int i = 0; i < resolution.Length + 1; i++)
        {
            if (i != resolution.Length && resolution[i]!='x')
            {
                word += resolution[i];
            }
            else
            {
                if (width == 0)
                    width = int.Parse(word);
                else
                    height = int.Parse(word);

                word = "";
            }
        }

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width == width && Screen.resolutions[i].height == height)
            {
                return i;
            }
        }

        return Screen.resolutions.Length - 1;
    }

    public void UpdateKeysSettingController()
    {
        foreach (KeyValuePair<string, KeyCode> key in InputManager.Instance.keys)
        {
            keysSettingsController[key.Key].ChangeName(key.Key);
            keysSettingsController[key.Key].ChangeKey(key.Value.ToString());
        }
    }
}
