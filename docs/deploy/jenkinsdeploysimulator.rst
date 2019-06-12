Jenkins deploys on simulator
============

MSBuild Configuration
-----------

::

    msbuild /p:Configuration="Test" /p:Platform="iPhoneSimulator" /t:Restore LiveScore.iOS.csproj
    msbuild /p:Configuration="Test" /p:Platform="iPhoneSimulator" /t:ReBuild LiveScore.iOS.csproj /p:MtouchArch=x86_64

*Notes: without "/p:MtouchArch=x86_64", the application cannot be installed on simulator

Install Application on Simulator
-----------

- Using xcrun commands
- List out iOS simulators
::

    xcrun instruments -s devices

- Uninstall application
::

    xcrun simctl uninstall {simalator_UDID} Score247.LiveScore
    xcrun simctl uninstall 13C278BF-A473-4654-B7AE-D1569ADA54E4 Score247.LiveScore

- Install application
::

    xcrun simctl install {simalator_UDID} LiveScoreApp.iOS.app
    xcrun simctl install 13C278BF-A473-4654-B7AE-D1569ADA54E4 LiveScoreApp.iOS.app