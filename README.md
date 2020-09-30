# SwitchDesktop
.net 4.7.2

## switch desktop
* HotKey sends arg[0] via UDP to 127.0.0.1:27000
* HotKeyAgent listen 127.0.0.1:27000, expects numbers 1-5 (as string), and switches Windows 10 virtual desktop
* used with keyboard MS ergo 4000

## FlipFlopWheel
* read HID devices in registry (@"Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum\HID")
* find "Device Parameters\FlipFlopWheel"
* Flip-Flop mouse Wheel (0->1) (admin privilege needed)

чукча не писатель
