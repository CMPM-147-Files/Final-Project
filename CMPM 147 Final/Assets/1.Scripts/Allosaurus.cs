using UnityEngine;
using System.Collections;

public class Allosaurus : MonoBehaviour {

	public float speed;

	public Vector3 facing;
	public AllosaurusFlockDar afd;

	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		this.facing.x = (Random.value * 2.0f) - 1.0f;
		this.facing.y = 0.0f;
		this.facing.z = (Random.value * 2.0f) - 1.0f;
		this.facing.Normalize();
		
		this.rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (afd.inRange.Count > 0) {
			this.alignment ();
			this.cohesion();
			this.separation();
		}
		this.boundaryMove();
		this.movement();
	}
	
	private void movement() {
		this.facing.y = 0.0f;
		this.facing.Normalize();
		transform.localRotation = Quaternion.LookRotation(facing * -1);
		this.rb.velocity = this.facing * this.speed;
	}
	
	private void alignment() {
		Vector3 generalDir = Vector3.zero;
		foreach (Allosaurus friends in afd.inRange) {
			generalDir += friends.GetComponent<Rigidbody>().velocity;
		}
		generalDir /= afd.inRange.Count;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.1f);
	}
	
	private void cohesion() {
		Vector3 generalDir = Vector3.zero;
		foreach (Allosaurus friends in afd.inRange) {
			generalDir += friends.transform.position;
		}
		generalDir /= afd.inRange.Count;
		generalDir = generalDir - this.transform.position;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.0001f);
	}
	
	private void separation() {
		Vector3 generalDir = Vector3.zero;
		foreach (Allosaurus friends in afd.inRange) {
			generalDir += friends.transform.position - this.transform.position;
		}
		generalDir /= afd.inRange.Count;
		generalDir *= -1;
		generalDir = generalDir - this.transform.position;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.001f);
	}
	
	private void boundaryMove() {
		if (afd.boundaryInRange.Count > 0) {
			Vector3 towardsCenter = (this.transform.position * -1).normalized;
			this.facing = Vector3.Slerp (this.facing, towardsCenter, 0.00025f * Vector3.Distance(Vector3.zero, this.transform.position));
		}
	}
}
