using UnityEngine;
using System.Collections;

public class RandomEnemy : MonoBehaviour {

	public static RandomEnemy Instance;

	public int CountEnemy = 10;
	public int respTime = 2;

	public int countEnemyOnScene = 0;

	private float delta;

	public void EnemyDead(){

		countEnemyOnScene -=1;

	}
	
	public void EnemyResp(){

		countEnemyOnScene +=1;

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		delta += Time.deltaTime;
		//Debug.Log(delta>=respTime?"PEW!":"not PEW" + " | delta: "+delta);

		if(delta>=respTime){
			delta = 0;

			CreateEnemy();
		}
		else{



		}

	}
	
	void CreateEnemy(){
		while(countEnemyOnScene <=CountEnemy){
			countEnemyOnScene +=1;
		}
	}
}
