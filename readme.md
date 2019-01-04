## Phoenix Diagnostics Client
**Note has been replaced with https://github.com/CrossTheRoadElec/Phoenix-Tuner-Release.**

**However this can be used as a reference for custome diagnostics interfaces.**

![image](https://user-images.githubusercontent.com/28712271/45908233-7db6ac80-bdc9-11e8-970b-1b5e313c55bf.png)
### What is this?
This is a repository that provides everything someone will need to put the 2018 Diagnostic Server onto a RoboRIO and test with it. The Diagnostic Server is the 2019 replacement for the Web-Based Configuration Utility that was provided during the previous years. It currently includes all the features of the Web-Based Config with aspirations to include more.

Note **2019 FRC teams** should use the Tuner release that is packaged with *Phoenix 2019 Installer* as it contains the 2019 binaries.
Otherwise download the binary [releases](https://github.com/CrossTheRoadElec/Phoenix-diagnostics-client/releases).

### Contents:
 - Source Code for the client, ready to be built and ran out of the box.
 - Binary files under CTRE.Phoenix.Diagnostics/Binary/ that contains the Server Executable and startup scripts to automatically run the server on RoboRIO startup.

### How to get it running
1. Build application using Visual Studio 2017
2. Run application
3. Connect RoboRIO to pc over USB
4. Press "Update Binaries" button under Prepare the Target Robot Controller
5. Type "172.22.11.2:1250" inside the Diagnostic Server Address Bar
6. Wait for a connection, and you're good to go

### How it works:
The server hosts an http server that the client is able to query and retrieve information from. The client does this with http requests, while the server responds with the specified information.

#### Details on how it works
Every http request is sent with a modified url. The url is based on this template:
`http://<address>:<port>/?device=<model>&id=<id>&action=<action>&<furtherArgs>`
 - address - This is the address of the target the server is on. For a RoboRIO it will be 172.22.11.2 over USB, and 10.TE.AM.x over ethernet/wifi.
 - port - This is the port of the serveras specified in the startup script. This can be changed in the script by the user if they want a different port. By default it is **1250**.
 - model - This is a string literal of the model of the target device. It can be: 
    - srx
    - spx
    - canif
    - pigeon
    - ribbonPigeon
    - pcm
    - pdp
 - id - This is the id of the target device.
 - action - This is the action to do with regards to the target device. It can include:
    - getversion
    - getdevices
    - blink
    - setid (newid=)
    - setname (newname=)
    - selftest
    - fieldupgrade
    - progress
    - getconfig
    - setconfig
 - furtherArgs - These are arguments specific to a certain command. As of the time of this writing this includes only setid and setname, as a new id/name must be specified in the url to set these names to.

