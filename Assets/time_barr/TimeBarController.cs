using UnityEngine;
using UnityEngine.UIElements;

public class TimeBarController : MonoBehaviour
{
    [Header("UI Configuration")]
    public UIDocument uiDocument;
    
    [Header("Bar Settings")]
    [Range(0f, 1f)]
    public float fillAmount = 1f; // Wartość 1f oznacza 100% pełny pasek

    private VisualElement _fillElement;

    private void OnEnable()
    {
        if (uiDocument == null)
            uiDocument = GetComponent<UIDocument>();

        // Pobranie głównego drzewa wizualnego
        var root = uiDocument.rootVisualElement;
        
        // Zlokalizowanie elementu Z MASKĄ po jego nazwie w UXML
        _fillElement = root?.Q<VisualElement>("time-bar-fill-mask");
        
        UpdateBar();
    }

    private void Update()
    {
        // Opcjonalnie aktualizuje pasek co klatkę (możesz to modyfikować i uruchamiać np. tylko przy zmianie zmiennej)
        UpdateBar();
    }

    /// <summary>
    /// Metoda aktualizująca pasek na podstawie wartości fillAmount (0 - 1)
    /// </summary>
    public void UpdateBar()
    {
        if (_fillElement != null)
        {
            // Przelicz z zakresu 0 - 1 na 0% - 100% szerokości i zastosuj w stylach MASKI UI
            _fillElement.style.width = new Length(fillAmount * 100f, LengthUnit.Percent);
        }
    }
}
