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
    #Get data from app for match 1
    ${date1_dd}    Convert to Integer    ${ac_date}
    Wait Until Page Contains Element    accessibility_id=${date1_dd}
    Click Element    accessibility_id=${date1_dd}
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test01
    ${leaguename}=    Get Text    accessibility_id=LeagueName-sr:tournament:38
    ${league_date_app}=    Get Text    accessibility_id=LeagueEventDate-sr:tournament:38
    ${match1_status}=    Get Element Attribute    accessibility_id=MatchStatus-sr:match:test01    value
    ${homescore_match1}=    Get Element Attribute    accessibility_id=HomeScore-sr:match:test01    value
    ${awayscore_match1}=    Get Element Attribute    accessibility_id=AwayScore-sr:match:test01    value
    ${homename_match1}=    Get Element Attribute    accessibility_id=HomeTeamName-sr:match:test01    value
    ${awayname_match1}=    Get Element Attribute    accessibility_id=AwayTeamName-sr:match:test01    value
    Should Be Equal As Strings    '${leaguename}'    'CHAMPIONS LEAGUE - QUALIFICATION'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    'AB'
    Should Be Equal As Strings    '${homename_match1}'    'FC Astana'
    Should Be Equal As Strings    '${awayname_match1}'    'CFR Cluj'
    Should Be Equal As Integers    ${homescore_match1}    2
    Should Be Equal As Integers    ${awayscore_match1}    1
    #Get data from app for match 2
    ${match2_status}=    Get Element Attribute    accessibility_id=MatchStatus-sr:match:test02    value
    ${homescore_match2}=    Get Element Attribute    accessibility_id=HomeScore-sr:match:test02    value
    ${awayscore_match2}=    Get Element Attribute    accessibility_id=AwayScore-sr:match:test02    value
    ${homename_match2}=    Get Element Attribute    accessibility_id=HomeTeamName-sr:match:test02    value
    ${awayname_match2}=    Get Element Attribute    accessibility_id=AwayTeamName-sr:match:test02    value
    Should Be Equal As Strings    '${match2_status}'    'FT'
    Should Be Equal As Strings    '${homename_match2}'    'Ararat Armenia'
    Should Be Equal As Strings    '${awayname_match2}'    'AIK'
    Should Be Equal As Strings    ${homescore_match2}    2
    Should Be Equal As Strings    ${awayscore_match2}    1

SP2_Score_Post_Pre_Match_Date2
    [Documentation]    Verify data of Post-Match in the day that happend 2 days before current day
    ${ac_date}    ${ac_month}=    Get date from value    -2
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    #Get data from app for match 1
    ${date2_dd}    Convert to Integer    ${ac_date}
    Wait Until Page Contains Element    accessibility_id=${date2_dd}
    Click Element    accessibility_id=${date2_dd}
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test03
    ${leaguename}=    Get Text    accessibility_id=LeagueName-sr:tournament:38
    ${league_date_app}=    Get Text    accessibility_id=LeagueEventDate-sr:tournament:38
    ${match1_status}=    Get Element Attribute    accessibility_id=MatchStatus-sr:match:test03    value
    ${homescore_match1}=    Get Element Attribute    accessibility_id=HomeScore-sr:match:test03    value
    ${awayscore_match1}=    Get Element Attribute    accessibility_id=AwayScore-sr:match:test03    value
    ${homename_match1}=    Get Element Attribute    accessibility_id=HomeTeamName-sr:match:test03    value
    ${awayname_match1}=    Get Element Attribute    accessibility_id=AwayTeamName-sr:match:test03    value
    Should Be Equal As Strings    '${leaguename}'    'EUROPA LEAGUE - QUALIFICATION'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    'FT'
    Should Be Equal As Strings    '${homename_match1}'    'Gzira United'
    Should Be Equal As Strings    '${awayname_match1}'    'Hajduk Split'
    Should Be Equal As Integers    ${homescore_match1}    2
    Should Be Equal As Integers    ${awayscore_match1}    1
    #Get data from app for match 2
    ${match2_status}=    Get Element Attribute    accessibility_id=MatchStatus-sr:match:test04    value
    ${homescore_match2}=    Get Element Attribute    accessibility_id=HomeScore-sr:match:test04    value
    ${awayscore_match2}=    Get Element Attribute    accessibility_id=AwayScore-sr:match:test04    value
    ${homename_match2}=    Get Element Attribute    accessibility_id=HomeTeamName-sr:match:test04    value
    ${awayname_match2}=    Get Element Attribute    accessibility_id=AwayTeamName-sr:match:test04    value
    Should Be Equal As Strings    '${match2_status}'    'FT'
    Should Be Equal As Strings    '${homename_match2}'    'AS Jeunesse Esch'
    Should Be Equal As Strings    '${awayname_match2}'    'Tobol Kostanay'
    Should Be Equal As Strings    ${homescore_match2}    2
    Should Be Equal As Strings    ${awayscore_match2}    0

SP2_Score_Post_Pre_Match_Date3
    [Documentation]    Verify data of Post-Match in the day that happend 1 days before current day
    ${ac_date}    ${ac_month}=    Get date from value    -1
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    #Get data from app for match 1
    ${date3_dd}    Convert to Integer    ${ac_date}
    Wait Until Page Contains Element    accessibility_id=${date3_dd}
    Click Element    accessibility_id=${date3_dd}
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test05
    ${leaguename}=    Get Text    accessibility_id=LeagueName-sr:tournament:38
    ${league_date_app}=    Get Text    accessibility_id=LeagueEventDate-sr:tournament:38
    ${match1_status}=    Get Element Attribute    accessibility_id=MatchStatus-sr:match:test05    value
    ${homescore_match1}=    Get Element Attribute    accessibility_id=HomeScore-sr:match:test05    value
    ${awayscore_match1}=    Get Element Attribute    accessibility_id=AwayScore-sr:match:test05    value
    ${homename_match1}=    Get Element Attribute    accessibility_id=HomeTeamName-sr:match:test05    value
    ${awayname_match1}=    Get Element Attribute    accessibility_id=AwayTeamName-sr:match:test05    value
    Should Be Equal As Strings    '${leaguename}'    'INTERNATIONAL - CLUB FRIENDLIES'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    'AET'
    Should Be Equal As Strings    '${homename_match1}'    'Livingston'
    Should Be Equal As Strings    '${awayname_match1}'    'Alloa Athletic'
    Should Be Equal As Integers    ${homescore_match1}    2
    Should Be Equal As Integers    ${awayscore_match1}    0
    #Get data from app for match 2
    ${match2_status}=    Get Element Attribute    accessibility_id=MatchStatus-sr:match:test06    value
    ${homescore_match2}=    Get Element Attribute    accessibility_id=HomeScore-sr:match:test06    value
    ${awayscore_match2}=    Get Element Attribute    accessibility_id=AwayScore-sr:match:test06    value
    ${homename_match2}=    Get Element Attribute    accessibility_id=HomeTeamName-sr:match:test06    value
    ${awayname_match2}=    Get Element Attribute    accessibility_id=AwayTeamName-sr:match:test06    value
    Should Be Equal As Strings    '${match2_status}'    'AP'
    Should Be Equal As Strings    '${homename_match2}'    'Carlisle United'
    Should Be Equal As Strings    '${awayname_match2}'    'Hibernian'
    Should Be Equal As Strings    ${homescore_match2}    2
    Should Be Equal As Strings    ${awayscore_match2}    0

