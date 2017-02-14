# SumoVizUnity


This repository constitutes the Unity project that we at accu:rate are using to create 3D visualizations that are based on the 2D output of our crowd simulation software. It builds on [the work of Daniel Büchele](https://github.com/danielbuechele/SumoVizUnity).

![Screenshot: Post-Visualization of a crowd simulation done for Hanse Sail](https://raw.githubusercontent.com/accu-rate/SumoVizUnity/develop/homepage-screen1.png "Screenshot: Post-Visualization of a crowd simulation done for Hanse Sail")


## Features

The functionality is divided into two parts. Firstly the scenario is imported at editing time into an Unity scene. Changes can be made like assigning materials, colors, terrains and so on. Secondly the trajectories are being read in at runtime (pressing Play in Unity) and the pedestrians will walk along them in an infinite loop.

So far we have used this setup to create three types of 3D output:
- **Virtual Reality Apps** for Google Cardboard or Gear VR, where the viewer is immersed in the scene and can either walk around in it freely or see through the eyes of one of the pedestrians
- **Videos** of flying along a tour through the scenery with a defined viewing angle in each waypoint
- **360° Videos** of flying along a tour through the scenary, the viewing angle is up to the viewer of the video


## License

The software is licensed under the [MIT License](LICENSE.txt):

> The MIT License is a permissive license that is short and to the point. It lets people do anything they want with your code as long as they provide attribution back to you and don’t hold you liable.

*source: [choosealicense.com](http://www.choosealicense.com/)*

### Required attribution

> Copyright (c) 2015, 2016 accu:rate GmbH, http://www.accu-rate.de/  
> Copyright (c) 2014 Daniel Büchele

Whenever you use our code (In scientific papers, in your own programs - binary as well as source code, etc.), provide proper attribution.

### Restrictions

The license above does not apply to and no license is granted for any Military Use.

