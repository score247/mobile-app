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
    ...    Verify each shortcut with text whould be highlighted when user clicks on (M012)
    [Tags]    SP1
    #####    Set variables
    #####    Main Steps
    #    VP1    Verify function TV, News should display when user clicks on [More] icon
    Click Element    ${btn_More}
    sleep    3s
    Run Keyword And Continue On Failure    Element Should Be Visible    ${btn_TV}
    Run Keyword And Continue On Failure    Element Should Be Visible    ${btn_News}
    #    VP2    Verify each shortcut with text whould be highlighted when user clicks on
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
    @{date1}    Get date from value    -3
    @{date2}    Get date from value    -2
    @{date3}    Get date from value    -1
    @{date4}    Get date from value    0
    @{date5}    Get date from value    1
    @{date6}    Get date from value    2
    @{date7}    Get date from value    3
    ${date1}    Convert To Integer    @{date1}[0]
    ${date2}    Convert To Integer    @{date2}[0]
    ${date3}    Convert To Integer    @{date3}[0]
    ${date4}    Convert To Integer    @{date4}[0]
    ${date5}    Convert To Integer    @{date5}[0]
    ${date6}    Convert To Integer    @{date6}[0]
    ${date7}    Convert To Integer    @{date7}[0]
    Page Should Contain Element    accessibility_id=${date1}
    Page Should Contain Element    accessibility_id=${date2}
    Page Should Contain Element    accessibility_id=${date3}
    Page Should Contain Element    accessibility_id=${date4}
    Page Should Contain Element    accessibility_id=${date5}
    Page Should Contain Element    accessibility_id=${date6}
    Page Should Contain Element    accessibility_id=${date7}

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
    ${l_name}    Get Element Location    accessibility_id=LeagueName-sr:tournament:38
    ${l_date}    Get Element Location    accessibility_id=LeagueEventDate-sr:tournament:38
    ${l_detail_icon}    Get Element Location    accessibility_id=LeagueDetailBtn-sr:tournament:38
    ${l_iconv}    Get Dictionary Values    ${l_icon}
    ${l_namev}    Get Dictionary Values    ${l_name}
    ${l_datev}    Get Dictionary Values    ${l_date}
    ${l_detail_iconv}    Get Dictionary Values    ${l_detail_icon}
    Run Keyword And Continue On Failure    should be true    ${l_iconv}[0]<${l_namev}[0]<${l_datev}[0]<${l_detail_iconv}[0]

SP2_Score_Post_Pre_Match_Date1
    [Documentation]    Verify data of Post-Match in the day that happend 3 days before current day
    ${ac_date}    ${ac_month}=    Get date from value    -3
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    ${result}=    Load JSON From File    Template_Files/template_post_pre_match.txt
    #Get data from app for match 1
    ${date1_dd}    Convert to Integer    ${ac_date}
    Click Element    //XCUIElementTypeStaticText[@name="${date1_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test01
    ${league_name}    Get Value From Json    ${result}    $[0][League][Name]
    ${league_name}    Catenate    @{league_name}
    Element Attribute Should Match    accessibility_id=LeagueName-sr:tournament:38    value    ${league_name}
    Element Attribute Should Match    accessibility_id=LeagueEventDate-sr:tournament:38    value    ${league_date_db}
    Element Attribute Should Match    accessibility_id=MatchStatus-sr:match:test01    value    AB
    ${match1_home_name}    Get Value From Json    ${result}    $[0][Teams][0][Name]
    ${match1_home_name}    Catenate    @{match1_home_name}
    Element Attribute Should Match    accessibility_id=HomeTeamName-sr:match:test01    value    ${match1_home_name}
    ${match1_away_name}    Get Value From Json    ${result}    $[0][Teams][1][Name]
    ${match1_away_name}    Catenate    @{match1_away_name}
    Element Attribute Should Match    accessibility_id=AwayTeamName-sr:match:test01    value    ${match1_away_name}
    ${match1_home_score}    Get Value From Json    ${result}    $[0][MatchResult][HomeScore]
    ${match1_home_score}    Catenate    @{match1_home_score}
    Element Attribute Should Match    accessibility_id=HomeScore-sr:match:test01    value    ${match1_home_score}
    ${match1_away_score}    Get Value From Json    ${result}    $[0][MatchResult][AwayScore]
    ${match1_away_score}    Catenate    @{match1_away_score}
    Element Attribute Should Match    accessibility_id=AwayScore-sr:match:test01    value    ${match1_away_score}
    #Get data from app for match 2
    Element Attribute Should Match    accessibility_id=MatchStatus-sr:match:test02    value    FT
    ${match2_home_name}    Get Value From Json    ${result}    $[1][Teams][0][Name]
    ${match2_home_name}    Catenate    @{match2_home_name}
    Element Attribute Should Match    accessibility_id=HomeTeamName-sr:match:test02    value    ${match2_home_name}
    ${match2_away_name}    Get Value From Json    ${result}    $[1][Teams][1][Name]
    ${match2_away_name}    Catenate    @{match2_away_name}
    Element Attribute Should Match    accessibility_id=AwayTeamName-sr:match:test02    value    ${match2_away_name}
    ${match2_home_score}    Get Value From Json    ${result}    $[1][MatchResult][HomeScore]
    ${match2_home_score}    Catenate    @{match2_home_score}
    Element Attribute Should Match    accessibility_id=HomeScore-sr:match:test02    value    ${match2_home_score}
    ${match2_away_score}    Get Value From Json    ${result}    $[1][MatchResult][AwayScore]
    ${match2_away_score}    Catenate    @{match2_away_score}
    Element Attribute Should Match    accessibility_id=AwayScore-sr:match:test02    value    ${match2_away_score}

SP2_Score_Post_Pre_Match_Date2
    [Documentation]    Verify data of Post-Match in the day that happend 2 days before current day
    ${ac_date}    ${ac_month}=    Get date from value    -2
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    ${result}=    Load JSON From File    Template_Files/template_post_pre_match.txt
    #Get data from app for match 1
    ${date2_dd}    Convert to Integer    ${ac_date}
    Click Element    //XCUIElementTypeStaticText[@name="${date2_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test03
    ${league_name}    Get Value From Json    ${result}    $[2][League][Name]
    ${league_name}    Catenate    @{league_name}
    Element Attribute Should Match    accessibility_id=LeagueName-sr:tournament:38    value    ${league_name}
    Element Attribute Should Match    accessibility_id=LeagueEventDate-sr:tournament:38    value    ${league_date_db}
    Element Attribute Should Match    accessibility_id=MatchStatus-sr:match:test03    value    FT
    ${match1_home_name}    Get Value From Json    ${result}    $[2][Teams][0][Name]
    ${match1_home_name}    Catenate    @{match1_home_name}
    Element Attribute Should Match    accessibility_id=HomeTeamName-sr:match:test03    value    ${match1_home_name}
    ${match1_away_name}    Get Value From Json    ${result}    $[2][Teams][1][Name]
    ${match1_away_name}    Catenate    @{match1_away_name}
    Element Attribute Should Match    accessibility_id=AwayTeamName-sr:match:test03    value    ${match1_away_name}
    ${match1_home_score}    Get Value From Json    ${result}    $[2][MatchResult][HomeScore]
    ${match1_home_score}    Catenate    @{match1_home_score}
    Element Attribute Should Match    accessibility_id=HomeScore-sr:match:test03    value    ${match1_home_score}
    ${match1_away_score}    Get Value From Json    ${result}    $[2][MatchResult][AwayScore]
    ${match1_away_score}    Catenate    @{match1_away_score}
    Element Attribute Should Match    accessibility_id=AwayScore-sr:match:test03    value    ${match1_away_score}
    #Get data from app for match 2
    Element Attribute Should Match    accessibility_id=MatchStatus-sr:match:test04    value    FT
    ${match2_home_name}    Get Value From Json    ${result}    $[3][Teams][0][Name]
    ${match2_home_name}    Catenate    @{match2_home_name}
    Element Attribute Should Match    accessibility_id=HomeTeamName-sr:match:test04    value    ${match2_home_name}
    ${match2_away_name}    Get Value From Json    ${result}    $[3][Teams][1][Name]
    ${match2_away_name}    Catenate    @{match2_away_name}
    Element Attribute Should Match    accessibility_id=AwayTeamName-sr:match:test04    value    ${match2_away_name}
    ${match2_home_score}    Get Value From Json    ${result}    $[3][MatchResult][HomeScore]
    ${match2_home_score}    Catenate    @{match2_home_score}
    Element Attribute Should Match    accessibility_id=HomeScore-sr:match:test04    value    ${match2_home_score}
    ${match2_away_score}    Get Value From Json    ${result}    $[3][MatchResult][AwayScore]
    ${match2_away_score}    Catenate    @{match2_away_score}
    Element Attribute Should Match    accessibility_id=AwayScore-sr:match:test04    value    ${match2_away_score}

