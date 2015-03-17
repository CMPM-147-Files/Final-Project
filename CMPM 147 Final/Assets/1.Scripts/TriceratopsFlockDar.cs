using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriceratopsFlockDar : MonoBehaviour {

	public List<GameObject> boundaryInRange;
	public List<Triceratops> inRange;
	
	// Use this for initialization
	void Start () {
		inRange = new List<Triceratops>();
		boundaryInRange = new List<GameObject>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other) {
		Triceratops friend = other.GetComponent<Triceratops>();
		if (friend != null) {
			inRange.Add(friend); 
		}
		
		if (other.tag == "Boundary") {
			boundaryInRange.Add(other.gameObject);
		}
	}
	
	void OnTriggerExit(Collider other) {
		Triceratops friend = other.GetComponent<Triceratops>();
		if (friend != null) {
			if (inRange.Contains(friend)) {
				inRange.Remove(friend);
			}
		}
		
		if (other.tag == "Boundary" && boundaryInRange.Contains(other.gameObject)) {
			boundaryInRange.Remove(other.gameObject);
		}
	}
}
