using System;
using System.Collections;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Trash Linear Damping (inversely related to speed")] 
    [SerializeField] private float linearDamping = 5.0f;
    [SerializeField] private float minLinearDamping = 2.0f;
    [SerializeField] private float linearDampingDecRate = 100f;
    
    
    //Sensitivity and values
    [Header("Sliders")]
    [SerializeField]private Slider musicSlider;
    [SerializeField]private Slider sfxSlider;
    [SerializeField]private Slider sensitivitySlider;
    
    public static float MusicVolume;
    public static float SfxVolume;
    public static float Sensitivity;

    public static bool CanMove = true;
    
    [SerializeField]private GameObject spawnner;
    
    //private TopBannerAd topBannerAd;
    public static GameManager Instance;
    
    [Header("Game UI")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseUI;
    
    [Header("Scene Prefabs")]
    [SerializeField] private GameObject player;

    private int _score;
    private int _garbageDropped;
    
    [Header("UI Texts")]
    [FormerlySerializedAs("_scoreText")] [SerializeField] private TMP_Text scoreText;
    [FormerlySerializedAs("_garbageDropped")] [SerializeField] private TMP_Text garbageDropped;
    [FormerlySerializedAs("_finalScoreText")] [SerializeField] private TMP_Text finalScoreText;
    void Awake()
    {
        //topBannerAd = GameObject.Find("AdsManager").GetComponent<TopBannerAd>();
        Instance = this;
    }
    
    
    
    void Start()
    {
        spawnner.SetActive(true);
        MusicVolume = musicSlider.value;
        SfxVolume = sfxSlider.value;
        Sensitivity = sensitivitySlider.value;
        sensitivitySlider.minValue = 0.1f;
        sensitivitySlider.maxValue = 3.5f;
        gameUI.gameObject.SetActive(true);
    }

    private void Update()
    {
        //decrease linear damping overtime
        if (linearDamping >= minLinearDamping)
        {
            linearDamping -= Time.deltaTime * linearDampingDecRate;
        }
    }

    public float GetLinearDamping()
    {
        return linearDamping;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        spawnner.SetActive(true);
        //topBannerAd.DestroyBannerAd();
        //topBannerAd.HideBannerAd();
        TogglePause(false);
    }

    public void GarbageDropped()
    {
        _garbageDropped++;
        garbageDropped.text = _garbageDropped.ToString() + "/5";
        if (_garbageDropped == 5)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        spawnner.SetActive(false);
        TogglePause(true);
        //topBannerAd.ShowInterstitialAd();
        finalScoreText.text = "Score: " + _score;
        gameUI.SetActive(false);
        gameOverUI.SetActive(true);
    }

    public void NewLife()
    {
        _garbageDropped = 0;
        garbageDropped.text = _garbageDropped.ToString() + "/5";
        player.transform.position = new Vector3(0f, player.transform.position.y, player.transform.position.z);
        spawnner.SetActive(true);
        gameUI.SetActive(true);
        gameOverUI.SetActive(false);
        DestroyAllTrash();
        StartCoroutine(DelayedTimeScaleChange());

        IEnumerator DelayedTimeScaleChange()
        {
            yield return new WaitForSecondsRealtime(1f);
            TogglePause(false);
        }
        
    }

    private void DestroyAllTrash()
    {
        GameObject[] trash = GameObject.FindGameObjectsWithTag("Trash");
        foreach (GameObject t in trash)
        {
            Destroy(t);
        }
    }

    public void ScoreUp()
    {
        _score++;
        scoreText.text = _score.ToString();
    }

    private void TogglePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
        CanMove = !isPaused;
    }

    public void PauseGame()
    {
        TogglePause(true);
        pauseUI.SetActive(true);
        //topBannerAd.ShowBannerAd();
    }

    public void ResumeGame()
    {
        pauseUI.SetActive(false);
        TogglePause(false);
        //topBannerAd.HideBannerAd();
    }

    public void OnSensitivitySliderChanged()
    {
        Sensitivity = sensitivitySlider.value;
    }
}
