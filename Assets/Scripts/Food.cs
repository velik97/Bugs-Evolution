using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour{

	public bool isBomb;

	public float value;
	public RandomFoodGeneration rfg;

	public void Die () {
		if (rfg != null)
			rfg.SpawnClone (isBomb);
		Destroy (this.gameObject);
	}
}
