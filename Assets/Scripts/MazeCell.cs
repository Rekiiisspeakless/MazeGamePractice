using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell{

    private bool visit = false;
    public GameObject east, west, north, south, floor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetVisit(bool visit)
    {
        this.visit = visit;
    }

    public bool GetVisit()
    {
        return visit;
    }
}
