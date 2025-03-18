Please read the Manual.pdf to learn how to set up the demos correctly ("Explore the demos > In editor" section).

These demos use "Input Manager" instead of "Input System", if you are using Input System, you can change the input setting to "Both" to make the demos work. The setting is under "Project Settings > Player > Other Settings > Active Input Handling".

The demos are a great learning resource about how to achieve different gameplays with Projectile Toolkit. You can also use the scripts and prefabs of the demos directly for fast prototyping.

------

In version 3.0, some demo scripts have been rewritten to have "Curated control methods", which enhances their reusability. You can invoke them through your own scripts or visual scripts. The supported classes and usage are listed below (new supported classes will be added aperiodically):

 Prefab Workflow

– CurvyTest:
1. In scene "05 Aerodynamic Movement", drag and drop "Ball Launcher" into your folder to make a prefab,
2. turn on "Controlled By Another Script" of "CurvyTest" component on the prefab, disable "CurvyTestParamsUI" component,
3. create an instance of the prefab in your scene, and now you are able to call "Aim(...)" and "Launch()" instance methods in your own script / visual script.

Note: the field "Ball Prefab" and "Predictor" have value "None", you need to create them in your scene and drag and drop to reference them.

– CannonLike:
1. In scene "03 Cannon-Like Weapon", drag and drop "Cannon" into your folder to make a prefab,
2. turn on "Controlled By Another Script" of "CannonLike" component on the prefab,
3. create an instance of the prefab in your scene, and now you are able to call "Aim(...)" and "Launch()" instance methods in your own script / visual script.

– JumpTester:
1. In scene "00 Jump", drag and drop "cube1" into your folder to make a prefab,
2. turn on "Controlled By Another Script" of "JumpTester" component on the prefab,
3. create an instance of the prefab in your scene, and now you are able to call "JumpToPosition(...)" instance method in your own script / visual script.

 Scene Workflow

The process is similar to the prefab workflow but you directly modify the GameObjects in the demo scenes or duplicated scenes, instead of making prefabs to do so. This is great if you just want to tinker around. Caveats: scene 00: make sure to disable "ClickToJump" component on the Camera.

This list does not imply that you can only control the listed scripts and methods. For the demo scripts, all the variables shown in the inspector are public (not best practice but for the sake of simplicity) and you can change them through your scripts during runtime.