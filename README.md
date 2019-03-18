# HueHue
### An Ambilight like project using the NZXT Hue+

> Not really for any type of production use

## Requirements

- dotnet core 2.2
- Visual Studio Code 2017/2019

## Light setup

Currently it's static for a 24" screen with the input being from bottom right (on the right side) to the top, left then bottom


## How it works

Basically I use SharpDX to pull the frames from the screen, get the RGB values from the pointer, send that data to a buffer and update the Hue+ every 33 milliseconds using a timer (roughly 30hz polling rate). This method is the fastest I could think of by pulling data from the graphics card and using minimal CPU usage in the process.