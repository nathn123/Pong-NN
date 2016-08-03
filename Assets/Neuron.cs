using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
public class Neuron {

	// Use this for initialization
    float input, output;
    List<float> weights;
    List<Neuron> Outputs;

    [XmlAttribute("name")]
    string Name;

    public Neuron(string Name_,List<float> weights_, List<Neuron> Outputs_)
    {
        Name = Name_;
        weights = weights_;
        Outputs = Outputs_;
    }
    public void Input(float inputval)
    {

    }

    void Output()
    {
        if(EvaluationFunction() > 0)
        {
            for(int i = 0; i < Outputs.Count; i++)
            {
                Outputs[i].Input(1 * weights[i]);
            }
        }

    }
    float EvaluationFunction()
    {
        return 0;
    }
    void Save()
    {

    }
}
