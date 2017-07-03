using UnityEngine;
using System.Collections;

public class BugControll : MonoBehaviour {

	private Bug bug;

	void Awake () {
		bug = GetComponent <Bug> ();
	}

	void Update () {
		bug.GO = Input.GetAxisRaw ("Vertical");
		bug.TU = -Input.GetAxisRaw ("Horizontal");
	}
}
