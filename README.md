# HighResolutionVideoSample

Created: March 1, 2022 10:25 AM\
Last Edited Time: Aug. 1, 2022 10:09 AM\
Stakeholders: Digital Doc\
Status: Complete\
Type: CodeSample\
Author: James Petko (JPetko@Digi-Doc.com || Petko.James@gmail.com)

### Summary

Windows Form application that will stream High resolution video sources and capture with an integrated game controller listener. This particular application sample does not have integrated handling for older directshow protocols./
To properly implement a full video solution, first consider the scope of supported devices.

### Background

The currently released and upcoming IOC products and High Resolution Color Imaging devices in the dental industry are accelerating faster than Dental Imaging Acquisition Software can keep up.

### Goals

Digital Doc has provided this code for the explicit purpose of generating successful integrations for all of its current and legacy products. 

### Non-Goals

This application is not designed to serve as a replacement for an integrated imaging module in any Dental Imaging Acquisition platform. It should only serve as a code reference.

### Proposed Solution

A Dental Imaging Acquisition Software Provider may use this code as reference in implementation of High Resolution Video Devices. The Tested outputs are listed below. 

##### Using Media Foundation:
- 4200 x 3120 20FPS MJPEG
- 4096 x 2160 30FPS MJPEG
- 3840 x 2160 30FPS MJPEG
- 2880 x 2160 30FPS MJPEG
- 1920 x 1080 60FPS UYVY & MJPEG
- 1440 x 1080 45FPS UYVY & MJPEG
- 1280 x 720 60FPS UYVY & MJPEG
- 640 x 480 120FPS UYVY & MJPEG

##### Using Direct X:
- 640 x 480 ~30FPS YUY2
- 1280 x 720 ~30FPS YUY2
- 720 x 576 ~30FPS YUY2

### Open Questions

##### What are the contents of TwCameraLib.dll & TwCameraLibNET.dll

TwCameraLib is a c++ class, generated from part of the Windows SDK package for Microsoft Media Foundation. Reference of how to use the library file can be found below./
TwCameraLibNET is a wrapper class to import the functionality into Windows Forms./
SlimDX is an open source library which will funciton for a listener for Game Controllers.

[msdn-code-gallery-microsoft/Official Windows Platform Sample/CaptureEngine video capture sample at master · microsoftarchive/msdn-code-gallery-microsoft](https://github.com/microsoftarchive/msdn-code-gallery-microsoft/tree/master/Official%20Windows%20Platform%20Sample/CaptureEngine%20video%20capture%20sample)

### How To Run the Sample

Open the solution (.sln) in Visual Studios 2015 or newer. .NET Framework 4.8 is required to build the sample but older versions are also supported.

- Open the solution (.sln) in Visual Studios 2015 or newer. .NET Framework 4.8 is required to build the sample but older versions are also supported.
- Once open, press F5 to start the build.
- If you get a 'BadImage' exception, modify the build target from Any CPU to x86.\Removing Game Controller functionality will then allow operating a camera in another platform.
- If you get any 'DLL missing' exceptions, make sure to put a copy of 'TwCameraLib.dll' & 'TwCameraLibNET.dll' in the same directory as the executable.

### Alternatives to Consider

Capture may also funciton from a keystroke on the keyboard.\
If this is the case, Digital Doc would implement a listener script that will pass the Game Controller signal to a keystroke(or series of keystrokes).