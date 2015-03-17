using UnityEngine;
using System.Collections;

public class Triceratops : MonoBehaviour {
	
	public int maxHp;
	public int hp;
	public int damage;
	public int defense;

	public float speed;
	
	public Vector3 facing;
	public PackCounter pack;
	public TriceratopsFlockDar tfd;
	
	protected Rigidbody rb;
	protected float atkTime;
	
	protected virtual void Start () {
		this.facing.x = (Random.value * 2.0f) - 1.0f;
		this.facing.y = 0.0f;
		this.facing.z = (Random.value * 2.0f) - 1.0f;
		this.facing.Normalize();
		
		this.rb = this.GetComponent<Rigidbody>();
		//this.hp = 20;
		//this.damage = 4;
		this.atkTime = 5.0f;
	}
	
	protected virtual void Update () {
		if (this.atkTime > 2.0f) {
			if (tfd.inRange.Count > 0) {
				this.alignment ();
				this.cohesion();
				this.separation();
				this.packCohesion();
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
	
	public void setData(int[] dna) {
		this.hp = dna[0];
		this.maxHp = dna[0];
		this.damage = dna[1];
		this.defense = dna[2];
	}
	
	protected virtual void movement() {
		this.facing.y = 0.0f;
		this.facing.Normalize();
		transform.localRotation = Quaternion.LookRotation(facing);
		this.rb.velocity = this.facing * this.speed;
	}
	
	protected virtual void alignment() {
		Vector3 generalDir = Vector3.zero;
		foreach (Triceratops friends in tfd.inRange) {
			if (friends) generalDir += friends.GetComponent<Rigidbody>().velocity;
		}
		generalDir /= tfd.inRange.Count;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.5f);
	}
	
	protected virtual void cohesion() {
		Vector3 generalDir = Vector3.zero;
		foreach (Triceratops friends in tfd.inRange) {
			if (friends) generalDir += friends.transform.position;
		}
		generalDir /= tfd.inRange.Count;
		generalDir = generalDir - this.transform.position;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.0005f);
	}
	
	protected virtual void separation() {
		Vector3 generalDir = Vector3.zero;
		foreach (Triceratops friends in tfd.inRange) {
			if (friends) generalDir += friends.transform.position - this.transform.position;
		}
		generalDir /= tfd.inRange.Count;
		generalDir *= -1;
		generalDir = generalDir - this.transform.position;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.001f);
	}
	
	protected virtual void packCohesion() {
		Vector3 generalDir = Vector3.zero;
		foreach (GameObject friends in pack.triceraHerd) {
			if (friends) generalDir += friends.transform.position;
		}
		generalDir /= pack.triceraHerd.Count;
		generalDir *= -1;
		generalDir = generalDir - this.transform.position;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.0005f);
	}
	
	protected virtual void boundaryMove() {
		if (tfd.boundaryInRange.Count > 0) {
			Vector3 towardsCenter = (this.transform.position * -1).normalized;
			this.facing = Vector3.Slerp (this.facing, towardsCenter, 0.001f * Vector3.Distance(Vector3.zero, this.transform.position));
		}
	}
	
	public virtual void attacked(int damage, GameObject source) {
		this.hp -= (int) Mathf.Clamp((float)(damage - this.defense), 1.0f, 20.0f);
		if (this.pack.protectAgainst == null) {
			this.pack.protectAgainst = source;
			this.pack.protect = this.gameObject;
		}
		if (this.hp <= 0) Destroy(this.gameObject);
	}
	
	protected void goToTarget() {
		if (this.pack.protectAgainst) {
			Vector3 generalDir = this.pack.protectAgainst.transform.position - this.transform.position;
			generalDir.y = 0.0f;
			generalDir.Normalize();
			this.facing = Vector3.Slerp (this.facing, generalDir, 0.25f);
		}
	}
	
	private void inRangeToAttack() {
		if (this.pack.protectAgainst && Vector3.Distance(this.transform.position, this.pack.protectAgainst.transform.position) < 10.0f) {
			this.pack.protectAgainst.GetComponent<Allosaurus>().attacked(this.damage);
			this.atkTime = 0.0f;
		}
	}
}
