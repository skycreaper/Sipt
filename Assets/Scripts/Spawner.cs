using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject [] prefabs;
	public float delay = 2.0f; //Delay de 2 segundos
	public bool active = true; //para saber si el spawner esta activo
	public Vector2 delayRange = new Vector2(1, 2);
	
	void Start () {
		ResetDelay();
		StartCoroutine(EnemyGenerator() ); // inicia una rutina aparte del loop del juego para generar el spawn de obstaculos
	}
	IEnumerator EnemyGenerator(){
		yield return new WaitForSeconds(delay);
		if (active){
			var newTransform = transform;
			GameObjectUtil.Instantiate(prefabs[Random.Range(0, prefabs.Length)], newTransform.position);			
			ResetDelay();
		}
		StartCoroutine(EnemyGenerator() );
	}
	void ResetDelay(){
		delay = Random.Range(delayRange.x, delayRange.y);
	}
}
