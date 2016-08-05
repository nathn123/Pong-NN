using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AiController : MonoBehaviour {

	// Use this for initialization
    Neuron Output, BallPosX,BallPosY,BallVelX, BallVelY;
    List<Neuron> HiddenLayer;
    int hiddenlayersize, saveinteval;
    bool save;
	private GameObject Ball;

    
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Ball == null)
			Ball = GameObject.Find ("Ball");

		var velocity = Ball.rigidbody2D.velocity;
		var position = Ball.rigidbody2D.position;

		float VelInputX,VelInputY

		//pass values to the first layer
		BallPosX.Input();
		BallPosY
	
	}
    public void Gen()
    {
        //due to constructor of the neuron, we create the network back to front
        Output = new Neuron("Output",new List<float>(),new List<Neuron>());
        HiddenLayer = new List<Neuron>();
        var OutputList = new List<Neuron>();
        var WeightList = new List<float>();
        OutputList.Add(Output);
        for(int i = 0; i < hiddenlayersize;++i)
        {
            WeightList.Add(Random.Range(-1.0f,1.0f));
            HiddenLayer.Add(new Neuron("HiddenNeuron" + i.ToString(), WeightList, OutputList));
            WeightList.Clear(); // only one weight needed
        }

        for (int i = 0; i < HiddenLayer.Count; ++i)
        {
            WeightList.Add(Random.Range(-1.0f, 1.0f));
        }
        BallPosX = new Neuron("BallPosX", WeightList, HiddenLayer);
        WeightList.Clear();

        for (int i = 0; i < HiddenLayer.Count; ++i)
        {
            WeightList.Add(Random.Range(-1.0f, 1.0f));
        }
        BallPosY = new Neuron("BallPosX", WeightList, HiddenLayer);
        WeightList.Clear();

        for (int i = 0; i < HiddenLayer.Count; ++i)
        {
            WeightList.Add(Random.Range(-1.0f, 1.0f));
        }
        BallVelX = new Neuron("BallPosX", WeightList, HiddenLayer);
        WeightList.Clear();

        for (int i = 0; i < HiddenLayer.Count; ++i)
        {
            WeightList.Add(Random.Range(-1.0f, 1.0f));
        }
        BallVelY = new Neuron("BallPosX", WeightList, HiddenLayer);
        WeightList.Clear();

    }
	public void PointLoss()
	{
	}
	public void PointGain()
	{
	}

    public void Save()
    {

    }
    public void Load()
    {

    }
}