SP2_Score_Post_Pre_Match_Date3
    [Documentation]    Verify data of Post-Match in the day that happend 1 days before current day
    ${ac_date}    ${ac_month}=    Get date from value    -1
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    ${result}=    Load JSON From File    Template_Files/template_post_pre_match.txt
    #Get data from app for match 1
    ${date3_dd}    Convert to Integer    ${ac_date}
    Click Element    //XCUIElementTypeStaticText[@name="${date3_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test05
    ${league_name}    Get Value From Json    ${result}    $[4][League][Name]
    ${league_name}    Catenate    @{league_name}
    Element Attribute Should Match    accessibility_id=LeagueName-sr:tournament:38    value    ${league_name}
    Element Attribute Should Match    accessibility_id=LeagueEventDate-sr:tournament:38    value    ${league_date_db}
    Element Attribute Should Match    accessibility_id=MatchStatus-sr:match:test05    value    AET
    ${match1_home_name}    Get Value From Json    ${result}    $[4][Teams][0][Name]
    ${match1_home_name}    Catenate    @{match1_home_name}
    Element Attribute Should Match    accessibility_id=HomeTeamName-sr:match:test05    value    ${match1_home_name}
    ${match1_away_name}    Get Value From Json    ${result}    $[4][Teams][1][Name]
    ${match1_away_name}    Catenate    @{match1_away_name}
    Element Attribute Should Match    accessibility_id=AwayTeamName-sr:match:test05    value    ${match1_away_name}
    ${match1_home_score}    Get Value From Json    ${result}    $[4][MatchResult][HomeScore]
    ${match1_home_score}    Catenate    @{match1_home_score}
    Element Attribute Should Match    accessibility_id=HomeScore-sr:match:test05    value    ${match1_home_score}
    ${match1_away_score}    Get Value From Json    ${result}    $[4][MatchResult][AwayScore]
    ${match1_away_score}    Catenate    @{match1_away_score}
    Element Attribute Should Match    accessibility_id=AwayScore-sr:match:test05    value    ${match1_away_score}
    #Get data from app for match 2
    Element Attribute Should Match    accessibility_id=MatchStatus-sr:match:test06    value    AP
    ${match2_home_name}    Get Value From Json    ${result}    $[5][Teams][0][Name]
    ${match2_home_name}    Catenate    @{match2_home_name}
    Element Attribute Should Match    accessibility_id=HomeTeamName-sr:match:test06    value    ${match2_home_name}
    ${match2_away_name}    Get Value From Json    ${result}    $[5][Teams][1][Name]
    ${match2_away_name}    Catenate    @{match2_away_name}
    Element Attribute Should Match    accessibility_id=AwayTeamName-sr:match:test06    value    ${match2_away_name}
    ${match2_home_score}    Get Value From Json    ${result}    $[5][MatchResult][HomeScore]
    ${match2_home_score}    Catenate    @{match2_home_score}
    Element Attribute Should Match    accessibility_id=HomeScore-sr:match:test06    value    ${match2_home_score}
    ${match2_away_score}    Get Value From Json    ${result}    $[5][MatchResult][AwayScore]
    ${match2_away_score}    Catenate    @{match2_away_score}
    Element Attribute Should Match    accessibility_id=AwayScore-sr:match:test06    value    ${match2_away_score}

SP2_Score_Post_Pre_Match_Date5
    [Documentation]    Verify data of Post-Match in the day that happend 1 days after current day
    ${ac_date}    ${ac_month}=    Get date from value    1
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    ${result}=    Load JSON From File    Template_Files/template_post_pre_match.txt
    #Get data from app for match 1
    ${date5_dd}    Convert to Integer    ${ac_date}
    Click Element    //XCUIElementTypeStaticText[@name="${date5_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test07
    ${league_name}    Get Value From Json    ${result}    $[6][League][Name]
    ${league_name}    Catenate    @{league_name}
    Element Attribute Should Match    accessibility_id=LeagueName-sr:tournament:38    value    ${league_name}
    Element Attribute Should Match    accessibility_id=LeagueEventDate-sr:tournament:38    value    ${league_date_db}
    Element Attribute Should Match    accessibility_id=MatchStatus-sr:match:test07    value    17:00
    ${match1_home_name}    Get Value From Json    ${result}    $[6][Teams][0][Name]
    ${match1_home_name}    Catenate    @{match1_home_name}
    Element Attribute Should Match    accessibility_id=HomeTeamName-sr:match:test07    value    ${match1_home_name}
    ${match1_away_name}    Get Value From Json    ${result}    $[6][Teams][1][Name]
    ${match1_away_name}    Catenate    @{match1_away_name}
    Element Attribute Should Match    accessibility_id=AwayTeamName-sr:match:test07    value    ${match1_away_name}
    #Get data from app for match 2
    Element Attribute Should Match    accessibility_id=MatchStatus-sr:match:test08    value    Postp.
    ${match2_home_name}    Get Value From Json    ${result}    $[7][Teams][0][Name]
    ${match2_home_name}    Catenate    @{match2_home_name}
    Element Attribute Should Match    accessibility_id=HomeTeamName-sr:match:test08    value    ${match2_home_name}
    ${match2_away_name}    Get Value From Json    ${result}    $[7][Teams][1][Name]
    ${match2_away_name}    Catenate    @{match2_away_name}
    Element Attribute Should Match    accessibility_id=AwayTeamName-sr:match:test08    value    ${match2_away_name}

SP2_Score_Post_Pre_Match_Date6
    [Documentation]    Verify data of Post-Match in the day that happend 2 days after current day
    ${ac_date}    ${ac_month}=    Get date from value    2
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    ${result}=    Load JSON From File    Template_Files/template_post_pre_match.txt
    #Get data from app for match 1
    ${date6_dd}    Convert to Integer    ${ac_date}
    Click Element    //XCUIElementTypeStaticText[@name="${date6_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test09
    ${league_name}    Get Value From Json    ${result}    $[8][League][Name]
    ${league_name}    Catenate    @{league_name}
    Element Attribute Should Match    accessibility_id=LeagueName-sr:tournament:892    value    ${league_name}
    Element Attribute Should Match    accessibility_id=LeagueEventDate-sr:tournament:892    value    ${league_date_db}
    Element Attribute Should Match    accessibility_id=MatchStatus-sr:match:test09    value    Start Delayed
    ${match1_home_name}    Get Value From Json    ${result}    $[8][Teams][0][Name]
    ${match1_home_name}    Catenate    @{match1_home_name}
    Element Attribute Should Match    accessibility_id=HomeTeamName-sr:match:test09    value    ${match1_home_name}
    ${match1_away_name}    Get Value From Json    ${result}    $[8][Teams][1][Name]
    ${match1_away_name}    Catenate    @{match1_away_name}
    Element Attribute Should Match    accessibility_id=AwayTeamName-sr:match:test09    value    ${match1_away_name}

SP2_Score_Post_Pre_Match_Date7
    [Documentation]    Verify data of Post-Match in the day that happend 3 days after current day
    ${ac_date}    ${ac_month}=    Get date from value    3
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    ${result}=    Load JSON From File    Template_Files/template_post_pre_match.txt
    #Get data from app for match 1
    ${date7_dd}    Convert to Integer    ${ac_date}
    Click Element    //XCUIElementTypeStaticText[@name="${date7_dd}"]
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test10
    ${league_name}    Get Value From Json    ${result}    $[9][League][Name]
    ${league_name}    Catenate    @{league_name}
    Element Attribute Should Match    accessibility_id=LeagueName-sr:tournament:892    value    ${league_name}
    Element Attribute Should Match    accessibility_id=LeagueEventDate-sr:tournament:892    value    ${league_date_db}
    Element Attribute Should Match    accessibility_id=MatchStatus-sr:match:test10    value    Canc.
    ${match1_home_name}    Get Value From Json    ${result}    $[9][Teams][0][Name]
    ${match1_home_name}    Catenate    @{match1_home_name}
    Element Attribute Should Match    accessibility_id=HomeTeamName-sr:match:test10    value    ${match1_home_name}
    ${match1_away_name}    Get Value From Json    ${result}    $[9][Teams][1][Name]
    ${match1_away_name}    Catenate    @{match1_away_name}
    Element Attribute Should Match    accessibility_id=AwayTeamName-sr:match:test10    value    ${match1_away_name}

SP3_SP4_List_Event_Of_Match1
    Update_Template_List_Event_Of_Match1
    ${file}=    Get File    ${CURDIR}/Template_Files/Run/List_event_data_template1.txt
    #Push events
    Post    ${Push_File}    ${file}
    Integer    response status    200
    Output
    ${result}=    Load JSON From File    Template_Files/Run/List_event_data_template1.txt
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    //XCUIElementTypeStaticText[@name="${current_date}"]
    Wait Until Element Is Visible    accessibility_id=Aston Villa
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Aston Villa
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_Yellowcard_1st
    Element Attribute Should Match    accessibility_id=MatchTime-3-yellow_card    value    10'
    ${player_name_1}    Get Value From Json    ${result}    $[2][payload][event][player][name]
    ${player_name_1}    Catenate    @{player_name_1}
    Element Attribute Should Match    accessibility_id=HomePlayerName-3-yellow_card    value    ${player_name_1}
    Element Attribute Should Match    accessibility_id=HomeImage-3-yellow_card    name    HomeImage-3-yellow_card
    #Event2_redcard
    Element Attribute Should Match    accessibility_id=MatchTime-4-red_card    value    15'
    ${player_name_2}    Get Value From Json    ${result}    $[3][payload][event][player][name]
    ${player_name_2}    Catenate    @{player_name_2}
    Element Attribute Should Match    accessibility_id=AwayPlayerName-4-red_card    value    ${player_name_2}
    Element Attribute Should Match    accessibility_id=AwayImage-4-red_card    name    AwayImage-4-red_card
    #Event3_score_change_for_away_1st
    Element Attribute Should Match    accessibility_id=MatchTime-5-score_change    value    30'
    Element Attribute Should Match    accessibility_id=Score-5-score_change    label    0 - 1
    ${player_name_3}    Get Value From Json    ${result}    $[4][payload][event][goal_scorer][name]
    ${player_name_3}    Catenate    @{player_name_3}
    Element Attribute Should Match    accessibility_id=AwayPlayerName-5-score_change    value    ${player_name_3}
    ${player_name_4}    Get Value From Json    ${result}    $[4][payload][event][assist][name]
    ${player_name_4}    Catenate    @{player_name_4}
    Element Attribute Should Match    accessibility_id=AwayAssistPlayerName-5-score_change    value    ${player_name_4}
    Element Attribute Should Match    accessibility_id=AwayImage-5-score_change    name    AwayImage-5-score_change
    #Event4_score_change_for_home_1st
    Element Attribute Should Match    accessibility_id=MatchTime-6-score_change    value    35'
    Element Attribute Should Match    accessibility_id=Score-6-score_change    label    1 - 1
    ${player_name_5}    Get Value From Json    ${result}    $[5][payload][event][goal_scorer][name]
    ${player_name_5}    Catenate    @{player_name_5}
    Element Attribute Should Match    accessibility_id=HomePlayerName-6-score_change    value    ${player_name_5}
    Element Attribute Should Match    accessibility_id=HomeImage-6-score_change    name    HomeImage-6-score_change
    #Event5_yellowredcard
    Element Attribute Should Match    accessibility_id=MatchTime-8-yellow_red_card    value    45+2'
    ${player_name_6}    Get Value From Json    ${result}    $[7][payload][event][player][name]
    ${player_name_6}    Catenate    @{player_name_6}
    Element Attribute Should Match    accessibility_id=AwayPlayerName-8-yellow_red_card    value    ${player_name_6}
    Element Attribute Should Match    accessibility_id=AwayImage-8-yellow_red_card    name    AwayImage-8-yellow_red_card
    #Event6_halftime
    Element Attribute Should Match    accessibility_id=MainEventStatus-9-break_start    value    Half Time
    Element Attribute Should Match    accessibility_id=Score-9-break_start    label    1 - 1
    #Event7_fulltime
    Element Attribute Should Match    accessibility_id=MainEventStatus-11-match_ended    value    Full Time
    Element Attribute Should Match    accessibility_id=Score-11-match_ended    label    1 - 1
    Remove File    Template_Files/Run/List_event_data_template1.txt
    Click Element    ${btn_Scores}

