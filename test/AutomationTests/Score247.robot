*** Settings ***
Suite Setup       Start Appium Server
Suite Teardown    Terminate Process    appiumserver    kill=True
Library           AppiumLibrary
Library           Process
Resource          Actions/Common_Actions.robot
Resource          Actions/Page_Actions.robot

*** Test Cases ***
Demo_TC_Simulator
    Open Application On Simulator    iPhone 8
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

Demo_TC_Real Device
    [Template]
    Open Application On Real Ios Device    Iphone6    34a775db8a3839d4651f0f066d28675b6756623a
    Click Element At Coordinates    123    705
    sleep    3s
    Click Element At Coordinates    206    705
    sleep    3s
    Click Element At Coordinates    291    705
    sleep    5s
    Click Element At Coordinates    365    705
    sleep    3s
    Close All Applications

*** Keywords ***
