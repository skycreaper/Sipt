﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private bool gameStarted;
    private bool savedScore;
    public Text continueText;
    public Text scoreText;
    public Text won;
    public Text hurryUp;
    public Image hurry;
    private float timeElapsed = 0f;
    private float bestTime = 0f;
    private float blinkTime;
    private bool blink;
    public GameObject playerPrefab;
    public GameObject busPrefab;
    private TimeManager timeManager;
    private GameObject player;
    private GameObject bus;
    private GameObject floor;
    private Spawner spawner;
    private bool beatBestTime;
    private int pressKey = 0;

    public AudioClip gameOverClip;
    public AudioClip gameMusicClip;

    private AudioSource backgroundMusic;

    private int randomTimeBus;

    void Awake()
    {
        floor = GameObject.Find("Foreground");
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        timeManager = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        backgroundMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        randomTimeBus = UnityEngine.Random.Range(5, 10);
        GameScore.InitScoresFolders();
        var floorHeight = floor.transform.localScale.y;
        var pos = floor.transform.position;
        pos.x = 0;
        pos.y = -((Screen.height / PixelPerfectCamera.pixelsToUnits) / 2) + (floorHeight / 2);
        floor.transform.position = pos;
        spawner.active = false;
        spawner.activeBus = false;
        Time.timeScale = 0;
        continueText.text = "Presione la tecla ENTER para empezar!";
        hurry.enabled = true;
        hurryUp.text = "¡¡ Rapido alcanza al bus";
        bestTime = GameScore.GetBestTime();
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameStarted && Time.timeScale == 0)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                timeManager.ManipulateTime(1, 1f);
                ResetGame();
            }
        }
        if (!gameStarted)
        {
            blinkTime++;
            if (blinkTime % 40 == 0)
            {
                blink = !blink;
            }
            won.canvasRenderer.SetAlpha(0);
            continueText.canvasRenderer.SetAlpha(blink ? 0 : 1);
            hurryUp.canvasRenderer.SetAlpha(blink ? 0 : 1);
            var textColor = beatBestTime ? "#FF0" : "#FFF";
            scoreText.text = "TIEMPO: " + FormatTime(timeElapsed) + "\n<color=" + textColor + ">MEJOR TIEMPO: " + FormatTime(bestTime) + "</color>";
        }
        else
        {

            timeElapsed += Time.deltaTime;
            scoreText.text = "TIEMPO: " + FormatTime(timeElapsed);
            hurry.enabled = false;            
            ManageTime();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Menu");
                DontDestroyOnLoad(GameObject.Find("TimeManager"));
                //DontDestroyOnLoad(GameObject.Find("Foreground"));
                foreach(var prefab in spawner.prefabs) {
                    
                    if(GameObject.Find(prefab.name+"Object Pool")!=null){
                        Destroy(GameObject.Find(prefab.name+"Object Pool"));
                    }
                }
                
            }
        }
        
    }
    void ManageTime()
    {
        if (timeElapsed > (45+randomTimeBus))
        {
            spawner.active = false;
            spawner.activeBus = true;
            blinkTime++;
            if (blinkTime % 10 == 0)
            {
                blink = !blink;
            }
            if (pressKey < 11)
            {
                continueText.transform.position = new Vector3(0, 45, -2);
                continueText.text = "<color=#FF0> Ya viene el bus presiona varias veces C para tomarlo </color>";
                continueText.canvasRenderer.SetAlpha(blink ? 0 : 1);
            }
            else
            {
                if (!savedScore) 
                {
                    GameScore.SaveScore("V|" + timeElapsed.ToString() + "|" + spawner.enemigosCreados);
                    bestTime = GameScore.GetBestTime();

                    if (timeElapsed >= bestTime)
                    {
                        beatBestTime = true;
                        bestTime = timeElapsed;
                    }
                }

                var textColor = beatBestTime ? "#FF0" : "#FFF";
                scoreText.text = "TIEMPO: " + FormatTime(timeElapsed) + "\n<color=" + textColor + ">MEJOR TIEMPO: " + FormatTime(bestTime) + "</color>";
                continueText.canvasRenderer.SetAlpha(0);
                
                won.text = " ¡¡ Felicidades lo lograste !!";
                savedScore = true;
                won.canvasRenderer.SetAlpha(1);
                Time.timeScale = 0;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                pressKey += 1;
            }
        }
    }
    void OnPlayerKilled()
    {
        spawner.active = false;
        spawner.activeBus = false;
        var playerDestroyScript = player.GetComponent<DestroyOffscreen>();
        playerDestroyScript.DestroyCallback -= OnPlayerKilled;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        timeManager.ManipulateTime(0, 5.5f);
        gameStarted = false;
        won.text = "";
        continueText.text = "Presione la tecla ENTER para reiniciar!";
        // Manejo de tiempos
        GameScore.SaveScore("D|"+timeElapsed.ToString()+"|"+spawner.enemigosCreados);
        bestTime = GameScore.GetBestTime();

        // Música de GameOver
        backgroundMusic.clip = gameOverClip;
        backgroundMusic.Play();
    }
    void ResetGame()
    {
        GameObject busClone = GameObject.Find("Bus2(Clone)");
        if (busClone != null) {
            GameObject.Destroy(busClone);
        }
        
        spawner.activeBus = false;
        spawner.active = true;
        player = GameObjectUtil.Instantiate(playerPrefab, new Vector3(0, (Screen.height / PixelPerfectCamera.pixelsToUnits) / 2 + 100, 0));
        bus = GameObjectUtil.Instantiate(busPrefab, new Vector3(10, -40, -2));
        var playerDestroyScript = player.GetComponent<DestroyOffscreen>();
        playerDestroyScript.DestroyCallback += OnPlayerKilled;
        gameStarted = true;
        won.text = "";
        hurry.enabled = true;
        continueText.canvasRenderer.SetAlpha(0);
        hurryUp.canvasRenderer.SetAlpha(0);
        timeElapsed = 0;
        beatBestTime = false;

        // Música de GameOver
        backgroundMusic.clip = gameMusicClip;
        backgroundMusic.Play();
    }

    public static string FormatTime(float value)
    {
        TimeSpan t = TimeSpan.FromSeconds(value);
        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}
