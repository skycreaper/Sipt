﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffscreen : MonoBehaviour{

    public float offset = 16f;
    public delegate void OnDestroy();
    public event OnDestroy DestroyCallback;
    private bool offscreen;
    private float offscreenX = 0;
    private Rigidbody2D body2d;

    public int contadorFuera = 0;
    void Awake(){
        body2d = GetComponent<Rigidbody2D> ();
    }
    // Start is called before the first frame update
    void Start(){
        offscreenX = (Screen.width / TiledBackground.pixelsToUnits) / 2 + offset;
    }

    // Update is called once per frame
    void Update(){
        var posX = transform.position.x;
        var dirX = body2d.velocity.x;
        if (Mathf.Abs(posX) > offscreenX){
            if (dirX < 0 && posX < -offscreenX){
                offscreen = true;                
            }else if(dirX > 0 && posX > offscreenX){
                offscreen = true;
            }
        }else{
            offscreen = false;
        }
        if (offscreen){
            OnOutOfBounds();
        }
    }
    public void OnOutOfBounds(){
        offscreen = false;
        GameObjectUtil.Destroy (gameObject);
        contadorFuera++;
        if (DestroyCallback != null){
            DestroyCallback();
        }
    }
}
