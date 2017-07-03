using UnityEngine;
using System.Collections;

public class Bug : MonoBehaviour {

	[Header("Moving")]
	public float speed;
	public float steeringSpeed;

	[Header("Eyes")]
	public float dist;
	public float angle;
	public float insideAngle;

	[Header("Lives")]
	public float maxlives;
	public float livesLoseInHungrySec;
	private float lives;
	public Color liveColor;
	public Color deadColor;

	[Header("Layers")]
	public LayerMask foodMask;
	public LayerMask bombMask;

	public LineRenderer rrLineRenderer;
	public LineRenderer llLineRenderer;
	public LineRenderer rlLineRenderer;
	public LineRenderer lrLineRenderer;
	private SpriteRenderer sr;

	// Input
	private float rr;
	private float rl;
 	private float il;
	private float ll;
	private float lr;

	// Output
	public float go;
	public float tu;

	public float RR { get { return rr; } }
	public float RL { get { return rl; } }
	public float IL { get { return .5f + il * .5f; } }
	public float LL { get { return ll; } }
	public float LR { get { return lr; } }

	public float GO { set { go = value; } }
	public float TU { set { tu = value; } }

	void Awake () {
		lives = maxlives;
		sr = GetComponent <SpriteRenderer> ();
	}

	void FixedUpdate () {
		transform.position += transform.up * go * Time.fixedDeltaTime * speed;
		transform.Rotate(0, 0, tu * Time.fixedDeltaTime * steeringSpeed);
	}

	void Update () {
		float zAngle = -transform.eulerAngles.z;

		float rrGlobalAngle = angle + zAngle;
		float llGlobalAngle = -angle + zAngle;
		float rlGlobalAngle = insideAngle + zAngle;
		float lrGlobalAngle = -insideAngle + zAngle;

		rr = SignalFromGlobalAngle (dist, rrGlobalAngle, foodMask, rrLineRenderer);
		ll = SignalFromGlobalAngle (dist, llGlobalAngle, foodMask, llLineRenderer);
		rl = SignalFromGlobalAngle (dist, rlGlobalAngle, bombMask, rlLineRenderer);
		lr = SignalFromGlobalAngle (dist, lrGlobalAngle, bombMask, lrLineRenderer);

		if (lives > maxlives)
			lives = maxlives;

		lives -= livesLoseInHungrySec * Time.deltaTime;
		if (lives < 0f)
			Die ();

		il = (maxlives - lives) / maxlives;

		sr.color = Color.Lerp (liveColor, deadColor, il);
	}

	public void Die () {
		GetComponent <NeuralBugControll> ().Die ();
		Destroy (this.gameObject);
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.layer == Mathf.Log (foodMask.value, 2) || other.gameObject.layer == Mathf.Log (bombMask.value, 2)) {
			if (Vector3.SqrMagnitude (other.transform.position - transform.position) < 1f) {
				Food food = other.gameObject.GetComponent <Food> ();
				if (food == null)
					print ("null");
				lives += food.value;
				food.Die ();
			}
		}
	}

	float SignalFromGlobalAngle (float distance, float globalAngle, LayerMask mask, LineRenderer lineRenderer) {
		Vector3 direction = new Vector3 (Mathf.Sin (globalAngle * Mathf.Deg2Rad), Mathf.Cos (globalAngle * Mathf.Deg2Rad), 0f);

		RaycastHit2D hit = Physics2D.Raycast (transform.position + direction * .55f, direction, distance, mask);
		float distToTarget;

		if (hit.collider != null) 
			distToTarget = hit.distance;
		else 
			distToTarget = distance; 

		lineRenderer.SetPosition (0, transform.position + direction * .55f);
		lineRenderer.SetPosition (1, transform.position + direction * .55f + direction * distToTarget);

		return (distance - distToTarget) / distance;
	}
		
}
