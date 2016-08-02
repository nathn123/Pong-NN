using UnityEngine;
using System.Collections;

public class Wall_Coll : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ba;; hit trigger");
    }
}
