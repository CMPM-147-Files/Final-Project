using UnityEngine;
using System.Collections;

public class BabyTriceratops : Triceratops {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	
	protected override void cohesion() {
		Vector3 generalDir = Vector3.zero;
		foreach (Triceratops friends in tfd.inRange) {
			if (friends) generalDir += friends.transform.position;
		}
		generalDir /= tfd.inRange.Count;
		generalDir = generalDir - this.transform.position;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.005f);
	}
	
	protected override void packCohesion() {
		Vector3 generalDir = Vector3.zero;
		foreach (GameObject friends in pack.triceraHerd) {
			if (friends) generalDir += friends.transform.position;
		}
		generalDir /= pack.triceraHerd.Count;
		generalDir = generalDir - this.transform.position;
		generalDir.Normalize();
		this.facing = Vector3.Slerp (this.facing, generalDir, 0.1f);
	}
	
	public override void attacked(int damage, GameObject source) {
		this.hp -= damage;
		if (this.pack.protectAgainst == null) {
			this.pack.protectAgainst = source;
			this.pack.protect = this.gameObject;
		} else if (this.pack.protectAgainst.GetComponent<BabyTriceratops>() == null) {
			this.pack.protectAgainst = source;
			this.pack.protect = this.gameObject;
		}
		if (this.hp <= 0) Destroy(this.gameObject);
	}
}
