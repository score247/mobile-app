*** Settings ***
Suite Setup       Init_Simulator    iPhone 8
Suite Teardown    Suite TearDown
Library           AppiumLibrary    #Suite Teardown    Suite TearDown    #Suite Teardown    Delete_Match_Table
Library           Process
Resource          Actions/Common_Actions.robot
Resource          Actions/Page_Actions.robot
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
    ${date1}=    Subtract Time From Date    ${current_date}    3 days    result_format=%Y-%m-%d    #1st date before current date
    ${date1}=    Catenate    ${date1}    10:00:00+00
    #Get data from database
    Connect To Database    psycopg2    ${database}    ${user}    ${password}    ${host}    ${port}
    ${match_results}=    Query    SELECT "EventDate" AS EventDate , "Value"->'League'->>'Name' AS LeagueName, "Value"->'MatchResult'->'MatchStatus'->>'Value' AS MatchSatus,"Value"->'Teams'->0->> 'Name' AS HomeName,"Value"->'Teams'->1->> 'Name' AS AwayName,"Value"->'MatchResult'->>'HomeScore' AS HomeScore,"Value"->'MatchResult'->>'AwayScore' AS AwayScore FROM "Match" WHERE "EventDate"='${date1}'
    Log Many    ${match_results[0][0]}, ${match_results[0][1]}, ${match_results[0][2]}, ${match_results[0][3]}, ${match_results[0][4]}, ${match_results[0][5]}, ${match_results[0][6]}, ${match_results[1][0]}, ${match_results[1][1]}, ${match_results[1][2]}, ${match_results[1][3]}, ${match_results[1][4]}, ${match_results[1][5]}, ${match_results[1][6]}
    ${date1_dd}    Convert Date    ${match_results[0][0]}    result_format=%d
    ${date1_mm}    Convert Date    ${match_results[0][0]}    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date1_mm}]
    ${league_date_db}    Catenate    ${date1_dd}    ${m}
    #Get data from app for match 1
    Click Element    ${btn_Scores}
    ${date1_dd}    Convert to Integer    ${date1_dd}
    Click Element    accessibility_id=${date1_dd}
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=${match_results[0][3]}
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]/preceding::XCUIElementTypeStaticText[2]
    ${homename_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]
    ${awayname_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[1]
    ${homescore_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[2]
    ${awayscore_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[3]
    ${match1_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match1_status}=    Set Variable If    '${match1_status}'=='AET'    aet    '${match1_status}'=='AP'    ap    '${match1_status}'=='Postp.'
    ...    postponed    '${match1_status}'=='Can.'    cancelled    '${match1_status}'=='Start Delayed'    start_delayed    '${match1_status}'=='FT'
    ...    full-time    '${match1_status}'=='AB'    abandoned    '${match1_status}'!='AET' and '${match1_status}'!='AP' and '${match1_status}'!='Post.' and '${match1_status}'!='Can.' and '${match1_status}'!='Can.' and '${match1_status}'!='Start Delayed' and '${match1_status}'!='FT' and '${match1_status}'!='AB'    not_started
    #Compare data between database and app for match 1
    Should Be Equal As Strings    ${leaguename.lower()}    ${match_results[0][1].lower()}
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    ${match1_status}    ${match_results[0][2]}
    Should Be Equal As Strings    ${homename_match1}    ${match_results[0][3]}
    Should Be Equal As Strings    ${awayname_match1}    ${match_results[0][4]}
    Should Be Equal As Strings    ${homescore_match1}    ${match_results[0][5]}
    Should Be Equal As Strings    ${awayscore_match1}    ${match_results[0][6]}
    #Get data from app for match 2
    ${homename_match2}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]
    ${awayname_match2}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[1]
    ${homescore_match2}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[2]
    ${awayscore_match2}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[3]
    ${match2_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match2_status}=    Set Variable If    '${match2_status}'=='AET'    aet    '${match2_status}'=='AP'    ap    '${match2_status}'=='Postp.'
    ...    postponed    '${match2_status}'=='Can.'    cancelled    '${match2_status}'=='Start Delayed'    start_delayed    '${match2_status}'=='FT'
    ...    full-time    '${match2_status}'=='AB'    abandoned    '${match2_status}'!='AET' and '${match2_status}'!='AP' and '${match2_status}'!='Post.' and '${match2_status}'!='Can.' and '${match2_status}'!='Can.' and '${match2_status}'!='Start Delayed' and '${match2_status}'!='FT' and '${match2_status}'!='AB'    not_started
    ${match2_status}=    Create List    ${match2_status}
    #Compare data between database and app for match 2
    Should Contain Any    ${match2_status}    full-time    ended    closed
    Should Be Equal As Strings    ${homename_match2}    ${match_results[1][3]}
    Should Be Equal As Strings    ${awayname_match2}    ${match_results[1][4]}
    Should Be Equal As Strings    ${homescore_match2}    ${match_results[1][5]}
    Should Be Equal As Strings    ${awayscore_match2}    ${match_results[1][6]}

SP2_Score_Post_Pre_Match_Date2
    [Documentation]    Verify data of Post-Match in the day that happend 2 days before current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date2}=    Subtract Time From Date    ${current_date}    2 days    result_format=%Y-%m-%d    #1st date before current date
    ${date2}=    Catenate    ${date2}    10:00:00+00
    #Get data from database
    Connect To Database    psycopg2    ${database}    ${user}    ${password}    ${host}    ${port}
    ${match_results}=    Query    SELECT "EventDate" AS EventDate , "Value"->'League'->>'Name' AS LeagueName, "Value"->'MatchResult'->'MatchStatus'->>'Value' AS MatchSatus,"Value"->'Teams'->0->> 'Name' AS HomeName,"Value"->'Teams'->1->> 'Name' AS AwayName,"Value"->'MatchResult'->>'HomeScore' AS HomeScore,"Value"->'MatchResult'->>'AwayScore' AS AwayScore FROM "Match" WHERE "EventDate"='${date2}'
    Log Many    ${match_results[0][0]}, ${match_results[0][1]}, ${match_results[0][2]}, ${match_results[0][3]}, ${match_results[0][4]}, ${match_results[0][5]}, ${match_results[0][6]}, ${match_results[1][0]}, ${match_results[1][1]}, ${match_results[1][2]}, ${match_results[1][3]}, ${match_results[1][4]}, ${match_results[1][5]}, ${match_results[1][6]}
    ${date2_dd}    Convert Date    ${match_results[0][0]}    result_format=%d
    ${date2_mm}    Convert Date    ${match_results[0][0]}    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date2_mm}]
    ${league_date_db}    Catenate    ${date2_dd}    ${m}
    #Get data from app for match 1
    Click Element    ${btn_Scores}
    Click Element    ${btn_currentdate-2}
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=${match_results[0][3]}
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]/preceding::XCUIElementTypeStaticText[2]
    ${homename_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]
    ${awayname_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[1]
    ${homescore_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[2]
    ${awayscore_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[3]
    ${match1_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match1_status}=    Set Variable If    '${match1_status}'=='AET'    aet    '${match1_status}'=='AP'    ap    '${match1_status}'=='Postp.'
    ...    postponed    '${match1_status}'=='Can.'    cancelled    '${match1_status}'=='Start Delayed'    start_delayed    '${match1_status}'=='FT'
    ...    full-time    '${match1_status}'=='AB'    abandoned    '${match1_status}'!='AET' and '${match1_status}'!='AP' and '${match1_status}'!='Post.' and '${match1_status}'!='Can.' and '${match1_status}'!='Can.' and '${match1_status}'!='Start Delayed' and '${match1_status}'!='FT' and '${match1_status}'!='AB'    not_started
    ${list_status1}=    Create List    full-time    ended    closed
    #Compare data between database and app for match 1
    Should Be Equal As Strings    ${leaguename.lower()}    ${match_results[0][1].lower()}
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Contain Any    ${list_status1}    ${match1_status}
    Should Be Equal As Strings    ${homename_match1}    ${match_results[0][3]}
    Should Be Equal As Strings    ${awayname_match1}    ${match_results[0][4]}
    Should Be Equal As Strings    ${homescore_match1}    ${match_results[0][5]}
    Should Be Equal As Strings    ${awayscore_match1}    ${match_results[0][6]}
    #Get data from app for match 2
    ${homename_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]
    ${awayname_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[1]
    ${homescore_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[2]
    ${awayscore_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[3]
    ${match2_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match2_status}=    Set Variable If    '${match2_status}'=='AET'    aet    '${match2_status}'=='AP'    ap    '${match2_status}'=='Postp.'
    ...    postponed    '${match2_status}'=='Can.'    cancelled    '${match2_status}'=='Start Delayed'    start_delayed    '${match2_status}'=='FT'
    ...    full-time    '${match2_status}'=='AB'    abandoned    '${match2_status}'!='AET' and '${match2_status}'!='AP' and '${match2_status}'!='Post.' and '${match2_status}'!='Can.' and '${match2_status}'!='Can.' and '${match2_status}'!='Start Delayed' and '${match2_status}'!='FT' and '${match2_status}'!='AB'    not_started
    ${list_status2}=    Create List    full-time    ended    closed
    #Compare data between database and app for match 2
    Should Contain Any    ${list_status2}    ${match2_status}
    Should Be Equal As Strings    ${homename_match1}    ${match_results[1][3]}
    Should Be Equal As Strings    ${awayname_match1}    ${match_results[1][4]}
    Should Be Equal As Strings    ${homescore_match1}    ${match_results[1][5]}
    Should Be Equal As Strings    ${awayscore_match1}    ${match_results[1][6]}

SP2_Score_Post_Pre_Match_Date3
    [Documentation]    Verify data of Post-Match in the day that happend 1 day before current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date3}=    Subtract Time From Date    ${current_date}    1 days    result_format=%Y-%m-%d    #1st date before current date
    ${date3}=    Catenate    ${date3}    10:00:00+00
    #Get data from database
    Connect To Database    psycopg2    ${database}    ${user}    ${password}    ${host}    ${port}
    ${match_results}=    Query    SELECT "EventDate" AS EventDate , "Value"->'League'->>'Name' AS LeagueName, "Value"->'MatchResult'->'MatchStatus'->>'Value' AS MatchSatus,"Value"->'Teams'->0->> 'Name' AS HomeName,"Value"->'Teams'->1->> 'Name' AS AwayName,"Value"->'MatchResult'->>'HomeScore' AS HomeScore,"Value"->'MatchResult'->>'AwayScore' AS AwayScore FROM "Match" WHERE "EventDate"='${date3}'
    Log Many    ${match_results[0][0]}, ${match_results[0][1]}, ${match_results[0][2]}, ${match_results[0][3]}, ${match_results[0][4]}, ${match_results[0][5]}, ${match_results[0][6]}, ${match_results[1][0]}, ${match_results[1][1]}, ${match_results[1][2]}, ${match_results[1][3]}, ${match_results[1][4]}, ${match_results[1][5]}, ${match_results[1][6]}
    ${date3_dd}    Convert Date    ${match_results[0][0]}    result_format=%d
    ${date3_mm}    Convert Date    ${match_results[0][0]}    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date3_mm}]
    ${league_date_db}    Catenate    ${date3_dd}    ${m}
    #Get data from app for match 1
    Click Element    ${btn_Scores}
    ${date3_dd}    Convert to Integer    ${date3_dd}
    Click Element    accessibility_id=${date3_dd}
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=${match_results[0][3]}
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]/preceding::XCUIElementTypeStaticText[2]
    ${homename_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]
    ${awayname_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[1]
    ${homescore_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[2]
    ${awayscore_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[3]
    ${match1_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match1_status}    Run Keyword If    '${match1_status}'=='AET' or '${match1_status}'=='AP'    Remove String    ${match1_status.lower()}    .    ELSE If
    ...    '${match1_status}'=='Postp.'    Set Variable    postponed    ELSE If    '${match1_status}'=='Can.'    Set Variable
    ...    cancelled    ELSE If    '${match1_status}'=='Start Delayed'    Set Variable    start_delayed    ELSE If
    ...    '${match1_status}'=='FT'    Set Variable    full-time    ELSE If    '${match1_status}'=='AB'    Set Variable
    ...    abandoned    Else    Set Variable    not_started
    #Compare data between database and app for match 1
    Should Be Equal As Strings    ${leaguename.lower()}    ${match_results[0][1].lower()}
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    ${match1_status}    ${match_results[0][2]}
    Should Be Equal As Strings    ${homename_match1}    ${match_results[0][3]}
    Should Be Equal As Strings    ${awayname_match1}    ${match_results[0][4]}
    Should Be Equal As Strings    ${homescore_match1}    ${match_results[0][5]}
    Should Be Equal As Strings    ${awayscore_match1}    ${match_results[0][6]}
    #Get data from app for match 2
    ${homename_match2}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]
    ${awayname_match2}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[1]
    ${homescore_match2}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[2]
    ${awayscore_match2}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[3]
    ${match2_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match2_status}    Run Keyword If    '${match2_status}'=='AET' or '${match2_status}'=='AP'    Remove String    ${match2_status.lower()}    .    ELSE If
    ...    '${match2_status}'=='Postp.'    Set Variable    postponed    ELSE If    '${match2_status}'=='Canc.'    Set Variable
    ...    cancelled    ELSE If    '${match2_status}'=='AB'    Set Variable    abandoned    ELSE If
    ...    '${match2_status}'=='Start Delayed'    Set Variable    start_delayed    ELSE If    '${match2_status}'=='FT'    Set Variable
    ...    full-time    Else    Set Variable    not_started
    #Compare data between database and app for match 2
    Should Be Equal As Strings    ${match2_status}    ${match_results[1][2]}
    Should Be Equal As Strings    ${homename_match2}    ${match_results[1][3]}
    Should Be Equal As Strings    ${awayname_match2}    ${match_results[1][4]}
    Should Be Equal As Strings    ${homescore_match2}    ${match_results[1][5]}
    Should Be Equal As Strings    ${awayscore_match2}    ${match_results[1][6]}

SP2_Score_Post_Pre_Match_Date5
    [Documentation]    Verify data of Pre-Match in the day that will happend 1 days after current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date5}=    Add Time To Date    ${current_date}    1 days    result_format=%Y-%m-%d    #1st date before current date
    ${date5}=    Catenate    ${date5}    10:00:00+00
    Connect To Database    psycopg2    ${database}    ${user}    ${password}    ${host}    ${port}
    ${match_results}=    Query    SELECT "EventDate" AS EventDate , "Value"->'League'->>'Name' AS LeagueName, "Value"->'MatchResult'->'MatchStatus'->>'Value' AS MatchSatus,"Value"->'Teams'->0->> 'Name' AS HomeName,"Value"->'Teams'->1->> 'Name' AS AwayName,"Value"->'MatchResult'->>'HomeScore' AS HomeScore,"Value"->'MatchResult'->>'AwayScore' AS AwayScore FROM "Match" WHERE "EventDate"='${date5}'
    Log Many    ${match_results[0][0]}, ${match_results[0][1]}, ${match_results[0][2]}, ${match_results[0][3]}, ${match_results[0][4]}, ${match_results[0][5]}, ${match_results[0][6]}, ${match_results[1][0]}, ${match_results[1][1]}, ${match_results[1][2]}, ${match_results[1][3]}, ${match_results[1][4]}, ${match_results[1][5]}, ${match_results[1][6]}
    ${date5_dd}    Convert Date    ${match_results[0][0]}    result_format=%d
    ${date5_mm}    Convert Date    ${match_results[0][0]}    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date5_mm}]
    ${league_date_db}    Catenate    ${date5_dd}    ${m}
    Click Element    ${btn_Scores}
    ${date5_dd}    Convert to Integer    ${date5_dd}
    Click Element    accessibility_id=${date5_dd}
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=${match_results[0][3]}
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]
    Should Be Equal As Strings    ${leaguename.lower()}    ${match_results[0][1].lower()}
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]/preceding::XCUIElementTypeStaticText[2]
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    ${homename_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]
    ${awayname_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[1]
    ${match1_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match1_status}=    Set Variable If    '${match1_status}'=='AET'    aet    '${match1_status}'=='AP'    ap    '${match1_status}'=='Postp.'
    ...    postponed    '${match1_status}'=='Can.'    cancelled    '${match1_status}'=='Start Delayed'    start_delayed    '${match1_status}'=='FT'
    ...    full-time    '${match1_status}'=='AB'    abandoned    '${match1_status}'!='AET' and '${match1_status}'!='AP' and '${match1_status}'!='Post.' and '${match1_status}'!='Can.' and '${match1_status}'!='Can.' and '${match1_status}'!='Start Delayed' and '${match1_status}'!='FT' and '${match1_status}'!='AB'    not_started
    Should Be Equal As Strings    ${match1_status}    ${match_results[0][2]}
    Should Be Equal As Strings    ${homename_match1}    ${match_results[0][3]}
    Should Be Equal As Strings    ${awayname_match1}    ${match_results[0][4]}
    ${homename_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]
    ${awayname_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[1]
    ${homescore_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[2]
    ${awayscore_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/following::XCUIElementTypeStaticText[3]
    ${match2_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[1][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match2_status}=    Set Variable If    '${match2_status}'=='AET'    aet    '${match2_status}'=='AP'    ap    '${match2_status}'=='Postp.'
    ...    postponed    '${match2_status}'=='Can.'    cancelled    '${match2_status}'=='Start Delayed'    start_delayed    '${match2_status}'=='FT'
    ...    full-time    '${match2_status}'=='AB'    abandoned    '${match2_status}'!='AET' and '${match2_status}'!='AP' and '${match2_status}'!='Post.' and '${match2_status}'!='Can.' and '${match2_status}'!='Can.' and '${match2_status}'!='Start Delayed' and '${match2_status}'!='FT' and '${match2_status}'!='AB'    not_started
    Should Be Equal As Strings    ${match2_status}    ${match_results[1][2]}
    Should Be Equal As Strings    ${homename_match1}    ${match_results[1][3]}
    Should Be Equal As Strings    ${awayname_match1}    ${match_results[1][4]}
    Should Be Equal As Strings    ${homescore_match1}    ${match_results[1][5]}
    Should Be Equal As Strings    ${awayscore_match1}    ${match_results[1][6]}

SP2_Score_Post_Pre_Match_Date6
    [Documentation]    Verify data of Pre-Match in the day that will happend 2 days after current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date6}=    Add Time To Date    ${current_date}    2 days    result_format=%Y-%m-%d    #1st date before current date
    ${date6}=    Catenate    ${date6}    10:00:00+00
    #Get data from database
    Connect To Database    psycopg2    ${database}    ${user}    ${password}    ${host}    ${port}
    ${match_results}=    Query    SELECT "EventDate" AS EventDate , "Value"->'League'->>'Name' AS LeagueName, "Value"->'MatchResult'->'MatchStatus'->>'Value' AS MatchSatus,"Value"->'Teams'->0->> 'Name' AS HomeName,"Value"->'Teams'->1->> 'Name' AS AwayName,"Value"->'MatchResult'->>'HomeScore' AS HomeScore,"Value"->'MatchResult'->>'AwayScore' AS AwayScore FROM "Match" WHERE "EventDate"='${date6}'
    Log Many    ${match_results[0][0]}, ${match_results[0][1]}, ${match_results[0][2]}, ${match_results[0][3]}, ${match_results[0][4]}, ${match_results[0][5]}, ${match_results[0][6]}
    ${date6_dd}    Convert Date    ${match_results[0][0]}    result_format=%d
    ${date6_mm}    Convert Date    ${match_results[0][0]}    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date6_mm}]
    ${league_date_db}    Catenate    ${date6_dd}    ${m}
    #Get data from app
    Click Element    ${btn_Scores}
    ${date6_dd}    Convert to Integer    ${date6_dd}
    Click Element    accessibility_id=${date6_dd}
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=${match_results[0][3]}
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]/preceding::XCUIElementTypeStaticText[2]
    ${homename_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]
    ${awayname_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[1]
    ${match1_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match1_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match1_status}=    Set Variable If    '${match1_status}'=='AET'    aet    '${match1_status}'=='AP'    ap    '${match1_status}'=='Postp.'
    ...    postponed    '${match1_status}'=='Can.'    cancelled    '${match1_status}'=='Start Delayed'    start_delayed    '${match1_status}'=='FT'
    ...    full-time    '${match1_status}'=='AB'    abandoned    '${match1_status}'!='AET' and '${match1_status}'!='AP' and '${match1_status}'!='Post.' and '${match1_status}'!='Can.' and '${match1_status}'!='Can.' and '${match1_status}'!='Start Delayed' and '${match1_status}'!='FT' and '${match1_status}'!='AB'    not_started
    #Compare data between database and app
    Should Be Equal As Strings    ${leaguename.lower()}    ${match_results[0][1].lower()}
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    ${match1_status}    ${match_results[0][2]}
    Should Be Equal As Strings    ${homename_match1}    ${match_results[0][3]}
    Should Be Equal As Strings    ${awayname_match1}    ${match_results[0][4]}

SP2_Score_Post_Pre_Match_Date7
    [Documentation]    Verify data of Pre-Match in the day that will happend 3 days after current day
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date7}=    Add Time To Date    ${current_date}    2 days    result_format=%Y-%m-%d    #1st date before current date
    ${date7}=    Catenate    ${date7}    10:00:00+00
    #Get data from database
    Connect To Database    psycopg2    ${database}    ${user}    ${password}    ${host}    ${port}
    ${match_results}=    Query    SELECT "EventDate" AS EventDate , "Value"->'League'->>'Name' AS LeagueName, "Value"->'MatchResult'->'MatchStatus'->>'Value' AS MatchSatus,"Value"->'Teams'->0->> 'Name' AS HomeName,"Value"->'Teams'->1->> 'Name' AS AwayName,"Value"->'MatchResult'->>'HomeScore' AS HomeScore,"Value"->'MatchResult'->>'AwayScore' AS AwayScore FROM "Match" WHERE "EventDate"='${date7}'
    Log Many    ${match_results[0][0]}, ${match_results[0][1]}, ${match_results[0][2]}, ${match_results[0][3]}, ${match_results[0][4]}, ${match_results[0][5]}, ${match_results[0][6]}
    ${date7_dd}    Convert Date    ${match_results[0][0]}    result_format=%d
    ${date7_mm}    Convert Date    ${match_results[0][0]}    result_format=%m
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${m}=    set variable    @{MONTHS}[${date7_mm}]
    ${league_date_db}    Catenate    ${date7_dd}    ${m}
    Click Element    ${btn_Scores}
    ${date7_dd}    Convert to Integer    ${date7_dd}
    #Get data from app
    Click Element    accessibility_id=${date7_dd}
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=${match_results[0][3]}
    ${leaguename}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]
    ${league_date_app}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][1].upper()}"]/preceding::XCUIElementTypeStaticText[2]
    ${homename_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]
    ${awayname_match1}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/following::XCUIElementTypeStaticText[1]
    ${match1_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match1_status}=    Get Text    //XCUIElementTypeStaticText[@label="${match_results[0][3]}"]/preceding::XCUIElementTypeStaticText[1]
    ${match1_status}=    Set Variable If    '${match1_status}'=='AET'    aet    '${match1_status}'=='AP'    ap    '${match1_status}'=='Postp.'
    ...    postponed    '${match1_status}'=='Can.'    cancelled    '${match1_status}'=='Start Delayed'    start_delayed    '${match1_status}'=='FT'
    ...    full-time    '${match1_status}'=='AB'    abandoned    '${match1_status}'!='AET' and '${match1_status}'!='AP' and '${match1_status}'!='Post.' and '${match1_status}'!='Can.' and '${match1_status}'!='Can.' and '${match1_status}'!='Start Delayed' and '${match1_status}'!='FT' and '${match1_status}'!='AB'    not_started
    #Compare data between database and app
    Should Be Equal As Strings    ${leaguename.lower()}    ${match_results[0][1].lower()}
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    ${match1_status}    ${match_results[0][2]}
    Should Be Equal As Strings    ${homename_match1}    ${match_results[0][3]}
    Should Be Equal As Strings    ${awayname_match1}    ${match_results[0][4]}

