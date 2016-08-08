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
    public bool AllowLearning, testing;
	private GameObject Ball;

    
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (!Generated && testing)
			Gen ();
		else
			Load(Application.dataPath+"/NetworkSaves/right_8_TestCase.txt");

		if (controller == null)
			controller = this.GetComponent<Paddle_Control> ();

		if (controller.human)
			return;

		if (Ball == null)
			Ball = GameObject.Find ("Ball");
        if (Input.GetKeyDown(KeyCode.Q))
            Save();
        if (Input.GetKeyDown(KeyCode.T))
            Load();

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
        BallPosY = new Neuron("BallPosY", WeightList, HiddenLayer);
        WeightList.Clear();

        for (int i = 0; i < HiddenLayer.Count; ++i)
        {
            WeightList.Add(Random.Range(-1.0f, 1.0f));
        }
        BallVelX = new Neuron("BallVelX", WeightList, HiddenLayer);
        WeightList.Clear();

        for (int i = 0; i < HiddenLayer.Count; ++i)
        {
            WeightList.Add(Random.Range(-1.0f, 1.0f));
        }
        BallVelY = new Neuron("BallVelY", WeightList, HiddenLayer);

		Generated = true;
        Save();
    }
	void Learn()
	{
        if (!AllowLearning)
            return;
		// we need to test something to see whether the last change was good.
		// we must first get the distance between the paddle and the ball
        Debug.Log("Learning");
        float distance = Ball.transform.position.y - this.transform.position.y;
		
		// using the number of hits as a metric for success, to limit change of weights for one odd result due to dist
		//float hitdif  = SuccesfulHits - PrevSuccesfulHits;

		//combine the values together
        float error = distance /*+ (hitdif * 0.10f)*/;
		error *= 0.1f;


		float delta_output = Output.FinalOutput() * (1.0f - Output.FinalOutput()) * (error - Output.FinalOutput());
        float[] delta_hidden = new float[HiddenLayer.Count];
        for (int i = 0; i < HiddenLayer.Count; i++)
		{
			 delta_hidden[i] = HiddenLayer[i].FinalOutput() * (1.0f - HiddenLayer[i].FinalOutput()) * (HiddenLayer[i].weights[0] *delta_output);
             var test = learningRate * delta_output * HiddenLayer[i].FinalOutput();
             HiddenLayer[i].UpdateWeights(0, HiddenLayer[i].weights[0] + (learningRate * (delta_output * HiddenLayer[i].FinalOutput())));
		}

        for (int i = 0; i < HiddenLayer.Count; i++)
		{
            var test = learningRate * delta_hidden[i] * BallPosX.FinalOutput();
            BallPosX.UpdateWeights(i, BallPosX.weights[i] + (learningRate * delta_hidden[i] * BallPosX.FinalOutput()));
		}
                for (int i = 0; i < HiddenLayer.Count; i++)
		{
            var test = learningRate * delta_hidden[i] * BallPosY.FinalOutput();
            BallPosY.UpdateWeights(i, BallPosY.weights[i] + (learningRate * delta_hidden[i] * BallPosY.FinalOutput()));
		}
                for (int i = 0; i < HiddenLayer.Count; i++)
		{
            var test = learningRate * delta_hidden[i] * BallVelX.FinalOutput();
            BallVelX.UpdateWeights(i, BallVelX.weights[i] + (learningRate * delta_hidden[i] * BallVelX.FinalOutput()));
		}
                for (int i = 0; i < HiddenLayer.Count; i++)
		{
            var test = learningRate * delta_hidden[i] * BallVelY.FinalOutput();
            BallVelY.UpdateWeights(i, BallVelY.weights[i] + (learningRate * delta_hidden[i] * BallVelY.FinalOutput()));
		}

	}
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
        Learn();
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
//        StreamReader filereader = new StreamReader( UnityEditor.EditorUtility.OpenFilePanel("Select Network", "", "txt"));
//
//        BallPosX.Load(filereader.ReadLine());
//        BallPosY.Load(filereader.ReadLine());
//        BallVelX.Load(filereader.ReadLine());
//        BallVelY.Load(filereader.ReadLine());
//
//        for (int i = 0; i < HiddenLayer.Count; ++i)
//            HiddenLayer[i].Load(filereader.ReadLine());
//
//        Output.Load(filereader.ReadLine());
//
    }
	public void Load(string path)
	{
		StreamReader filereader = new StreamReader(path);
		Gen ();
		BallPosX.Load(filereader.ReadLine());
		BallPosY.Load(filereader.ReadLine());
		BallVelX.Load(filereader.ReadLine());
		BallVelY.Load(filereader.ReadLine());

		for (int i = 0; i < HiddenLayer.Count; ++i)
			HiddenLayer[i].Load(filereader.ReadLine());

		Output.Load(filereader.ReadLine());

	}
}
