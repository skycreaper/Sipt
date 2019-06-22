using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DinamicList : MonoBehaviour
{

	public ScrollRect scrollView;
	public GameObject scrollContent;
	public GameObject scrollItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
    	string[] items = GameScore.GetScores();
    	for(int i = 0; i < 5; i++) 
    	{
    		generateItem((i+1)+". "+GameManager.FormatTime(GameScore.ConvertFloat(items[i])));
    	}
    	scrollView.verticalNormalizedPosition = 1;
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
