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
- In `Other settings` also set `Scripting runtime version` to `.NET 4.6 Equivalent`.
- Build and run.

More info can be found [here](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Using-TensorFlow-Sharp-in-Unity.md).

***To use your own model:***

- Make sure your model trained with TensorFlow 1.4 if you use 0.3 version of the Unity plugin that I linked above. You can also try 0.4 version that use TensorFlow 1.7.1.
- Change extension of your model from .pb to .bytes.
- Put your model and labels in Resources.
- Set Model file and Labels file to your model and labels in main camera object of the scene you chose.
- If neccesary, change `classifyImageSize`, `IMAGE_MEAN`, `IMAGE_STD`, `INPUT_NAME` and `OUTPUT_NAME` to suit your model.

# Notes

I'm neither Unity nor TensorFlow expert, so if you found any problems with this example feel free to open an issue.
