using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Notificaciones")]
    [SerializeField] private GameObject panelUI;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private GameObject panelUI2;
    [SerializeField] private TextMeshProUGUI textUI2;

    [Header("Barra de Jefe")]
    [SerializeField] private GameObject bossBarContainer; 
    [SerializeField] private Image bossLifeBar;           


    [Header("Menú de Pausa")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject options;
    [SerializeField] private Slider volumeSlider;
    private bool isPaused = false;

    [Header("Pantallas de Fin de Partida")]
    [SerializeField] private GameObject youLose;
    [SerializeField] private GameObject youDied;
    [SerializeField] private GameObject youWin;
    [SerializeField] private TextMeshProUGUI youReached;
    [SerializeField] private TextMeshProUGUI difficultyDisplay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Aseguramos que los paneles inicien apagados
        if (panelUI != null) panelUI.SetActive(false);
        if (panelUI2 != null) panelUI2.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (options != null) options.SetActive(false);

        if (volumeSlider != null)
        {
            volumeSlider.value = AudioManager.Instance.GetVolume();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    // --- Notificaciones ---

    public void ShowNotification(string notification)
    {
        textUI.text = notification;
        panelUI.SetActive(true);
    }

    public void HideNotification()
    {
        panelUI.SetActive(false);
    }

    public void ShowNotification2(string notification)
    {
        textUI2.text = notification;
        panelUI2.SetActive(true);
    }

    public void HideNotification2()
    {
        panelUI2.SetActive(false);
    }

    public void UpdateDifficultyDisplay(float difficulty)
    {
        difficultyDisplay.text = ((int)difficulty).ToString();
    }

    // --- Control de Flujo / Pausa ---

    public void Pause()
    {
        Time.timeScale = 0;
        isPaused = true;            // Actualiza el estado
        pauseMenu.SetActive(true);  // Abre el menu
    }

    public void Resume()
    {
        Time.timeScale = 1;
        isPaused = false;           // Actualiza el estado
        pauseMenu.SetActive(false); // Cierra el menu
        options.SetActive(false); // Cierra el menu
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void Volumen(float valor)
    {
        AudioManager.Instance.SetVolume(valor);
    }

    // --- Fin de Partida ---

    public void OnPlayerDeath(float difficulty)
    {
        Time.timeScale = 0f; // Congela el juego al morir

        if (difficulty < 4)
        {
            // Muerte estándar antes de completar el ciclo
            if (youLose != null) youLose.SetActive(true);
        }
        else
        {
            // Muerte en bucles infinitos o dificultad avanzada
            if (youDied != null) youDied.SetActive(true);

            if (youReached != null)
            {
                youReached.text = $"DIFFICULTY REACHED: {(float)difficulty}";
            }
        }
    }

    public void OnWin()
    {
        Time.timeScale = 0f;
        youWin.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- Barra de vida del boss ---

    public void ShowBossBar()
    {
        if (bossBarContainer != null)
        {
            bossBarContainer.SetActive(true);
        }

        if (bossLifeBar != null)
        {
            bossLifeBar.fillAmount = 1f;
        }
    }
    public void UpdateBossHealth(float currentHealth, float maxHealth)
    {
        if (bossLifeBar != null && maxHealth > 0)
        {
            bossLifeBar.fillAmount = currentHealth / maxHealth;
        }
    }

    public void HideBossBar()
    {
        if (bossBarContainer != null)
        {
            bossBarContainer.SetActive(false);
        }
    }

}
