using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class GameScore : MonoBehaviour
{

	private static string scoresFilesPath = "Scores/best_scores";

    public static void SaveScore(string time)
    {
        using (System.IO.StreamWriter file = 
            new System.IO.StreamWriter(@scoresFilesPath, true))
        {
            file.WriteLine(time);
        }
    }

    public static string[] GetBestScores() {
    	return System.IO.File.ReadAllLines(@scoresFilesPath);;
    }

    public static float GetAverageTime() {
    	float acumulador = 0;
    	string[] scores = System.IO.File.ReadAllLines(@scoresFilesPath);
    	for (int i = 0; i < scores.Length; i++)
    	{
    		acumulador += float.Parse(scores[i], CultureInfo.InvariantCulture.NumberFormat);
    	}
    	return acumulador/scores.Length;
    }

    public static float GetBestTime() 
    {
    	float bestTime, actualTime;
    	string[] scores = System.IO.File.ReadAllLines(@scoresFilesPath);
    	int bestTimePos = 0;
    	for (int i = 1; i < scores.Length; i++)
    	{	
    		bestTime = ConvertFloat(scores[bestTimePos]);
    		actualTime = ConvertFloat(scores[i]);
    		if (actualTime > bestTime) {
    			bestTimePos = i;
    		}
    	}
    	return ConvertFloat(scores[bestTimePos]);
    }

    public static float ConvertFloat(string value) {
    	return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    } 
}