SP2_Score_Post_Pre_Match_Date5
    [Documentation]    Verify data of Post-Match in the day that happend 1 days after current day
    ${ac_date}    ${ac_month}=    Get date from value    1
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    #Get data from app for match 1
    ${date5_dd}    Convert to Integer    ${ac_date}
    Wait Until Page Contains Element    accessibility_id=${date5_dd}
    Click Element    accessibility_id=${date5_dd}
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test07
    ${leaguename}=    Get Text    accessibility_id=LeagueName-sr:tournament:38
    ${league_date_app}=    Get Text    accessibility_id=LeagueEventDate-sr:tournament:38
    ${match1_status}=    Get Element Attribute    accessibility_id=MatchStatus-sr:match:test07    value
    ${homename_match1}=    Get Element Attribute    accessibility_id=HomeTeamName-sr:match:test07    value
    ${awayname_match1}=    Get Element Attribute    accessibility_id=AwayTeamName-sr:match:test07    value
    Should Be Equal As Strings    '${leaguename}'    'INTERNATIONAL - CLUB FRIENDLIES'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    '17:00'
    Should Be Equal As Strings    '${homename_match1}'    'Accrington Stanley'
    Should Be Equal As Strings    '${awayname_match1}'    'Marseille'
    #Get data from app for match 2
    ${match2_status}=    Get Element Attribute    accessibility_id=MatchStatus-sr:match:test08    value
    ${homename_match2}=    Get Element Attribute    accessibility_id=HomeTeamName-sr:match:test08    value
    ${awayname_match2}=    Get Element Attribute    accessibility_id=AwayTeamName-sr:match:test08    value
    Should Be Equal As Strings    '${match2_status}'    'Postp.'
    Should Be Equal As Strings    '${homename_match2}'    'Tranmere Rovers'
    Should Be Equal As Strings    '${awayname_match2}'    'Liverpool'

SP2_Score_Post_Pre_Match_Date6
    [Documentation]    Verify data of Post-Match in the day that happend 2 days after current day
    ${ac_date}    ${ac_month}=    Get date from value    2
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    #Get data from app for match 1
    ${date6_dd}    Convert to Integer    ${ac_date}
    Wait Until Page Contains Element    accessibility_id=${date6_dd}
    Click Element    accessibility_id=${date6_dd}
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test09
    ${leaguename}=    Get Text    accessibility_id=LeagueName-sr:tournament:892
    ${league_date_app}=    Get Text    accessibility_id=LeagueEventDate-sr:tournament:892
    ${match1_status}=    Get Element Attribute    accessibility_id=MatchStatus-sr:match:test09    value
    ${homename_match1}=    Get Element Attribute    accessibility_id=HomeTeamName-sr:match:test09    value
    ${awayname_match1}=    Get Element Attribute    accessibility_id=AwayTeamName-sr:match:test09    value
    Should Be Equal As Strings    '${leaguename}'    'SWEDEN - ALLSVENSKAN'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    'Start Delayed'
    Should Be Equal As Strings    '${homename_match1}'    'AFC Eskilstuna'
    Should Be Equal As Strings    '${awayname_match1}'    'Kalmar FF'

SP2_Score_Post_Pre_Match_Date7
    [Documentation]    Verify data of Post-Match in the day that happend 3 days after current day
    ${ac_date}    ${ac_month}=    Get date from value    3
    ${day_monthname}=    Return day month    ${ac_date}    ${ac_month}
    ${league_date_db}    Catenate    ${day_monthname}
    #Get data from app for match 1
    ${date7_dd}    Convert to Integer    ${ac_date}
    Wait Until Page Contains Element    accessibility_id=${date7_dd}
    Click Element    accessibility_id=${date7_dd}
    Sleep    5
    Capture Page Screenshot
    Wait Until Page Contains Element    accessibility_id=MatchStatus-sr:match:test10
    ${leaguename}=    Get Text    accessibility_id=LeagueName-sr:tournament:892
    ${league_date_app}=    Get Text    accessibility_id=LeagueEventDate-sr:tournament:892
    ${match1_status}=    Get Element Attribute    accessibility_id=MatchStatus-sr:match:test10    value
    ${homename_match1}=    Get Element Attribute    accessibility_id=HomeTeamName-sr:match:test10    value
    ${awayname_match1}=    Get Element Attribute    accessibility_id=AwayTeamName-sr:match:test10    value
    Should Be Equal As Strings    '${leaguename}'    'DENMARK - SUPERLIGA'
    Should Be Equal As Strings    ${league_date_app}    ${league_date_db}
    Should Be Equal As Strings    '${match1_status}'    'Canc.'
    Should Be Equal As Strings    '${homename_match1}'    'Broendby IF'
    Should Be Equal As Strings    '${awayname_match1}'    'Silkeborg'

