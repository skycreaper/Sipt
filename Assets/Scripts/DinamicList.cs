using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinamicList : MonoBehaviour
{

    public ScrollRect scrollView;
    public GameObject scrollContent;
    public GameObject scrollItemPrefab;
    public Text averageTime;
	public Text averageTimeOb;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            string[] items = GameScore.GetScores();
            if (items.Length > 0)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    string time = items[i].Split('|')[0];
                    string obstaculos = items[i].Split('|')[1];
                    generateItem((i + 1) + ". Tiempo: " + GameManager.FormatTime(GameScore.ConvertFloat(time)) + ", Obstaculos: " + obstaculos);
                    if (i == 5) break;
                }
                scrollView.verticalNormalizedPosition = 1;
                averageTime.text = GameManager.FormatTime(GameScore.GetAverageTime());
				averageTimeOb.text = GameScore.GetAverageObstacles().ToString();

            }
            else
            {
                generateItem("No existen puntuaciones registradas");
                averageTime.text = "";
				averageTimeOb.text = "";
            }
        }
        catch
        {
            generateItem("No existen puntuaciones registradas");
            averageTime.text = "";
			averageTimeOb.text = "";
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    void generateItem(string item)
    {
        GameObject scrollItemObj = Instantiate(scrollItemPrefab);
        scrollItemObj.transform.SetParent(scrollContent.transform, false);
        // scrollItemObj.transform.Find("scrollItem").Find("score").gameObject.GetComponent<Text>().text = GameManager.FormatTime(GameScore.ConvertFloat(item));
        scrollItemObj.transform.Find("score").gameObject.GetComponent<Text>().text = item;
    }
}
