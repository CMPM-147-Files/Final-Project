using UnityEngine;
using System.Collections;

public class Allosaurus : MonoBehaviour {

	public int hp;
	public int damage;
	public float speed;

	public Vector3 facing;
	public AllosaurusFlockDar afd;
	public PackCounter pack;

	private float atkTime;
	private Rigidbody rb;
	

	// Use this for initialization
	void Start () {
		this.facing.x = (Random.value * 2.0f) - 1.0f;
		this.facing.y = 0.0f;
		this.facing.z = (Random.value * 2.0f) - 1.0f;
		this.facing.Normalize();
		
		this.rb = this.GetComponent<Rigidbody>();
		this.atkTime = 5.0f;
		
		this.damage = 10;
		this.hp = 10;
	}
	
	
	// Update is called once per frame
	void Update () {
		if (this.atkTime > 2.0f) {
			if (afd.inRange.Count > 0) {
				this.alignment ();
				this.cohesion();
				this.separation();
			}
			this.boundaryMove();
			this.goToTarget();
			this.movement();
			this.inRangeToAttack();
		} else {
			this.atkTime += Time.deltaTime;
			this.rb.velocity = Vector3.zero;
		}
		
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
			if (friends) generalDir += friends.GetComponent<Rigidbody>().velocity;
		}
		generalDir /= afd.inRange.Count;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.1f);
	}
	
	private void cohesion() {
		Vector3 generalDir = Vector3.zero;
		foreach (Allosaurus friends in afd.inRange) {
			if (friends) generalDir += friends.transform.position;
		}
		generalDir /= afd.inRange.Count;
		generalDir = generalDir - this.transform.position;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.001f);
	}
	
	private void separation() {
		Vector3 generalDir = Vector3.zero;
		foreach (Allosaurus friends in afd.inRange) {
			if (friends) generalDir += friends.transform.position - this.transform.position;
		}
		generalDir /= afd.inRange.Count;
		generalDir *= -1;
		generalDir = generalDir - this.transform.position;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.01f);
	}
	
	private void boundaryMove() {
		if (afd.boundaryInRange.Count > 0) {
			Vector3 towardsCenter = (this.transform.position * -1).normalized;
			this.facing = Vector3.Slerp (this.facing, towardsCenter, 0.00025f * Vector3.Distance(Vector3.zero, this.transform.position));
		}
	}
	
	private void goToTarget() {
		if (this.pack.target) {
			Vector3 generalDir = this.pack.target.transform.position - this.transform.position;
			generalDir.y = 0.0f;
			generalDir.Normalize();
			this.facing = Vector3.Slerp (this.facing, generalDir, 0.25f);
		} else {
			if (this.afd.foodInRange.Count > 0) {
				while (this.afd.foodInRange.Count > 0) {
					if (this.afd.foodInRange[0] != null) {
						this.pack.target = this.afd.foodInRange[0].gameObject;
						break;
					} else {
						this.afd.foodInRange.RemoveAt(0);
					}
				}
			}
		}
	}
	
	private void inRangeToAttack() {
		if (this.pack.target && Vector3.Distance(this.transform.position, this.pack.target.transform.position) < 10.0f) {
			this.pack.target.GetComponent<Triceratops>().attacked(this.damage, this.gameObject);
			this.atkTime = 0.0f;
		}
	}
	
	public void attacked(int damage) {
		this.hp -= damage;
		if (this.hp <= 0) Destroy(this.gameObject);
	}
}
