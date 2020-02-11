[source](https://stackoverflow.com/questions/2604727/how-can-i-connect-to-android-with-adb-over-tcp)

From a computer, if you have USB access already (no root required)
It is even easier to switch to using Wi-Fi, if you already have USB. From a command line on the computer that has the device connected via USB, issue the commands

    adb tcpip 5555
    adb connect 192.168.0.101:5555

Be sure to replace 192.168.0.101 with the IP address that is actually assigned to your device. Once you are done, you can disconnect from the adb tcp session by running:

    adb disconnect 192.168.0.101:5555
You can find the IP address of a tablet in two ways:

## Manual IP Discovery:

 Go into Android's WiFi settings, click the menu button in the action bar (the vertical ellipsis), hit Advanced and see the IP address at the bottom of the screen.

## Use ADB to discover IP:

 Execute the following command via adb:

    adb shell ip -f inet addr show wlan0
## To tell the ADB daemon return to listening over USB

    adb usb
