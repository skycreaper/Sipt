﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour{
    public void CambiarEscena(string nameScene){
        SceneManager.LoadScene(nameScene);
    }
    public void salir(){
        Application.Quit();
    }
}
