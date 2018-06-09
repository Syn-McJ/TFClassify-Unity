# What

This is an example of using model trained with TensorFlow in Unity application for image classification and object detection. It's a quick port of [TF Classify and TF Detect examples](https://github.com/tensorflow/tensorflow/tree/master/tensorflow/examples/android) from TensorFlow repo, using [TensorFlowSharp](https://github.com/migueldeicaza/TensorFlowSharp) for gluing it all together.

Classify results:

![](https://raw.githubusercontent.com/Syn-McJ/TFClassify-Unity/master/SampleImages/classify1.jpg)
![](https://raw.githubusercontent.com/Syn-McJ/TFClassify-Unity/master/SampleImages/classify2.jpg)
![](https://raw.githubusercontent.com/Syn-McJ/TFClassify-Unity/master/SampleImages/classify3.jpg)

Detect results:

![](https://raw.githubusercontent.com/Syn-McJ/TFClassify-Unity/master/SampleImages/detect1.png)
![](https://raw.githubusercontent.com/Syn-McJ/TFClassify-Unity/master/SampleImages/detect2.png)
![](https://raw.githubusercontent.com/Syn-McJ/TFClassify-Unity/master/SampleImages/detect3.png)

Note that performance is worse than in TensorFlow Android example and at this moment I'm not quite sure how to improve that. Hopefully this will be enough to get you started.

# How

You'll need Unity 2017.1 or above and [Unity TensorFlow Plugin](https://s3.amazonaws.com/unity-ml-agents/0.3/TFSharpPlugin.unitypackage).

- Open the project in Unity.
- Install TensorFlow plugin.
- Open Classify or Detect scene in Assets folder.
- In `Edit -> Player Settings -> Other settings` add `ENABLE_TENSORFLOW` to the `Scripting Define Symbols` for the target platform.
- In `Other settings` also set `Scripting runtime version` to `Experimental (.NET 4.6 Equivalent)`.
- Build and run.

More info can be found [here](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Using-TensorFlow-Sharp-in-Unity.md).

If you want to use your own model, simply rename your file extension from .pb to .bytes, put it in Resources and set Model File and Labels File to your model and labels in main camera object of the scene you chose.

# Notes

I'm neither Unity nor TensorFlow expert, so if you found any problems with this example feel free to open an issue.
