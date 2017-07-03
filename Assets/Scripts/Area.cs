using UnityEngine;
using System.Collections;

public class Area : MonoBehaviour {

	public float width, height;

	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 1, 1, .3f);
		Gizmos.DrawLine (new Vector3 (width / 2f, height / 2f, 0), new Vector3 (width / 2f, -height / 2f, 0));
		Gizmos.DrawLine (new Vector3 (width / 2f, -height / 2f, 0), new Vector3 (-width / 2f, -height / 2f, 0));
		Gizmos.DrawLine (new Vector3 (-width / 2f, -height / 2f, 0), new Vector3 (-width / 2f, height / 2f, 0));
		Gizmos.DrawLine (new Vector3 (-width / 2f, height / 2f, 0), new Vector3 (width / 2f, height / 2f, 0));
	}
}