SP3_SP4_List_Event_Of_Match2
    ${file}=    Get File    ${CURDIR}/Template_Files/List_event_data_template2.txt
    #Push events
    Post    ${Push_File}    ${file}
    Integer    response status    200
    Output
    ${result}=    Load JSON From File    Template_Files/List_event_data_template2.txt
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    //XCUIElementTypeStaticText[@name="${current_date}"]
    Wait Until Element Is Visible    accessibility_id=West Ham
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=West Ham
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_halftime
    Element Attribute Should Match    accessibility_id=MainEventStatus-9-break_start    value    Half Time
    Element Attribute Should Match    accessibility_id=Score-9-break_start    label    0 - 0
    #Event2_score_change_for_home_2nd
    Element Attribute Should Match    accessibility_id=MatchTime-11-score_change    value    55'
    Element Attribute Should Match    accessibility_id=Score-11-score_change    label    1 - 0
    ${player_name_1}    Get Value From Json    ${result}    $[4][payload][event][goal_scorer][name]
    ${player_name_1}    Catenate    @{player_name_1}
    Element Attribute Should Match    accessibility_id=HomePlayerName-11-score_change    value    ${player_name_1}
    Element Attribute Should Match    accessibility_id=HomeImage-11-score_change    name    HomeImage-11-score_change
    #Event3_score_change_for_away_2nd_by_own_goal
    Element Attribute Should Match    accessibility_id=MatchTime-12-score_change    value    75'
    Element Attribute Should Match    accessibility_id=Score-12-score_change    label    1 - 1
    ${player_name_2}    Get Value From Json    ${result}    $[5][payload][event][goal_scorer][name]
    ${player_name_2}    Catenate    @{player_name_2}
    Element Attribute Should Match    accessibility_id=AwayPlayerName-12-score_change    value    ${player_name_2}
    Element Attribute Should Match    accessibility_id=AwayImage-12-score_change    name    AwayImage-12-score_change
    #Event4_score_change_for_home_2nd
    Element Attribute Should Match    accessibility_id=MatchTime-13-penalty_missed    value    80'
    Element Attribute Should Match    accessibility_id=Score-13-penalty_missed    label    1 - 1
    ${player_name_3}    Get Value From Json    ${result}    $[6][payload][event][player][name]
    ${player_name_3}    Catenate    @{player_name_3}
    Element Attribute Should Match    accessibility_id=HomePlayerName-13-penalty_missed    value    ${player_name_3}
    Element Attribute Should Match    accessibility_id=HomeImage-13-penalty_missed    name    HomeImage-13-penalty_missed
    #Event5_fulltime
    Element Attribute Should Match    accessibility_id=MainEventStatus-14-match_ended    value    Full Time
    Element Attribute Should Match    accessibility_id=Score-14-match_ended    label    1 - 1
    Click Element    ${btn_Scores}

SP3_SP4_List_Event_Of_Match3
    ${file}=    Get File    ${CURDIR}/Template_Files/List_event_data_template3.txt
    #Push events
    Post    ${Push_File}    ${file}
    Integer    response status    200
    Output
    ${result}=    Load JSON From File    Template_Files/List_event_data_template3.txt
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    //XCUIElementTypeStaticText[@name="${current_date}"]
    Wait Until Element Is Visible    accessibility_id=Liverpool
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Liverpool
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_halftime
    Element Attribute Should Match    accessibility_id=MainEventStatus-3-break_start    value    Half Time
    Element Attribute Should Match    accessibility_id=Score-3-break_start    label    0 - 0
    #Event2_fulltime
    Element Attribute Should Match    accessibility_id=MainEventStatus-5-break_start    value    Full Time
    Element Attribute Should Match    accessibility_id=Score-5-break_start    label    0 - 0
    #Event3_score_change_for_home_extra_time_1st
    Element Attribute Should Match    accessibility_id=MatchTime-7-score_change    value    96'
    Element Attribute Should Match    accessibility_id=Score-7-score_change    label    1 - 0
    ${player_name_1}    Get Value From Json    ${result}    $[6][payload][event][goal_scorer][name]
    ${player_name_1}    Catenate    @{player_name_1}
    Element Attribute Should Match    accessibility_id=HomePlayerName-7-score_change    value    ${player_name_1}
    Element Attribute Should Match    accessibility_id=HomeImage-7-score_change    name    HomeImage-7-score_change
    #Event4_redcard_extra_time_2nd_injury_time_shown
    Element Attribute Should Match    accessibility_id=MatchTime-8-red_card    value    105+2'
    ${player_name_2}    Get Value From Json    ${result}    $[7][payload][event][player][name]
    ${player_name_2}    Catenate    @{player_name_2}
    Element Attribute Should Match    accessibility_id=AwayPlayerName-8-red_card    value    ${player_name_2}
    Element Attribute Should Match    accessibility_id=AwayImage-8-red_card    name    AwayImage-8-red_card
    #Event5_afterextratime
    Element Attribute Should Match    accessibility_id=MainEventStatus-11-match_ended    value    After Extra Time
    Element Attribute Should Match    accessibility_id=Score-11-match_ended    label    1 - 0
    Click Element    ${btn_Scores}

SP3_SP4_List_Event_Of_Match4
    ${file}=    Get File    ${CURDIR}/Template_Files/List_event_data_template4.txt
    #Push events
    Post    ${Push_File}    ${file}
    Integer    response status    200
    Output
    ${result}=    Load JSON From File    Template_Files/List_event_data_template4.txt
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    //XCUIElementTypeStaticText[@name="${current_date}"]
    Sleep    3
    ${Kick_Of_Time}=    Get Element Attribute    accessibility_id=LeagueEventDate-sr:tournament:25839    value
    Wait Until Element Is Visible    accessibility_id=Chelsea
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Chelsea
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_halftime
    Element Attribute Should Match    accessibility_id=MainEventStatus-9-break_start    value    Half Time
    Element Attribute Should Match    accessibility_id=Score-9-break_start    label    0 - 0
    #Event2_fulltime
    Element Attribute Should Match    accessibility_id=MainEventStatus-17-break_start    value    Full Time
    Element Attribute Should Match    accessibility_id=Score-17-break_start    label    0 - 0
    #Event3_afterextratime
    Element Attribute Should Match    accessibility_id=MainEventStatus-25-break_start    value    After Extra Time
    Element Attribute Should Match    accessibility_id=Score-25-break_start    label    0 - 0
    #Event4_penalty_shoot_out
    Element Attribute Should Match    accessibility_id=MainEventStatus-26-period_start    value    Penalty Shoot-Out
    Element Attribute Should Match    accessibility_id=Score-26-period_start    label    3 - 2
    #Event5_penalty_shoot_out_round1
    ${player_name_1}    Get Value From Json    ${result}    $[8][payload][event][player][name]
    ${player_name_1}    Catenate    @{player_name_1}
    Element Attribute Should Match    accessibility_id=HomePlayerName-28-penalty_shootout    value    ${player_name_1}
    Element Attribute Should Match    accessibility_id=HomeImage-28-penalty_shootout    name    HomeImage-28-penalty_shootout
    Element Attribute Should Match    accessibility_id=Score-28-penalty_shootout    label    0 - 1
    ${player_name_2}    Get Value From Json    ${result}    $[9][payload][event][player][name]
    ${player_name_2}    Catenate    @{player_name_2}
    Element Attribute Should Match    accessibility_id=AwayPlayerName-28-penalty_shootout    value    ${player_name_2}
    Element Attribute Should Match    accessibility_id=AwayImage-28-penalty_shootout    name    AwayImage-28-penalty_shootout
    #Event6_penalty_shoot_out_round2
    ${player_name_3}    Get Value From Json    ${result}    $[10][payload][event][player][name]
    ${player_name_3}    Catenate    @{player_name_3}
    Element Attribute Should Match    accessibility_id=HomePlayerName-30-penalty_shootout    value    ${player_name_3}
    Element Attribute Should Match    accessibility_id=HomeImage-30-penalty_shootout    name    HomeImage-30-penalty_shootout
    Element Attribute Should Match    accessibility_id=Score-30-penalty_shootout    label    1 - 2
    ${player_name_4}    Get Value From Json    ${result}    $[11][payload][event][player][name]
    ${player_name_4}    Catenate    @{player_name_4}
    Element Attribute Should Match    accessibility_id=AwayPlayerName-30-penalty_shootout    value    ${player_name_4}
    Element Attribute Should Match    accessibility_id=AwayImage-30-penalty_shootout    name    AwayImage-30-penalty_shootout
    #Event7_penalty_shoot_out_round3
    ${player_name_5}    Get Value From Json    ${result}    $[12][payload][event][player][name]
    ${player_name_5}    Catenate    @{player_name_5}
    Element Attribute Should Match    accessibility_id=HomePlayerName-32-penalty_shootout    value    ${player_name_5}
    Element Attribute Should Match    accessibility_id=HomeImage-32-penalty_shootout    name    HomeImage-32-penalty_shootout
    Element Attribute Should Match    accessibility_id=Score-32-penalty_shootout    label    2 - 2
    ${player_name_6}    Get Value From Json    ${result}    $[13][payload][event][player][name]
    ${player_name_6}    Catenate    @{player_name_6}
    Element Attribute Should Match    accessibility_id=AwayPlayerName-32-penalty_shootout    value    ${player_name_6}
    Element Attribute Should Match    accessibility_id=AwayImage-32-penalty_shootout    name    AwayImage-32-penalty_shootout
    #Event8_penalty_shoot_out_round4
    ${player_name_7}    Get Value From Json    ${result}    $[14][payload][event][player][name]
    ${player_name_7}    Catenate    @{player_name_7}
    Element Attribute Should Match    accessibility_id=HomePlayerName-34-penalty_shootout    value    ${player_name_7}
    Element Attribute Should Match    accessibility_id=HomeImage-34-penalty_shootout    name    HomeImage-34-penalty_shootout
    Element Attribute Should Match    accessibility_id=Score-34-penalty_shootout    label    3 - 2
    ${player_name_8}    Get Value From Json    ${result}    $[15][payload][event][player][name]
    ${player_name_8}    Catenate    @{player_name_8}
    Element Attribute Should Match    accessibility_id=AwayPlayerName-34-penalty_shootout    value    ${player_name_8}
    Element Attribute Should Match    accessibility_id=AwayImage-34-penalty_shootout    name    AwayImage-34-penalty_shootout
    #Event9_penalty_shoot_out_round5
    ${player_name_9}    Get Value From Json    ${result}    $[16][payload][event][player][name]
    ${player_name_9}    Catenate    @{player_name_9}
    Element Attribute Should Match    accessibility_id=HomePlayerName-36-penalty_shootout    value    ${player_name_9}
    Element Attribute Should Match    accessibility_id=HomeImage-36-penalty_shootout    name    HomeImage-36-penalty_shootout
    Element Attribute Should Match    accessibility_id=Score-36-penalty_shootout    label    3 - 2
    ${player_name_10}    Get Value From Json    ${result}    $[17][payload][event][player][name]
    ${player_name_10}    Catenate    @{player_name_10}
    Element Attribute Should Match    accessibility_id=AwayPlayerName-36-penalty_shootout    value    ${player_name_10}
    Element Attribute Should Match    accessibility_id=AwayImage-36-penalty_shootout    name    AwayImage-36-penalty_shootout
    Page Should Contain Element    accessibility_id=HomeTeamPenalty
    Page Should Contain Element    accessibility_id=HomeTeamSecondLeg
    Page Should Contain Element    accessibility_id=SecondLegDetai
    Page Should Contain Element    accessibility_id=PenaltyDetail
    Element Attribute Should Match    accessibility_id=RefereeName    value    Anthony Taylor
    Element Attribute Should Match    accessibility_id=VenueName    value    ${space}Emirates
    Element Attribute Should Match    accessibility_id=Attendance    value    1,653,490
    ${Kick_Of_Time}=    Catenate    17:00    ${Kick_Of_Time}
    ${Kick_Of_Time}=    Catenate    SEPARATOR=    ${Kick_Of_Time}    ,
    ${current_date}=    Get Current Date    result_format=%Y
    ${Kick_Of_Time}=    Catenate    ${Kick_Of_Time}    ${current_date}
    Element Attribute Should Match    accessibility_id=EventDate    value    ${Kick_Of_Time}
    Click Element    ${btn_Scores}

