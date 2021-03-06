﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllosaurusFlockDar : MonoBehaviour {

	public List<GameObject> boundaryInRange;
	public List<Allosaurus> inRange;
	
	public List<Triceratops> foodInRange;

	// Use this for initialization
	void Start () {
		inRange = new List<Allosaurus>();
		boundaryInRange = new List<GameObject>();
		foodInRange = new List<Triceratops>();
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
		
		Triceratops enemy = other.GetComponent<Triceratops>();
		if (enemy != null) {
			foodInRange.Add(enemy); 
		}
	}
	
	void OnTriggerExit(Collider other) {
		Allosaurus friend = other.GetComponent<Allosaurus>();
		if (friend != null) {
			inRange.Remove(friend);
		}
		
		if (other.tag == "Boundary" && boundaryInRange.Contains(other.gameObject)) {
			boundaryInRange.Remove(other.gameObject);
		}
		
		Triceratops enemy = other.GetComponent<Triceratops>();
		if (enemy != null) {
			foodInRange.Remove(enemy);
		}
	}
}
