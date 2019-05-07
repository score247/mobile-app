*** Settings ***
Library           Process
Library           AppiumLibrary

*** Keywords ***
Start Appium Server
    Start Process    /usr/local/bin/appium    shell=True
    Sleep    30S

Open Application On Real Ios Device
    [Arguments]    ${deviceName}    ${udid}
    Open Application    http://0.0.0.0:4723/wd/hub    platformName=iOS    platformVersion=12.2    deviceName=${deviceName}    bundleId=Score247.LiveScore    udid=${udid}
    ...    xcodeOrgId=FFHZ4F8L88    xcodeSigningId=iPhone Developer    newCommandTimeout=1500    usePrebuiltWDA=false    newCommandTimeout=120    #
    ...    # ${EMPTY}    # FFHZ4F8L88    iPhone Developer"    updatedWDABundleId=WebDriverAgentRunner.WebDriverAgentRunner
    sleep    3s    \    #    deviceName=Iphone6    udid=34a775db8a3839d4651f0f066d28675b6756623a

Open Application On Simulator
    [Arguments]    ${deviceName}
    Open Application    http://0.0.0.0:4723/wd/hub    platformName=iOS    platformVersion=12.2    deviceName=${deviceName}    bundleId=Score247.LiveScore    newCommandTimeout=120
    sleep    3s

Init_Simulator
    [Arguments]    ${simulator_name}
    Start Appium Server
    Open Application On Simulator    ${simulator_name}

Init_Real Device
    [Arguments]    ${device_name}    ${udid}
    Start Appium Server
    Open Application On Real Ios Device    ${device_name}    ${udid}

Suite TearDown
    Start Process    /usr/bin/pkill -f node|grep appium    shell=True