SP3_SP4_List_Event_Of_Match1
    Update_Template_List_Event_Of_Match1
    ${file}=    Get File    ${CURDIR}/Template_Files/Run/List_event_data_template1.txt
    #Push events
    Post    ${Push_File}    ${file}
    Integer    response status    200
    Output
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    accessibility_id=${current_date}
    Wait Until Element Is Visible    accessibility_id=Aston Villa
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Aston Villa
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_Yellowcard_1st
    ${event1_matchtime}=    Get Element Attribute    accessibility_id=MatchTime-3-yellow_card    value
    Should Be Equal As Strings    '${event1_matchtime}'    '10''
    ${event1_player}=    Get Element Attribute    accessibility_id=HomePlayerName-3-yellow_card    value
    Should Be Equal As Strings    '${event1_player}'    'Gershom Cox'
    ${event1_yellowcard_icon}=    Get Element Attribute    accessibility_id=HomeImage-3-yellow_card    name
    Should Be Equal As Strings    '${event1_yellowcard_icon}'    'HomeImage-3-yellow_card'
    #Event2_redcard
    ${event2_matchtime}=    Get Element Attribute    accessibility_id=MatchTime-4-red_card    value
    Should Be Equal As Strings    '${event2_matchtime}'    '15''
    ${event2_player}=    Get Element Attribute    accessibility_id=AwayPlayerName-4-red_card    value
    Should Be Equal As Strings    '${event2_player}'    'Duncan McLean'
    ${event2_redcard_icon}=    Get Element Attribute    accessibility_id=AwayImage-4-red_card    name
    Should Be Equal As Strings    '${event2_redcard_icon}'    'AwayImage-4-red_card'
    #Event3_score_change_for_away_1st
    ${event3_matchtime}=    Get Element Attribute    accessibility_id=MatchTime-5-score_change    value
    Should Be Equal As Strings    '${event3_matchtime}'    '30''
    ${event3_score}=    Get Element Attribute    accessibility_id=Score-5-score_change    label
    Should Be Equal As Strings    '${event3_score}'    '0 - 1'
    ${event3_scorer}=    Get Element Attribute    accessibility_id=AwayPlayerName-5-score_change    value
    Should Be Equal As Strings    '${event3_scorer}'    'Charlie Parry'
    ${event3_assist}=    Get Element Attribute    accessibility_id=AwayAssistPlayerName-5-score_change    value
    Should Be Equal As Strings    '${event3_assist}'    'Johnny Holt'
    ${event3_ball_icon}=    Get Element Attribute    accessibility_id=AwayImage-5-score_change    name
    Should Be Equal As Strings    '${event3_ball_icon}'    'AwayImage-5-score_change'
    #Event4_score_change_for_home_1st
    ${event4_matchtime}=    Get Element Attribute    accessibility_id=MatchTime-6-score_change    value
    Should Be Equal As Strings    '${event4_matchtime}'    '35''
    ${event4_score}=    Get Element Attribute    accessibility_id=Score-6-score_change    label
    Should Be Equal As Strings    '${event4_score}'    '1 - 1'
    ${event4_scorer}=    Get Element Attribute    accessibility_id=HomePlayerName-6-score_change    value
    Should Be Equal As Strings    '${event4_scorer}'    'Howard Vaughton'
    ${event4_ball_icon}=    Get Element Attribute    accessibility_id=HomeImage-6-score_change    name
    Should Be Equal As Strings    '${event4_ball_icon}'    'HomeImage-6-score_change'
    #Event5_yellowredcard
    ${event5_matchtime}=    Get Element Attribute    accessibility_id=MatchTime-8-yellow_red_card    value
    Should Be Equal As Strings    '${event5_matchtime}'    '45+2''
    ${event5_player}=    Get Element Attribute    accessibility_id=AwayPlayerName-8-yellow_red_card    value
    Should Be Equal As Strings    '${event5_player}'    'Dan Doyle'
    ${event5_yellowredcard_icon}=    Get Element Attribute    accessibility_id=AwayImage-8-yellow_red_card    name
    Should Be Equal As Strings    '${event5_yellowredcard_icon}'    'AwayImage-8-yellow_red_card'
    #Event6_halftime
    ${event6_halftime_text}=    Get Element Attribute    accessibility_id=MainEventStatus-9-break_start    value
    Should Be Equal As Strings    '${event6_halftime_text}'    'Half Time'
    ${event6_score}=    Get Element Attribute    accessibility_id=Score-9-break_start    label
    Should Be Equal As Strings    '${event6_score}'    '1 - 1'
    #Event7_fulltime
    ${event7_fulltime_text}=    Get Element Attribute    accessibility_id=MainEventStatus-11-match_ended    value
    Should Be Equal As Strings    '${event7_fulltime_text}'    'Full Time'
    ${event7_score}=    Get Element Attribute    accessibility_id=Score-11-match_ended    label
    Should Be Equal As Strings    '${event7_score}'    '1 - 1'
    Remove File    Template_Files/Run/List_event_data_template1.txt
    Click Element    ${btn_Scores}

SP3_SP4_List_Event_Of_Match2
    ${file}=    Get File    ${CURDIR}/Template_Files/List_event_data_template2.txt
    #Push events
    Post    ${Push_File}    ${file}
    Integer    response status    200
    Output
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    accessibility_id=${current_date}
    Wait Until Element Is Visible    accessibility_id=West Ham
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=West Ham
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_halftime
    ${event1_halftime_text}=    Get Element Attribute    accessibility_id=MainEventStatus-9-break_start    value
    Should Be Equal As Strings    '${event1_halftime_text}'    'Half Time'
    ${event1_score}=    Get Element Attribute    accessibility_id=Score-9-break_start    label
    Should Be Equal As Strings    '${event1_score}'    '0 - 0'
    #Event2_score_change_for_home_2nd
    ${event2_matchtime}=    Get Element Attribute    accessibility_id=MatchTime-11-score_change    value
    Should Be Equal As Strings    '${event2_matchtime}'    '55''
    ${event2_score}=    Get Element Attribute    accessibility_id=Score-11-score_change    label
    Should Be Equal As Strings    '${event2_score}'    '1 - 0'
    ${event2_scorer}=    Get Element Attribute    accessibility_id=HomePlayerName-11-score_change    value
    Should Be Equal As Strings    '${event2_scorer}'    'Aaron Cresswell'
    ${event2_ball_icon}=    Get Element Attribute    accessibility_id=HomeImage-11-score_change    name
    Should Be Equal As Strings    '${event2_ball_icon}'    'HomeImage-11-score_change'
    #Event3_score_change_for_away_2nd_by_own_goal
    ${event3_matchtime}=    Get Element Attribute    accessibility_id=MatchTime-12-score_change    value
    Should Be Equal As Strings    '${event3_matchtime}'    '75''
    ${event3_score}=    Get Element Attribute    accessibility_id=Score-12-score_change    label
    Should Be Equal As Strings    '${event3_score}'    '1 - 1'
    ${event3_scorer}=    Get Element Attribute    accessibility_id=AwayPlayerName-12-score_change    value
    Should Be Equal As Strings    '${event3_scorer}'    'Angelo Ogbonna'
    ${event3_ball_icon}=    Get Element Attribute    accessibility_id=AwayImage-12-score_change    name
    Should Be Equal As Strings    '${event3_ball_icon}'    'AwayImage-12-score_change'
    #Event4_score_change_for_home_2nd
    ${event4_matchtime}=    Get Element Attribute    accessibility_id=MatchTime-13-penalty_missed    value
    Should Be Equal As Strings    '${event4_matchtime}'    '80''
    ${event4_score}=    Get Element Attribute    accessibility_id=Score-13-penalty_missed    label
    Should Be Equal As Strings    '${event4_score}'    '1 - 1'
    ${event4_player}=    Get Element Attribute    accessibility_id=HomePlayerName-13-penalty_missed    value
    Should Be Equal As Strings    '${event4_player}'    'Ryan Fredericks'
    ${event4_ball_icon}=    Get Element Attribute    accessibility_id=HomeImage-13-penalty_missed    name
    Should Be Equal As Strings    '${event4_ball_icon}'    'HomeImage-13-penalty_missed'
    #Event5_fulltime
    ${event5_fulltime_text}=    Get Element Attribute    accessibility_id=MainEventStatus-14-match_ended    value
    Should Be Equal As Strings    '${event5_fulltime_text}'    'Full Time'
    ${event5_score}=    Get Element Attribute    accessibility_id=Score-14-match_ended    label
    Should Be Equal As Strings    '${event5_score}'    '1 - 1'
    Click Element    ${btn_Scores}