SP6_Odd_Movement_Of_Bettype_1x2
    Update_Template_Odds_Movement_Of_Match1_Bettype_1x2
    ${file}=    Get File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt
    #Push events to insert odds
    Post    ${Push_odds}    ${file}
    Integer    response status    200
    Output
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    ${btn_Scores}
    Click Element    //XCUIElementTypeStaticText[@name="${current_date}"]
    Wait Until Element Is Visible    accessibility_id=Aston Villa
    #Go to Match Info page at ODDS tab from Scores page
    Click Element    accessibility_id=Aston Villa
    ${result}=    Load JSON From File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt
    ${BookmakerName}    Get Value From Json    ${result}    $[sport_events][0][markets][0][books][0][name]
    ${BookmakerName}    Catenate    @{BookmakerName}
    ${BookmakerName_id}    Catenate    SEPARATOR=    BookmakerName-    ${BookmakerName}
    Wait Until Element Is Visible    accessibility_id=${BookmakerName_id}
    Page Should Contain Element    accessibility_id=MatchOdds-BetTypeOneXTwo
    #Go to Odds Movement
    Click Element    accessibility_id=${BookmakerName_id}
    ${Title}    Catenate    ${BookmakerName}    - 1X2 Odds
    Element Attribute Should Match    accessibility_id=NavigationTitle-    value    ${Title}
    Page Should Contain Element    accessibility_id=images/common/odd_movement_chart.png
    Page Should Contain Element    accessibility_id=MatchOddsMovement
    #Check values of Odds Movement
    ##Check values of row 1
    ${UpdateTime 1}    Get Value From Json    ${result}    $[sport_events][9][markets_last_updated]
    ${UpdateTime 1}    Catenate    @{UpdateTime 1}
    ${UpdateTime 1_file}    Add Time To Date    ${UpdateTime 1}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 1}    Add Time To Date    ${UpdateTime 1}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 1_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 1}
    Element Attribute Should Match    ${MatchTime 1_id}    value    HT
    ${MatchScore 1_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 1}
    ${MatchScore 1_app}    Get Element Attribute    ${MatchScore 1_id}    value
    Element Attribute Should Match    ${MatchScore 1_id}    value    1 - 1
    ${HomeOdds 1_file}    Get Value From Json    ${result}    $[sport_events][9][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 1_file}    Catenate    @{HomeOdds 1_file}
    ${HomeOdds 1_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 1}
    Element Attribute Should Match    ${HomeOdds 1_id}    value    ${HomeOdds 1_file}
    ${AwayOdds 1_file}    Get Value From Json    ${result}    $[sport_events][9][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 1_file}    Catenate    @{AwayOdds 1_file}
    ${AwayOdds 1_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 1}
    Element Attribute Should Match    ${AwayOdds 1_id}    value    ${AwayOdds 1_file}
    ${DrawOdds 1_file}    Get Value From Json    ${result}    $[sport_events][9][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 1_file}    Catenate    @{DrawOdds 1_file}
    ${DrawOdds 1_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 1}
    Element Attribute Should Match    ${DrawOdds 1_id}    value    ${DrawOdds 1_file}
    ${UpdateTime 1_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 1}
    Element Attribute Should Match    ${UpdateTime 1_id}    value    ${UpdateTime 1_file}
    ##Check values of row 2
    ${UpdateTime 2}    Get Value From Json    ${result}    $[sport_events][8][markets_last_updated]
    ${UpdateTime 2}    Catenate    @{UpdateTime 2}
    ${UpdateTime 2_file}    Add Time To Date    ${UpdateTime 2}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 2}    Add Time To Date    ${UpdateTime 2}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 2_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 2}
    Element Attribute Should Match    ${MatchTime 2_id}    value    HT
    ${MatchScore 2_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 2}
    ${MatchScore 2_app}    Get Element Attribute    ${MatchScore 2_id}    value
    Element Attribute Should Match    ${MatchScore 2_id}    value    1 - 1
    ${HomeOdds 2_file}    Get Value From Json    ${result}    $[sport_events][8][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 2_file}    Catenate    @{HomeOdds 2_file}
    ${HomeOdds 2_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 2}
    Element Attribute Should Match    ${HomeOdds 2_id}    value    ${HomeOdds 2_file}
    ${AwayOdds 2_file}    Get Value From Json    ${result}    $[sport_events][8][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 2_file}    Catenate    @{AwayOdds 2_file}
    ${AwayOdds 2_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 2}
    Element Attribute Should Match    ${AwayOdds 2_id}    value    ${AwayOdds 2_file}
    ${DrawOdds 2_file}    Get Value From Json    ${result}    $[sport_events][8][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 2_file}    Catenate    @{DrawOdds 2_file}
    ${DrawOdds 2_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 2}
    Element Attribute Should Match    ${DrawOdds 2_id}    value    ${DrawOdds 2_file}
    ${UpdateTime 2_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 2}
    Element Attribute Should Match    ${UpdateTime 2_id}    value    ${UpdateTime 2_file}
    ##Check values of row 3
    ${UpdateTime 3}    Get Value From Json    ${result}    $[sport_events][7][markets_last_updated]
    ${UpdateTime 3}    Catenate    @{UpdateTime 3}
    ${UpdateTime 3_file}    Add Time To Date    ${UpdateTime 3}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 3}    Add Time To Date    ${UpdateTime 3}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 3_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 3}
    Element Attribute Should Match    ${MatchTime 3_id}    value    35'
    ${MatchScore 3_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 3}
    ${MatchScore 3_app}    Get Element Attribute    ${MatchScore 3_id}    value
    Element Attribute Should Match    ${MatchScore 3_id}    value    1 - 1
    ${HomeOdds 3_file}    Get Value From Json    ${result}    $[sport_events][7][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 3_file}    Catenate    @{HomeOdds 3_file}
    ${HomeOdds 3_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 3}
    Element Attribute Should Match    ${HomeOdds 3_id}    value    ${HomeOdds 3_file}
    ${AwayOdds 3_file}    Get Value From Json    ${result}    $[sport_events][7][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 3_file}    Catenate    @{AwayOdds 3_file}
    ${AwayOdds 3_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 3}
    Element Attribute Should Match    ${AwayOdds 3_id}    value    ${AwayOdds 3_file}
    ${DrawOdds 3_file}    Get Value From Json    ${result}    $[sport_events][7][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 3_file}    Catenate    @{DrawOdds 3_file}
    ${DrawOdds 3_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 3}
    Element Attribute Should Match    ${DrawOdds 3_id}    value    ${DrawOdds 3_file}
    ${UpdateTime 3_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 3}
    Element Attribute Should Match    ${UpdateTime 3_id}    value    ${UpdateTime 3_file}
    ##Check values of row 4
    ${UpdateTime 4}    Get Value From Json    ${result}    $[sport_events][6][markets_last_updated]
    ${UpdateTime 4}    Catenate    @{UpdateTime 4}
    ${UpdateTime 4_file}    Add Time To Date    ${UpdateTime 4}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 4}    Add Time To Date    ${UpdateTime 4}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 4_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 4}
    Element Attribute Should Match    ${MatchTime 4_id}    value    30'
    ${MatchScore 4_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 4}
    ${MatchScore 4_app}    Get Element Attribute    ${MatchScore 4_id}    value
    Element Attribute Should Match    ${MatchScore 4_id}    value    0 - 1
    ${HomeOdds 4_file}    Get Value From Json    ${result}    $[sport_events][6][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 4_file}    Catenate    @{HomeOdds 4_file}
    ${HomeOdds 4_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 4}
    Element Attribute Should Match    ${HomeOdds 4_id}    value    ${HomeOdds 4_file}
    ${AwayOdds 4_file}    Get Value From Json    ${result}    $[sport_events][6][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 4_file}    Catenate    @{AwayOdds 4_file}
    ${AwayOdds 4_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 4}
    Element Attribute Should Match    ${AwayOdds 4_id}    value    ${AwayOdds 4_file}
    ${DrawOdds 4_file}    Get Value From Json    ${result}    $[sport_events][6][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 4_file}    Catenate    @{DrawOdds 4_file}
    ${DrawOdds 4_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 4}
    Element Attribute Should Match    ${DrawOdds 4_id}    value    ${DrawOdds 4_file}
    ${UpdateTime 4_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 4}
    Element Attribute Should Match    ${UpdateTime 4_id}    value    ${UpdateTime 4_file}
    ##Check values of row 5
    ${UpdateTime 5}    Get Value From Json    ${result}    $[sport_events][5][markets_last_updated]
    ${UpdateTime 5}    Catenate    @{UpdateTime 5}
    ${UpdateTime 5_file}    Add Time To Date    ${UpdateTime 5}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 5}    Add Time To Date    ${UpdateTime 5}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 5_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 5}
    Element Attribute Should Match    ${MatchTime 5_id}    value    20'
    ${MatchScore 5_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 5}
    ${MatchScore 5_app}    Get Element Attribute    ${MatchScore 5_id}    value
    Element Attribute Should Match    ${MatchScore 5_id}    value    0 - 0
    ${HomeOdds 5_file}    Get Value From Json    ${result}    $[sport_events][5][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 5_file}    Catenate    @{HomeOdds 5_file}
    ${HomeOdds 5_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 5}
    Element Attribute Should Match    ${HomeOdds 5_id}    value    ${HomeOdds 5_file}
    ${AwayOdds 5_file}    Get Value From Json    ${result}    $[sport_events][5][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 5_file}    Catenate    @{AwayOdds 5_file}
    ${AwayOdds 5_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 5}
    Element Attribute Should Match    ${AwayOdds 5_id}    value    ${AwayOdds 5_file}
    ${DrawOdds 5_file}    Get Value From Json    ${result}    $[sport_events][5][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 5_file}    Catenate    @{DrawOdds 5_file}
    ${DrawOdds 5_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 5}
    Element Attribute Should Match    ${DrawOdds 5_id}    value    ${DrawOdds 5_file}
    ${UpdateTime 5_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 5}
    Element Attribute Should Match    ${UpdateTime 5_id}    value    ${UpdateTime 5_file}
    ##Check values of row 6
    ${UpdateTime 6}    Get Value From Json    ${result}    $[sport_events][4][markets_last_updated]
    ${UpdateTime 6}    Catenate    @{UpdateTime 6}
    ${UpdateTime 6_file}    Add Time To Date    ${UpdateTime 6}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 6}    Add Time To Date    ${UpdateTime 6}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 6_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 6}
    Element Attribute Should Match    ${MatchTime 6_id}    value    KO
    ${MatchScore 6_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 6}
    ${MatchScore 6_app}    Get Element Attribute    ${MatchScore 6_id}    value
    Element Attribute Should Match    ${MatchScore 6_id}    value    0 - 0
    ${HomeOdds 6_file}    Get Value From Json    ${result}    $[sport_events][4][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 6_file}    Catenate    @{HomeOdds 6_file}
    ${HomeOdds 6_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 6}
    Element Attribute Should Match    ${HomeOdds 6_id}    value    ${HomeOdds 6_file}
    ${AwayOdds 6_file}    Get Value From Json    ${result}    $[sport_events][4][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 6_file}    Catenate    @{AwayOdds 6_file}
    ${AwayOdds 6_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 6}
    Element Attribute Should Match    ${AwayOdds 6_id}    value    ${AwayOdds 6_file}
    ${DrawOdds 6_file}    Get Value From Json    ${result}    $[sport_events][4][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 6_file}    Catenate    @{DrawOdds 6_file}
    ${DrawOdds 6_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 6}
    Element Attribute Should Match    ${DrawOdds 6_id}    value    ${DrawOdds 6_file}
    ${UpdateTime 6_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 6}
    Element Attribute Should Match    ${UpdateTime 6_id}    value    ${UpdateTime 6_file}
    ##Check values of row 7
    ${UpdateTime 7}    Get Value From Json    ${result}    $[sport_events][3][markets_last_updated]
    ${UpdateTime 7}    Catenate    @{UpdateTime 7}
    ${UpdateTime 7_file}    Add Time To Date    ${UpdateTime 7}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 7}    Add Time To Date    ${UpdateTime 7}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 7_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 7}
    Element Attribute Should Match    ${MatchTime 7_id}    value    Live
    ${HomeOdds 7_file}    Get Value From Json    ${result}    $[sport_events][3][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 7_file}    Catenate    @{HomeOdds 7_file}
    ${HomeOdds 7_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 7}
    Element Attribute Should Match    ${HomeOdds 7_id}    value    ${HomeOdds 7_file}
    ${AwayOdds 7_file}    Get Value From Json    ${result}    $[sport_events][3][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 7_file}    Catenate    @{AwayOdds 7_file}
    ${AwayOdds 7_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 7}
    Element Attribute Should Match    ${AwayOdds 7_id}    value    ${AwayOdds 7_file}
    ${DrawOdds 7_file}    Get Value From Json    ${result}    $[sport_events][3][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 7_file}    Catenate    @{DrawOdds 7_file}
    ${DrawOdds 7_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 7}
    Element Attribute Should Match    ${DrawOdds 7_id}    value    ${DrawOdds 7_file}
    ${UpdateTime 7_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 7}
    Element Attribute Should Match    ${UpdateTime 7_id}    value    ${UpdateTime 7_file}
    ##Check values of row 8
    ${UpdateTime 8}    Get Value From Json    ${result}    $[sport_events][2][markets_last_updated]
    ${UpdateTime 8}    Catenate    @{UpdateTime 8}
    ${UpdateTime 8_file}    Add Time To Date    ${UpdateTime 8}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 8}    Add Time To Date    ${UpdateTime 8}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 8_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 8}
    Element Attribute Should Match    ${MatchTime 8_id}    value    Live
    ${HomeOdds 8_file}    Get Value From Json    ${result}    $[sport_events][2][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 8_file}    Catenate    @{HomeOdds 8_file}
    ${HomeOdds 8_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 8}
    Element Attribute Should Match    ${HomeOdds 8_id}    value    ${HomeOdds 8_file}
    ${AwayOdds 8_file}    Get Value From Json    ${result}    $[sport_events][2][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 8_file}    Catenate    @{AwayOdds 8_file}
    ${AwayOdds 8_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 8}
    Element Attribute Should Match    ${AwayOdds 8_id}    value    ${AwayOdds 8_file}
    ${DrawOdds 8_file}    Get Value From Json    ${result}    $[sport_events][2][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 8_file}    Catenate    @{DrawOdds 8_file}
    ${DrawOdds 8_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 8}
    Element Attribute Should Match    ${DrawOdds 8_id}    value    ${DrawOdds 8_file}
    ${UpdateTime 8_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 8}
    Element Attribute Should Match    ${UpdateTime 8_id}    value    ${UpdateTime 8_file}
    ##Check values of row 9
    ${UpdateTime 9}    Get Value From Json    ${result}    $[sport_events][1][markets_last_updated]
    ${UpdateTime 9}    Catenate    @{UpdateTime 9}
    ${UpdateTime 9_file}    Add Time To Date    ${UpdateTime 9}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 9}    Add Time To Date    ${UpdateTime 9}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 9_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 9}
    Element Attribute Should Match    ${MatchTime 9_id}    value    Live
    ${HomeOdds 9_file}    Get Value From Json    ${result}    $[sport_events][1][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 9_file}    Catenate    @{HomeOdds 9_file}
    ${HomeOdds 9_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 9}
    Element Attribute Should Match    ${HomeOdds 9_id}    value    ${HomeOdds 9_file}
    ${AwayOdds 9_file}    Get Value From Json    ${result}    $[sport_events][1][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 9_file}    Catenate    @{AwayOdds 9_file}
    ${AwayOdds 9_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 9}
    Element Attribute Should Match    ${AwayOdds 9_id}    value    ${AwayOdds 9_file}
    ${DrawOdds 9_file}    Get Value From Json    ${result}    $[sport_events][1][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 9_file}    Catenate    @{DrawOdds 9_file}
    ${DrawOdds 9_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 9}
    Element Attribute Should Match    ${DrawOdds 9_id}    value    ${DrawOdds 9_file}
    ${UpdateTime 9_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 9}
    Element Attribute Should Match    ${UpdateTime 9_id}    value    ${UpdateTime 9_file}
    ##Check values of row 10
    ${UpdateTime 10}    Get Value From Json    ${result}    $[sport_events][0][markets_last_updated]
    ${UpdateTime 10}    Catenate    @{UpdateTime 10}
    ${UpdateTime 10_file}    Add Time To Date    ${UpdateTime 10}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 10}    Add Time To Date    ${UpdateTime 10}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 10_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 10}
    Element Attribute Should Match    ${MatchTime 10_id}    value    Opening
    ${HomeOdds 10_file}    Get Value From Json    ${result}    $[sport_events][0][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 10_file}    Catenate    @{HomeOdds 10_file}
    ${HomeOdds 10_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 10}
    Element Attribute Should Match    ${HomeOdds 10_id}    value    ${HomeOdds 10_file}
    ${AwayOdds 10_file}    Get Value From Json    ${result}    $[sport_events][0][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 10_file}    Catenate    @{AwayOdds 10_file}
    ${AwayOdds 10_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 10}
    Element Attribute Should Match    ${AwayOdds 10_id}    value    ${AwayOdds 10_file}
    ${DrawOdds 10_file}    Get Value From Json    ${result}    $[sport_events][0][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 10_file}    Catenate    @{DrawOdds 10_file}
    ${DrawOdds 10_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 10}
    Element Attribute Should Match    ${DrawOdds 10_id}    value    ${DrawOdds 10_file}
    ${UpdateTime 10_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 10}
    Element Attribute Should Match    ${UpdateTime 10_id}    value    ${UpdateTime 10_file}
    Remove File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt

