using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public GameObject       canvas;
    public GameObject       header;
    public GameObject       buttons;
    public Slider           musicVolumeSlider;
    public TextMeshProUGUI  bestScoreText;
    public int              bestScore;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            LoadMusic();
        }
        else
            LoadMusic();
        LoadBestScore();
    }

    void Update()
    {
        ButtonActions();
    }

    void ButtonActions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("PlayButton"))
                    StartCoroutine(SceneChangerAndWaiter());
                if (hit.collider.gameObject.CompareTag("SettingButton"))
                {
                    canvas.SetActive(true);
                    header.SetActive(false);
                    buttons.SetActive(false);
                    bestScoreText.enabled = false;
                }
                if (hit.collider.gameObject.CompareTag("Exit"))
                    Application.Quit();
            }
        }
    }

    public void BackButton()
    {
        canvas.SetActive(false);
        header.SetActive(true);
        buttons.SetActive(true);
        bestScoreText.enabled = true;
    }

    IEnumerator SceneChangerAndWaiter()
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetVolume()
    {
        AudioListener.volume = musicVolumeSlider.value;
        SaveMusic();
    }

    void LoadMusic()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    void SaveMusic()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
    }

    void SaveBestScore()
    {
        PlayerPrefs.SetInt("bestScore", bestScore);
        bestScoreText.text = "Best Score " + bestScore.ToString();
    }

    void LoadBestScore()
    {
        if (PlayerPrefs.GetInt("score") > PlayerPrefs.GetInt("bestScore"))
        {
            int score = PlayerPrefs.GetInt("score");
            bestScore = score;
            SaveBestScore();
        }
        else
        {
            bestScore = PlayerPrefs.GetInt("bestScore");
            bestScoreText.text = "Best Score " + bestScore.ToString();
        }
    }
}
