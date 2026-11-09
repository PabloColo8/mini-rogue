using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Slider volumeSlider;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        float savedVolume = PlayerPrefs.GetFloat("GameVolume", 0.28f);
        AudioListener.volume = savedVolume;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeSlider.value = AudioManager.Instance.GetVolume();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void SetVolume(float value)
    {
        AudioManager.Instance.SetVolume(value);
    }

}