SP5_Odds_1x2_Post_Match
    #Go to current date to view data
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    ${btn_Scores}
    Click Element    //XCUIElementTypeStaticText[@name="${current_date}"]
    Sleep    3
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=HomeTeamName-sr:match:test14
    Sleep    3
    Comment    Click Element    accessibility_id=1X2
    # Get All Bookmaker of Match
    ${json_file}=    Load Json From File    ${CURDIR}/Template_Files/Data_Odds_1x2_auto.json
    ${Bookmaker_name}=    Get Value From Json    ${json_file}    $..books..name
    ${Bookmaker_name1}=    BuiltIn.Catenate    @{Bookmaker_name}[0]
    ${Bookmaker_name2}=    BuiltIn.Catenate    @{Bookmaker_name}[1]
    ${Bookmaker_name3}=    BuiltIn.Catenate    @{Bookmaker_name}[2]
    ${Bookmaker_name4}=    BuiltIn.Catenate    @{Bookmaker_name}[3]
    ${list_name}=    BuiltIn.Create List    ${Bookmaker_name1}    ${Bookmaker_name2}    ${Bookmaker_name3}    ${Bookmaker_name4}
    Sort List    ${list_name}
    Log    ${list_name}
    # Get Odd infor of each Bookmaker
    ${Bk1_list_odd1x2}=    GetOdds_1x2    ${json_file}    ${list_name[0]}
    ${Bk2_list_odd1x2}=    GetOdds_1x2    ${json_file}    ${list_name[1]}
    ${Bk3_list_odd1x2}=    GetOdds_1x2    ${json_file}    ${list_name[2]}
    ${Bk4_list_odd1x2}=    GetOdds_1x2    ${json_file}    ${list_name[3]}
    #Compare data of Bookmaker 1
    ${id_name}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk1_list_odd1x2}[0]    # Create “Id” attribute of element by bookmaker’s name
    ${id_homelive_1}=    Catenate    SEPARATOR=    accessibility_id=HomeLiveOdds-    ${Bk1_list_odd1x2}[0]
    ${id_drawlive_1}=    Catenate    SEPARATOR=    accessibility_id=DrawLiveOdds-    ${Bk1_list_odd1x2}[0]
    ${id_awaylive_1}=    Catenate    SEPARATOR=    accessibility_id=AwayLiveOdds-    ${Bk1_list_odd1x2}[0]
    ${id_home_open_1}=    Catenate    SEPARATOR=    accessibility_id=HomeOpeningOdds-    ${Bk1_list_odd1x2}[0]
    ${id_draw_open_1}=    Catenate    SEPARATOR=    accessibility_id=DrawOpeningOdds-    ${Bk1_list_odd1x2}[0]
    ${id_away_open_1}=    Catenate    SEPARATOR=    accessibility_id=AwayOpeningOdds-    ${Bk1_list_odd1x2}[0]
    ${Bk_name_app_1}=    Get Element Attribute    ${id_name}    value
    Element Attribute Should Match    ${id_name}    value    ${Bk1_list_odd1x2}[0]
    Element Attribute Should Match    ${id_homelive_1}    value    ${Bk1_list_odd1x2}[1]
    Element Attribute Should Match    ${id_drawlive_1}    value    ${Bk1_list_odd1x2}[2]
    Element Attribute Should Match    ${id_awaylive_1}    value    ${Bk1_list_odd1x2}[3]
    Element Attribute Should Match    ${id_home_open_1}    value    ${Bk1_list_odd1x2}[4]
    Element Attribute Should Match    ${id_draw_open_1}    value    ${Bk1_list_odd1x2}[5]
    Element Attribute Should Match    ${id_away_open_1}    value    ${Bk1_list_odd1x2}[6]
    #Compare data of Bookmaker 2
    ${id_name_2}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk2_list_odd1x2}[0]
    ${id_homelive_2}=    Catenate    SEPARATOR=    accessibility_id=HomeLiveOdds-    ${Bk2_list_odd1x2}[0]
    ${id_drawlive_2}=    Catenate    SEPARATOR=    accessibility_id=DrawLiveOdds-    ${Bk2_list_odd1x2}[0]
    ${id_awaylive_2}=    Catenate    SEPARATOR=    accessibility_id=AwayLiveOdds-    ${Bk2_list_odd1x2}[0]
    ${id_home_open_2}=    Catenate    SEPARATOR=    accessibility_id=HomeOpeningOdds-    ${Bk2_list_odd1x2}[0]
    ${id_draw_open_2}=    Catenate    SEPARATOR=    accessibility_id=DrawOpeningOdds-    ${Bk2_list_odd1x2}[0]
    ${id_away_open_2}=    Catenate    SEPARATOR=    accessibility_id=AwayOpeningOdds-    ${Bk2_list_odd1x2}[0]
    ${Bk_name_app_2}=    Get Element Attribute    ${id_name_2}    value
    Element Attribute Should Match    ${id_name_2}    value    ${Bk2_list_odd1x2}[0]
    Element Attribute Should Match    ${id_homelive_2}    value    ${Bk2_list_odd1x2}[1]
    Element Attribute Should Match    ${id_drawlive_2}    value    ${Bk2_list_odd1x2}[2]
    Element Attribute Should Match    ${id_awaylive_2}    value    ${Bk2_list_odd1x2}[3]
    Element Attribute Should Match    ${id_home_open_2}    value    ${Bk2_list_odd1x2}[4]
    Element Attribute Should Match    ${id_draw_open_2}    value    ${Bk2_list_odd1x2}[5]
    Element Attribute Should Match    ${id_away_open_2}    value    ${Bk2_list_odd1x2}[6]
    #Compare data of Bookmaker 3
    ${id_name_3}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk3_list_odd1x2}[0]
    ${id_homelive_3}=    Catenate    SEPARATOR=    accessibility_id=HomeLiveOdds-    ${Bk3_list_odd1x2}[0]
    ${id_drawlive_3}=    Catenate    SEPARATOR=    accessibility_id=DrawLiveOdds-    ${Bk3_list_odd1x2}[0]
    ${id_awaylive_3}=    Catenate    SEPARATOR=    accessibility_id=AwayLiveOdds-    ${Bk3_list_odd1x2}[0]
    ${id_home_open_3}=    Catenate    SEPARATOR=    accessibility_id=HomeOpeningOdds-    ${Bk3_list_odd1x2}[0]
    ${id_draw_open_3}=    Catenate    SEPARATOR=    accessibility_id=DrawOpeningOdds-    ${Bk3_list_odd1x2}[0]
    ${id_away_open_3}=    Catenate    SEPARATOR=    accessibility_id=AwayOpeningOdds-    ${Bk3_list_odd1x2}[0]
    ${Bk_name_app_3}=    Get Element Attribute    ${id_name_3}    value
    Element Attribute Should Match    ${id_name_3}    value    ${Bk3_list_odd1x2}[0]
    Element Attribute Should Match    ${id_homelive_3}    value    ${Bk3_list_odd1x2}[1]
    Element Attribute Should Match    ${id_drawlive_3}    value    ${Bk3_list_odd1x2}[2]
    Element Attribute Should Match    ${id_awaylive_3}    value    ${Bk3_list_odd1x2}[3]
    Element Attribute Should Match    ${id_home_open_3}    value    ${Bk3_list_odd1x2}[4]
    Element Attribute Should Match    ${id_draw_open_3}    value    ${Bk3_list_odd1x2}[5]
    Element Attribute Should Match    ${id_away_open_3}    value    ${Bk3_list_odd1x2}[6]
    #Compare data of Bookmaker 4
    ${id_name_4}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${BK4_list_odd1x2}[0]
    ${id_homelive_4}=    Catenate    SEPARATOR=    accessibility_id=HomeLiveOdds-    ${BK4_list_odd1x2}[0]
    ${id_drawlive_4}=    Catenate    SEPARATOR=    accessibility_id=DrawLiveOdds-    ${BK4_list_odd1x2}[0]
    ${id_awaylive_4}=    Catenate    SEPARATOR=    accessibility_id=AwayLiveOdds-    ${BK4_list_odd1x2}[0]
    ${id_home_open_4}=    Catenate    SEPARATOR=    accessibility_id=HomeOpeningOdds-    ${BK4_list_odd1x2}[0]
    ${id_draw_open_4}=    Catenate    SEPARATOR=    accessibility_id=DrawOpeningOdds-    ${BK4_list_odd1x2}[0]
    ${id_away_open_4}=    Catenate    SEPARATOR=    accessibility_id=AwayOpeningOdds-    ${BK4_list_odd1x2}[0]
    ${Bk_name_app_4}=    Get Element Attribute    ${id_name_4}    value
    Element Attribute Should Match    ${id_name_4}    value    ${BK4_list_odd1x2}[0]
    Element Attribute Should Match    ${id_homelive_4}    value    ${BK4_list_odd1x2}[1]
    Element Attribute Should Match    ${id_drawlive_4}    value    ${BK4_list_odd1x2}[2]
    Element Attribute Should Match    ${id_awaylive_4}    value    ${BK4_list_odd1x2}[3]
    Element Attribute Should Match    ${id_home_open_4}    value    ${BK4_list_odd1x2}[4]
    Element Attribute Should Match    ${id_draw_open_4}    value    ${BK4_list_odd1x2}[5]
    Element Attribute Should Match    ${id_away_open_4}    value    ${BK4_list_odd1x2}[6]
    ${list_name_app}=    Create List    ${Bk_name_app_1}    ${Bk_name_app_2}    ${Bk_name_app_3}    ${Bk_name_app_4}
    Lists Should Be Equal    ${list_name_app}    ${list_name}
    Remove Files    Template_Files/Run/Data_Odds_1x2_auto.json

