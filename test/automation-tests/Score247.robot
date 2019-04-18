*** Settings ***
Suite Setup       Start Appium Server
Suite Teardown    Terminate Process    appiumserver    kill=True
Library           AppiumLibrary
Library           Process
Resource          Actions/Common_Actions.robot
Resource          Actions/Page_Actions.robot
Resource          UI/Main_Function_Bar.robot
Library           Collections

*** Test Cases ***
Demo_TC_Simulator
    [Tags]    Demo
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
    [Tags]    Demo
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

SP1_Main_Function_Bar_TC
    [Documentation]    Verify function bar should display 4 shortcuts and [More] icon (M002)
    ...
    ...    Verify function bar shoud be orderd by: Scores, Live, Favorites, Leagues, More icon (M003)
    ...
    ...    Verify function TV, News should display when user clicks on [More] icon (M004)
    ...
    ...    Verify size of shortcuts are equal to each other (M006)
    ...
    ...    Verify header of each page needs to display the name of selected function accordingly (M009)
    [Tags]    SP1
    Open Application On Simulator    iPhone 8
    #####    Set variables
    @{function_bar_items}    create list    ${btn_Scores}    ${btn_Live}    ${btn_Favorites}    ${btn_Leagues}    ${btn_More}
    @{lst_bar_width}    create list
    @{lst_bar_height}    create list
    #####    Main Steps
    #    VP1    Verify function bar should display 4 shortcuts and [More] icon
    :FOR    ${tm_i}    IN    @{function_bar_items}
    \    Run Keyword And Continue On Failure    Element Should Be Visible    ${tm_i}
    sleep    2s
    #    VP2    Verify function bar shoud be orderd by: Scores, Live, Favorites, Leagues, More icon
    ${scores_location}    Get Element Location    ${btn_Scores}
    ${live_location}    Get Element Location    ${btn_Live}
    ${favorites_location}    Get Element Location    ${btn_Favorites}
    ${leagues_location}    Get Element Location    ${btn_Leagues}
    ${more_location}    Get Element Location    ${btn_More}
    ${score_val}    Get Dictionary Values    ${scores_location}
    ${live_val}    Get Dictionary Values    ${live_location}
    ${favorites_val}    Get Dictionary Values    ${favorites_location}
    ${leagues_val}    Get Dictionary Values    ${leagues_location}
    ${more_val}    Get Dictionary Values    ${more_location}
    Run Keyword And Continue On Failure    should be true    ${score_val}[0]<${live_val}[0]<${favorites_val}[0]<${leagues_val}[0]<${more_val}[0]
    #    VP3    Verify size of shortcuts are equal to each other
    ${Scores_bar}    Get Element Size    ${btn_Scores}
    ${live_bar}    Get Element Size    ${btn_Live}
    ${favorites_bar}    Get Element Size    ${btn_Favorites}
    ${leagues_bar}    Get Element Size    ${btn_Leagues}
    ${more_bar}    Get Element Size    ${btn_More}
    Run Keyword And Continue On Failure    Dictionaries Should Be Equal    ${Scores_bar}    ${more_bar}
    Run Keyword And Continue On Failure    Dictionaries Should Be Equal    ${Scores_bar}    ${live_bar}    ${favorites_bar}    ${leagues_bar}
    #    VP4    Verify function TV, News should display when user clicks on [More] icon
    Click Element    ${btn_More}
    sleep    3s
    Run Keyword And Continue On Failure    Element Should Be Visible    ${btn_TV}
    Run Keyword And Continue On Failure    Element Should Be Visible    ${btn_News}
    #    VP5    Verify header of each page needs to display the name of selected function accordingly    come back later (wait for har support)
    Close All Applications

*** Keywords ***
