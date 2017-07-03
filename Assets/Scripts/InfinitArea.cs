using UnityEngine;
using System.Collections;

public class InfinitArea : MonoBehaviour {

	private Area area;

	void Awake () {
		area = GameObject.FindWithTag ("Area").GetComponent <Area> ();
	}

	void Update () {
		if (transform.position.x > area.width/2) {
			transform.position -= Vector3.right * area.width;
		}
		if (transform.position.x < -area.width/2) {
			transform.position += Vector3.right * area.width;
		}
		if (transform.position.y > area.height/2) {
			transform.position -= Vector3.up * area.height;
		}
		if (transform.position.y < -area.height/2) {
			transform.position += Vector3.up * area.height;
		}
	}
}
