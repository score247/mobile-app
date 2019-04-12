*** Settings ***
Library           Process
Library           AppiumLibrary

*** Keywords ***
Start Appium Server
    Start Process    /usr/local/bin/appium \ -p \ 4723    shell=True    alias=appiumserver    stdout=${CURDIR}/appium_stdout.txt    stderr=${CURDIR}/appium_stderr.txt
    Process.Process Should Be Running    appiumserver
    Sleep    10s

Open Application On Real Ios Device
    [Arguments]    ${deviceName}    ${udid}
    Open Application    http://127.0.0.1:4723/wd/hub    platformName=iOS    platformVersion=12.2    deviceName=${deviceName}    bundleId=Score247.LiveScore    udid=${udid}
    ...    xcodeOrgId=FFHZ4F8L88    xcodeSigningId=iPhone Developer    newCommandTimeout=1500    usePrebuiltWDA=false    #    appName=LiveScoreApp
    ...    # ${EMPTY}    # FFHZ4F8L88    iPhone Developer"    updatedWDABundleId=WebDriverAgentRunner.WebDriverAgentRunner
    sleep    3s    \    #    deviceName=Iphone6    \    udid=34a775db8a3839d4651f0f066d28675b6756623a

Open Application On Simulator
    [Arguments]    ${deviceName}
    Open Application    http://127.0.0.1:4723/wd/hub    platformName=iOS    platformVersion=12.2    deviceName=${deviceName}    bundleId=Score247.LiveScore
    sleep    3s
