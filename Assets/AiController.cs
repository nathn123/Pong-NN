using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class AiController : MonoBehaviour {

	// Use this for initialization
    Neuron Output, BallPosX,BallPosY,BallVelX, BallVelY;
    List<Neuron> HiddenLayer;
	public int hiddenlayersize, saveinteval , savenum;
    public float learningRate;
	int SuccesfulHits, PrevSuccesfulHits = 0; // the gradient
	Paddle_Control controller;
    bool save, Generated;
	private GameObject Ball;

    
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (!Generated)
			Gen ();

		if (controller == null)
			controller = this.GetComponent<Paddle_Control> ();

		if (Ball == null)
			Ball = GameObject.Find ("Ball");

		var velocity = Ball.GetComponent<Rigidbody2D>().velocity;
		var position = Ball.GetComponent<Rigidbody2D>().position;

		float VelInputX, VelInputY, PosInputX, PosInputY;

		//we need to know which paddle we are controlling
		if (this.gameObject.transform.position.x > 0) {
			if (position.x > 0) // on the same side
				PosInputX = 1;
			else
				PosInputX = -1;
		} else {
			if (position.x < 0) // on the same side
				PosInputX = 1;
			else
				PosInputX = -1;
		}
		// this test checkes if the ball  is above of below the paddle
		if (this.gameObject.transform.position.y > position.y)
			PosInputY = 1;
		else 
			PosInputY = -1;
		//this check tests if the ball is moving above or below the paddle
		if (velocity.y > 0)
			VelInputY = 1;
		else
			VelInputY = -1;

		// this check tests if the ball is moving towards or away from the paddle
		if (this.gameObject.transform.position.x > 0) {
			if (velocity.x > 0) // on the same side
				VelInputX = 1;
			else
				VelInputX = -1;
		} else {
			if (velocity.x < 0) // on the same side
				VelInputX = 1;
			else
				VelInputX = -1;
		}
		

		//pass values to the first layer
		BallPosX.Input(PosInputX);
		BallPosY.Input(PosInputY);
		BallVelX.Input(VelInputX);
		BallVelY.Input(VelInputY);

		BallPosX.Output ();
		BallPosY.Output ();
		BallVelX.Output ();
		BallVelY.Output ();

		foreach (var node in HiddenLayer)
			node.Output ();
		Output.Output ();
		var finalval = Output.FinalOutput ();
		if (finalval > 0.5f)
			controller.Move (true);
		else if (finalval < -0.5f)
			controller.Move (false);
			
	
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

		Generated = true;
        Save();
    }
	void Learn()
	{
		// we need to test something to see whether the last change was good.
		// we must first get the distance between the paddle and the ball
        Debug.Log("Learning");
		float distance  = Vector2.Distance(new Vector2(this.transform.position.x,this.transform.position.y),
											new Vector2(Ball.transform.position.x,Ball.transform.position.y));
		
		// using the number of hits as a metric for success, to limit change of weights for one odd result due to dist
		float hitdif  = SuccesfulHits - PrevSuccesfulHits;

		//combine the values together
		float error = distance +(hitdif*0.10f);
		error *= 0.1f;


		float delta_output = Output.FinalOutput() * (1.0f - Output.FinalOutput()) * (error - Output.FinalOutput());
        float[] delta_hidden = new float[HiddenLayer.Count];
        for (int i = 0; i < HiddenLayer.Count; i++)
		{
			 delta_hidden[i] = HiddenLayer[i].FinalOutput() * (1.0f - HiddenLayer[i].FinalOutput()) * (HiddenLayer[i].weights[0] *delta_output);
            HiddenLayer[i].weights[0] += learningRate * delta_output * HiddenLayer[i].FinalOutput();
		}

        for (int i = 0; i < HiddenLayer.Count; i++)
		{
			BallPosX.weights[i] += learningRate * delta_hidden[i] * BallPosX.FinalOutput();
		}
                for (int i = 0; i < HiddenLayer.Count; i++)
		{
			BallPosY.weights[i] += learningRate * delta_hidden[i] * BallPosY.FinalOutput();
		}
                for (int i = 0; i < HiddenLayer.Count; i++)
		{
			BallVelX.weights[i] += learningRate * delta_hidden[i] * BallVelX.FinalOutput();
		}
                for (int i = 0; i < HiddenLayer.Count; i++)
		{
			BallVelY.weights[i] += learningRate * delta_hidden[i] * BallVelY.FinalOutput();
		}

	}
    //{
    //    //get momentum values (delta values from last pass)
    //    double[] delta_hidden = new double[nn.NumberOfHidden + 1];
    //    double[] delta_outputs = new double[nn.NumberOfOutputs];

    //    // Get the delta value for the output layer
    //    for (int i = 0; i < nn.NumberOfOutputs; i++)
    //    {
    //        delta_outputs[i] =
    //            nn.Outputs[i] * (1.0 - nn.Outputs[i]) * (target[i] - nn.Outputs[i]);
    //    }
    //    // Get the delta value for the hidden layer
    //    for (int i = 0; i < nn.NumberOfHidden + 1; i++)
    //    {
    //        double error = 0.0;
    //        for (int j = 0; j < nn.NumberOfOutputs; j++)
    //        {
    //            error += nn.HiddenToOutputWeights[i, j] * delta_outputs[j];
    //        }
    //        delta_hidden[i] = nn.Hidden[i] * (1.0 - nn.Hidden[i]) * error;
    //    }
    //    // Now update the weights between hidden & output layer
    //    for (int i = 0; i < nn.NumberOfOutputs; i++)
    //    {
    //        for (int j = 0; j < nn.NumberOfHidden + 1; j++)
    //        {
    //            //use momentum (delta values from last pass),
    //            //to ensure moved in correct direction
    //            nn.HiddenToOutputWeights[j, i] += nn.LearningRate * delta_outputs[i] * nn.Hidden[j];
    //        }
    //    }
    //    // Now update the weights between input & hidden layer
    //    for (int i = 0; i < nn.NumberOfHidden; i++)
    //    {
    //        for (int j = 0; j < nn.NumberOfInputs + 1; j++)
    //        {
    //            //use momentum (delta values from last pass),
    //            //to ensure moved in correct direction
    //            nn.InputToHiddenWeights[j, i] += nn.LearningRate * delta_hidden[i] * nn.Inputs[j];
    //        }
    //    }
    //}
	public void PointLoss()
	{
		Debug.Log ("Point Lost");
		SuccesfulHits--;
        Learn();
        Reset();

	}
	public void PointGain()
	{
		Debug.Log ("Point Gained");
		SuccesfulHits++;
        //Learn();
        Reset();
	}

    public void Reset()
    {
        this.transform.position = new Vector3(this.transform.position.x, 0);
        savenum++;
        if (savenum % saveinteval == 0)
            Save();
    }

    public void Save()
    {
        string name;
        if (this.name == "Left Paddle")
            name = "left";
        else
            name = "right";
        string path = Application.dataPath + "/NetworkSaves/"+name+"_"+savenum.ToString()+".txt";
        StreamWriter fileWriter = File.CreateText(path);
        //,,, 
        var test = BallPosX.Save();
        fileWriter.WriteLine(BallPosX.Save());
        fileWriter.WriteLine(BallPosY.Save());
        fileWriter.WriteLine(BallVelX.Save());
        fileWriter.WriteLine(BallVelY.Save());

        for (int i = 0; i < HiddenLayer.Count; ++i)
            fileWriter.WriteLine(HiddenLayer[i].Save());

        fileWriter.WriteLine(Output.Save());
        fileWriter.Close();
    }
    public void Load()
    {

    }
}
