# DOTween

A Unity C# animation/tween engine. HOTween v2  
Developed by Daniele Giardini - http://www.demigiant.com

Check the docs on DOTween's website - http://dotween.demigiant.com

# Building

Requirements:
- Windows, the scripts that are executed in post build events are *.bat files.
- .NET SDK
- Unity installation of the version defined in file [_DOTween.Assembly/DOTween/Directory.Build.Props](_DOTween.Assembly/DOTween/Directory.Build.Props)

Open the solution and build at least the DOTween and DOTweenEditor projects. These projects have several post build events with *.bat files that copy files to the [Package](Package) folder structure.

The repository has a workflow defined that checks in several versions of Unity that they compile the DOTween and DOTweenEditor assemblies correctly to prevent breaking changes in the Unity API.