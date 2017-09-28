XOutput is a simple DirectInput to XInput wrapper made in C#. It uses the SCPDriver has a backend.
This version has been fixed in order to support XBCD controllers, however currently there is support for only one controller

Setup in XBCD must go like this:

![alt tag](https://raw.githubusercontent.com/joao678/XOutput/master/setup.png)

# Building

XOutput can be easily built using the build.bat file inside the XOutput directory or using Visual Studio 2013+. It depends on the SlimDX developer SDK which can be found [here](http://slimdx.org/)

# How to Set Up

1. Download and install the official Xbox 360 Controller driver [here](http://www.microsoft.com/hardware/en-us/d/xbox-360-controller-for-windows) (Not necessary for Windows 8+ users)
2. Run ScpDriver.exe
3. Click install, wait until it finishes to close it
4. Run XOutput and set up your controller mappings
5. Click "Start"
