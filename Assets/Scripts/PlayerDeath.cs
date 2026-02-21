using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerDeath : MonoBehaviour
{
    [Header("Ustawienia Czasu")]
    [Tooltip("Sztywno ustawiony czas do śmierci gracza")]
    public float czasNaPrzezycie = 60f; 
    private float aktualnyCzas;

    [Header("UI i Efekty")]
    [Tooltip("Przypisz tutaj obiekt z komponentem UIDocument powiązany z interfejsem śmierci")]
    public UIDocument ekranSmierciUI; 
    [Tooltip("Opcjonalnie: Przypisz Particle System odpowiadający za krew emitowaną w świecie 3D")]
    public ParticleSystem efektKrwi; 

    [Header("Skrypty do wyłączenia")]
    [Tooltip("Przypisz skrypt ruchu gracza z obiektu Player")]
    public PlayerMovement skryptRuchuGracza;
    [Tooltip("Przypisz skrypt poruszania kamerą (znajdujący się na graczu lub kamerze)")]
    public CameraMovement skryptRuchuKamery;
    [Tooltip("Przypisz skrypt Claws (ataku)")]
    public Claws skryptClaws;
    [Tooltip("Przypisz skrypt Grapling (haka)")]
    public Grapling skryptGrapling;

    private bool isDead = false;
    private VisualElement uiRoot;

    void Start()
    {
        aktualnyCzas = czasNaPrzezycie;
        
        // Ukrywamy UI na starcie, jeśli zostało podpięte
        if (ekranSmierciUI != null)
        {
            uiRoot = ekranSmierciUI.rootVisualElement;
            if (uiRoot != null)
            {
                uiRoot.style.display = DisplayStyle.None;
            }
        }
    }

    void Update()
    {
        // Jeśli gracz jest martwy czekamy tylko na wciśnięcie klawisza R
        if (isDead)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartPoziomu();
            }
            return;
        }

        // Odliczanie czasu
        if (aktualnyCzas > 0)
        {
            aktualnyCzas -= Time.deltaTime;
        }
        else
        {
            SmiercGracza();
        }
    }

    private void SmiercGracza()
    {
        isDead = true;

        // Pokazujemy UI z UI Buildera
        if (uiRoot != null)
        {
            uiRoot.style.display = DisplayStyle.Flex;
        }

        // Start emisji systemu cząsteczek krwi wokół postaci
        if (efektKrwi != null)
        {
            efektKrwi.Play();
        }

        // Wyłączenie skryptu odpowiadającego za ruch gracza (jeśli podpięty)
        if (skryptRuchuGracza != null)
        {
            skryptRuchuGracza.enabled = false;
        }

        // Wyłączenie skryptu kamery (jeśli podpięty)
        if (skryptRuchuKamery != null)
        {
            skryptRuchuKamery.enabled = false;
        }

        // Wyłączenie skryptu Claws (jeśli podpięty)
        if (skryptClaws != null)
        {
            skryptClaws.enabled = false;
        }

        // Wyłączenie skryptu Grapling (jeśli podpięty)
        if (skryptGrapling != null)
        {
            skryptGrapling.enabled = false;
        }
    }

    private void RestartPoziomu()
    {
        // Ponowne załadowanie aktualnie odpalonej sceny
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
