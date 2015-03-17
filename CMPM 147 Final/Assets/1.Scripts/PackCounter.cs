using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PackCounter : MonoBehaviour {

	public int summons;

	public GameObject raptor;
	public GameObject tricera;
	public GameObject babyTri;

	public List<GameObject> triceraHerd;
	public List<GameObject> raptorPack;
	public GameObject target;
	public GameObject protect;
	public GameObject protectAgainst;

	public int[] triceratopsDna;

	private float timeToUpdate;
	
	// Use this for initialization
	void Start () {
		if (summons == 1 || summons == 3) {
			this.triceraHerd = new List<GameObject>();
			for (int i = 0; i < 6; i++) {
				GameObject newTri = Instantiate(tricera, new Vector3(Random.value * 30.0f, 1.4f, Random.value * 30.0f), Quaternion.identity) as GameObject;
				newTri.GetComponent<Triceratops>().pack = this;
				this.triceraHerd.Add(newTri);
				newTri.GetComponent<Triceratops>().setData(initDna());
			}
			for (int i = 0; i < 4; i++) {
				GameObject newTri = Instantiate(babyTri, new Vector3(Random.value * 30.0f, 1.4f, Random.value * 30.0f), Quaternion.identity) as GameObject;
				newTri.GetComponent<Triceratops>().pack = this;
				this.triceraHerd.Add(newTri);
				newTri.GetComponent<Triceratops>().setData(initDna());
			}
		}
		
		if (summons == 2 || summons == 3) {
			this.raptorPack = new List<GameObject>();
			for (int i = 0; i < 12; i++) {
				GameObject newRap = Instantiate(raptor, new Vector3(Random.value * -15.0f, 1.4f, Random.value * -15.0f), Quaternion.identity) as GameObject;
				newRap.GetComponent<Allosaurus>().pack = this;
				this.raptorPack.Add(newRap);
			}
		
		}
		
		
		this.target = null;
		this.protect = null;
		this.protectAgainst = null;
		
		this.timeToUpdate = 0.0f;
	}
	
	private int[] initDna() {
		triceratopsDna = new int[3];
		int totalStats = 30;
		
		float randomHp = (float)(Random.value/3 + 0.33f);
		int hpSub = (int) (totalStats * randomHp);
		triceratopsDna[0] = hpSub;
		totalStats -= hpSub;
		
		float randomAttack = (float)(Random.value/2 + 0.4f);
		int atkSub = (int) (totalStats * randomAttack);
		triceratopsDna[1] = atkSub;
		totalStats -= atkSub;
		
		triceratopsDna[2] = totalStats;
		
		return triceratopsDna;
	}
	
	// Update is called once per frame
	void Update () {
		if (summons == 3) {
			if (this.timeToUpdate > 10.0f) {
				this.nextGen();
			} else {
				this.timeToUpdate += Time.deltaTime;
			}
		}
		
	}
	
	private void nextGen() {
		int count = 0;
		int[] parentDna = new int[3];
		
		for (int i = 0; i < triceraHerd.Count; i++) {
			if (triceraHerd[i] != null) {
				count++;
				parentDna[0] += triceraHerd[i].GetComponent<Triceratops>().maxHp;
				parentDna[1] += triceraHerd[i].GetComponent<Triceratops>().damage;
				parentDna[2] += triceraHerd[i].GetComponent<Triceratops>().defense;
			}
		}
		
		if (count == 0) {
			for (int i = 0; i < triceraHerd.Count; i++) {
				if (triceraHerd[i] != null) Destroy(triceraHerd[i]);
			}
			triceraHerd.Clear();
			
			for (int i = 0; i < 6; i++) {
				GameObject newTri = Instantiate(tricera, new Vector3(Random.value * 30.0f, 1.4f, Random.value * 30.0f), Quaternion.identity) as GameObject;
				newTri.GetComponent<Triceratops>().pack = this;
				this.triceraHerd.Add(newTri);
				newTri.GetComponent<Triceratops>().setData(initDna());
			}
			for (int i = 0; i < 4; i++) {
				GameObject newTri = Instantiate(babyTri, new Vector3(Random.value * 30.0f, 1.4f, Random.value * 30.0f), Quaternion.identity) as GameObject;
				newTri.GetComponent<Triceratops>().pack = this;
				this.triceraHerd.Add(newTri);
				newTri.GetComponent<Triceratops>().setData(initDna());
			}
			
			for (int i = 0; i < raptorPack.Count; i++) {
				if (raptorPack[i] != null) Destroy(raptorPack[i]);
			}
			raptorPack.Clear ();
			
			for (int i = 0; i < 12; i++) {
				GameObject newRap = Instantiate(raptor, new Vector3(Random.value * -15.0f, 1.4f, Random.value * -15.0f), Quaternion.identity) as GameObject;
				newRap.GetComponent<Allosaurus>().pack = this;
				this.raptorPack.Add(newRap);
			}
			
		} else {
			parentDna[0] = parentDna[0]/count + 1;
			parentDna[1] = parentDna[1]/count + 1;
			parentDna[2] = parentDna[2]/count + 1;
			
			for (int i = 0; i < triceraHerd.Count; i++) {
				if (triceraHerd[i] != null) Destroy(triceraHerd[i]);
			}
			triceraHerd.Clear();
			
			for (int i = 0; i < raptorPack.Count; i++) {
				if (raptorPack[i] != null) Destroy(raptorPack[i]);
			}
			raptorPack.Clear ();
			
			for (int i = 0; i < 6; i++) {
				GameObject newTri = Instantiate(tricera, new Vector3(Random.value * 30.0f, 1.4f, Random.value * 30.0f), Quaternion.identity) as GameObject;
				newTri.GetComponent<Triceratops>().pack = this;
				this.triceraHerd.Add(newTri);
				newTri.GetComponent<Triceratops>().setData(newGenDna(parentDna));
			}
			for (int i = 0; i < 4; i++) {
				GameObject newTri = Instantiate(babyTri, new Vector3(Random.value * 30.0f, 1.4f, Random.value * 30.0f), Quaternion.identity) as GameObject;
				newTri.GetComponent<Triceratops>().pack = this;
				this.triceraHerd.Add(newTri);
				newTri.GetComponent<Triceratops>().setData(newGenDna(parentDna));
			}
			
			for (int i = 0; i < 12; i++) {
				GameObject newRap = Instantiate(raptor, new Vector3(Random.value * -15.0f, 1.4f, Random.value * -15.0f), Quaternion.identity) as GameObject;
				newRap.GetComponent<Allosaurus>().pack = this;
				this.raptorPack.Add(newRap);
			}
		}
		
		
		
		this.target = null;
		this.protect = null;
		this.protectAgainst = null;
		
		this.timeToUpdate = 0.0f;
	}
	
	private int[] newGenDna(int[] dna) {
		triceratopsDna = new int[3];
		
		int randomHp = Mathf.RoundToInt (Random.value * 4.0f - 2.0f);
		triceratopsDna[0] = dna[0] + randomHp;
		
		int randomAttack = Mathf.RoundToInt(Random.value * 1 + 1.0f);
		triceratopsDna[1] = dna[1] + randomAttack;

		int randomDefense = Mathf.RoundToInt(Random.value * 1 + 1.0f);
		triceratopsDna[2] = dna[2] + randomDefense;
		
		return triceratopsDna;
	}
}
