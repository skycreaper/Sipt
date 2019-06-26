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
    	float acumulador = 0;
    	string[] scores = System.IO.File.ReadAllLines(@scoresFilesPath);
    	for (int i = 0; i < scores.Length; i++)
    	{
    		acumulador += float.Parse(scores[i].Split('|')[0], CultureInfo.InvariantCulture.NumberFormat);
    	}
    	return acumulador/scores.Length;
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
                bestTime = ConvertFloat(scores[bestTimePos].Split('|')[0]);  
	    		actualTime = ConvertFloat(scores[i].Split('|')[0]); 
	    		if (actualTime > bestTime) {
	    			bestTimePos = i;
	    		}
	    	}
	    	return ConvertFloat(scores[bestTimePos].Split('|')[0]); 
    	} 
    	catch 
    	{
    		return 0;
    	}
    }

    public static float ConvertFloat(string value) {
    	return float.Parse(value, CultureInfo.CreateSpecificCulture("es-ES"));
    } 
}
