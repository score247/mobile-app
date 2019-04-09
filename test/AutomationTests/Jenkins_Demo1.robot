*** Settings ***
Suite Setup       start appium server
Suite Teardown    Terminate Process    appiumserver    kill=True
Library           AppiumLibrary
Library           Process

*** Test Cases ***
TC_01
    Open Application    http://127.0.0.1:4723/wd/hub    platformName=iOS    platformVersion=12.2    deviceName=iPhone 8    bundleId=Score247.LiveScore
    #    appName=LiveScoreApp
    sleep    3s
    Click Element At Coordinates    40    638    #click bottom tab 1st
    sleep    3s
    Click Element At Coordinates    113    638
    sleep    3s
    Click Element At Coordinates    183    638
    sleep    3s
    Click Element At Coordinates    261    638
    sleep    5s
    Click Element At Coordinates    335    638
    sleep    3s
    Close All Applications

*** Keywords ***
start appium server
    Start Process    /usr/local/bin/appium \ -p \ 4723    shell=True    alias=appiumserver    stdout=${CURDIR}/appium_stdout.txt    stderr=${CURDIR}/appium_stderr.txt
    Process.Process Should Be Running    appiumserver
    Sleep    10s
