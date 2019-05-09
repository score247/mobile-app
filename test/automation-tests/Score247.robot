*** Settings ***
Suite Setup       Init_Simulator    iPhone 8
Suite Teardown
Library           AppiumLibrary
Library           Process
Resource          Actions/Common_Actions.robot
Resource          Actions/Page_Actions.robot
Resource          UI/Main_Function_Bar.robot
Library           Collections
Resource          UI/Date_Bar.robot
Resource          UI/Leagues_Content.robot

*** Test Cases ***
SP1_Main_Function_Bar_TC01
    [Documentation]    Verify function bar should display 4 shortcuts and [More] icon (M002)
    ...
    ...    Verify function bar shoud be orderd by: Scores, Live, Favorites, Leagues, More icon (M003)
    ...
    ...    Verify size of shortcuts are equal to each other (M006)
    [Tags]    SP1
    #####    Set variables
    @{function_bar_items}    create list    ${btn_Scores}    ${btn_Live}    ${btn_Favorites}    ${btn_Leagues}    ${btn_More}
    #####    Main Steps
    #    VP1    Verify function bar should display 4 shortcuts and [More] icon
    : FOR    ${tm_i}    IN    @{function_bar_items}
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
    ####    Post-Conditions

SP1_Main_Function_Bar_TC02
    [Documentation]    Verify function TV, News should display when user clicks on [More] icon (M004)
    ...
    ...    Verify header of each page needs to display the name of selected function (Scores, Live, Favorites, Leagues) accordingly (M009)
    ...
    ...    Verify each shortcut with text whould be highlighted when user clicks on (M012)
    [Tags]    SP1
    #####    Set variables
    #####    Main Steps
    #    VP1    Verify function TV, News should display when user clicks on [More] icon
    Click Element    ${btn_More}
    sleep    3s
    Run Keyword And Continue On Failure    Element Should Be Visible    ${btn_TV}
    Run Keyword And Continue On Failure    Element Should Be Visible    ${btn_News}
    #    VP2    Verify header of each page needs to display the name of selected function (Scores, Live, Favorites, Leagues) accordingly
    Click Element    ${btn_Scores}
    ${header_Scores}    Get Element Attribute    xpath=//XCUIElementTypeStaticText[@name="Scores"]    value
    Run Keyword And Continue On Failure    Should Be Equal As Strings    ${header_Scores}    Scores
    Click Element    ${btn_Live}
    ${header_Live}    Get Element Attribute    xpath=//XCUIElementTypeStaticText[@name="Live"]    value
    Run Keyword And Continue On Failure    Should Be Equal As Strings    ${header_Live}    Live
    Click Element    ${btn_Favorites}
    ${header_Favorites}    Get Element Attribute    xpath=//XCUIElementTypeStaticText[@name='Favorites']    value
    Run Keyword And Continue On Failure    Should Be Equal As Strings    ${header_Favorites}    Favorites
    Click Element    ${btn_Leagues}
    ${header_Leagues}    Get Element Attribute    xpath=//XCUIElementTypeStaticText[@name="Leagues"]    value
    Run Keyword And Continue On Failure    Should Be Equal As Strings    ${header_Leagues}    Leagues
    #    VP3    Verify each shortcut with text whould be highlighted when user clicks on
    Click Element    ${btn_Scores}
    ${highlighted_Scores}    Get Element Attribute    ${btn_Scores}    value
    Run Keyword And Continue On Failure    Should Be True    ${highlighted_Scores}==1
    Click Element    ${btn_Live}
    ${highlighted_Live}    Get Element Attribute    ${btn_Live}    value
    Run Keyword And Continue On Failure    Should Be True    ${highlighted_Live}==1
    Click Element    ${btn_Favorites}
    ${highlighted_Favorites}    Get Element Attribute    ${btn_Favorites}    value
    Run Keyword And Continue On Failure    Should Be True    ${highlighted_Favorites}==1
    Click Element    ${btn_Leagues}
    ${highlighted_Leagues}    Get Element Attribute    ${btn_Leagues}    value
    Run Keyword And Continue On Failure    Should Be True    ${highlighted_Leagues}==1
    Click Element    ${btn_More}
    ${highlighted_More}    Get Element Attribute    ${btn_More}    value
    Run Keyword And Continue On Failure    Should Be True    ${highlighted_More}==1
    ####    Post-Conditions

