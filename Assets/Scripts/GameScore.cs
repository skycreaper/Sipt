using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.IO;

public class GameScore : MonoBehaviour
{
	private static string scoresFilesPath = "Scores/best_scores";

	public static void InitScoresFolders() 
	{
		Directory.CreateDirectory(@"Scores");
	}

    public static void SaveScore(string time)
    {
        using (System.IO.StreamWriter file = 
            new System.IO.StreamWriter(@scoresFilesPath, true))
        {
            file.WriteLine(time);
        }
    }

    public static string[] GetScores() {
		return System.IO.File.ReadAllLines(@scoresFilesPath);
    }

    public static float GetAverageTime() {
    	float acumulador = 0f;
    	string[] scores = System.IO.File.ReadAllLines(@scoresFilesPath);
    	for (int i = 0; i < scores.Length; i++)
    	{
    		acumulador += ConvertFloat(scores[i].Split('|')[1]);
    	}
    	return acumulador/scores.Length;
    }

	public static int GetAverageObstacles() {
		float acumulador = 0;
		string[] lines = System.IO.File.ReadAllLines(@scoresFilesPath);
		foreach (var line in lines) {
			acumulador += int.Parse(line.Split('|')[2]);
		}
		return (int) acumulador/lines.Length;
	}

    public static float GetBestTime() 
    {
    	float bestTime, actualTime;
    	int bestTimePos = 0;
    	try 
    	{
    		string[] scores = System.IO.File.ReadAllLines(@scoresFilesPath);
	    	for (int i = 1; i < scores.Length; i++)
	    	{	
                bestTime = ConvertFloat(scores[bestTimePos].Split('|')[1]);  
	    		actualTime = ConvertFloat(scores[i].Split('|')[1]); 
	    		if (actualTime < bestTime && scores[i].Split('|')[0] == "V") {
	    			bestTimePos = i;
	    		}
	    	}
			if (scores[bestTimePos].Split('|')[0] == "D") {
				return 0;
			}
	    	return ConvertFloat(scores[bestTimePos].Split('|')[1]); 
    	} 
    	catch 
    	{
    		return 0;
    	}
    }

    public static float ConvertFloat(string value) {
		return float.Parse(value);
    } 
}
