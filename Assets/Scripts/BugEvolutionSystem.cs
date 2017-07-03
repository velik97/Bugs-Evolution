using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BugEvolutionSystem : MonoBehaviour {

	private Area area;
	public int population;
	private NeuralBugControll[] bugs;

	private float[][,] firstLayerOfDendritesOfMostAddapted;
	private float[][,] secondLayerOfDendritesOfMostAddapted;
	private float[] timeOfLiveOfMostAddapted;
	private float[] distOfMostAddapted;
	private float[] angleOfMostAddapted;
	private float[] insideAngleOfMostAddapted;
	private float[] speedOfMostAddapted;
	private float[] steeringSpeedOfMostAddapted;

	public int individualsToHybrid;

	public float minDist;
	public float maxDist;
	public float minAngle;
	public float maxAngle;
	public float minInsideAngle;
	public float maxInsideAngle;
	public float minSpeed;
	public float maxSpeed;
	public float minSteeringSpeed;
	public float maxSteeringSpeed;

	private float[] lastTwentyTimesOfLive;
	private int lastDeadNum;

	public NeuralBugControll bugController;

	public float mutationChance;
	public float evolutionForce;

	public Text longestLiveText;
	public Text generationText;
	private int generation;

	void Awake () {
		generation = 0;
		lastTwentyTimesOfLive = new float[20];

		bugs = new NeuralBugControll[population];
		area = GameObject.FindWithTag ("Area").GetComponent <Area> ();
		for (int i = 0; i < population; i++) {
			bugs [i] = SpawnRandomly ();
			bugs [i].bug.dist = Random.Range (minDist, maxDist);
			bugs [i].bug.angle = Random.Range (minAngle, maxAngle);
			bugs [i].bug.insideAngle = Random.Range (minInsideAngle, maxInsideAngle);
			bugs [i].bug.speed = Random.Range (minSpeed, maxSpeed);
			bugs [i].bug.steeringSpeed = Random.Range (minSteeringSpeed, maxSteeringSpeed);
			bugs [i].number = i;
		}

		timeOfLiveOfMostAddapted = new float[individualsToHybrid];
		distOfMostAddapted = new float[individualsToHybrid];
		angleOfMostAddapted = new float[individualsToHybrid];
		insideAngleOfMostAddapted = new float[individualsToHybrid];
		speedOfMostAddapted = new float[individualsToHybrid];
		steeringSpeedOfMostAddapted = new float[individualsToHybrid];

		firstLayerOfDendritesOfMostAddapted = new float[individualsToHybrid][,];
		secondLayerOfDendritesOfMostAddapted = new float[individualsToHybrid][,];

		for (int i = 0; i < individualsToHybrid; i++) {
			timeOfLiveOfMostAddapted [i] = 0f;
			distOfMostAddapted [i] = (minDist + maxDist) / 2f;
			angleOfMostAddapted [i] = (minDist + maxDist) / 2f;
			insideAngleOfMostAddapted [i] = (minInsideAngle + maxInsideAngle) / 2f;
			speedOfMostAddapted [i] = (minSpeed + maxSpeed) / 2f;
			steeringSpeedOfMostAddapted [i] = (minSteeringSpeed + maxSteeringSpeed) / 2f;

			firstLayerOfDendritesOfMostAddapted [i] = bugs [0].firstLayerOfDendrites;
			secondLayerOfDendritesOfMostAddapted [i] = bugs [0].secondLayerOfDendrites;
		}

	}

	NeuralBugControll SpawnRandomly () {
		float randomX = Random.Range (-area.width/2, area.width/2);
		float randomY = Random.Range (-area.height/2, area.height/2);

		NeuralBugControll newBugController = Instantiate (bugController, new Vector3 (randomX, randomY, 0f), Quaternion.identity) as NeuralBugControll;

		newBugController.SetWeightsRandomly ();
		newBugController.bes = this as BugEvolutionSystem;
		newBugController.secondOfBirth = Time.time;

		return newBugController;
	}

	public void SpawnNewHybrid (int numberOfDead) {

		float timeOfLive = Time.time - bugs [numberOfDead].secondOfBirth;

		lastTwentyTimesOfLive [lastDeadNum] = timeOfLive;
		lastDeadNum = lastDeadNum + 1;
		if (lastDeadNum == 20) {
			generation++;
			lastDeadNum = 0;
			generationText.text = "Generation: " + generation;
		}

		float totalLiveTime = 0;
		foreach (float time in lastTwentyTimesOfLive) {
			totalLiveTime += time;
		}

		print (totalLiveTime / 20);

		for (int i = 0; i < individualsToHybrid; i++) {
			if (timeOfLive > timeOfLiveOfMostAddapted [i]) {
				timeOfLiveOfMostAddapted [i] = timeOfLive;
				firstLayerOfDendritesOfMostAddapted [i] = bugs [numberOfDead].firstLayerOfDendrites;
				secondLayerOfDendritesOfMostAddapted [i] = bugs [numberOfDead].secondLayerOfDendrites;
				distOfMostAddapted [i] = bugs [numberOfDead].bug.dist;
				angleOfMostAddapted [i] = bugs [numberOfDead].bug.angle;
				insideAngleOfMostAddapted [i] = bugs [numberOfDead].bug.insideAngle;
				speedOfMostAddapted [i] = bugs [numberOfDead].bug.speed;
				steeringSpeedOfMostAddapted [i] = bugs [numberOfDead].bug.steeringSpeed;
				longestLiveText.text = "Longest Live: " + timeOfLiveOfMostAddapted[0].ToString ();
				break;
			} 
		}

		float[,] firstLayerOfDendrites = new float[5,6];
		float[,] secondLayerOfDendrites = new float[6,2];
		float dist;
		float angle;
		float insideAngle;
		float speed;
		float steeringSpeed;

		int individualChance;
		float mutation;

		individualChance = Random.Range (0, individualsToHybrid);
		dist = distOfMostAddapted [individualChance];
		mutation = Random.value;
		if (mutation > mutationChance)
			dist = Mathf.Clamp (dist + (Random.value - .5f) * (minDist + maxDist) * evolutionForce / 2f, minDist, maxDist);

		individualChance = Random.Range (0, individualsToHybrid);
		angle = angleOfMostAddapted [individualChance];
		mutation = Random.value;
		if (mutation > mutationChance)
			angle = Mathf.Clamp (angle + (Random.value - .5f) * (minAngle + maxAngle) * evolutionForce / 2f, minAngle, maxAngle);

		individualChance = Random.Range (0, individualsToHybrid);
		insideAngle = insideAngleOfMostAddapted [individualChance];
		mutation = Random.value;
		if (mutation > mutationChance)
			insideAngle = Mathf.Clamp (insideAngle + (Random.value - .5f) * (minInsideAngle + maxInsideAngle) * evolutionForce / 2f, minInsideAngle, maxInsideAngle);

		individualChance = Random.Range (0, individualsToHybrid);
		speed = speedOfMostAddapted [individualChance];
		mutation = Random.value;
		if (mutation > mutationChance)
			speed = Mathf.Clamp (speed + (Random.value - .5f) * (minSpeed + maxSpeed) * evolutionForce / 2f, minSpeed, maxSpeed);

		individualChance = Random.Range (0, individualsToHybrid);
		steeringSpeed = steeringSpeedOfMostAddapted [individualChance];
		mutation = Random.value;
		if (mutation > mutationChance)
			steeringSpeed = Mathf.Clamp (steeringSpeed + (Random.value - .5f) * (minSteeringSpeed + maxSteeringSpeed) * evolutionForce / 2f, minSteeringSpeed, maxSteeringSpeed);

		for (int i = 0; i < 5; i++) {
			for (int j = 0; j < 6; j++) {
				mutation = Random.value;
				individualChance = Random.Range (0, individualsToHybrid);
				firstLayerOfDendrites [i, j] = firstLayerOfDendritesOfMostAddapted [individualChance] [i, j];
				if (mutation > mutationChance) {
					firstLayerOfDendrites [i, j] += Random.Range (-1f, 1f) * evolutionForce;
				}
			}
		}

		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < 2; j++) {
				mutation = Random.value;
				individualChance = Random.Range (0, individualsToHybrid);
				secondLayerOfDendrites [i, j] = secondLayerOfDendritesOfMostAddapted [individualChance] [i, j];
				if (mutation > mutationChance) {
					secondLayerOfDendrites [i, j] += Random.Range (-1f, 1f) * evolutionForce;
				}
			}
		}
			

		float randomX = Random.Range (-area.width/2, area.width/2);
		float randomY = Random.Range (-area.height/2, area.height/2);

		NeuralBugControll newBugController = Instantiate (bugController, new Vector3 (randomX, randomY, 0f), Quaternion.identity) as NeuralBugControll;

		newBugController.SetWeights (firstLayerOfDendrites, secondLayerOfDendrites);
		newBugController.bug.speed = speed;
		newBugController.bug.steeringSpeed = steeringSpeed;
		newBugController.bug.dist = dist;
		newBugController.bug.angle = angle;
		newBugController.bug.insideAngle = insideAngle;

		newBugController.bes = this as BugEvolutionSystem;
		newBugController.number = numberOfDead;
		newBugController.secondOfBirth = Time.time;

		bugs [numberOfDead] = newBugController;
	}


}
