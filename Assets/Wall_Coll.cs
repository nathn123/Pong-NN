using UnityEngine;
using System.Collections;

public class Wall_Coll : MonoBehaviour {

	// Use this for initialization
	public bool left;
    public GameObject LeftPaddle, RightPaddle, ball;

	void Start () {
        LeftPaddle = GameObject.Find("Left Paddle");
        RightPaddle = GameObject.Find("Right Paddle");
        ball = GameObject.Find("Ball");

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
		Debug.Log("Ball hit trigger");

		var UITextEle = FindObjectsOfType<GUIText> ();
		if (left)
		{
			foreach (var ele in UITextEle)
				if (ele.name.Contains ("Right")) {
					ele.text = ((int.Parse(ele.text))+1).ToString ();
				}
            LeftPaddle.GetComponent<AiController>().PointLoss();
            RightPaddle.GetComponent<AiController>().PointGain();

		}
        else
        {
            foreach (var ele in UITextEle)
                if (ele.name.Contains("Left"))
                {
                    ele.text = ((int.Parse(ele.text)) + 1).ToString();
                }
            LeftPaddle.GetComponent<AiController>().PointGain();
            RightPaddle.GetComponent<AiController>().PointLoss();

        }
        ball.GetComponent<Ball_Control>().Go();


    }
}
