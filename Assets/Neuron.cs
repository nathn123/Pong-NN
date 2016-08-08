using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
public class Neuron {

	// Use this for initialization
    float input, output;
    public List<float> weights;
    List<Neuron> Outputs;

    [XmlAttribute("name")]
    string Name;

    public Neuron(string Name_,List<float> weights_, List<Neuron> Outputs_)
    {
        Name = Name_;
		weights = new List<float>( weights_);
        Outputs = Outputs_;
    }
    public void Input(float inputval)
    {
        input += inputval; // sum inputs from prev layer
    }

    public void UpdateWeights(int pos, float val)
    {
        weights.RemoveAt(pos);
        weights.Insert(pos, val);
    }

    public void Output()
    {
		if (EvaluationFunction () > 0.5f) {
			for (int i = 0; i < Outputs.Count; i++) {
				Outputs [i].Input (1 * weights [i]);
			}
		} else if (EvaluationFunction () < -0.5f) {
			for (int i = 0; i < Outputs.Count; i++) {
				Outputs [i].Input (-1 * weights [i]);
			}
		}
		output = EvaluationFunction();
		input = 0; // reset input for next run

    }
	public float FinalOutput()
	{
		return output;
	}
    float EvaluationFunction()
    {
		return (float)(2 / (1 + System.Math.Exp(-2 * input)) - 1);
    }
    public string Save()
    {
        string Data;

        Data = Name + " ";

        for(int i = 0; i < weights.Count; i++)
            Data+= weights[i].ToString() + " ";

        return Data;
    }
    public void Load(string data)
    {
       var splitdata =  data.Split(' ');
       Name = splitdata[0];
        weights = new List<float>();
        for (int i = 1; i < splitdata.Length-1; ++i)
            weights.Add(float.Parse(splitdata[i]));

    }
}
