#!/bin/sh
osascript -e 'activate application "/Applications/Utilities/Terminal.app"'
cd '/Users/nunomonteiro/Documents/GitHub/RedRunner/android/Red Runner'
if [ "$('/Users/nunomonteiro/Library/Android/sdk/platform-tools/adb' get-state)" == "device" ]
then
'/Users/nunomonteiro/Library/Android/sdk/platform-tools/adb' shell am start -n 'com.aptoide.redrunner2/com.aptoide.redrunner2.UnityPlayerActivity' 2>&1 2>'/Users/nunomonteiro/Documents/GitHub/RedRunner/Assets/AppcoinsUnity/Tools/ProcessLog.out'
else
echo error: no usb device found > '/Users/nunomonteiro/Documents/GitHub/RedRunner/Assets/AppcoinsUnity/Tools/ProcessLog.out'
fi
echo 'done' > '/Users/nunomonteiro/Documents/GitHub/RedRunner/Assets/AppcoinsUnity/Tools/ProcessCompleted.out'
exit