SP6_Odds_HDP_PostMatch
    #Go to current date to view data
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    ${btn_Scores}
    Click Element    //XCUIElementTypeStaticText[@name="${current_date}"]
    Sleep    3
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=HomeTeamName-sr:match:test14
    Sleep    3
    #Go to Asian Handicap Tab
    Click Element    accessibility_id=Asian HDP
    # Get All Bookmaker of Match
    ${json_file}=    Load JSON From File    ${CURDIR}/Template_Files/Data_Odds_AsianHDP_live.json
    ${json_file_opening}=    Load JSON From File    ${CURDIR}/Template_Files/Data_Odds_AsianHDP.json
    ${bookmaker_name}=    Get Value From Json    ${json_file}    $..books..name
    ${bookmaker_name1}=    BuiltIn.Catenate    @{bookmaker_name}[0]
    ${bookmaker_name2}=    BuiltIn.Catenate    @{bookmaker_name}[1]
    ${bookmaker_name3}=    BuiltIn.Catenate    @{bookmaker_name}[2]
    ${bookmaker_name4}=    BuiltIn.Catenate    @{bookmaker_name}[3]
    ${list_name}=    BuiltIn.Create List    ${bookmaker_name1}    ${bookmaker_name2}    ${bookmaker_name3}    ${bookmaker_name4}
    Sort List    ${list_name}
    log    ${list_name}
    log    ${list_name}[0]
    # Get odd_opening_info of Bookmaker
    ${Bk1_Hdp_opening_infor}=    Get_Odds_HDP    ${json_file_opening}    ${list_name}[0]    # Bookmaker_1
    ${Bk2_Hdp_opening_infor}=    Get_Odds_HDP    ${json_file_opening}    ${list_name}[1]
    ${Bk3_Hdp_opening_infor}=    Get_Odds_HDP    ${json_file_opening}    ${list_name}[2]
    ${Bk4_Hdp_opening_infor}=    Get_Odds_HDP    ${json_file_opening}    ${list_name}[3]
    #Get Odd_Live_Info
    ${Bk1_Hdp_live_infor}=    Get_Odds_HDP    ${json_file}    ${list_name}[0]
    ${Bk2_Hdp_live_infor}=    Get_Odds_HDP    ${json_file}    ${list_name}[1]
    ${Bk3_Hdp_live_infor}=    Get_Odds_HDP    ${json_file}    ${list_name}[2]
    ${Bk4_Hdp_live_infor}=    Get_Odds_HDP    ${json_file}    ${list_name}[3]
    # Verify odd of Bookmaker 1
    ${id_name_hdp_1}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk1_Hdp_live_infor}[0]    # Create “Id” attribute of element by bookmaker’s name
    ${id_homelive_hdp_1}=    Catenate    SEPARATOR=    accessibility_id=HomeLiveOdds-    ${Bk1_Hdp_live_infor}[0]
    ${id_hdplive_1}=    Catenate    SEPARATOR=    accessibility_id=LiveHdp-    ${Bk1_Hdp_live_infor}[0]
    ${id_awaylive_hdp_1}=    Catenate    SEPARATOR=    accessibility_id=AwayLiveOdds-    ${Bk1_Hdp_live_infor}[0]
    ${id_home_open_hdp_1}=    Catenate    SEPARATOR=    accessibility_id=HomeOpeningOdds-    ${Bk1_Hdp_live_infor}[0]
    ${id_hdp_open_1}=    Catenate    SEPARATOR=    accessibility_id=OpeningHdp-    ${Bk1_Hdp_live_infor}[0]
    ${id_away_open_hdp_1}=    Catenate    SEPARATOR=    accessibility_id=AwayOpeningOdds-    ${Bk1_Hdp_live_infor}[0]
    ${Bookmaker_HDP_1}=    Get Element Attribute    ${id_name_hdp_1}    value
    Element Attribute Should Match    ${id_name_hdp_1}    value    ${Bk1_Hdp_live_infor}[0]
    Element Attribute Should Match    ${id_homelive_hdp_1}    value    ${Bk1_Hdp_live_infor}[1]
    Element Attribute Should Match    ${id_hdplive_1}    value    ${Bk1_Hdp_live_infor}[2]
    Element Attribute Should Match    ${id_awaylive_hdp_1}    value    ${Bk1_Hdp_live_infor}[3]
    Element Attribute Should Match    ${id_home_open_hdp_1}    value    ${Bk1_Hdp_opening_infor}[1]
    Element Attribute Should Match    ${id_hdp_open_1}    value    ${Bk1_Hdp_opening_infor}[2]
    Element Attribute Should Match    ${id_away_open_hdp_1}    value    ${Bk1_Hdp_opening_infor}[3]
    # Verify odd of Bookmaker 2
    ${id_name_hdp_2}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk2_Hdp_live_infor}[0]
    ${id_homelive_hdp_2}=    Catenate    SEPARATOR=    accessibility_id=HomeLiveOdds-    ${Bk2_Hdp_live_infor}[0]
    ${id_hdplive_2}=    Catenate    SEPARATOR=    accessibility_id=LiveHdp-    ${Bk2_Hdp_live_infor}[0]
    ${id_awaylive_hdp_2}=    Catenate    SEPARATOR=    accessibility_id=AwayLiveOdds-    ${Bk2_Hdp_live_infor}[0]
    ${id_home_open_hdp_2}=    Catenate    SEPARATOR=    accessibility_id=HomeOpeningOdds-    ${Bk2_Hdp_live_infor}[0]
    ${id_hdp_open_2}=    Catenate    SEPARATOR=    accessibility_id=OpeningHdp-    ${Bk2_Hdp_live_infor}[0]
    ${id_away_open_hdp_2}=    Catenate    SEPARATOR=    accessibility_id=AwayOpeningOdds-    ${Bk2_Hdp_live_infor}[0]
    ${Bookmaker_HDP_2}=    Get Element Attribute    ${id_name_hdp_2}    value
    Element Attribute Should Match    ${id_name_hdp_2}    value    ${Bk2_Hdp_live_infor}[0]
    Element Attribute Should Match    ${id_homelive_hdp_2}    value    ${Bk2_Hdp_live_infor}[1]
    Element Attribute Should Match    ${id_hdplive_2}    value    ${Bk2_Hdp_live_infor}[2]
    Element Attribute Should Match    ${id_awaylive_hdp_2}    value    ${Bk2_Hdp_live_infor}[3]
    Element Attribute Should Match    ${id_home_open_hdp_2}    value    ${Bk2_Hdp_opening_infor}[1]
    Element Attribute Should Match    ${id_hdp_open_2}    value    ${Bk2_Hdp_opening_infor}[2]
    Element Attribute Should Match    ${id_away_open_hdp_2}    value    ${Bk2_Hdp_opening_infor}[3]
    # Verify odd of Bookmaker 3
    ${id_name_hdp_3}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk3_Hdp_live_infor}[0]
    ${id_homelive_hdp_3}=    Catenate    SEPARATOR=    accessibility_id=HomeLiveOdds-    ${Bk3_Hdp_live_infor}[0]
    ${id_hdplive_3}=    Catenate    SEPARATOR=    accessibility_id=LiveHdp-    ${Bk3_Hdp_live_infor}[0]
    ${id_awaylive_hdp_3}=    Catenate    SEPARATOR=    accessibility_id=AwayLiveOdds-    ${Bk3_Hdp_live_infor}[0]
    ${id_home_open_hdp_3}=    Catenate    SEPARATOR=    accessibility_id=HomeOpeningOdds-    ${Bk3_Hdp_live_infor}[0]
    ${id_hdp_open_3}=    Catenate    SEPARATOR=    accessibility_id=OpeningHdp-    ${Bk3_Hdp_live_infor}[0]
    ${id_away_open_hdp_3}=    Catenate    SEPARATOR=    accessibility_id=AwayOpeningOdds-    ${Bk3_Hdp_live_infor}[0]
    ${Bookmaker_HDP_3}=    Get Element Attribute    ${id_name_hdp_3}    value
    Element Attribute Should Match    ${id_name_hdp_3}    value    ${Bk3_Hdp_live_infor}[0]
    Element Attribute Should Match    ${id_homelive_hdp_3}    value    ${Bk3_Hdp_live_infor}[1]
    Element Attribute Should Match    ${id_hdplive_3}    value    ${Bk3_Hdp_live_infor}[2]
    Element Attribute Should Match    ${id_awaylive_hdp_3}    value    ${Bk3_Hdp_live_infor}[3]
    Element Attribute Should Match    ${id_home_open_hdp_3}    value    ${Bk3_Hdp_opening_infor}[1]
    Element Attribute Should Match    ${id_hdp_open_3}    value    ${Bk3_Hdp_opening_infor}[2]
    Element Attribute Should Match    ${id_away_open_hdp_3}    value    ${Bk3_Hdp_opening_infor}[3]
    # Verify odd of Bookmaker 4
    ${id_name_hdp_4}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk4_Hdp_live_infor}[0]
    ${id_homelive_hdp_4}=    Catenate    SEPARATOR=    accessibility_id=HomeLiveOdds-    ${Bk4_Hdp_live_infor}[0]
    ${id_hdplive_4}=    Catenate    SEPARATOR=    accessibility_id=LiveHdp-    ${Bk4_Hdp_live_infor}[0]
    ${id_awaylive_hdp_4}=    Catenate    SEPARATOR=    accessibility_id=AwayLiveOdds-    ${Bk4_Hdp_live_infor}[0]
    ${id_home_open_hdp_4}=    Catenate    SEPARATOR=    accessibility_id=HomeOpeningOdds-    ${Bk4_Hdp_live_infor}[0]
    ${id_hdp_open_4}=    Catenate    SEPARATOR=    accessibility_id=OpeningHdp-    ${Bk4_Hdp_live_infor}[0]
    ${id_away_open_hdp_4}=    Catenate    SEPARATOR=    accessibility_id=AwayOpeningOdds-    ${Bk4_Hdp_live_infor}[0]
    ${Bookmaker_HDP_4}=    Get Element Attribute    ${id_name_hdp_4}    value
    Element Attribute Should Match    ${id_name_hdp_4}    value    ${Bk4_Hdp_live_infor}[0]
    Element Attribute Should Match    ${id_homelive_hdp_4}    value    ${Bk4_Hdp_live_infor}[1]
    Element Attribute Should Match    ${id_hdplive_4}    value    ${Bk4_Hdp_live_infor}[2]
    Element Attribute Should Match    ${id_awaylive_hdp_4}    value    ${Bk4_Hdp_live_infor}[3]
    Element Attribute Should Match    ${id_home_open_hdp_4}    value    ${Bk4_Hdp_opening_infor}[1]
    Element Attribute Should Match    ${id_hdp_open_4}    value    ${Bk4_Hdp_opening_infor}[2]
    Element Attribute Should Match    ${id_away_open_hdp_4}    value    ${Bk4_Hdp_opening_infor}[3]
    # Verify List name of Bookmaker is ascending abc
    ${list_name_app}=    Create List    ${Bookmaker_HDP_1}    ${Bookmaker_HDP_2}    ${Bookmaker_HDP_3}    ${Bookmaker_HDP_4}
    Lists Should Be Equal    ${list_name_app}    ${list_name}
    Remove Files    Template_Files/Run/Data_Odds_AsianHDP.json    Template_Files/Run/Data_Odds_AsianHDP_live.json

