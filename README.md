# HighResolutionVideoSample

Created: March 1, 2022 10:25 AM
Last Edited Time: March 1, 2022 10:54 AM
Stakeholders: Digital Doc
Status: Complete
Type: CodeSample

# Summary

Windows Form application that will stream High resolution video sources and capture with an integrated game controller listener. 

# Background

The currently released and upcoming IOC products and High Resolution Color Imaging devices in the dental industry are accelerating faster than Dental Imaging Acquisition Software can keep up.

# Goals

Digital Doc has provided this code for the explicit purpose of generating successful integrations for all of its products. 

### Non-Goals

This application is not designed to serve as a replacement for an integrated imaging module in any Dental Imaging Acquisition platform. It should only serve as a code reference.

# Proposed Solution

A Dental Imaging Acquisition Software Provider may use this code as reference in implementation of High Resolution Video Devices. The Tested outputs are listed below 

- 4200 x 3120 20FPS MJPEG
- 4096 x 2160 30FPS MJPEG
- 3840 x 2160 30FPS MJPEG
- 2880 x 2160 30FPS MJPEG
- 1920 x 1080 60FPS UYVY & MJPEG
- 1440 x 1080 45FPS UYVY & MJPEG
- 1280 x 720 60FPS UYVY & MJPEG
- 640 x 480 120FPS UYVY & MJPEG

### Open Questions

What are the contents of TwCameraLib.dll & TwCameraLibNET.dll

TwCameraLib is a c++ class, generated from part of the Windows SDK package for Microsoft Media Foundation. Reference of how to use the library file can be found below. TwCameraLibNET is a wrapper class to import the functionality into Windows Forms.

[msdn-code-gallery-microsoft/Official Windows Platform Sample/CaptureEngine video capture sample at master · microsoftarchive/msdn-code-gallery-microsoft](https://github.com/microsoftarchive/msdn-code-gallery-microsoft/tree/master/Official%20Windows%20Platform%20Sample/CaptureEngine%20video%20capture%20sample)

## How To Run the Sample

Open the solution (.sln) in Visual Studios 2015 or newer. .NET Framework 4.8 is required to build the sample but older versions are also supported.

- Open the solution (.sln) in Visual Studios 2015 or newer. .NET Framework 4.8 is required to build the sample but older versions are also supported.
- Once open, press F5 to start the build.
- If you get a 'BadImage' exception, modify the build target from Any CPU to x86.
- If you get any 'DLL missing' exceptions, make sure to put a copy of 'TwCameraLib.dll' & 'TwCameraLibNET.dll' in the same directory as the executable.
- If SlimDX