SP3_SP4_List_Event_Of_Match1
    ${json}=    Get File    ${CURDIR}/Template_Files/List_event_data_template1.json
    #Push events
    Post    ${Push_File}    ${json}
    Integer    response status    200
    Output
    Sleep    10
    Click Element    ${btn_Scores}
    #Go to current date to view data
    Click Element    ${btn_currentdate}
    Sleep    5
    Wait Until Element Is Visible    accessibility_id=Chelsea
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Chelsea
    Sleep    5
    Click Element    accessibility_id=INFO
    Sleep    5
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
    Sleep    5
    Comment    Click Element    ${btn_Scores}
    #Go to current date to view data
    Comment    Click Element    ${btn_currentdate}
    Comment    Sleep    5
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
    Post    http://rocketor.nexdev.net/Release/RefreshCache
    Post    ${Push_File}    ${json}
    Integer    response status    200
    Output
    Sleep    5
    Click Element    ${btn_Scores}
    #Go to current date to view data
    Comment    Click Element    ${btn_currentdate}
    Comment    Sleep    5
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
    Comment    ${json}=    Get File    ${CURDIR}/Template_Files/List_event_data_template4.json
    Comment    #Push events
    Comment    Post    ${Push_File}    ${json}
    Comment    Integer    response status    200
    Comment    Output
    Comment    Sleep    5
    Click Element    ${btn_Scores}
    #Go to current date to view data
    Click Element    ${btn_currentdate}
    Capture Page Screenshot
    Sleep    5
    ${Kick_Of_Time}=    Get Element Attribute    accessibility_id=MatchDateLbl    value
    Wait Until Element Is Visible    accessibility_id=Brazil
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Brazil
    Sleep    5
    Click Element    accessibility_id=INFO
    Sleep    5
    Comment    #Event1_halftime
    Comment    ${event1_halftime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Comment    Should Be Equal As Strings    '${event1_halftime_text}'    'Half Time'
    Comment    ${event1_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[1]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Comment    Should Be Equal As Strings    '${event1_score}'    '0 - 0'
    Comment    #Event2_fulltime
    Comment    ${event2_fulltime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Comment    Should Be Equal As Strings    '${event2_fulltime_text}'    'Full Time'
    Comment    ${event2_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[2]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Comment    Should Be Equal As Strings    '${event2_score}'    '0 - 0'
    Comment    #Event3_afterextratime
    Comment    ${event3_afterextratime_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Comment    Should Be Equal As Strings    '${event3_afterextratime_text}'    'After Extra Time'
    Comment    ${event3_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[3]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Comment    Should Be Equal As Strings    '${event3_score}'    '0 - 0'
    Comment    #Event4_penalty_shoot_out
    Comment    ${event4_penalty_shoot_out_text}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText    value
    Comment    Should Be Equal As Strings    '${event4_penalty_shoot_out_text}'    'Penalty Shoot-Out'
    Comment    ${event4_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[4]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Comment    Should Be Equal As Strings    '${event4_score}'    '3 - 2'
    Comment    #Event5_penalty_shoot_out_round1
    Comment    ${event5_home_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Comment    Should Be Equal As Strings    '${event5_home_player}'    'Walter Bettridge'
    Comment    ${event5_home_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeImage[2]    name
    Comment    Should Be Equal As Strings    '${event5_home_ball_icon}'    'images/common/missed_penalty_goal.png'
    Comment    ${event5_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Comment    Should Be Equal As Strings    '${event5_score}'    '0 - 1'
    Comment    ${event5_away_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Comment    Should Be Equal As Strings    '${event5_away_player}'    'John Anderson'
    Comment    ${event5_away_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[5]/XCUIElementTypeOther[1]/XCUIElementTypeImage[1]    name
    Comment    Should Be Equal As Strings    '${event5_away_ball_icon}'    'images/common/penalty_goal.png'
    Comment    #Event6_penalty_shoot_out_round2
    Comment    ${event6_home_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Comment    Should Be Equal As Strings    '${event6_home_player}'    'Nils Middelboe'
    Comment    ${event6_home_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeImage[2]    name
    Comment    Should Be Equal As Strings    '${event6_home_ball_icon}'    'images/common/penalty_goal.png'
    Comment    ${event6_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Comment    Should Be Equal As Strings    '${event6_score}'    '1 - 2'
    Comment    ${event6_away_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Comment    Should Be Equal As Strings    '${event6_away_player}'    'Chuks Aneke'
    Comment    ${event6_away_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[6]/XCUIElementTypeOther[1]/XCUIElementTypeImage[1]    name
    Comment    Should Be Equal As Strings    '${event6_away_ball_icon}'    'images/common/penalty_goal.png'
    Comment    #Event7_penalty_shoot_out_round3
    Comment    ${event7_home_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Comment    Should Be Equal As Strings    '${event7_home_player}'    'Jack Harrow'
    Comment    ${event7_home_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeImage[2]    name
    Comment    Should Be Equal As Strings    '${event7_home_ball_icon}'    'images/common/penalty_goal.png'
    Comment    ${event7_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Comment    Should Be Equal As Strings    '${event7_score}'    '2 - 2'
    Comment    ${event7_away_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Comment    Should Be Equal As Strings    '${event7_away_player}'    'Nicolas Anelka'
    Comment    ${event7_away_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[7]/XCUIElementTypeOther[1]/XCUIElementTypeImage[1]    name
    Comment    Should Be Equal As Strings    '${event7_away_ball_icon}'    'images/common/missed_penalty_goal.png'
    Comment    #Event8_penalty_shoot_out_round4
    Comment    ${event8_home_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[8]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Comment    Should Be Equal As Strings    '${event8_home_player}'    'Harold Halse'
    Comment    ${event8_home_player}=    Get Element Attribute    accessibility_id=Harold Halse    value
    Comment    Should Be Equal As Strings    '${event8_home_player}'    'Harold Halse'
    Comment    ${event8_home_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[8]/XCUIElementTypeOther[1]/XCUIElementTypeImage[2]    name
    Comment    Should Be Equal As Strings    '${event8_home_ball_icon}'    'images/common/penalty_goal.png'
    Comment    ${event8_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[8]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Comment    Should Be Equal As Strings    '${event8_score}'    '3 - 2'
    Comment    ${event8_away_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[8]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Comment    Should Be Equal As Strings    '${event8_away_player}'    'Martin Angha'
    Comment    ${event8_away_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[8]/XCUIElementTypeOther[1]/XCUIElementTypeImage[1]    name
    Comment    Should Be Equal As Strings    '${event8_away_ball_icon}'    'images/common/missed_penalty_goal.png'
    Comment    #Event9_penalty_shoot_out_round5
    Comment    ${event9_home_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[9]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[1]    value
    Comment    Should Be Equal As Strings    '${event9_home_player}'    'Bob McNeil'
    Comment    ${event9_home_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[9]/XCUIElementTypeOther[1]/XCUIElementTypeImage[2]    name
    Comment    Should Be Equal As Strings    '${event9_home_ball_icon}'    'images/common/missed_penalty_goal.png'
    Comment    ${event9_score}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[9]/XCUIElementTypeOther[1]/XCUIElementTypeButton    name
    Comment    Should Be Equal As Strings    '${event9_score}'    '3 - 2'
    Comment    ${event9_away_player}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[9]/XCUIElementTypeOther[1]/XCUIElementTypeStaticText[2]    value
    Comment    Should Be Equal As Strings    '${event9_away_player}'    'George Armstrong'
    Comment    ${event9_away_ball_icon}=    Get Element Attribute    //XCUIElementTypeTable[@name="MatchDetailInfo"]/XCUIElementTypeCell[9]/XCUIElementTypeOther[1]/XCUIElementTypeImage[1]    name
    Comment    Should Be Equal As Strings    '${event9_away_ball_icon}'    'images/common/missed_penalty_goal.png'
    Comment    Page Should Contain Element    accessibility_id=Referee:
    Comment    Page Should Contain Element    accessibility_id=Anthony Taylor
    Comment    Page Should Contain Element    accessibility_id=Venue:
    Comment    Page Should Contain Element    //XCUIElementTypeStaticText[@name=" Emirates"]
    Comment    Page Should Contain Element    accessibility_id=Spectators:
    Comment    Page Should Contain Element    accessibility_id=1,653,490
    Comment    Page Should Contain Element    accessibility_id=Kick Off Time:
    ${Kick_Of_Time}=    Catenate    17:00    ${Kick_Of_Time}
    ${Kick_Of_Time}=    Catenate    SEPARATOR=    ${Kick_Of_Time}    ,
    ${current_date}=    Get Current Date    result_format=%Y
    ${Kick_Of_Time}=    Catenate    ${Kick_Of_Time}    ${current_date}
    Page Should Contain Element    accessibility_id=${Kick_Of_Time}
    Comment    Page Should Contain Element    accessibility_id=5:00 28 Jun, 87

*** Keywords ***
