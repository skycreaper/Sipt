using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    private bool gameStarted;
    public Text continueText;
    public Text scoreText;
    public Text won;
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

    void Awake()
    {
        floor = GameObject.Find("Foreground");
        spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
        timeManager = GetComponent<TimeManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
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
        bestTime = GameScore.GetBestTime(); ;
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
            var textColor = beatBestTime ? "#FF0" : "#FFF";
            scoreText.text = "TIEMPO: " + FormatTime(timeElapsed) + "\n<color=" + textColor + ">MEJOR TIEMPO: " + FormatTime(bestTime) + "</color>";
        }
        else
        {
            timeElapsed += Time.deltaTime;
            scoreText.text = "TIEMPO: " + FormatTime(timeElapsed);
            ManageTime();
        }
    }
    void ManageTime()
    {
        if (timeElapsed > 40)
        {
            spawner.active = false;
            spawner.activeBus = true;
            blinkTime++;
            if (blinkTime % 10 == 0)
            {
                blink = !blink;
            }
            if (pressKey < 14)
            {
                continueText.text = "<color=#FF0> Ya viene el bus presiona varias veces C para tomarlo </color>";
                continueText.canvasRenderer.SetAlpha(blink ? 0 : 1);
            }
            else
            {
                continueText.canvasRenderer.SetAlpha(0);
                won.text = " ¡¡ Enhorabuena lo lograste !!";
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
        GameScore.SaveScore(timeElapsed.ToString());
        bestTime = GameScore.GetBestTime();

        // bestTime = timeElapsed > bestTime || bestTime == 0;
        if (timeElapsed >= bestTime)
        {
            beatBestTime = true;
        }
    }
    void ResetGame()
    {
        spawner.activeBus = false;
        spawner.active = true;
        player = GameObjectUtil.Instantiate(playerPrefab, new Vector3(0, (Screen.height / PixelPerfectCamera.pixelsToUnits) / 2 + 100, 0));
        bus = GameObjectUtil.Instantiate(busPrefab, new Vector3(0, -13, -2));
        var playerDestroyScript = player.GetComponent<DestroyOffscreen>();
        playerDestroyScript.DestroyCallback += OnPlayerKilled;
        gameStarted = true;
        won.text = "";
        continueText.canvasRenderer.SetAlpha(0);
        timeElapsed = 0;
        beatBestTime = false;
    }

    public static string FormatTime(float value)
    {
        TimeSpan t = TimeSpan.FromSeconds(value);
        return string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }
}
