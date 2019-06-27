using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DinamicList : MonoBehaviour
{

    public ScrollRect scrollView;
    public GameObject scrollContent;
    public GameObject scrollItemPrefab;
    public Text averageTime;
    public Text averageTimeOb;

    public Text victoriesText;

    public Text defeatsText;

    // Start is called before the first frame update
    void Start()
    {
        string gameState;
        int victorias, derrotas;

        try
        {
            string[] items = GameScore.GetScores();
            victorias = 0;
            derrotas = 0;
            if (items.Length > 0)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    string time = items[i].Split('|')[1];
                    string obstaculos = items[i].Split('|')[2];
                    if (items[i].Split('|')[0] == "D")
                    {
                        gameState = "Derrota";
                        generateItem((i + 1) + ". " + gameState + ",Tiempo: " + GameManager.FormatTime(GameScore.ConvertFloat(time)) + ", Obstaculos: " + obstaculos, false);
                        derrotas++;
                    }
                    else
                    {
                        gameState = "Victoria";
                        generateItem((i + 1) + ". " + gameState + ",Tiempo: " + GameManager.FormatTime(GameScore.ConvertFloat(time)) + ", Obstaculos: " + obstaculos, true);
                        victorias++;
                    }


                    //if (i == 5) break;
                }
                scrollView.verticalNormalizedPosition = 1;
                averageTime.text = GameManager.FormatTime(GameScore.GetAverageTime());
                averageTimeOb.text = GameScore.GetAverageObstacles().ToString();
                victoriesText.text = victorias.ToString();
                defeatsText.text = derrotas.ToString();

            }
            else
            {
                generateItem("No existen puntuaciones registradas", false);
                averageTime.text = "";
                averageTimeOb.text = "";
                victoriesText.text = "";
                defeatsText.text = "";
            }
        }
        catch
        {
            generateItem("No existen puntuaciones registradas", false);
            averageTime.text = "";
            averageTimeOb.text = "";
            victoriesText.text = "";
            defeatsText.text = "";
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }

    }

    void generateItem(string item, bool victory)
    {
        GameObject scrollItemObj = Instantiate(scrollItemPrefab);
        scrollItemObj.transform.SetParent(scrollContent.transform, false);
        if (victory)
        {
            scrollItemObj.GetComponent<Image>().color = Color.green;
        }
        else
        {
            scrollItemObj.GetComponent<Image>().color = Color.yellow;
        }

        scrollItemObj.transform.Find("score").gameObject.GetComponent<Text>().text = item;
    }
}
