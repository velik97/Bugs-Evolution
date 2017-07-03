using UnityEngine;
using System.Collections;

public class RandomFoodGeneration : MonoBehaviour {

	public Food foodPrefab;
	public Food bombPrefab;

	public float minFoodValue;
	public float maxFoodValue;

	public float minBombValue;
	public float maxBombValue;

	public int foodCount;
	public int bombCount;

	private Area area;

	void Awake () {
		area = GameObject.FindWithTag ("Area").GetComponent <Area> ();
		for (int i = 0; i < foodCount; i++) {
			SpawnRandomly (foodPrefab, minFoodValue, maxFoodValue);
		}
		for (int i = 0; i < bombCount; i++) {
			SpawnRandomly (bombPrefab, -maxBombValue, -minBombValue);
		}
	}

	void SpawnRandomly (Food prefab, float minValue, float maxValue) {
		float randomX = Random.Range (-area.width/2, area.width/2);
		float randomY = Random.Range (-area.height/2, area.height/2);

		float randomValue = Random.Range (minValue, maxValue);

		Food newFood = Instantiate (prefab, new Vector3 (randomX, randomY), Quaternion.identity) as Food;

		newFood.transform.localScale *= randomValue / 10f;
		newFood.value = randomValue;
		newFood.rfg = this as RandomFoodGeneration;
	}

	public void SpawnClone (bool isBomb) {
		if (isBomb)
			SpawnRandomly (bombPrefab, -maxBombValue, -minBombValue);
		else
			SpawnRandomly (foodPrefab, minFoodValue, maxFoodValue);
	}
}
