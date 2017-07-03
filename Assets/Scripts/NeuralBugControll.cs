using UnityEngine;
using System.Collections;

public class NeuralBugControll : MonoBehaviour {

	public Bug bug;
	public int number;
	public float secondOfBirth;

	public BugEvolutionSystem bes;

	[HideInInspector]
	public float[,] firstLayerOfDendrites;
	[HideInInspector]
	public float[,] secondLayerOfDendrites;

	private float[] mediumLayerOfNeurons;

	void Awake () {
		bug = GetComponent <Bug> ();
		SetWeights (RandomLayer (5,6), RandomLayer (6,2));
		mediumLayerOfNeurons = new float[6];
	}

	void Update () {
		for (int j = 0; j < 6; j++) {
			mediumLayerOfNeurons [j] = 0f;
			mediumLayerOfNeurons [j] += bug.RR * firstLayerOfDendrites [0, j];
			mediumLayerOfNeurons [j] += bug.LL * firstLayerOfDendrites [1, j];
			mediumLayerOfNeurons [j] += bug.RL * firstLayerOfDendrites [2, j];
			mediumLayerOfNeurons [j] += bug.LR * firstLayerOfDendrites [3, j];
			mediumLayerOfNeurons [j] += bug.IL * firstLayerOfDendrites [4, j];
		}

		float go = 0f;
		float tu = 0f;
		for (int j = 0; j < 6; j++) {
			go += (Mathf.Abs (mediumLayerOfNeurons [j]) < .5f ? mediumLayerOfNeurons[j] : .5f * Mathf.Sign (mediumLayerOfNeurons[j])) * secondLayerOfDendrites [j, 0];
			tu += (Mathf.Abs (mediumLayerOfNeurons [j]) < .5f ? mediumLayerOfNeurons[j] : .5f * Mathf.Sign (mediumLayerOfNeurons[j])) * secondLayerOfDendrites [j, 1];
		}

		bug.GO = go;
		bug.TU = tu;
	}

	public void SetWeights (float[,] firstLayer, float[,] secondLayer) {
		firstLayerOfDendrites = firstLayer;
		secondLayerOfDendrites = secondLayer;
	}

	public void SetWeightsRandomly () {
		SetWeights (RandomLayer (5,6), RandomLayer (6,2));
	}

	public void Die () {
		bes.SpawnNewHybrid (number);
	}

	float[,] RandomLayer (int width, int heigth) {
		float[,] randomLayer = new float[width, heigth];

		for (int i = 0; i < width; i++) {
			for (int j = 0; j < heigth; j++) {
				randomLayer [i, j] = Random.Range (-1f, 1f);
			}
		}

		return randomLayer;
	}

}
