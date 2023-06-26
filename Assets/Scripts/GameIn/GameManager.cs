using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
//*********** Public Values Zone ************//
    public static int           score = 0;
    public BombBalloon          bmbBl;
    public MoveUp               mvUp;
    public BarbedBalloon        brbdBl;
    public AirBalloonControl    airBl;
    public SpawnManager         spwnMng;
    public Text                 scoreTxt;
    public Text                 airCptText;
    public AudioClip[]          bonusSfxClips;
    public GameObject           pauseMenu;
    public GameObject           areyousureWindow;
    public GameObject           pauseButton;
    public GameObject           gameOverText;
    public Slider               gameinSliderVolume;

//*********** Private Values Zone **********//
    private PlayerController        plyrCtrl;
    private Animator                drLightAnim;
    private GameObject              plyr;
    private GameObject              plyrChildSpot;
    private GameObject              plyrChildBarbal;
    private Rigidbody               plyrRgdb;
    private PostProcessVolume[]     ppVols;
    private ColorGrading            clGrad;
    private AudioSource[]           audioSources;
    private const float             slowMotionTimeScale = 0.3f;
    private float                   startTimeScale;
    private float                   startFixedDeltaTime;
    private float                   currentTimescale;
    private int                     booster = 0;

    delegate IEnumerator bonusFuncs();

    void Start()
    {
        score = 0;
        scoreTxt.text = score.ToString();
        bmbBl.randomSpeedMax = 2.0f;
        mvUp.speed = 0.7f;
        brbdBl.randomSpeedMax = 2.0f;
        airBl.randomSpeedMax = 2.0f;

        drLightAnim = GameObject.Find("Directional Light").GetComponent<Animator>();
        plyrRgdb = FindObjectOfType<PlayerController>().GetComponent<Rigidbody>();
        plyr = GameObject.Find("Player");
        plyrChildSpot = plyr.transform.Find("Spot Light").gameObject;
        plyrChildBarbal = plyr.transform.Find("Barbal").gameObject;
        ppVols = FindObjectOfType<Camera>().GetComponents<PostProcessVolume>();
        clGrad = ppVols[1].profile.GetSetting<ColorGrading>();
        plyrCtrl = plyr.GetComponent<PlayerController>();
        audioSources = GetComponents<AudioSource>();

        startTimeScale = Time.timeScale;
        startFixedDeltaTime = Time.fixedDeltaTime;

        gameinSliderVolume.value = PlayerPrefs.GetFloat("musicVolume");
    }

    void Update()
    {
        scoreTxt.text = score.ToString();
        if(plyr)
            airCptText.text = "%" + plyrCtrl.airCapacity.ToString("0.0");
        Speeder();
        if(plyrCtrl.isPlayerLive == false)
            StartCoroutine(GameOver());
    }

    void Speeder()
    {
        if (score >= booster + 5)
        {
            booster += 5;
            bmbBl.randomSpeedMax += 1;
            mvUp.speed += 0.3f;
            brbdBl.randomSpeedMax += 1;
            airBl.randomSpeedMax += 1;
        }
    }

// Bonus Balloon Properties
    IEnumerator SlowMotion()
    {
        audioSources[0].PlayOneShot(bonusSfxClips[0]);
        audioSources[1].pitch = 0.6f;
        ppVols[0].enabled = true;
        Time.timeScale = slowMotionTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime * slowMotionTimeScale;
        yield return new WaitForSeconds(2);
        Time.timeScale = startTimeScale;
        Time.fixedDeltaTime = startFixedDeltaTime;
        ppVols[0].enabled = false;
        audioSources[1].pitch = 1;
    }

    IEnumerator Darkness()
    {
        audioSources[0].PlayOneShot(bonusSfxClips[1]);
        Material defaultSkybox = RenderSettings.skybox;
        RenderSettings.skybox = null;
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientSkyColor = Color.black;
        if (drLightAnim != null)
            drLightAnim.SetTrigger("TrDarkness");
        plyrRgdb.freezeRotation = true;
        plyr.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        plyrChildSpot.SetActive(true);

        yield return new WaitForSeconds(5);

        RenderSettings.skybox = defaultSkybox;
        RenderSettings.ambientMode = AmbientMode.Skybox;
        drLightAnim.SetTrigger("TrRevDarkness");
        if(plyrChildSpot)
            plyrChildSpot.SetActive(false);
        if(plyrRgdb)
            plyrRgdb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
    }

    IEnumerator Colorfuly()
    {
        audioSources[0].PlayOneShot(bonusSfxClips[3]);
        ppVols[1].enabled = true;
        clGrad.hueShift.value = Random.Range(-180f, 180f);
        yield return new WaitForSeconds(5);
        ppVols[1].enabled = false;
    }

    IEnumerator Puffing()
    {
        plyrCtrl.puffCtrl = true;
        plyrRgdb.freezeRotation = true;
        plyr.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        plyrCtrl.puffRandomNbr = Random.Range(1, 3);
        if(plyrCtrl.puffRandomNbr == 1)
        {
            plyrCtrl.partEffects[1].Play();
            plyrCtrl.audioSfxs[1].PlayOneShot(plyrCtrl.audioSfxs[1].clip);
        }
        else
        {
            plyrCtrl.partEffects[2].Play();
            plyrCtrl.audioSfxs[2].PlayOneShot(plyrCtrl.audioSfxs[2].clip);
        }
        yield return new WaitForSeconds(8);
        plyrCtrl.isPuffTouchCtrl = false;
        plyrCtrl.puffCtrl = false;
        if(plyrRgdb)
            plyrRgdb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
    }

    IEnumerator LikeIron()
    {
        audioSources[0].PlayOneShot(bonusSfxClips[2]);
        plyrCtrl.isLikeIron = true;
        plyrChildBarbal.SetActive(true);
        yield return new WaitForSeconds(10);
        plyrCtrl.isLikeIron = false;
        plyrChildBarbal.SetActive(false);
    }

    public void DefineFuncs()
    {
        int i = Random.Range(0, 5);
        bonusFuncs[] myBonusFuncs = { SlowMotion, Darkness, Puffing, LikeIron, Colorfuly };
        StartCoroutine(myBonusFuncs[2]());
    }

// UI Section
    IEnumerator GameOver()
    {
        pauseButton.SetActive(false);
        gameOverText.SetActive(true);
        PlayerPrefs.SetInt("score", score);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void PauseButton()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if(rb != null)
                rb.isKinematic = true;
        }
        currentTimescale = Time.timeScale;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void MainMenuButton()
    {
        pauseMenu.SetActive(false);
        areyousureWindow.SetActive(true);
    }

    public void YesButton()
    {
        Time.timeScale = currentTimescale;
        AudioListener.pause = false;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void NoButton()
    {
        pauseMenu.SetActive(true);
        areyousureWindow.SetActive(false);
    }

    public void PauseMenuBackButton()
    {
        Time.timeScale = currentTimescale;
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            if(rb != null)
                rb.isKinematic = false;
        }
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void GameinMenuSoundVolume()
    {
        AudioListener.volume = gameinSliderVolume.value;
        Save();
    }
    
    void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", gameinSliderVolume.value);
    }
}
