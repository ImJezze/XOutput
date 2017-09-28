XOutput is a simple DirectInput to XInput wrapper made in C#. It uses the SCPDriver has a backend.

# About this fork

I created this fork mainly as a playground for myself to learn and work on my programming a bit. That said, I also thought that the frontend of XOutput could use some upgrades.
What I added:
* Controllers are now shown in a list and more than four controllers are supported.
* For quicker key binding, inputs can now automatically be detected in the controller options.
* The program minimizes to system tray now.
* The (system specific) instance GUID is now used for storing configs instead of the non-unique device name.
* You can directly access the systems controller control panel from the application.

# Building

XOutput can be easily built using the build.bat file inside the XOutput directory or using Visual Studio 2013+. It depends on the SlimDX developer SDK which can be found [here](http://slimdx.org/)

# How to Set Up

1. Download and install the official Xbox 360 Controller driver [here](http://www.microsoft.com/hardware/en-us/d/xbox-360-controller-for-windows) (Not necessary for Windows 8+ users)
2. Run ScpDriver.exe
3. Click install, wait until it finishes to close it
4. Run XOutput and set up your controller mappings
5. Click "Start"
