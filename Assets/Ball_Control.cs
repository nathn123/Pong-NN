using UnityEngine;
using System.Collections;

public class Ball_Control : MonoBehaviour {

    public float StartForce, maxSpeed;
    public bool Testing;
    public int bounces;
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
		if(GetComponent<Rigidbody2D>().velocity.magnitude > maxSpeed){
			GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maxSpeed);
		}
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Ball hit trigger");
        bounces++;
        if (bounces > 50)
            Go();

        Reset();
    }

    public void Go()
    {
        //pick random direction and fire
        bounces = 0;
		float xForce  = Random.Range((float)(StartForce*0.1),StartForce);
		float yForce = StartForce - xForce;	
		if (Random.Range(0,2) == 0)
			this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce, yForce));
        else
			this.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce*-1, yForce*-1));
    }
    public void Reset()
    {
        this.transform.position = new Vector3(0, 0, 0);
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
