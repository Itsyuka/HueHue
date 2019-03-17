# HueHue
### An Ambilight like project using the NZXT Hue+

> Not really for any type of production use, has a few bugs with momentary CPU spikes and slight overuse of RAM

## Requirements

- dotnet core preview 3 (it'll work with lower dotnet versions)
- Windows 8(.1)/10 (maybe 7 /shrug)
- Visual Studio Code 2019 (2017 might work)

## Light setup

Currently it's static for a 24" screen with the input being from bottom right


## How it works

Basically I use SharpDX to pull the frames from the screen, get the RGB values from the pointer, send that data to a buffer and update the Hue+ every 33 milliseconds using a timer (roughly 30hz polling rate). This method is the fastest I could think of by pulling data from the graphics card and using minimal CPU usage in the process.