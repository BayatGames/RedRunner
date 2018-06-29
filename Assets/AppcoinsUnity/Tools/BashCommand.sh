#!/bin/sh
osascript -e 'activate application "/Applications/Utilities/Terminal.app"'
cd '/Users/nunomonteiro/Documents/GitHub/RedRunner/Android/Red Runner'
'/Users/nunomonteiro/Library/Android/sdk/platform-tools/adb' install -r './build/outputs/apk/release/Red Runner-release.apk'
echo 'done' > '/Users/nunomonteiro/Documents/GitHub/RedRunner/Assets/AppcoinsUnity/Tools/ProcessCompleted.out'
exit
