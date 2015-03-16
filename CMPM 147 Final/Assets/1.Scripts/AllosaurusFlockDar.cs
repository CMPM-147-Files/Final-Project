using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllosaurusFlockDar : MonoBehaviour {

	public List<GameObject> boundaryInRange;
	public List<Allosaurus> inRange;

	// Use this for initialization
	void Start () {
		inRange = new List<Allosaurus>();
		boundaryInRange = new List<GameObject>();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
		Allosaurus friend = other.GetComponent<Allosaurus>();
		if (friend != null) {
			inRange.Add(friend); 
		}
		
		if (other.tag == "Boundary") {
			boundaryInRange.Add(other.gameObject);
		}
	}
	
	void OnTriggerExit(Collider other) {
		Allosaurus friend = other.GetComponent<Allosaurus>();
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
