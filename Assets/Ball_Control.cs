using UnityEngine;
using System.Collections;

public class Ball_Control : MonoBehaviour {

    public float StartForce;
    public bool Testing;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Testing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                Reset();
            if (Input.GetKeyDown(KeyCode.C))
                Go();
        }
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Ba;; hit trigger");
        Reset();
    }

    public void Go()
    {
        //pick random direction and fire
        if (Time.time % 2 == 0)
            this.rigidbody2D.AddForce(new Vector2(StartForce, 0.0f));
        else
            this.rigidbody2D.AddForce(new Vector2(StartForce*-1, 0.0f));
    }
    public void Reset()
    {
        this.transform.position = new Vector3(0, 0, 0);
        this.rigidbody2D.velocity = new Vector2(0, 0);
    }
}
