using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject [] prefabs;
	public GameObject busPrefab;
	public float delay = 3.0f; //Delay de 2 segundos
	public bool active = true; //para saber si el spawner esta activo
	public bool activeBus = true;
	public Vector2 delayRange = new Vector2(1, 2);
	
	void Start () {
		ResetDelay();
		StartCoroutine(EnemyGenerator() ); // inicia una rutina aparte del loop del juego para generar el spawn de obstaculos
		StartCoroutine(BusGenerator() );
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
	IEnumerator BusGenerator(){
		yield return new WaitForSeconds(delay);
		if (activeBus){
			var newTransform = transform;
			busPrefab.GetComponent<Rigidbody2D>().velocity.Set(-10, 0);
			GameObjectUtil.Instantiate(busPrefab, newTransform.position );
			delay = 1000;
		}
		StartCoroutine(BusGenerator() );
	}
	void ResetDelay(){
		delay = Random.Range(delayRange.x, delayRange.y);
	}
}
