using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TensorFlow;
using UnityEngine;


namespace TFClassify
{
    public class BoxOutline
    {
        public float YMin { get; set; } = 0;
        public float XMin { get; set; } = 0;
        public float YMax { get; set; } = 0;
        public float XMax { get; set; } = 0;
        public string Label { get; set; }
        public float Score { get; set; }
    }

    public class Detector
    {
        private static int IMAGE_MEAN = 117;
        private static float IMAGE_STD = 1;
        
        // Minimum detection confidence to track a detection.
        private static float MINIMUM_CONFIDENCE = 0.6f;

        private int inputSize;
        private TFGraph graph;
        private string[] labels;

        public Detector(byte[] model, string[] labels, int inputSize)
        {
#if UNITY_ANDROID
            TensorFlowSharp.Android.NativeBinding.Init();
#endif
            this.labels = labels;
            this.inputSize = inputSize;
            this.graph = new TFGraph();
            this.graph.Import(new TFBuffer(model));
        }


        public Task<List<BoxOutline>> DetectAsync(Color32[] data)
        {
            return Task.Run(() =>
            {
                using (var session = new TFSession(this.graph))
                {
                    using(var tensor = TransformInput(data, this.inputSize, this.inputSize))
                    {
                        var runner = session.GetRunner();
                        runner.AddInput(this.graph["image_tensor"][0], tensor)
                              .Fetch(this.graph["detection_boxes"][0],
                                     this.graph["detection_scores"][0],
                                     this.graph["detection_classes"][0],
                                     this.graph["num_detections"][0]);
                        var output = runner.Run();

                        var boxes = (float[,,])output[0].GetValue(jagged: false);
                        var scores = (float[,])output[1].GetValue(jagged: false);
                        var classes = (float[,])output[2].GetValue(jagged: false);
                        
                        foreach(var ts in output)
                        {
                            ts.Dispose();
                        }

                        return GetBoxes(boxes, scores, classes, MINIMUM_CONFIDENCE);
                    }
                }
            });
        }


        public static TFTensor TransformInput(Color32[] pic, int width, int height)
        {
            byte[] floatValues = new byte[width * height * 3];

            for (int i = 0; i < pic.Length; ++i)
            {
                var color = pic[i];

                floatValues [i * 3 + 0] = (byte)((color.r - IMAGE_MEAN) / IMAGE_STD);
                floatValues [i * 3 + 1] = (byte)((color.g - IMAGE_MEAN) / IMAGE_STD);
                floatValues [i * 3 + 2] = (byte)((color.b - IMAGE_MEAN) / IMAGE_STD);
            }

            TFShape shape = new TFShape(1, width, height, 3);

            return TFTensor.FromBuffer(shape, floatValues, 0, floatValues.Length);
        }


        private List<BoxOutline> GetBoxes(float[,,] boxes, float[,] scores, float[,] classes, double minScore)
        {
            var x = boxes.GetLength(0);
            var y = boxes.GetLength(1);
            var z = boxes.GetLength(2);

            float ymin = 0, xmin = 0, ymax = 0, xmax = 0;
            var results = new List<BoxOutline>();

            for (int i = 0; i < x; i++) 
            {
                for (int j = 0; j < y; j++) 
                {
                    if (scores [i, j] < minScore) continue;

                    for (int k = 0; k < z; k++) 
                    {
                        var box = boxes [i, j, k];
                        switch (k) {
                        case 0:
                            ymin = box;
                            break;
                        case 1:
                            xmin = box;
                            break;
                        case 2:
                            ymax = box;
                            break;
                        case 3:
                            xmax = box;
                            break;
                        }
                    }

                    int value = Convert.ToInt32(classes[i, j]);
                    var label = this.labels[value];
                    var boxOutline = new BoxOutline
                    {
                        YMin = ymin,
                        XMin = xmin,
                        YMax = ymax,
                        XMax = xmax,
                        Label = label,
                        Score = scores[i, j],
                    };

                    results.Add(boxOutline);
                }
            }

            return results;
        }
    }
}