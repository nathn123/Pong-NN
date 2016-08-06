using UnityEngine;
using System.Collections;

public class Paddle_Control : MonoBehaviour {

    public float MoveAmount;
    public bool human;
	public bool Player1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (human) {
			if (Input.GetKey (KeyCode.UpArrow) && Player1)
				Move (true);
			else if (Input.GetKey (KeyCode.DownArrow) && Player1)
				Move (false);
			if (Input.GetKey (KeyCode.W) && !Player1)
				Move (true);
			else if (Input.GetKey (KeyCode.S) && !Player1)
				Move (false);
		}

	}

    public void Move(bool Up)
    {
        if (Up)
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y + MoveAmount);
        else
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y - MoveAmount);

        if (this.gameObject.transform.position.y > 3.479)
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, 3.479f);
        else if (this.gameObject.transform.position.y < -3.479)
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, -3.479f);

    }

}
