using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TensorFlow;
using UnityEngine;


namespace TFClassify
{
    public class Classifier
    {
        private static int INPUT_SIZE = 224;
        private static int IMAGE_MEAN = 117;
        private static float IMAGE_STD = 1;
        private static string INPUT_NAME = "input";
     	private static string OUTPUT_NAME = "output";

        private TFGraph graph;
	    private string[] labels;

        
        public Classifier(byte[] model, string[] labels)
        {
#if UNITY_ANDROID
            TensorFlowSharp.Android.NativeBinding.Init();
#endif
            this.labels = labels;
            this.graph = new TFGraph();
		    this.graph.Import(model, "");
        
        }


        public Task<List<KeyValuePair<string, float>>> ClassifyAsync(Color32[] data)
        {
            return Task.Run(() =>
            {
                var map = new List<KeyValuePair<string, float>>();

                using(var session = new TFSession(graph))
                {
                    var tensor = TransformInput(data, INPUT_SIZE, INPUT_SIZE);
                    var runner = session.GetRunner ();
                    runner.AddInput (graph [INPUT_NAME] [0], tensor).Fetch (graph [OUTPUT_NAME] [0]);
                    var output = runner.Run ();
                    
                    // output[0].Value() is a vector containing probabilities of
                    // labels for each image in the "batch". The batch size was 1.
                    // Find the most probably label index.

                    var result = output [0];
                    var rshape = result.Shape;
                    
                    if (result.NumDims != 2 || rshape [0] != 1) {
                        var shape = "";
                        foreach (var d in rshape) {
                            shape += $"{d} ";
                        }
                        
                        shape = shape.Trim ();
                        Debug.Log("Error: expected to produce a [1 N] shaped tensor where N is the number of labels, instead it produced one with shape [{shape}]");
                        Environment.Exit (1);
                    }

                    var probabilities = ((float [] [])result.GetValue (jagged: true)) [0];

                    for (int i = 0; i < labels.Length; i++) {
                        map.Add(new KeyValuePair<string, float>(labels[i], probabilities[i] * 100));
                    }
                }

                return map.OrderByDescending(x => x.Value).ToList();
            });
        }


        public static TFTensor TransformInput(Color32[] pic, int width, int height)
		{
			float[] floatValues = new float[width * height * 3];

			for (int i = 0; i < pic.Length; ++i)
			{
				var color = pic[i];

				floatValues [i * 3 + 0] = (color.r - IMAGE_MEAN) / IMAGE_STD;
				floatValues [i * 3 + 1] = (color.g - IMAGE_MEAN) / IMAGE_STD;
				floatValues [i * 3 + 2] = (color.b - IMAGE_MEAN) / IMAGE_STD;
			}

			TFShape shape = new TFShape(1, width, height, 3);

			return TFTensor.FromBuffer(shape, floatValues, 0, floatValues.Length);
		}
    }
}