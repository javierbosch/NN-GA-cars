using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    public int[] Layers;
    public double[] Weights;
    public double[] Biases;

    public void inst(int[] layers, double[] weights,double[] biases)
    {
        if(layers.Length<2)
        {
            return;
        }

        //Assign layers size
        this.Layers = layers;
        //Assign weights
        this.Weights = weights;
        //Assign biases
        this.Biases = biases;
    }

    public double Sigmoid(double x)
    {
        return 1 / (1 + Math.Exp(-x));
    }

    public double[] Run(double[] Input)
    {
        //Check if the input array size is equal to the input layer size
        if(Input.Length!=Layers[0])
        {
            return null;
        }

        int WeightsIndex = 0;

        double[] PrevLayers = Input ;

        //Calculate the output layer 
        for (int i=1;i<Layers.Length;i++)
        {
            double[] CurrentLayer=new double[Layers[i]];

            for(int j=0;j<Layers[i];j++)
            {
                double Sum = 0;

                for(int h=0;h<Layers[i-1];h++)
                {
                    Sum += PrevLayers[h] * Weights[WeightsIndex];
                    WeightsIndex++;
                }
                CurrentLayer[j] = Math.Tanh(Sum + Biases[i-1]);
            }
            PrevLayers = CurrentLayer;
        }
        double[] Output = PrevLayers;
        return Output;
    }
}