SP6_Odds_Over_Under_PostMatch
    #Go to current date to view data
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    ${btn_Scores}
    Click Element    //XCUIElementTypeStaticText[@name="${current_date}"]
    Sleep    3
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=HomeTeamName-sr:match:test14
    Sleep    3
    #Go to Over/Under Tab
    Click Element    accessibility_id=Over/Under
    # Get All Bookmaker of Match
    ${json_file}=    Load JSON From File    ${CURDIR}/Template_Files/Data_Odds_Over_Under_auto.json
    ${bookmaker_name}=    Get Value From Json    ${json_file}    $..books..name
    ${bookmaker_name1}=    BuiltIn.Catenate    @{bookmaker_name}[0]
    ${bookmaker_name2}=    BuiltIn.Catenate    @{bookmaker_name}[1]
    ${bookmaker_name3}=    BuiltIn.Catenate    @{bookmaker_name}[2]
    ${bookmaker_name4}=    BuiltIn.Catenate    @{bookmaker_name}[3]
    ${list_name}=    BuiltIn.Create List    ${bookmaker_name1}    ${bookmaker_name2}    ${bookmaker_name3}    ${bookmaker_name4}
    Sort List    ${list_name}
    log    ${list_name}
    log    ${list_name}[0]
    #Get Odds_Info
    ${Bk1_OU_Odds_infor}=    GetOdds_Over_Under    ${json_file}    ${list_name}[0]
    ${Bk2_OU_Odds_infor}=    GetOdds_Over_Under    ${json_file}    ${list_name}[1]
    ${Bk3_OU_Odds_infor}=    GetOdds_Over_Under    ${json_file}    ${list_name}[2]
    ${Bk4_OU_Odds_infor}=    GetOdds_Over_Under    ${json_file}    ${list_name}[3]
    # Verify odd of Bookmaker 1
    ${id_name_OU_1}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk1_OU_Odds_infor}[0]    # Create “Id” attribute of element by bookmaker’s name
    ${id_homelive_OU_1}=    Catenate    SEPARATOR=    accessibility_id=OverLiveOdds-    ${Bk1_OU_Odds_infor}[0]
    ${id_OU_value_live_1}=    Catenate    SEPARATOR=    accessibility_id=LiveOverOption-    ${Bk1_OU_Odds_infor}[0]
    ${id_awaylive_OU_1}=    Catenate    SEPARATOR=    accessibility_id=UnderLiveOdds-    ${Bk1_OU_Odds_infor}[0]
    ${id_home_open_OU_1}=    Catenate    SEPARATOR=    accessibility_id=OverOpeningOdds-    ${Bk1_OU_Odds_infor}[0]
    ${id_OU_value_open_1}=    Catenate    SEPARATOR=    accessibility_id=OpeningOverOption-    ${Bk1_OU_Odds_infor}[0]
    ${id_away_open_OU_1}=    Catenate    SEPARATOR=    accessibility_id=UnderOpeningOdds-    ${Bk1_OU_Odds_infor}[0]
    ${Bookmaker_OU_1}=    Get Element Attribute    ${id_name_OU_1}    value
    Element Attribute Should Match    ${id_name_OU_1}    value    ${Bk1_OU_Odds_infor}[0]
    Element Attribute Should Match    ${id_homelive_OU_1}    value    ${Bk1_OU_Odds_infor}[1]
    Element Attribute Should Match    ${id_OU_value_live_1}    value    ${Bk1_OU_Odds_infor}[2]
    Element Attribute Should Match    ${id_awaylive_OU_1}    value    ${Bk1_OU_Odds_infor}[3]
    Element Attribute Should Match    ${id_home_open_OU_1}    value    ${Bk1_OU_Odds_infor}[4]
    Element Attribute Should Match    ${id_OU_value_open_1}    value    ${Bk1_OU_Odds_infor}[5]
    Element Attribute Should Match    ${id_away_open_OU_1}    value    ${Bk1_OU_Odds_infor}[6]
    # Verify odd of Bookmaker 2
    ${id_name_OU_2}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk2_OU_Odds_infor}[0]
    ${id_homelive_OU_2}=    Catenate    SEPARATOR=    accessibility_id=OverLiveOdds-    ${Bk2_OU_Odds_infor}[0]
    ${id_OU_value_live_2}=    Catenate    SEPARATOR=    accessibility_id=LiveOverOption-    ${Bk2_OU_Odds_infor}[0]
    ${id_awaylive_OU_2}=    Catenate    SEPARATOR=    accessibility_id=UnderLiveOdds-    ${Bk2_OU_Odds_infor}[0]
    ${id_home_open_OU_2}=    Catenate    SEPARATOR=    accessibility_id=OverOpeningOdds-    ${Bk2_OU_Odds_infor}[0]
    ${id_OU_value_open_2}=    Catenate    SEPARATOR=    accessibility_id=OpeningOverOption-    ${Bk2_OU_Odds_infor}[0]
    ${id_away_open_OU_2}=    Catenate    SEPARATOR=    accessibility_id=UnderOpeningOdds-    ${Bk2_OU_Odds_infor}[0]
    ${Bookmaker_OU_2}=    Get Element Attribute    ${id_name_OU_2}    value
    Element Attribute Should Match    ${id_name_OU_2}    value    ${Bk2_OU_Odds_infor}[0]
    Element Attribute Should Match    ${id_homelive_OU_2}    value    ${Bk2_OU_Odds_infor}[1]
    Element Attribute Should Match    ${id_OU_value_live_2}    value    ${Bk2_OU_Odds_infor}[2]
    Element Attribute Should Match    ${id_awaylive_OU_2}    value    ${Bk2_OU_Odds_infor}[3]
    Element Attribute Should Match    ${id_home_open_OU_2}    value    ${Bk2_OU_Odds_infor}[4]
    Element Attribute Should Match    ${id_OU_value_open_2}    value    ${Bk2_OU_Odds_infor}[5]
    Element Attribute Should Match    ${id_away_open_OU_2}    value    ${Bk2_OU_Odds_infor}[6]
    # Verify odd of Bookmaker 3
    ${id_name_OU_3}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk3_OU_Odds_infor}[0]
    ${id_homelive_OU_3}=    Catenate    SEPARATOR=    accessibility_id=OverLiveOdds-    ${Bk3_OU_Odds_infor}[0]
    ${id_OU_value_live_3}=    Catenate    SEPARATOR=    accessibility_id=LiveOverOption-    ${Bk3_OU_Odds_infor}[0]
    ${id_awaylive_OU_3}=    Catenate    SEPARATOR=    accessibility_id=UnderLiveOdds-    ${Bk3_OU_Odds_infor}[0]
    ${id_home_open_OU_3}=    Catenate    SEPARATOR=    accessibility_id=OverOpeningOdds-    ${Bk3_OU_Odds_infor}[0]
    ${id_OU_value_open_3}=    Catenate    SEPARATOR=    accessibility_id=OpeningOverOption-    ${Bk3_OU_Odds_infor}[0]
    ${id_away_open_OU_3}=    Catenate    SEPARATOR=    accessibility_id=UnderOpeningOdds-    ${Bk3_OU_Odds_infor}[0]
    ${Bookmaker_OU_3}=    Get Element Attribute    ${id_name_OU_3}    value
    Element Attribute Should Match    ${id_name_OU_3}    value    ${Bk3_OU_Odds_infor}[0]
    Element Attribute Should Match    ${id_homelive_OU_3}    value    ${Bk3_OU_Odds_infor}[1]
    Element Attribute Should Match    ${id_OU_value_live_3}    value    ${Bk3_OU_Odds_infor}[2]
    Element Attribute Should Match    ${id_awaylive_OU_3}    value    ${Bk3_OU_Odds_infor}[3]
    Element Attribute Should Match    ${id_home_open_OU_3}    value    ${Bk3_OU_Odds_infor}[4]
    Element Attribute Should Match    ${id_OU_value_open_3}    value    ${Bk3_OU_Odds_infor}[5]
    Element Attribute Should Match    ${id_away_open_OU_3}    value    ${Bk3_OU_Odds_infor}[6]
    # Verify odd of Bookmaker 4
    ${id_name_OU_4}=    Catenate    SEPARATOR=    accessibility_id=BookmakerName-    ${Bk4_OU_Odds_infor}[0]
    ${id_homelive_OU_4}=    Catenate    SEPARATOR=    accessibility_id=OverLiveOdds-    ${Bk4_OU_Odds_infor}[0]
    ${id_OU_value_live_4}=    Catenate    SEPARATOR=    accessibility_id=LiveOverOption-    ${Bk4_OU_Odds_infor}[0]
    ${id_awaylive_OU_4}=    Catenate    SEPARATOR=    accessibility_id=UnderLiveOdds-    ${Bk4_OU_Odds_infor}[0]
    ${id_home_open_OU_4}=    Catenate    SEPARATOR=    accessibility_id=OverOpeningOdds-    ${Bk4_OU_Odds_infor}[0]
    ${id_OU_value_open_4}=    Catenate    SEPARATOR=    accessibility_id=OpeningOverOption-    ${Bk4_OU_Odds_infor}[0]
    ${id_away_open_OU_4}=    Catenate    SEPARATOR=    accessibility_id=UnderOpeningOdds-    ${Bk4_OU_Odds_infor}[0]
    ${Bookmaker_OU_4}=    Get Element Attribute    ${id_name_OU_4}    value
    Element Attribute Should Match    ${id_name_OU_4}    value    ${Bk4_OU_Odds_infor}[0]
    Element Attribute Should Match    ${id_homelive_OU_4}    value    ${Bk4_OU_Odds_infor}[1]
    Element Attribute Should Match    ${id_OU_value_live_4}    value    ${Bk4_OU_Odds_infor}[2]
    Element Attribute Should Match    ${id_awaylive_OU_4}    value    ${Bk4_OU_Odds_infor}[3]
    Element Attribute Should Match    ${id_home_open_OU_4}    value    ${Bk4_OU_Odds_infor}[4]
    Element Attribute Should Match    ${id_OU_value_open_4}    value    ${Bk4_OU_Odds_infor}[5]
    Element Attribute Should Match    ${id_away_open_OU_4}    value    ${Bk4_OU_Odds_infor}[6]
    ${list_value_app}=    Create List    ${Bookmaker_OU_1}    ${Bookmaker_OU_2}    ${Bookmaker_OU_3}    ${Bookmaker_OU_4}
    Lists Should Be Equal    ${list_value_app}    ${list_name}
    Remove Files    Template_Files/Run/Data_Odds_Over_Under_auto.json

*** Keywords ***