SP3_SP4_List_Event_Of_Match3
    ${file}=    Get File    ${CURDIR}/Template_Files/List_event_data_template3.txt
    #Push events
    Post    ${Push_File}    ${file}
    Integer    response status    200
    Output
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    accessibility_id=${current_date}
    Wait Until Element Is Visible    accessibility_id=Liverpool
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Liverpool
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_halftime
    ${event1_halftime_text}=    Get Element Attribute    accessibility_id=MainEventStatus-3-break_start    value
    Should Be Equal As Strings    '${event1_halftime_text}'    'Half Time'
    ${event1_score}=    Get Element Attribute    accessibility_id=Score-3-break_start    label
    Should Be Equal As Strings    '${event1_score}'    '0 - 0'
    #Event2_fulltime
    ${event2_fulltime_text}=    Get Element Attribute    accessibility_id=MainEventStatus-5-break_start    value
    Should Be Equal As Strings    '${event2_fulltime_text}'    'Full Time'
    ${event2_score}=    Get Element Attribute    accessibility_id=Score-5-break_start    label
    Should Be Equal As Strings    '${event2_score}'    '0 - 0'
    #Event3_score_change_for_home_extra_time_1st
    ${event3_matchtime}=    Get Element Attribute    accessibility_id=MatchTime-7-score_change    value
    Should Be Equal As Strings    '${event3_matchtime}'    '96''
    ${event3_score}=    Get Element Attribute    accessibility_id=Score-7-score_change    label
    Should Be Equal As Strings    '${event3_score}'    '1 - 0'
    ${event3_scorer}=    Get Element Attribute    accessibility_id=HomePlayerName-7-score_change    value
    Should Be Equal As Strings    '${event3_scorer}'    'Dejan Lovren'
    ${event3_ball_icon}=    Get Element Attribute    accessibility_id=HomeImage-7-score_change    name
    Should Be Equal As Strings    '${event3_ball_icon}'    'HomeImage-7-score_change'
    #Event4_redcard_extra_time_2nd_injury_time_shown
    ${event4_matchtime}=    Get Element Attribute    accessibility_id=MatchTime-8-red_card    value
    Should Be Equal As Strings    '${event4_matchtime}'    '105+2''
    ${event4_player}=    Get Element Attribute    accessibility_id=AwayPlayerName-8-red_card    value
    Should Be Equal As Strings    '${event4_player}'    'James Husband'
    ${event4_redcard_icon}=    Get Element Attribute    accessibility_id=AwayImage-8-red_card    name
    Should Be Equal As Strings    '${event4_redcard_icon}'    'AwayImage-8-red_card'
    #Event5_afterextratime
    ${event5_afterextratime_text}=    Get Element Attribute    accessibility_id=MainEventStatus-11-match_ended    value
    Should Be Equal As Strings    '${event5_afterextratime_text}'    'After Extra Time'
    ${event5_score}=    Get Element Attribute    accessibility_id=Score-11-match_ended    label
    Should Be Equal As Strings    '${event5_score}'    '1 - 0'
    Click Element    ${btn_Scores}

