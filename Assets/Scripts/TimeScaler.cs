using UnityEngine;
using System.Collections;

public class TimeScaler : MonoBehaviour {

	public void ScaleTime (float speed) {
		Time.timeScale = speed;
	}
}
