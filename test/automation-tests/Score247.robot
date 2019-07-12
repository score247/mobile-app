*** Settings ***
Suite Setup       Init_Simulator    iPhone 8
Suite Teardown    Suite TearDown
Library           AppiumLibrary
Library           Process
Resource          Actions/Common_Actions.robot
Resource          UI/Main_Function_Bar.robot
Library           Collections
Library           DateTime
Library           JSONLibrary
Library           OperatingSystem
Library           String
Library           DatabaseLibrary
Library           PostgreSQLDB
Library           REST
Resource          UI/Date_Bar.robot
Resource          UI/Leagues_Content.robot

*** Test Cases ***
SP1_Main_Function_Bar_Part1
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

SP1_Main_Function_Bar_Part2
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

SP1_Scores_Match_Leagues
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

SP2_Score_Post_Pre_Match_Date1
    [Documentation]    Verify data of Post-Match in the day that happend 3 days before current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date1_dd}=    Subtract Time From Date    ${current_date}    3 days    result_format=%d
    ${date1_mm}=    Subtract Time From Date    ${current_date}    3 days    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date1_mm}]
    ${league_date_db}    Catenate    ${date1_dd}    ${m}
    #Get data from app for match 1
    ${date1_dd}    Convert to Integer    ${date1_dd}
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="${date1_dd}"]
    Click Element    //XCUIElementTypeStaticText[@name="${date1_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="AB"]
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="CHAMPIONS LEAGUE - QUALIFICATION"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="CHAMPIONS LEAGUE - QUALIFICATION"]/preceding::XCUIElementTypeStaticText[2]
    ${match1_status}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AB"]    value
    ${homescore_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AB"]/following::XCUIElementTypeStaticText[1]    value
    ${awayscore_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AB"]/following::XCUIElementTypeStaticText[2]    value
    ${homename_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AB"]/following::XCUIElementTypeStaticText[3]    value
    ${awayname_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AB"]/following::XCUIElementTypeStaticText[4]    value
    Should Be Equal As Strings    '${leaguename}'    'CHAMPIONS LEAGUE - QUALIFICATION'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    'AB'
    Should Be Equal As Strings    '${homename_match1}'    'FC Astana'
    Should Be Equal As Strings    '${awayname_match1}'    'CFR Cluj'
    Should Be Equal As Integers    ${homescore_match1}    2
    Should Be Equal As Integers    ${awayscore_match1}    1
    #Get data from app for match 2
    ${match2_status}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="FT"]    value
    ${homescore_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="FT"]/following::XCUIElementTypeStaticText[1]    value
    ${awayscore_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="FT"]/following::XCUIElementTypeStaticText[2]    value
    ${homename_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="FT"]/following::XCUIElementTypeStaticText[3]    value
    ${awayname_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="FT"]/following::XCUIElementTypeStaticText[4]    value
    Should Be Equal As Strings    '${match2_status}'    'FT'
    Should Be Equal As Strings    '${homename_match2}'    'Ararat Armenia'
    Should Be Equal As Strings    '${awayname_match2}'    'AIK'
    Should Be Equal As Strings    ${homescore_match2}    2
    Should Be Equal As Strings    ${awayscore_match2}    1

SP2_Score_Post_Pre_Match_Date2
    [Documentation]    Verify data of Post-Match in the day that happend 2 days before current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date2_dd}=    Subtract Time From Date    ${current_date}    2 days    result_format=%d
    ${date2_mm}=    Subtract Time From Date    ${current_date}    2 days    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date2_mm}]
    ${league_date_db}    Catenate    ${date2_dd}    ${m}
    #Get data from app for match 1
    ${date2_dd}    Convert to Integer    ${date2_dd}
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="${date2_dd}"]
    Click Element    //XCUIElementTypeStaticText[@name="${date2_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="Gzira United"]
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="EUROPA LEAGUE - QUALIFICATION"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="EUROPA LEAGUE - QUALIFICATION"]/preceding::XCUIElementTypeStaticText[2]
    ${match1_status}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Gzira United"]/preceding::XCUIElementTypeStaticText[3]    value
    ${homescore_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Gzira United"]/preceding::XCUIElementTypeStaticText[2]    value
    ${awayscore_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Gzira United"]/preceding::XCUIElementTypeStaticText[1]    value
    ${homename_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Gzira United"]    value
    ${awayname_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Gzira United"]/following::XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${leaguename}'    'EUROPA LEAGUE - QUALIFICATION'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    'FT'
    Should Be Equal As Strings    '${homename_match1}'    'Gzira United'
    Should Be Equal As Strings    '${awayname_match1}'    'Hajduk Split'
    Should Be Equal As Integers    ${homescore_match1}    2
    Should Be Equal As Integers    ${awayscore_match1}    1
    #Get data from app for match 2
    ${match2_status}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AS Jeunesse Esch"]/preceding::XCUIElementTypeStaticText[3]    value
    ${homescore_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AS Jeunesse Esch"]/preceding::XCUIElementTypeStaticText[2]    value
    ${awayscore_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AS Jeunesse Esch"]/preceding::XCUIElementTypeStaticText[1]    value
    ${homename_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AS Jeunesse Esch"]    value
    ${awayname_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AS Jeunesse Esch"]/following::XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${match2_status}'    'FT'
    Should Be Equal As Strings    '${homename_match2}'    'AS Jeunesse Esch'
    Should Be Equal As Strings    '${awayname_match2}'    'Tobol Kostanay'
    Should Be Equal As Strings    ${homescore_match2}    2
    Should Be Equal As Strings    ${awayscore_match2}    0

SP2_Score_Post_Pre_Match_Date3
    [Documentation]    Verify data of Post-Match in the day that happend 1 days before current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date3_dd}=    Subtract Time From Date    ${current_date}    1 days    result_format=%d
    ${date3_mm}=    Subtract Time From Date    ${current_date}    1 days    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date3_mm}]
    ${league_date_db}    Catenate    ${date3_dd}    ${m}
    #Get data from app for match 1
    ${date3_dd}    Convert to Integer    ${date3_dd}
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="${date3_dd}"]
    Click Element    //XCUIElementTypeStaticText[@name="${date3_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="Livingston"]
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="INTERNATIONAL - CLUB FRIENDLIES"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="INTERNATIONAL - CLUB FRIENDLIES"]/preceding::XCUIElementTypeStaticText[2]
    ${match1_status}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Livingston"]/preceding::XCUIElementTypeStaticText[3]    value
    ${homescore_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Livingston"]/preceding::XCUIElementTypeStaticText[2]    value
    ${awayscore_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Livingston"]/preceding::XCUIElementTypeStaticText[1]    value
    ${homename_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Livingston"]    value
    ${awayname_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Livingston"]/following::XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${leaguename}'    'INTERNATIONAL - CLUB FRIENDLIES'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    'AET'
    Should Be Equal As Strings    '${homename_match1}'    'Livingston'
    Should Be Equal As Strings    '${awayname_match1}'    'Alloa Athletic'
    Should Be Equal As Integers    ${homescore_match1}    2
    Should Be Equal As Integers    ${awayscore_match1}    0
    #Get data from app for match 2
    ${match2_status}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Carlisle United"]/preceding::XCUIElementTypeStaticText[3]    value
    ${homescore_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Carlisle United"]/preceding::XCUIElementTypeStaticText[2]    value
    ${awayscore_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Carlisle United"]/preceding::XCUIElementTypeStaticText[1]    value
    ${homename_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Carlisle United"]    value
    ${awayname_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Carlisle United"]/following::XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${match2_status}'    'AP'
    Should Be Equal As Strings    '${homename_match2}'    'Carlisle United'
    Should Be Equal As Strings    '${awayname_match2}'    'Hibernian'
    Should Be Equal As Strings    ${homescore_match2}    2
    Should Be Equal As Strings    ${awayscore_match2}    0

SP2_Score_Post_Pre_Match_Date5
    [Documentation]    Verify data of Pre-Match in the day that will happend 1 days after current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date5_dd}=    Add Time To Date    ${current_date}    1 days    result_format=%d
    ${date5_mm}=    Add Time To Date    ${current_date}    1 days    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date5_mm}]
    ${league_date_db}    Catenate    ${date5_dd}    ${m}
    #Get data from app for match 1
    ${date5_dd}    Convert to Integer    ${date5_dd}
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="${date5_dd}"]
    Click Element    //XCUIElementTypeStaticText[@name="${date5_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="Accrington Stanley"]
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="INTERNATIONAL - CLUB FRIENDLIES"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="INTERNATIONAL - CLUB FRIENDLIES"]/preceding::XCUIElementTypeStaticText[2]
    ${match1_status}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Accrington Stanley"]/preceding::XCUIElementTypeStaticText[3]    value
    ${homename_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Accrington Stanley"]    value
    ${awayname_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Accrington Stanley"]/following::XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${leaguename}'    'INTERNATIONAL - CLUB FRIENDLIES'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    '17:00'
    Should Be Equal As Strings    '${homename_match1}'    'Accrington Stanley'
    Should Be Equal As Strings    '${awayname_match1}'    'Marseille'
    #Get data from app for match 2
    ${match2_status}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Tranmere Rovers"]/preceding::XCUIElementTypeStaticText[3]    value
    ${homename_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Tranmere Rovers"]    value
    ${awayname_match2}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Tranmere Rovers"]/following::XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${match2_status}'    'Postp.'
    Should Be Equal As Strings    '${homename_match2}'    'Tranmere Rovers'
    Should Be Equal As Strings    '${awayname_match2}'    'Liverpool'

SP2_Score_Post_Pre_Match_Date6
    [Documentation]    Verify data of Pre-Match in the day that will happend 2 days after current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date6_dd}=    Add Time To Date    ${current_date}    2 days    result_format=%d    #1st date before current date
    ${date6_mm}=    Add Time To Date    ${current_date}    2 days    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date6_mm}]
    ${league_date_db}    Catenate    ${date6_dd}    ${m}
    #Get data from app for match 1
    ${date6_dd}    Convert to Integer    ${date6_dd}
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="${date6_dd}"]
    Click Element    //XCUIElementTypeStaticText[@name="${date6_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="AFC Eskilstuna"]
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="SWEDEN - ALLSVENSKAN"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="SWEDEN - ALLSVENSKAN"]/preceding::XCUIElementTypeStaticText[2]
    ${match1_status}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AFC Eskilstuna"]/preceding::XCUIElementTypeStaticText[3]    value
    ${homename_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AFC Eskilstuna"]    value
    ${awayname_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="AFC Eskilstuna"]/following::XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${leaguename}'    'SWEDEN - ALLSVENSKAN'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    'Start Delayed'
    Should Be Equal As Strings    '${homename_match1}'    'AFC Eskilstuna'
    Should Be Equal As Strings    '${awayname_match1}'    'Kalmar FF'

SP2_Score_Post_Pre_Match_Date7
    [Documentation]    Verify data of Pre-Match in the day that will happend 3 days after current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date7_dd}=    Add Time To Date    ${current_date}    3 days    result_format=%d
    ${date7_mm}=    Add Time To Date    ${current_date}    3 days    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date7_mm}]
    ${league_date_db}    Catenate    ${date7_dd}    ${m}
    #Get data from app for match 1
    ${date7_dd}    Convert to Integer    ${date7_dd}
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="${date7_dd}"]
    Click Element    //XCUIElementTypeStaticText[@name="${date7_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    //XCUIElementTypeStaticText[@name="Broendby IF"]
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="DENMARK - SUPERLIGA"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="DENMARK - SUPERLIGA"]/preceding::XCUIElementTypeStaticText[2]
    ${match1_status}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Broendby IF"]/preceding::XCUIElementTypeStaticText[3]    value
    ${homename_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Broendby IF"]    value
    ${awayname_match1}=    Get Element Attribute    //XCUIElementTypeStaticText[@name="Broendby IF"]/following::XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${leaguename}'    'DENMARK - SUPERLIGA'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    'Canc.'
    Should Be Equal As Strings    '${homename_match1}'    'Broendby IF'
    Should Be Equal As Strings    '${awayname_match1}'    'Silkeborg'

SP3_SP4_List_Event_Of_Match1
    ${json}=    Get File    ${CURDIR}/Template_Files/List_event_data_template1.json
    #Push events
    Post    ${Push_File}    ${json}
    Integer    response status    200
    Output
    Click Element    ${btn_Home}
    Wait Until Element Is Visible    accessibility_id=Chelsea
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Chelsea
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_Yellowcard_1st
    ${event1_matchtime}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event1_matchtime}'    '10''
    ${event1_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event1_player}'    'George Hilsdon'
    ${event1_yellowcard_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeImage    name
    Should Be Equal As Strings    '${event1_yellowcard_icon}'    'images/common/yellow_card.png'
    #Event2_redcard
    ${event2_matchtime}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event2_matchtime}'    '15''
    ${event2_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event2_player}'    'Tony Adams'
    ${event2_redcard_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeImage    name
    Should Be Equal As Strings    '${event2_redcard_icon}'    'images/common/red_card.png'
    #Event3_score_change_for_away_1st
    ${event3_matchtime}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[3]    value
    Should Be Equal As Strings    '${event3_matchtime}'    '30''
    ${event3_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event3_score}'    '0 - 1'
    ${event3_scorer}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event3_scorer}'    'Colin Addison'
    ${event3_assist}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event3_assist}'    'Redes, Rodney'
    ${event3_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeImage    name
    Should Be Equal As Strings    '${event3_ball_icon}'    'images/common/ball.png'
    #Event4_score_change_for_home_1st
    ${event4_matchtime}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event4_matchtime}'    '35''
    ${event4_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event4_score}'    '1 - 1'
    ${event4_scorer}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event4_scorer}'    'Jock Cameron'
    ${event4_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeImage    name
    Should Be Equal As Strings    '${event4_ball_icon}'    'images/common/ball.png'
    #Event5_yellowredcard
    ${event5_matchtime}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event5_matchtime}'    '45+2''
    ${event5_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event5_player}'    'Emmanuel Adebayor'
    ${event5_yellowredcard_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeImage    name
    Should Be Equal As Strings    '${event5_yellowredcard_icon}'    'images/common/red_yellow_card.png'
    #Event6_halftime
    ${event6_halftime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event6_halftime_text}'    'Half Time'
    ${event6_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event6_score}'    '1 - 1'
    #Event7_fulltime
    ${event7_fulltime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event7_fulltime_text}'    'Full Time'
    ${event7_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event7_score}'    '1 - 1'

SP3_SP4_List_Event_Of_Match2
    ${json}=    Get File    ${CURDIR}/Template_Files/List_event_data_template2.json
    #Push events
    Post    ${Push_File}    ${json}
    Integer    response status    200
    Output
    Click Element    ${btn_Scores}
    Sleep    3
    Click Element    ${btn_Home}
    Wait Until Element Is Visible    accessibility_id=Germany
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Germany
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_halftime
    ${event1_halftime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event1_halftime_text}'    'Half Time'
    ${event1_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event1_score}'    '0 - 0'
    #Event2_score_change_for_home_2nd
    ${event2_matchtime}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event2_matchtime}'    '55''
    ${event2_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event2_score}'    '1 - 0'
    ${event2_scorer}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event2_scorer}'    'Ben Warren'
    ${event2_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeImage    name
    Should Be Equal As Strings    '${event2_ball_icon}'    'images/common/penalty_goal.png'
    #Event3_score_change_for_away_2nd_by_own_goal
    ${event3_matchtime}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event3_matchtime}'    '75''
    ${event3_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event3_score}'    '1 - 1'
    ${event3_scorer}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event3_scorer}'    'Vivian Woodward'
    ${event3_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeImage    name
    Should Be Equal As Strings    '${event3_ball_icon}'    'images/common/own_goal.png'
    #Event4_score_change_for_home_2nd
    ${event4_matchtime}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event4_matchtime}'    '80''
    ${event4_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event4_score}'    '1 - 1'
    ${event4_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event4_player}'    'Bob Whittingham'
    ${event4_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeImage    name
    Should Be Equal As Strings    '${event4_ball_icon}'    'images/common/missed_penalty_goal.png'
    #Event5_fulltime
    ${event5_fulltime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event5_fulltime_text}'    'Full Time'
    ${event5_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event5_score}'    '1 - 1'

SP3_SP4_List_Event_Of_Match3
    ${json}=    Get File    ${CURDIR}/Template_Files/List_event_data_template3.json
    #Push events
    Post    ${Push_File}    ${json}
    Integer    response status    200
    Output
    Click Element    ${btn_Scores}
    Sleep    3
    Click Element    ${btn_Home}
    Wait Until Element Is Visible    accessibility_id=France
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=France
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_halftime
    ${event1_halftime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event1_halftime_text}'    'Half Time'
    ${event1_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event1_score}'    '0 - 0'
    #Event2_fulltime
    ${event2_fulltime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event2_fulltime_text}'    'Full Time'
    ${event2_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event2_score}'    '0 - 0'
    #Event3_score_change_for_home_extra_time_1st
    ${event3_matchtime}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event3_matchtime}'    '96''
    ${event3_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event3_score}'    '1 - 0'
    ${event3_scorer}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event3_scorer}'    'Pacheco, German'
    ${event3_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeImage    name
    Should Be Equal As Strings    '${event3_ball_icon}'    'images/common/ball.png'
    #Event4_redcard_extra_time_2nd_injury_time_shown
    ${event4_matchtime}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event4_matchtime}'    '105+2''
    ${event4_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event4_player}'    'Charles Booth'
    ${event4_redcard_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeImage    name
    Should Be Equal As Strings    '${event4_redcard_icon}'    'images/common/red_card.png'
    #Event5_afterextratime
    ${event5_afterextratime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event5_afterextratime_text}'    'After Extra Time'
    ${event5_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event5_score}'    '1 - 0'

SP3_SP4_List_Event_Of_Match4
    ${json}=    Get File    ${CURDIR}/Template_Files/List_event_data_template4.json
    #Push events
    Post    ${Push_File}    ${json}
    Integer    response status    200
    Output
    Click Element    ${btn_Scores}
    Sleep    3
    Click Element    ${btn_Home}
    Sleep    3
    Click Element    ${btn_currentdate}
    Sleep    3
    ${Kick_Of_Time}=    Get Element Attribute    accessibility_id=MatchDateLbl    value
    Wait Until Element Is Visible    accessibility_id=Brazil
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Brazil
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_halftime
    ${event1_halftime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event1_halftime_text}'    'Half Time'
    ${event1_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event1_score}'    '0 - 0'
    #Event2_fulltime
    ${event2_fulltime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event2_fulltime_text}'    'Full Time'
    ${event2_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event2_score}'    '0 - 0'
    #Event3_afterextratime
    ${event3_afterextratime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event3_afterextratime_text}'    'After Extra Time'
    ${event3_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event3_score}'    '0 - 0'
    #Event4_penalty_shoot_out
    ${event4_penalty_shoot_out_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Should Be Equal As Strings    '${event4_penalty_shoot_out_text}'    'Penalty Shoot-Out'
    ${event4_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event4_score}'    '3 - 2'
    #Event5_penalty_shoot_out_round1
    ${event5_home_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event5_home_player}'    'Walter Bettridge'
    ${event5_home_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeImage[2]    name
    Should Be Equal As Strings    '${event5_home_ball_icon}'    'images/common/missed_penalty_goal.png'
    ${event5_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event5_score}'    '0 - 1'
    ${event5_away_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event5_away_player}'    'John Anderson'
    ${event5_away_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeImage[1]    name
    Should Be Equal As Strings    '${event5_away_ball_icon}'    'images/common/penalty_goal.png'
    #Event6_penalty_shoot_out_round2
    ${event6_home_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event6_home_player}'    'Nils Middelboe'
    ${event6_home_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeImage[2]    name
    Should Be Equal As Strings    '${event6_home_ball_icon}'    'images/common/penalty_goal.png'
    ${event6_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event6_score}'    '1 - 2'
    ${event6_away_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event6_away_player}'    'Chuks Aneke'
    ${event6_away_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeImage[1]    name
    Should Be Equal As Strings    '${event6_away_ball_icon}'    'images/common/penalty_goal.png'
    #Event7_penalty_shoot_out_round3
    ${event7_home_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event7_home_player}'    'Jack Harrow'
    ${event7_home_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeImage[2]    name
    Should Be Equal As Strings    '${event7_home_ball_icon}'    'images/common/penalty_goal.png'
    ${event7_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event7_score}'    '2 - 2'
    ${event7_away_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event7_away_player}'    'Nicolas Anelka'
    ${event7_away_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeImage[1]    name
    Should Be Equal As Strings    '${event7_away_ball_icon}'    'images/common/missed_penalty_goal.png'
    #Event8_penalty_shoot_out_round4
    ${event8_home_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[8]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event8_home_player}'    'Harold Halse'
    ${event8_home_player}=    Get Element Attribute    accessibility_id=Harold Halse    value
    Should Be Equal As Strings    '${event8_home_player}'    'Harold Halse'
    ${event8_home_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[8]/XCUIElementTypeOther[1]/XCUIElementTypeImage[2]    name
    Should Be Equal As Strings    '${event8_home_ball_icon}'    'images/common/penalty_goal.png'
    ${event8_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[8]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event8_score}'    '3 - 2'
    ${event8_away_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[8]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event8_away_player}'    'Martin Angha'
    ${event8_away_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[8]/XCUIElementTypeOther[1]/XCUIElementTypeImage[1]    name
    Should Be Equal As Strings    '${event8_away_ball_icon}'    'images/common/missed_penalty_goal.png'
    #Event9_penalty_shoot_out_round5
    ${event9_home_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[9]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Should Be Equal As Strings    '${event9_home_player}'    'Bob McNeil'
    ${event9_home_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[9]/XCUIElementTypeOther[1]/XCUIElementTypeImage[2]    name
    Should Be Equal As Strings    '${event9_home_ball_icon}'    'images/common/missed_penalty_goal.png'
    ${event9_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[9]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Should Be Equal As Strings    '${event9_score}'    '3 - 2'
    ${event9_away_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[9]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Should Be Equal As Strings    '${event9_away_player}'    'George Armstrong'
    ${event9_away_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[9]/XCUIElementTypeOther[1]/XCUIElementTypeImage[1]    name
    Should Be Equal As Strings    '${event9_away_ball_icon}'    'images/common/missed_penalty_goal.png'
    Page Should Contain Element    accessibility_id=images/common/penalty_winner.png
    Page Should Contain Element    accessibility_id=images/common/second_leg_winner.png
    Page Should Contain Element    accessibility_id=2nd Leg, Aggregate Score: 2 - 1
    Page Should Contain Element    accessibility_id=Penalty Shoot-Out: 3 - 2
    Page Should Contain Element    accessibility_id=Referee:
    Page Should Contain Element    accessibility_id=Anthony Taylor
    Page Should Contain Element    accessibility_id=Venue:
    Page Should Contain Element    //XCUIElementTypeStaticText[@name=" Emirates"]
    Page Should Contain Element    accessibility_id=Spectators:
    Page Should Contain Element    accessibility_id=1,653,490
    Page Should Contain Element    accessibility_id=Kick Off Time:
    ${Kick_Of_Time}=    Catenate    17:00    ${Kick_Of_Time}
    ${Kick_Of_Time}=    Catenate    SEPARATOR=    ${Kick_Of_Time}    ,
    ${current_date}=    Get Current Date    result_format=%Y
    ${Kick_Of_Time}=    Catenate    ${Kick_Of_Time}    ${current_date}
    Page Should Contain Element    accessibility_id=${Kick_Of_Time}

SP5_Odds_1x2_Post Match
    ${json}=    Get File    ${CURDIR}/Template_Files/Data_Odds_1x2_auto.json
    #Push events
    Post    ${Push_Odds}    ${json}
    Integer    response status    200
    Output
    Sleep    5
    Click Element    ${btn_Scores}
    #Go to current date to view data
    Sleep    5
    Click Element    ${btn_currentdate-2}
    Capture Page Screenshot
    Sleep    5
    Wait Until Element Is Visible    accessibility_id=Gzira United
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Gzira United
    Sleep    3
    # Verify odd of Bookmaker 1
    ${bmaker1}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[1]/XCUIElementTypeOther/XCUIElementTypeStaticText[6]    name    #name of bookmaker
    Should Be Equal As Strings    ${bmaker1}    Alibaba
    ${live_odd1_1}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[1]/XCUIElementTypeOther/XCUIElementTypeStaticText[7]    name    #verify value of home (1)
    Should Be Equal As Numbers    ${live_odd1_1}    3.75
    ${live_odd1_X}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[1]/XCUIElementTypeOther/XCUIElementTypeStaticText[1]    name    #verify value of Draw (X)
    Should Be Equal As Numbers    ${live_odd1_X}    4.90
    ${live_odd1_2}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[1]/XCUIElementTypeOther/XCUIElementTypeStaticText[2]    name    #verify value of away (2)
    Should Be Equal As Numbers    ${live_odd1_2}    2.40
    ${open_odd1_1}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[1]/XCUIElementTypeOther/XCUIElementTypeStaticText[3]    name
    Should Be Equal As Numbers    ${open_odd1_1}    3.95
    ${open_odd1_X}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[1]/XCUIElementTypeOther/XCUIElementTypeStaticText[4]    name
    Should Be Equal As Numbers    ${open_odd1_X}    4.90
    ${open_odd1_2}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[1]/XCUIElementTypeOther/XCUIElementTypeStaticText[5]    name
    Should Be Equal As Numbers    ${open_odd1_2}    1.57
    # Verify odd of Bookmaker 2
    ${bmaker2}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[2]/XCUIElementTypeOther/XCUIElementTypeStaticText[6]    name
    Should Be Equal As Strings    ${bmaker2}    BiTis
    ${live_odd2_1}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[2]/XCUIElementTypeOther/XCUIElementTypeStaticText[7]    name
    Should Be Equal As Numbers    ${live_odd2_1}    4.50
    ${live_odd2_X}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[2]/XCUIElementTypeOther/XCUIElementTypeStaticText[1]    name
    Should Be Equal As Numbers    ${live_odd2_X}    5.50
    ${live_odd2_2}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[2]/XCUIElementTypeOther/XCUIElementTypeStaticText[2]    name
    Should Be Equal As Numbers    ${live_odd2_2}    2.40
    ${open_odd2_1}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[2]/XCUIElementTypeOther/XCUIElementTypeStaticText[3]    name
    Should Be Equal As Numbers    ${open_odd2_1}    4.00
    ${open_odd2_X}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[2]/XCUIElementTypeOther/XCUIElementTypeStaticText[4]    name
    Should Be Equal As Numbers    ${open_odd2_X}    4.50
    ${open_odd2_2}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[2]/XCUIElementTypeOther/XCUIElementTypeStaticText[5]    name
    Should Be Equal As Numbers    ${open_odd2_2}    1.60
    # Verify odd of Bookmaker 3
    ${bmaker3}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[3]/XCUIElementTypeOther/XCUIElementTypeStaticText[6]    name
    Should Be Equal As Strings    ${bmaker3}    Sunny203
    ${live_odd3_1}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[3]/XCUIElementTypeOther/XCUIElementTypeStaticText[7]    name
    Should Be Equal As Numbers    ${live_odd3_1}    3.75
    ${live_odd3_X}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[3]/XCUIElementTypeOther/XCUIElementTypeStaticText[1]    name
    Should Be Equal As Numbers    ${live_odd3_X}    4.75
    ${live_odd3_2}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[3]/XCUIElementTypeOther/XCUIElementTypeStaticText[2]    name
    Should Be Equal As Numbers    ${live_odd3_2}    1.40
    ${open_odd3_1}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[3]/XCUIElementTypeOther/XCUIElementTypeStaticText[3]    name
    Should Be Equal As Numbers    ${open_odd3_1}    4.00
    ${open_odd3_X}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[3]/XCUIElementTypeOther/XCUIElementTypeStaticText[4]    name
    Should Be Equal As Numbers    ${open_odd3_X}    4.50
    ${open_odd3_2}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[3]/XCUIElementTypeOther/XCUIElementTypeStaticText[5]    name
    Should Be Equal As Numbers    ${open_odd3_2}    1.57
    # Verify odd of Bookmaker 4
    ${bmaker4}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[4]/XCUIElementTypeOther/XCUIElementTypeStaticText[6]    name
    Should Be Equal As Strings    ${bmaker4}    TigerBet
    ${live_odd4_1}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[4]/XCUIElementTypeOther/XCUIElementTypeStaticText[7]    name
    Should Be Equal As Numbers    ${live_odd4_1}    6.00
    ${live_odd4_X}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[4]/XCUIElementTypeOther/XCUIElementTypeStaticText[1]    name
    Should Be Equal As Numbers    ${live_odd4_X}    4.30
    ${live_odd4_2}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[4]/XCUIElementTypeOther/XCUIElementTypeStaticText[2]    name
    Should Be Equal As Numbers    ${live_odd4_2}    2.20
    ${open_odd4_1}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[4]/XCUIElementTypeOther/XCUIElementTypeStaticText[3]    name
    Should Be Equal As Numbers    ${open_odd4_1}    6.00
    ${open_odd4_X}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[4]/XCUIElementTypeOther/XCUIElementTypeStaticText[4]    name
    Should Be Equal As Numbers    ${open_odd4_X}    4.50
    ${open_odd4_2}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchOdds"]/XCUIElementTypeCell[4]/XCUIElementTypeOther/XCUIElementTypeStaticText[5]    name
    Should Be Equal As Numbers    ${open_odd4_2}    2.35
    #Verify Name of Bookmaker asc
    ${list_bookmaker_app}=    Create List    ${bmaker1}    ${bmaker2}    ${bmaker3}    ${bmaker4}    # list bookmaker name on app
    ${list_bookmaker_app_before}=    Create List    ${bmaker1}    ${bmaker2}    ${bmaker3}    ${bmaker4}    # list bookmaker name on app
    Sort List    ${list_bookmaker_app}    # Sort list bookmaker name on app acs anphalbe
    Log List    ${list_bookmaker_app}
    Lists Should Be Equal    ${list_bookmaker_app}    ${list_bookmaker_app_before}    # list after sorting asc and list name on app

*** Keywords ***