SP3_SP4_List_Event_Of_Match4
    ${file}=    Get File    ${CURDIR}/Template_Files/List_event_data_template4.txt
    #Push events
    Post    ${Push_File}    ${file}
    Integer    response status    200
    Output
    ${current_date}=    Get Current Date    result_format=%d    #current date
    ${current_date}=    Convert To Integer    ${current_date}
    Click Element    accessibility_id=${current_date}
    Sleep    3
    ${Kick_Of_Time}=    Get Element Attribute    accessibility_id=LeagueEventDate-sr:tournament:25839    value
    Wait Until Element Is Visible    accessibility_id=Chelsea
    #Go to Match Info page from Scores page
    Click Element    accessibility_id=Chelsea
    Sleep    3
    Click Element    accessibility_id=INFO
    Sleep    3
    #Event1_halftime
    ${event1_halftime_text}=    Get Element Attribute    accessibility_id=MainEventStatus-9-break_start    value
    Should Be Equal As Strings    '${event1_halftime_text}'    'Half Time'
    ${event1_score}=    Get Element Attribute    accessibility_id=Score-9-break_start    label
    Should Be Equal As Strings    '${event1_score}'    '0 - 0'
    #Event2_fulltime
    ${event2_fulltime_text}=    Get Element Attribute    accessibility_id=MainEventStatus-17-break_start    value
    Should Be Equal As Strings    '${event2_fulltime_text}'    'Full Time'
    ${event2_score}=    Get Element Attribute    accessibility_id=Score-17-break_start    label
    Should Be Equal As Strings    '${event2_score}'    '0 - 0'
    #Event3_afterextratime
    ${event3_afterextratime_text}=    Get Element Attribute    accessibility_id=MainEventStatus-25-break_start    value
    Should Be Equal As Strings    '${event3_afterextratime_text}'    'After Extra Time'
    ${event3_score}=    Get Element Attribute    accessibility_id=Score-25-break_start    label
    Should Be Equal As Strings    '${event3_score}'    '0 - 0'
    #Event4_penalty_shoot_out
    ${event4_penalty_shoot_out_text}=    Get Element Attribute    accessibility_id=MainEventStatus-26-period_start    value
    Should Be Equal As Strings    '${event4_penalty_shoot_out_text}'    'Penalty Shoot-Out'
    ${event4_score}=    Get Element Attribute    accessibility_id=Score-26-period_start    label
    Should Be Equal As Strings    '${event4_score}'    '3 - 2'
    #Event5_penalty_shoot_out_round1
    ${event5_home_player}=    Get Element Attribute    accessibility_id=HomePlayerName-28-penalty_shootout    value
    Should Be Equal As Strings    '${event5_home_player}'    'Walter Bettridge'
    ${event5_home_ball_icon}=    Get Element Attribute    accessibility_id=HomeImage-28-penalty_shootout    name
    Should Be Equal As Strings    '${event5_home_ball_icon}'    'HomeImage-28-penalty_shootout'
    ${event5_score}=    Get Element Attribute    accessibility_id=Score-28-penalty_shootout    label
    Should Be Equal As Strings    '${event5_score}'    '0 - 1'
    ${event5_away_player}=    Get Element Attribute    accessibility_id=AwayPlayerName-28-penalty_shootout    value
    Should Be Equal As Strings    '${event5_away_player}'    'John Anderson'
    ${event5_away_ball_icon}=    Get Element Attribute    accessibility_id=AwayImage-28-penalty_shootout    name
    Should Be Equal As Strings    '${event5_away_ball_icon}'    'AwayImage-28-penalty_shootout'
    #Event6_penalty_shoot_out_round2
    ${event6_home_player}=    Get Element Attribute    accessibility_id=HomePlayerName-30-penalty_shootout    value
    Should Be Equal As Strings    '${event6_home_player}'    'Nils Middelboe'
    ${event6_home_ball_icon}=    Get Element Attribute    accessibility_id=HomeImage-30-penalty_shootout    name
    Should Be Equal As Strings    '${event6_home_ball_icon}'    'HomeImage-30-penalty_shootout'
    ${event6_score}=    Get Element Attribute    accessibility_id=Score-30-penalty_shootout    label
    Should Be Equal As Strings    '${event6_score}'    '1 - 2'
    ${event6_away_player}=    Get Element Attribute    accessibility_id=AwayPlayerName-30-penalty_shootout    value
    Should Be Equal As Strings    '${event6_away_player}'    'Chuks Aneke'
    ${event6_away_ball_icon}=    Get Element Attribute    accessibility_id=AwayImage-30-penalty_shootout    name
    Should Be Equal As Strings    '${event6_away_ball_icon}'    'AwayImage-30-penalty_shootout'
    #Event7_penalty_shoot_out_round3
    ${event7_home_player}=    Get Element Attribute    accessibility_id=HomePlayerName-32-penalty_shootout    value
    Should Be Equal As Strings    '${event7_home_player}'    'Jack Harrow'
    ${event7_home_ball_icon}=    Get Element Attribute    accessibility_id=HomeImage-32-penalty_shootout    name
    Should Be Equal As Strings    '${event7_home_ball_icon}'    'HomeImage-32-penalty_shootout'
    ${event7_score}=    Get Element Attribute    accessibility_id=Score-32-penalty_shootout    label
    Should Be Equal As Strings    '${event7_score}'    '2 - 2'
    ${event7_away_player}=    Get Element Attribute    accessibility_id=AwayPlayerName-32-penalty_shootout    value
    Should Be Equal As Strings    '${event7_away_player}'    'Nicolas Anelka'
    ${event7_away_ball_icon}=    Get Element Attribute    accessibility_id=AwayImage-32-penalty_shootout    name
    Should Be Equal As Strings    '${event7_away_ball_icon}'    'AwayImage-32-penalty_shootout'
    #Event8_penalty_shoot_out_round4
    ${event8_home_player}=    Get Element Attribute    accessibility_id=HomePlayerName-34-penalty_shootout    value
    Should Be Equal As Strings    '${event8_home_player}'    'Harold Halse'
    ${event8_home_ball_icon}=    Get Element Attribute    accessibility_id=HomeImage-34-penalty_shootout    name
    Should Be Equal As Strings    '${event8_home_ball_icon}'    'HomeImage-34-penalty_shootout'
    ${event8_score}=    Get Element Attribute    accessibility_id=Score-34-penalty_shootout    label
    Should Be Equal As Strings    '${event8_score}'    '3 - 2'
    ${event8_away_player}=    Get Element Attribute    accessibility_id=AwayPlayerName-34-penalty_shootout    value
    Should Be Equal As Strings    '${event8_away_player}'    'Martin Angha'
    ${event8_away_ball_icon}=    Get Element Attribute    accessibility_id=AwayImage-34-penalty_shootout    name
    Should Be Equal As Strings    '${event8_away_ball_icon}'    'AwayImage-34-penalty_shootout'
    #Event9_penalty_shoot_out_round5
    ${event9_home_player}=    Get Element Attribute    accessibility_id=HomePlayerName-36-penalty_shootout    value
    Should Be Equal As Strings    '${event9_home_player}'    'Bob McNeil'
    ${event9_home_ball_icon}=    Get Element Attribute    accessibility_id=HomeImage-36-penalty_shootout    name
    Should Be Equal As Strings    '${event9_home_ball_icon}'    'HomeImage-36-penalty_shootout'
    ${event9_score}=    Get Element Attribute    accessibility_id=Score-36-penalty_shootout    label
    Should Be Equal As Strings    '${event9_score}'    '3 - 2'
    ${event9_away_player}=    Get Element Attribute    accessibility_id=AwayPlayerName-36-penalty_shootout    value
    Should Be Equal As Strings    '${event9_away_player}'    'George Armstrong'
    ${event9_away_ball_icon}=    Get Element Attribute    accessibility_id=AwayImage-36-penalty_shootout    name
    Should Be Equal As Strings    '${event9_away_ball_icon}'    'AwayImage-36-penalty_shootout'
    Page Should Contain Element    accessibility_id=HomeTeamPenalty
    Page Should Contain Element    accessibility_id=HomeTeamSecondLeg
    Page Should Contain Element    accessibility_id=SecondLegDetai
    Page Should Contain Element    accessibility_id=PenaltyDetail
    ${Referee}=    Get Element Attribute    accessibility_id=RefereeName    value
    Should Be Equal As Strings    '${Referee}'    'Anthony Taylor'
    ${Venue}=    Get Element Attribute    accessibility_id=VenueName    value
    Should Be Equal As Strings    '${Venue}'    ' Emirates'
    ${Spectators}=    Get Element Attribute    accessibility_id=Attendance    value
    Should Be Equal As Strings    '${Spectators}'    '1,653,490'
    ${Kick_Of_Time}=    Catenate    17:00    ${Kick_Of_Time}
    ${Kick_Of_Time}=    Catenate    SEPARATOR=    ${Kick_Of_Time}    ,
    ${current_date}=    Get Current Date    result_format=%Y
    ${Kick_Of_Time}=    Catenate    ${Kick_Of_Time}    ${current_date}
    ${Kick Off Time}=    Get Element Attribute    accessibility_id=EventDate    value
    Should Be Equal As Strings    '${Kick Off Time}'    '${Kick_Of_Time}'
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
    Click Element    accessibility_id=${current_date}
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
    ${NavigationTitle}=    Get Element Attribute    accessibility_id=NavigationTitle-    value
    Should Be Equal As Strings    '${NavigationTitle}'    '${Title}'
    Page Should Contain Element    accessibility_id=images/common/odd_movement_chart.png
    Page Should Contain Element    accessibility_id=MatchOddsMovement
    #Check values of Odds Movement
    ##Check values of row 1
    ${UpdateTime 1}    Get Value From Json    ${result}    $[sport_events][9][markets_last_updated]
    ${UpdateTime 1}    Catenate    @{UpdateTime 1}
    ${UpdateTime 1_file}    Add Time To Date    ${UpdateTime 1}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 1}    Add Time To Date    ${UpdateTime 1}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 1_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 1}
    ${MatchTime 1_app}    Get Element Attribute    ${MatchTime 1_id}    value
    Should Be Equal As Strings    '${MatchTime 1_app}'    'HT'
    ${MatchScore 1_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 1}
    ${MatchScore 1_app}    Get Element Attribute    ${MatchScore 1_id}    value
    Should Be Equal As Strings    '${MatchScore 1_app}'    '1 - 1'
    ${HomeOdds 1_file}    Get Value From Json    ${result}    $[sport_events][9][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 1_file}    Catenate    @{HomeOdds 1_file}
    ${HomeOdds 1_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 1}
    ${HomeOdds 1_app}    Get Element Attribute    ${HomeOdds 1_id}    value
    Should Be Equal As Strings    '${HomeOdds 1_app}'    '${HomeOdds 1_file}'
    ${AwayOdds 1_file}    Get Value From Json    ${result}    $[sport_events][9][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 1_file}    Catenate    @{AwayOdds 1_file}
    ${AwayOdds 1_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 1}
    ${AwayOdds 1_app}    Get Element Attribute    ${AwayOdds 1_id}    value
    Should Be Equal As Strings    '${AwayOdds 1_app}'    '${AwayOdds 1_file}'
    ${DrawOdds 1_file}    Get Value From Json    ${result}    $[sport_events][9][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 1_file}    Catenate    @{DrawOdds 1_file}
    ${DrawOdds 1_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 1}
    ${DrawOdds 1_app}    Get Element Attribute    ${DrawOdds 1_id}    value
    Should Be Equal As Strings    '${DrawOdds 1_app}'    '${DrawOdds 1_file}'
    ${UpdateTime 1_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 1}
    ${UpdateTime 1_app}    Get Element Attribute    ${UpdateTime 1_id}    value
    Should Be Equal As Strings    '${UpdateTime 1_app}'    '${UpdateTime 1_file}'
    ##Check values of row 2
    ${UpdateTime 2}    Get Value From Json    ${result}    $[sport_events][8][markets_last_updated]
    ${UpdateTime 2}    Catenate    @{UpdateTime 2}
    ${UpdateTime 2_file}    Add Time To Date    ${UpdateTime 2}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 2}    Add Time To Date    ${UpdateTime 2}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 2_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 2}
    ${MatchTime 2_app}    Get Element Attribute    ${MatchTime 2_id}    value
    Should Be Equal As Strings    '${MatchTime 2_app}'    'HT'
    ${MatchScore 2_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 2}
    ${MatchScore 2_app}    Get Element Attribute    ${MatchScore 2_id}    value
    Should Be Equal As Strings    '${MatchScore 2_app}'    '1 - 1'
    ${HomeOdds 2_file}    Get Value From Json    ${result}    $[sport_events][8][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 2_file}    Catenate    @{HomeOdds 2_file}
    ${HomeOdds 2_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 2}
    ${HomeOdds 2_app}    Get Element Attribute    ${HomeOdds 2_id}    value
    Should Be Equal As Strings    '${HomeOdds 2_app}'    '${HomeOdds 2_file}'
    ${AwayOdds 2_file}    Get Value From Json    ${result}    $[sport_events][8][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 2_file}    Catenate    @{AwayOdds 2_file}
    ${AwayOdds 2_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 2}
    ${AwayOdds 2_app}    Get Element Attribute    ${AwayOdds 2_id}    value
    Should Be Equal As Strings    '${AwayOdds 2_app}'    '${AwayOdds 2_file}'
    ${DrawOdds 2_file}    Get Value From Json    ${result}    $[sport_events][8][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 2_file}    Catenate    @{DrawOdds 2_file}
    ${DrawOdds 2_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 2}
    ${DrawOdds 2_app}    Get Element Attribute    ${DrawOdds 2_id}    value
    Should Be Equal As Strings    '${DrawOdds 2_app}'    '${DrawOdds 2_file}'
    ${UpdateTime 2_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 2}
    ${UpdateTime 2_app}    Get Element Attribute    ${UpdateTime 2_id}    value
    Should Be Equal As Strings    '${UpdateTime 2_app}'    '${UpdateTime 2_file}'
    ##Check values of row 3
    ${UpdateTime 3}    Get Value From Json    ${result}    $[sport_events][7][markets_last_updated]
    ${UpdateTime 3}    Catenate    @{UpdateTime 3}
    ${UpdateTime 3_file}    Add Time To Date    ${UpdateTime 3}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 3}    Add Time To Date    ${UpdateTime 3}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 3_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 3}
    ${MatchTime 3_app}    Get Element Attribute    ${MatchTime 3_id}    value
    Should Be Equal As Strings    '${MatchTime 3_app}'    '35''
    ${MatchScore 3_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 3}
    ${MatchScore 3_app}    Get Element Attribute    ${MatchScore 3_id}    value
    Should Be Equal As Strings    '${MatchScore 3_app}'    '1 - 1'
    ${HomeOdds 3_file}    Get Value From Json    ${result}    $[sport_events][7][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 3_file}    Catenate    @{HomeOdds 3_file}
    ${HomeOdds 3_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 3}
    ${HomeOdds 3_app}    Get Element Attribute    ${HomeOdds 3_id}    value
    Should Be Equal As Strings    '${HomeOdds 3_app}'    '${HomeOdds 3_file}'
    ${AwayOdds 3_file}    Get Value From Json    ${result}    $[sport_events][7][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 3_file}    Catenate    @{AwayOdds 3_file}
    ${AwayOdds 3_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 3}
    ${AwayOdds 3_app}    Get Element Attribute    ${AwayOdds 3_id}    value
    Should Be Equal As Strings    '${AwayOdds 3_app}'    '${AwayOdds 3_file}'
    ${DrawOdds 3_file}    Get Value From Json    ${result}    $[sport_events][7][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 3_file}    Catenate    @{DrawOdds 3_file}
    ${DrawOdds 3_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 3}
    ${DrawOdds 3_app}    Get Element Attribute    ${DrawOdds 3_id}    value
    Should Be Equal As Strings    '${DrawOdds 3_app}'    '${DrawOdds 3_file}'
    ${UpdateTime 3_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 3}
    ${UpdateTime 3_app}    Get Element Attribute    ${UpdateTime 3_id}    value
    Should Be Equal As Strings    '${UpdateTime 3_app}'    '${UpdateTime 3_file}'
    ##Check values of row 4
    ${UpdateTime 4}    Get Value From Json    ${result}    $[sport_events][6][markets_last_updated]
    ${UpdateTime 4}    Catenate    @{UpdateTime 4}
    ${UpdateTime 4_file}    Add Time To Date    ${UpdateTime 4}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 4}    Add Time To Date    ${UpdateTime 4}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 4_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 4}
    ${MatchTime 4_app}    Get Element Attribute    ${MatchTime 4_id}    value
    Should Be Equal As Strings    '${MatchTime 4_app}'    '30''
    ${MatchScore 4_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 4}
    ${MatchScore 4_app}    Get Element Attribute    ${MatchScore 4_id}    value
    Should Be Equal As Strings    '${MatchScore 4_app}'    '0 - 1'
    ${HomeOdds 4_file}    Get Value From Json    ${result}    $[sport_events][6][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 4_file}    Catenate    @{HomeOdds 4_file}
    ${HomeOdds 4_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 4}
    ${HomeOdds 4_app}    Get Element Attribute    ${HomeOdds 4_id}    value
    Should Be Equal As Strings    '${HomeOdds 4_app}'    '${HomeOdds 4_file}'
    ${AwayOdds 4_file}    Get Value From Json    ${result}    $[sport_events][6][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 4_file}    Catenate    @{AwayOdds 4_file}
    ${AwayOdds 4_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 4}
    ${AwayOdds 4_app}    Get Element Attribute    ${AwayOdds 4_id}    value
    Should Be Equal As Strings    '${AwayOdds 4_app}'    '${AwayOdds 4_file}'
    ${DrawOdds 4_file}    Get Value From Json    ${result}    $[sport_events][6][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 4_file}    Catenate    @{DrawOdds 4_file}
    ${DrawOdds 4_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 4}
    ${DrawOdds 4_app}    Get Element Attribute    ${DrawOdds 4_id}    value
    Should Be Equal As Strings    '${DrawOdds 4_app}'    '${DrawOdds 4_file}'
    ${UpdateTime 4_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 4}
    ${UpdateTime 4_app}    Get Element Attribute    ${UpdateTime 4_id}    value
    Should Be Equal As Strings    '${UpdateTime 4_app}'    '${UpdateTime 4_file}'
    ##Check values of row 5
    ${UpdateTime 5}    Get Value From Json    ${result}    $[sport_events][5][markets_last_updated]
    ${UpdateTime 5}    Catenate    @{UpdateTime 5}
    ${UpdateTime 5_file}    Add Time To Date    ${UpdateTime 5}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 5}    Add Time To Date    ${UpdateTime 5}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 5_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 5}
    ${MatchTime 5_app}    Get Element Attribute    ${MatchTime 5_id}    value
    Should Be Equal As Strings    '${MatchTime 5_app}'    '20''
    ${MatchScore 5_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 5}
    ${MatchScore 5_app}    Get Element Attribute    ${MatchScore 5_id}    value
    Should Be Equal As Strings    '${MatchScore 5_app}'    '0 - 0'
    ${HomeOdds 5_file}    Get Value From Json    ${result}    $[sport_events][5][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 5_file}    Catenate    @{HomeOdds 5_file}
    ${HomeOdds 5_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 5}
    ${HomeOdds 5_app}    Get Element Attribute    ${HomeOdds 5_id}    value
    Should Be Equal As Strings    '${HomeOdds 5_app}'    '${HomeOdds 5_file}'
    ${AwayOdds 5_file}    Get Value From Json    ${result}    $[sport_events][5][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 5_file}    Catenate    @{AwayOdds 5_file}
    ${AwayOdds 5_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 5}
    ${AwayOdds 5_app}    Get Element Attribute    ${AwayOdds 5_id}    value
    Should Be Equal As Strings    '${AwayOdds 5_app}'    '${AwayOdds 5_file}'
    ${DrawOdds 5_file}    Get Value From Json    ${result}    $[sport_events][5][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 5_file}    Catenate    @{DrawOdds 5_file}
    ${DrawOdds 5_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 5}
    ${DrawOdds 5_app}    Get Element Attribute    ${DrawOdds 5_id}    value
    Should Be Equal As Strings    '${DrawOdds 5_app}'    '${DrawOdds 5_file}'
    ${UpdateTime 5_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 5}
    ${UpdateTime 5_app}    Get Element Attribute    ${UpdateTime 5_id}    value
    Should Be Equal As Strings    '${UpdateTime 5_app}'    '${UpdateTime 5_file}'
    ##Check values of row 6
    ${UpdateTime 6}    Get Value From Json    ${result}    $[sport_events][4][markets_last_updated]
    ${UpdateTime 6}    Catenate    @{UpdateTime 6}
    ${UpdateTime 6_file}    Add Time To Date    ${UpdateTime 6}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 6}    Add Time To Date    ${UpdateTime 6}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 6_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 6}
    ${MatchTime 6_app}    Get Element Attribute    ${MatchTime 6_id}    value
    Should Be Equal As Strings    '${MatchTime 6_app}'    'KO'
    ${MatchScore 6_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 6}
    ${MatchScore 6_app}    Get Element Attribute    ${MatchScore 6_id}    value
    Should Be Equal As Strings    '${MatchScore 6_app}'    '0 - 0'
    ${HomeOdds 6_file}    Get Value From Json    ${result}    $[sport_events][4][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 6_file}    Catenate    @{HomeOdds 6_file}
    ${HomeOdds 6_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 6}
    ${HomeOdds 6_app}    Get Element Attribute    ${HomeOdds 6_id}    value
    Should Be Equal As Strings    '${HomeOdds 6_app}'    '${HomeOdds 6_file}'
    ${AwayOdds 6_file}    Get Value From Json    ${result}    $[sport_events][4][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 6_file}    Catenate    @{AwayOdds 6_file}
    ${AwayOdds 6_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 6}
    ${AwayOdds 6_app}    Get Element Attribute    ${AwayOdds 6_id}    value
    Should Be Equal As Strings    '${AwayOdds 6_app}'    '${AwayOdds 6_file}'
    ${DrawOdds 6_file}    Get Value From Json    ${result}    $[sport_events][4][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 6_file}    Catenate    @{DrawOdds 6_file}
    ${DrawOdds 6_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 6}
    ${DrawOdds 6_app}    Get Element Attribute    ${DrawOdds 6_id}    value
    Should Be Equal As Strings    '${DrawOdds 6_app}'    '${DrawOdds 6_file}'
    ${UpdateTime 6_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 6}
    ${UpdateTime 6_app}    Get Element Attribute    ${UpdateTime 6_id}    value
    Should Be Equal As Strings    '${UpdateTime 6_app}'    '${UpdateTime 6_file}'
    ##Check values of row 7
    ${UpdateTime 7}    Get Value From Json    ${result}    $[sport_events][3][markets_last_updated]
    ${UpdateTime 7}    Catenate    @{UpdateTime 7}
    ${UpdateTime 7_file}    Add Time To Date    ${UpdateTime 7}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 7}    Add Time To Date    ${UpdateTime 7}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 7_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 7}
    ${MatchTime 7_app}    Get Element Attribute    ${MatchTime 7_id}    value
    Should Be Equal As Strings    '${MatchTime 7_app}'    'Live'
    ${MatchScore 7_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 7}
    Page Should Contain Element    ${MatchScore 7_id}
    ${HomeOdds 7_file}    Get Value From Json    ${result}    $[sport_events][3][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 7_file}    Catenate    @{HomeOdds 7_file}
    ${HomeOdds 7_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 7}
    ${HomeOdds 7_app}    Get Element Attribute    ${HomeOdds 7_id}    value
    Should Be Equal As Strings    '${HomeOdds 7_app}'    '${HomeOdds 7_file}'
    ${AwayOdds 7_file}    Get Value From Json    ${result}    $[sport_events][3][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 7_file}    Catenate    @{AwayOdds 7_file}
    ${AwayOdds 7_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 7}
    ${AwayOdds 7_app}    Get Element Attribute    ${AwayOdds 7_id}    value
    Should Be Equal As Strings    '${AwayOdds 7_app}'    '${AwayOdds 7_file}'
    ${DrawOdds 7_file}    Get Value From Json    ${result}    $[sport_events][3][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 7_file}    Catenate    @{DrawOdds 7_file}
    ${DrawOdds 7_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 7}
    ${DrawOdds 7_app}    Get Element Attribute    ${DrawOdds 7_id}    value
    Should Be Equal As Strings    '${DrawOdds 7_app}'    '${DrawOdds 7_file}'
    ${UpdateTime 7_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 7}
    ${UpdateTime 7_app}    Get Element Attribute    ${UpdateTime 7_id}    value
    Should Be Equal As Strings    '${UpdateTime 7_app}'    '${UpdateTime 7_file}'
    ##Check values of row 8
    ${UpdateTime 8}    Get Value From Json    ${result}    $[sport_events][2][markets_last_updated]
    ${UpdateTime 8}    Catenate    @{UpdateTime 8}
    ${UpdateTime 8_file}    Add Time To Date    ${UpdateTime 8}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 8}    Add Time To Date    ${UpdateTime 8}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 8_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 8}
    ${MatchTime 8_app}    Get Element Attribute    ${MatchTime 8_id}    value
    Should Be Equal As Strings    '${MatchTime 8_app}'    'Live'
    ${MatchScore 8_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 8}
    Page Should Contain Element    ${MatchScore 8_id}
    ${HomeOdds 8_file}    Get Value From Json    ${result}    $[sport_events][2][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 8_file}    Catenate    @{HomeOdds 8_file}
    ${HomeOdds 8_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 8}
    ${HomeOdds 8_app}    Get Element Attribute    ${HomeOdds 8_id}    value
    Should Be Equal As Strings    '${HomeOdds 8_app}'    '${HomeOdds 8_file}'
    ${AwayOdds 8_file}    Get Value From Json    ${result}    $[sport_events][2][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 8_file}    Catenate    @{AwayOdds 8_file}
    ${AwayOdds 8_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 8}
    ${AwayOdds 8_app}    Get Element Attribute    ${AwayOdds 8_id}    value
    Should Be Equal As Strings    '${AwayOdds 8_app}'    '${AwayOdds 8_file}'
    ${DrawOdds 8_file}    Get Value From Json    ${result}    $[sport_events][2][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 8_file}    Catenate    @{DrawOdds 8_file}
    ${DrawOdds 8_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 8}
    ${DrawOdds 8_app}    Get Element Attribute    ${DrawOdds 8_id}    value
    Should Be Equal As Strings    '${DrawOdds 8_app}'    '${DrawOdds 8_file}'
    ${UpdateTime 8_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 8}
    ${UpdateTime 8_app}    Get Element Attribute    ${UpdateTime 8_id}    value
    Should Be Equal As Strings    '${UpdateTime 8_app}'    '${UpdateTime 8_file}'
    ##Check values of row 9
    ${UpdateTime 9}    Get Value From Json    ${result}    $[sport_events][1][markets_last_updated]
    ${UpdateTime 9}    Catenate    @{UpdateTime 9}
    ${UpdateTime 9_file}    Add Time To Date    ${UpdateTime 9}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 9}    Add Time To Date    ${UpdateTime 9}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 9_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 9}
    ${MatchTime 9_app}    Get Element Attribute    ${MatchTime 9_id}    value
    Should Be Equal As Strings    '${MatchTime 9_app}'    'Live'
    ${MatchScore 9_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 9}
    Page Should Contain Element    ${MatchScore 9_id}
    ${HomeOdds 9_file}    Get Value From Json    ${result}    $[sport_events][1][markets][0][books][0][outcomes][0][odds]
    ${HomeOdds 9_file}    Catenate    @{HomeOdds 9_file}
    ${HomeOdds 9_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 9}
    ${HomeOdds 9_app}    Get Element Attribute    ${HomeOdds 9_id}    value
    Should Be Equal As Strings    '${HomeOdds 9_app}'    '${HomeOdds 9_file}'
    ${AwayOdds 9_file}    Get Value From Json    ${result}    $[sport_events][1][markets][0][books][0][outcomes][1][odds]
    ${AwayOdds 9_file}    Catenate    @{AwayOdds 9_file}
    ${AwayOdds 9_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 9}
    ${AwayOdds 9_app}    Get Element Attribute    ${AwayOdds 9_id}    value
    Should Be Equal As Strings    '${AwayOdds 9_app}'    '${AwayOdds 9_file}'
    ${DrawOdds 9_file}    Get Value From Json    ${result}    $[sport_events][1][markets][0][books][0][outcomes][2][odds]
    ${DrawOdds 9_file}    Catenate    @{DrawOdds 9_file}
    ${DrawOdds 9_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 9}
    ${DrawOdds 9_app}    Get Element Attribute    ${DrawOdds 9_id}    value
    Should Be Equal As Strings    '${DrawOdds 9_app}'    '${DrawOdds 9_file}'
    ${UpdateTime 9_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 9}
    ${UpdateTime 9_app}    Get Element Attribute    ${UpdateTime 9_id}    value
    Should Be Equal As Strings    '${UpdateTime 9_app}'    '${UpdateTime 9_file}'
    ##Check values of row 10
    ${UpdateTime 10}    Get Value From Json    ${result}    $[sport_events][0][markets_last_updated]
    ${UpdateTime 10}    Catenate    @{UpdateTime 10}
    ${UpdateTime 10_file}    Add Time To Date    ${UpdateTime 10}    7 hours    result_format=%d-%m %H:%M    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${UpdateTime 10}    Add Time To Date    ${UpdateTime 10}    7 hours    result_format=%Y-%d-%m %H:%M:%S    date_format=%Y-%m-%dT%H:%M:%S+00:00
    ${MatchTime 10_id}    Catenate    SEPARATOR=    MatchTime-    ${UpdateTime 10}
    ${MatchTime 10_app}    Get Element Attribute    ${MatchTime 10_id}    value
    Should Be Equal As Strings    '${MatchTime 10_app}'    'Opening'
    ${MatchScore 10_id}    Catenate    SEPARATOR=    MatchScore-    ${UpdateTime 10}
    Page Should Contain Element    ${MatchScore 10_id}
    ${HomeOdds 10_file}    Get Value From Json    ${result}    $[sport_events][0][markets][0][books][0][outcomes][0][opening_odds]
    ${HomeOdds 10_file}    Catenate    @{HomeOdds 10_file}
    ${HomeOdds 10_id}    Catenate    SEPARATOR=    HomeOdds-    ${UpdateTime 10}
    ${HomeOdds 10_app}    Get Element Attribute    ${HomeOdds 10_id}    value
    Should Be Equal As Strings    '${HomeOdds 10_app}'    '${HomeOdds 10_file}'
    ${AwayOdds 10_file}    Get Value From Json    ${result}    $[sport_events][0][markets][0][books][0][outcomes][1][opening_odds]
    ${AwayOdds 10_file}    Catenate    @{AwayOdds 10_file}
    ${AwayOdds 10_id}    Catenate    SEPARATOR=    AwayOdds-    ${UpdateTime 10}
    ${AwayOdds 10_app}    Get Element Attribute    ${AwayOdds 10_id}    value
    Should Be Equal As Strings    '${AwayOdds 10_app}'    '${AwayOdds 10_file}'
    ${DrawOdds 10_file}    Get Value From Json    ${result}    $[sport_events][0][markets][0][books][0][outcomes][2][opening_odds]
    ${DrawOdds 10_file}    Catenate    @{DrawOdds 10_file}
    ${DrawOdds 10_id}    Catenate    SEPARATOR=    DrawOdds-    ${UpdateTime 10}
    ${DrawOdds 10_app}    Get Element Attribute    ${DrawOdds 10_id}    value
    Should Be Equal As Strings    '${DrawOdds 10_app}'    '${DrawOdds 10_file}'
    ${UpdateTime 10_id}    Catenate    SEPARATOR=    UpdateTime-    ${UpdateTime 10}
    ${UpdateTime 10_app}    Get Element Attribute    ${UpdateTime 10_id}    value
    Should Be Equal As Strings    '${UpdateTime 10_app}'    '${UpdateTime 10_file}'
    Remove File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt

*** Keywords ***
