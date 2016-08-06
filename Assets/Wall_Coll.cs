using UnityEngine;
using System.Collections;

public class Wall_Coll : MonoBehaviour {

	// Use this for initialization
	public bool left;

	void Start () {
	
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
				else if (ele.name.Contains ("Left")) {
					ele.text = ((int.Parse(ele.text))+1).ToString ();
				} 
		}
    }
}
