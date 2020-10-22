NativePicker is iOS/Android plugin that can display native date/time/custom item list pickers
To start working with NativePicker, put NativePicker prefab to the scene. Then you can access plugin functionality
using NativePicker.Instance object.
If script links are broken after loading test project, attach Plugins\NativePicker\NativePicker.cs to NativePicker object, and TestScene\TestScene.cs to Main Camera object
According to Apple UI guidelines picker should be displayed at botton of the screen on iPhone device, and only inside popover controller on iPad device, that's why Show* functions has position parameter.
You can fing example of usage of these methods in TestSceneScript.cs file.