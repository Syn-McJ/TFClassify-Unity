# What

This is an example of using model trained with TensorFlow in Unity application for image classification. It's a quick port of [TF Classify example](https://github.com/tensorflow/tensorflow/tree/master/tensorflow/examples/android) from TensorFlow repo, using [TensorFlowSharp](https://github.com/migueldeicaza/TensorFlowSharp) for gluing it all together.


# How

You'll need Unity 2017.1 or above and [Unity TensorFlow Plugin](https://s3.amazonaws.com/unity-ml-agents/0.3/TFSharpPlugin.unitypackage).

- Open the project in Unity.
- Install TensorFlow plugin.
- Open Scene1 in Assets folder.
- In `Edit -> Player Settings -> Other settings` add `ENABLE_TENSORFLOW` to the `Scripting Define Symbols` for the target platform.
- In `Other settings` also set `Scripting runtime version` to `Experimental (.NET 4.6 Equivalent)`.
- Build and run.

More info can be found [here](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Using-TensorFlow-Sharp-in-Unity.md).

If you want to use your own model, simply rename your file extension from .pb to .bytes and put it in Resources.

# Notes

I'm neither Unity nor TensorFlow expert, so if you found any problems with this example feel free to open an issue.
