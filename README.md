# AirClip - Lightweight Video Editor

## WARNING
This is less of a usable app and more of a learning project. 
Even if it barely counts as an editor though, I'd still like to showcase the code and the effort that went into it.

## Setting up your project environment
1. First, install .NET SDK 9.0.2 at https://dotnet.microsoft.com/en-us/download
2. Then, install Avalonia UI Templates by running the command `dotnet new install Avalonia.Templates` on your system terminal
3. Open JetBrains Rider and install the AvaloniaRider plugin by Friedrich von Never
4. Clone this repository and open the project solution (.sln) file with Rider

## Setting up FFmpeg
AirClip makes calls to FFmpeg processes on your machine, so it is a prerequisite that users install FFmpeg before running AirClip.
In future versions, AirClip will install FFmpeg automatically for the user.
For now, we ask that you install FFmpeg on your own and save the path to the FFmpeg `/bin` (binary) folder for later.
<br/><br/>
Regarding installation options, we recommend that:
- Windows users install it through Chocolatey (https://community.chocolatey.org/packages/ffmpeg), and
- macOS users install it through homebrew (https://formulae.brew.sh/formula/ffmpeg).

Regarding 32-bit vs. 64-bit architectures, FFMpegCore (the FFmpeg interface we use) has this to say: 
> # Supporting both 32 and 64 bit processes
> If you wish to support multiple client processor architectures, you can do so by creating two folders, `x64` and `x86`, in the `BinaryFolder` directory.
> Both folders should contain the binaries (`ffmpeg.exe` and `ffprobe.exe`) built for the respective architectures. 
>
> By doing so, the library will attempt to use either `/{BinaryFolder}/{ARCH}/(ffmpeg|ffprobe).exe`.
>
> If these folders are not defined, it will try to find the binaries in `/{BinaryFolder}/(ffmpeg|ffprobe.exe)`.
>
> (`.exe` is only appended on Windows)

## Running the app
Once your environment is fully set up, open the project with Rider and press the Run button in the Rider toolbar.