SP1_Scores_Date_Bar
    #    VP1    Verify date bar shoud be orderd from left to right by: Home button, Date frame, Calendar button
    Click Element    ${btn_Scores}
    ${home_button_location}    Get Element Location    ${btn_Home}
    ${date_frame_location}    Get Element Location    ${frm_date}
    ${calendar_button_location}    Get Element Location    ${btn_calendar}
    ${home_button_val}    Get Dictionary Values    ${home_button_location}
    ${date_frame_val}    Get Dictionary Values    ${date_frame_location}
    ${calendar_button_val}    Get Dictionary Values    ${calendar_button_location}
    Run Keyword And Continue On Failure    should be true    ${home_button_val}[0]<${date_frame_val}[0]<${calendar_button_val}[0]
    #    VP2    Verify date bar shoud be orderd from left to right by: 3 days before current day, current day, 3 days after current day
    ##    Verify location order form left to right
    ##    Verify date bar order by value from left to right in 7 days
    ${date1_val}    Get Element Attribute    ${btn_currentdate-3}    value
    ${date2_val}    Get Element Attribute    ${btn_currentdate-2}    value
    ${date3_val}    Get Element Attribute    ${btn_currentdate-1}    value
    ${date4_val}    Get Element Attribute    ${btn_currentdate}    value
    ${date5_val}    Get Element Attribute    ${btn_currentdate+1}    value
    ${date6_val}    Get Element Attribute    ${btn_currentdate+2}    value
    ${date7_val}    Get Element Attribute    ${btn_currentdate+3}    value
    #Run Keyword And Continue On Failure    Should Be True    ${date1_val}+3==${date2_val}+2==${date3_val}+1== ${date4_val}==${date5_val}-1==${date6_val}-2==${date7_val}-3
    ${datetime}    Get Current Date
    ${date1}    Subtract Time From Date    ${datetime}    3 days    result_format=%d
    ${date2}    Subtract Time From Date    ${datetime}    2 days    result_format=%d
    ${date3}    Subtract Time From Date    ${datetime}    1 days    result_format=%d
    ${current_date}    Subtract Time From Date    ${datetime}    0 days    result_format=%d
    ${date5}    Add Time To Date    ${datetime}    1 days    result_format=%d
    ${date6}    Add Time To Date    ${datetime}    2 days    result_format=%d
    ${date7}    Add Time To Date    ${datetime}    3 days    result_format=%d
    Should Be Equal As Integers    ${date1}    ${date1_val}
    Should Be Equal As Integers    ${date2}    ${date2_val}
    Should Be Equal As Integers    ${date3}    ${date3_val}
    Should Be Equal As Integers    ${current_date}    ${date4_val}
    Should Be Equal As Integers    ${date5}    ${date5_val}
    Should Be Equal As Integers    ${date6}    ${date6_val}
    Should Be Equal As Integers    ${date7}    ${date7_val}

SP1_Scores_Match_Leagues_TC01
    [Documentation]    Verify Score page should be default screen of the app when user opens app (S001)
    ...
    ...    Verify Score page UI should contain Date bar area, League area, Menu bar area (S003)
    ...
    ...    “Verify League bar should order from left to right:
    ...    - League icon.
    ...    - League name.
    ...    - The date that all matches take place
    ...    - Table icon”
    [Tags]    SP1
    #####    Set variables
    @{date_bar_controls}    create list    ${btn_Home}    ${btn_calendar}    ${frm_date}
    #####    Main Steps
    #    VP1    Verify Score page should be default screen of the app when user opens app
    Click Element    ${btn_Scores}
    #    VP2    Verify Score page UI should contain Date bar area, League area, Menu bar area
    ${hasdata}    Run Keyword And Return status    Element Should Be Visible    xpath=//XCUIElementTypeTable[@name='LeagueTableId']/XCUIElementTypeOther[1]//XCUIElementTypeImage
    Pass Execution If    '${hasdata}'=='False'    NO DATA
    Run Keyword And Continue On Failure    Element Should Be Visible    ${table_leagues}
    : FOR    ${tn_i}    IN    @{date_bar_controls}
    \    Run Keyword And Continue On Failure    Element Should Be Visible    ${tn_i}
    #    VP3    Verify League bar should order from left to right:League icon> league name> date> league detail icon
    ${l_icon}    Get Element Location    xpath=//XCUIElementTypeTable[@name='LeagueTableId']/XCUIElementTypeOther[1]//XCUIElementTypeImage
    ${l_name}    Get Element Location    xpath=//XCUIElementTypeTable[@name='LeagueTableId']/XCUIElementTypeOther[1]//XCUIElementTypeStaticText[3]
    ${l_date}    Get Element Location    xpath=//XCUIElementTypeTable[@name='LeagueTableId']/XCUIElementTypeOther[1]//XCUIElementTypeStaticText[1]
    ${l_tbl_icon}    Get Element Location    xpath=//XCUIElementTypeTable[@name='LeagueTableId']/XCUIElementTypeOther[1]//XCUIElementTypeStaticText[2]
    ${l_iconv}    Get Dictionary Values    ${l_icon}
    ${l_namev}    Get Dictionary Values    ${l_name}
    ${l_datev}    Get Dictionary Values    ${l_date}
    ${l_tbl_iconv}    Get Dictionary Values    ${l_tbl_icon}
    Run Keyword And Continue On Failure    should be true    ${l_iconv}[0]<${l_namev}[0]<${l_datev}[0]<${l_tbl_iconv}[0]

*** Keywords ***
