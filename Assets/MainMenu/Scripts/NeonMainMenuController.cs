using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class NeonMainMenuController : MonoBehaviour
{
    public UIDocument uiDocument;

    [Header("Menu Logic")]
    public string gameSceneName = "GameScene"; // Wpisz tu nazwę swojej sceny z grą

    [Header("Neon Colors")]
    private Color color1 = new Color32(178, 255, 143, 255); // Jasnozielony
    private Color color2 = new Color32(255, 152, 150, 255); // Łososiowy
    private Color color3 = new Color32(255, 0, 168, 255);   // Neonowy różowy
    private Color colorDefault = new Color32(255, 255, 255, 255); // Biały (gdy nie ma hover)
    private float animationSpeed = 1.5f;

    // Elementy UI
    private VisualElement mainMenuScreen;
    private VisualElement optionsScreen;
    private Button btnPlay;
    private Button btnOptions;
    private Button btnQuit;
    private Button btnBack;
    
    // Nowe elementy UI - Ustawienia Ekranu
    private DropdownField dropdownResolution;
    private DropdownField dropdownDisplay;

    // Lista podświetlonych przycisków
    private List<Button> hoveredButtons = new List<Button>();
    
    // Prywatne tablice do obsługi ustawień
    private Resolution[] resolutions;

    void OnEnable()
    {
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        if (uiDocument != null)
        {
            Invoke(nameof(InitializeUI), 0.1f);
        }
    }

    void OnDisable()
    {
        hoveredButtons.Clear();
    }

    private void InitializeUI()
    {
        if (uiDocument == null || uiDocument.rootVisualElement == null) return;

        var root = uiDocument.rootVisualElement;

        // Ekrany
        mainMenuScreen = root.Q<VisualElement>("MainMenuScreen");
        optionsScreen = root.Q<VisualElement>("OptionsScreen");

        // Przyciski
        btnPlay = root.Q<Button>("btn-play");
        btnOptions = root.Q<Button>("btn-options");
        btnQuit = root.Q<Button>("btn-quit");
        btnBack = root.Q<Button>("btn-back");

        // Opcje - Ustawienia
        dropdownResolution = root.Q<DropdownField>("dropdown-resolution");
        dropdownDisplay = root.Q<DropdownField>("dropdown-display");

        // Przypisz akcje do przycisków
        if (btnPlay != null) btnPlay.clicked += OnPlayClicked;
        if (btnOptions != null) btnOptions.clicked += OnOptionsClicked;
        if (btnQuit != null) btnQuit.clicked += OnQuitClicked;
        if (btnBack != null) btnBack.clicked += OnBackClicked;

        // Podepnij animacje do wszystkich przycisków "neon-button"
        var allButtons = root.Query<Button>(className: "neon-button").ToList();
        foreach (var button in allButtons)
        {
            button.RegisterCallback<MouseEnterEvent>(e => OnButtonHover(button));
            button.RegisterCallback<MouseLeaveEvent>(e => OnButtonLeave(button));
        }

        // Zainicjuj logikę opcji (Dropdowny)
        InitializeSettings();

        // Pokaż główne menu, ukryj opcje na start
        ShowScreen(mainMenuScreen, optionsScreen);
    }

    void Update()
    {
        if (hoveredButtons.Count > 0)
        {
            float time = Time.time * animationSpeed;

            foreach (var button in hoveredButtons)
            {
                AnimateBorder(button, time);
            }
        }
    }

    #region Settings Logic

    private void InitializeSettings()
    {
        if (dropdownResolution != null)
        {
            // Pobierz obsługiwane rozdzielczości z systemu
            resolutions = Screen.resolutions;
            List<string> options = new List<string>();
            int currentResolutionIndex = 0;

            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio.value.ToString("F0") + "Hz";
                options.Add(option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height &&
                    Mathf.Approximately((float)resolutions[i].refreshRateRatio.value, (float)Screen.currentResolution.refreshRateRatio.value))
                {
                    currentResolutionIndex = i;
                }
            }

            dropdownResolution.choices = options;
            dropdownResolution.index = currentResolutionIndex;
            
            // Reaguj na zmiany z poziomu UI
            dropdownResolution.RegisterValueChangedCallback(v => SetResolution(dropdownResolution.index));
        }

        if (dropdownDisplay != null)
        {
            List<string> displayOptions = new List<string>();
            int currentDisplayIndex = 0; // Unity w edytorze zazwyczaj działa nad Window/Display 0

            // Ekstrakcja liczby podłączonych monitorów 
            // W nowszych wersjach Unity uzycie `Screen.mainWindowDisplayInfo` może być potrzebne, ale Display.displays pasuje uniwersalnie
            for(int i = 0; i < Display.displays.Length; i++)
            {
                displayOptions.Add("Monitor " + (i + 1));
            }
            
            // Domyślnie zróbmy widoczne min. główny ekran jeśli tablica jest pusta
            if(displayOptions.Count == 0) displayOptions.Add("Monitor 1");

            dropdownDisplay.choices = displayOptions;
            dropdownDisplay.index = currentDisplayIndex;

            dropdownDisplay.RegisterValueChangedCallback(v => MoveToDisplay(dropdownDisplay.index));
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        if(resolutions == null || resolutionIndex < 0 || resolutionIndex >= resolutions.Length) return;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    public void MoveToDisplay(int displayIndex)
    {
        // Ta funkcja jest wspierana głównie na buildach (szczególnie Windows), w edytorze zmiana monitora może nie działać
        if(displayIndex < Display.displays.Length)
        {
            // Unity 2021+: Screen.MoveMainWindowTo(Display.displays[displayIndex], Screen.mainWindowPosition);
            // Standalone bezpieczny sposób na stare/nowe unity - aktywowanie odpowiedniego displaya
            Display.displays[displayIndex].Activate();
        }
    }

    #endregion

    #region Menu Logic

    private void OnPlayClicked()
    {
        // Odkomentuj gdy dodasz scenę do Build Settings:
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnOptionsClicked()
    {
        ShowScreen(optionsScreen, mainMenuScreen);
    }

    private void OnBackClicked()
    {
        ShowScreen(mainMenuScreen, optionsScreen);
    }

    private void OnQuitClicked()
    {
        Application.Quit();
    }

    private void ShowScreen(VisualElement toShow, VisualElement toHide)
    {
        if (toShow != null) toShow.style.display = DisplayStyle.Flex;
        if (toHide != null) toHide.style.display = DisplayStyle.None;
    }

    #endregion

    #region Neon Animation

    private void OnButtonHover(Button button)
    {
        if (!hoveredButtons.Contains(button))
        {
            hoveredButtons.Add(button);
            button.style.transitionProperty = new StyleList<StylePropertyName>(new List<StylePropertyName> { new StylePropertyName("scale"), new StylePropertyName("translate"), new StylePropertyName("background-color") });
        }
    }

    private void OnButtonLeave(Button button)
    {
        if (hoveredButtons.Contains(button))
        {
            hoveredButtons.Remove(button);
            ResetBorder(button);
            button.style.transitionProperty = new StyleList<StylePropertyName>(new List<StylePropertyName> { new StylePropertyName("all") });
        }
    }

    private void AnimateBorder(Button button, float time)
    {
        float tTop = time;
        float tRight = time - 0.25f;
        float tBottom = time - 0.5f;
        float tLeft = time - 0.75f;

        button.style.borderTopColor = GetGradientColor(tTop);
        button.style.borderRightColor = GetGradientColor(tRight);
        button.style.borderBottomColor = GetGradientColor(tBottom);
        button.style.borderLeftColor = GetGradientColor(tLeft);
    }

    private void ResetBorder(Button button)
    {
        button.style.borderTopColor = colorDefault;
        button.style.borderRightColor = colorDefault;
        button.style.borderBottomColor = colorDefault;
        button.style.borderLeftColor = colorDefault;
    }

    private Color GetGradientColor(float timeValue)
    {
        float normalized = Mathf.Repeat(timeValue, 1f); 
        
        if (normalized < 0.3333f)
        {
            return Color.Lerp(color1, color2, normalized * 3f);
        }
        else if (normalized < 0.6666f)
        {
            return Color.Lerp(color2, color3, (normalized - 0.3333f) * 3f);
        }
        else
        {
            return Color.Lerp(color3, color1, (normalized - 0.6666f) * 3f);
        }
    }

    #endregion
}
