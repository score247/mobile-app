*** Settings ***
Library           AppiumLibrary
Library           Process
Library           Collections
Library           DateTime
Library           JSONLibrary
Library           OperatingSystem
Library           String
Library           DatabaseLibrary
Library           PostgreSQLDB
Library           REST

*** Variables ***
${database}       Score247.AutomationTest
${user}           postgres
${password}       1234aa
${host}           10.18.200.110
${port}           5444
${Push_File}      https://api.nexdev.net/V4/api/Mock/PushMatchEvents
${Push_odds}      https://api.nexdev.net/V4/api/Mock/InsertOdds?forceInsert=true
${cf_timeout}     10s

*** Keywords ***
Start Appium Server
    Start Process    /usr/local/bin/appium --session-override    shell=True
    Sleep    10s

Open Application On Real Ios Device
    [Arguments]    ${deviceName}    ${udid}
    Open Application    http://0.0.0.0:4723/wd/hub    platformName=iOS    platformVersion=12.2    deviceName=${deviceName}    bundleId=Score247.LiveScore    udid=${udid}
    ...    xcodeOrgId=FFHZ4F8L88    xcodeSigningId=iPhone Developer    newCommandTimeout=1500    usePrebuiltWDA=false    newCommandTimeout=120
    sleep    3s

Open Application On Simulator
    [Arguments]    ${deviceName}
    Open Application    http://0.0.0.0:4723/wd/hub    platformName=iOS    platformVersion=12.2    deviceName=${deviceName}    bundleId=Score247.LiveScore    newCommandTimeout=120
    sleep    3s

Init_Simulator
    [Arguments]    ${simulator_name}
    Comment    Start Appium Server
    Insert_Matches
    Push_Odds_For_PostMatch
    ###    Post data for tcs SP3-SP4
    Update_Template_List_Event_Of_Match1
    Push_Event    ${EXECDIR}/Template_Files/Run/List_event_data_template1.txt
    Push_Event    ${EXECDIR}/Template_Files/List_event_data_template2.txt
    Push_Event    ${EXECDIR}/Template_Files/List_event_data_template3.txt
    Push_Event    ${EXECDIR}/Template_Files/List_event_data_template4.txt
    ###    Complete push data test
    Open Application On Simulator    ${simulator_name}

Init_Real Device
    [Arguments]    ${device_name}    ${udid}
    Start Appium Server
    Open Application On Real Ios Device    ${device_name}    ${udid}

Suite TearDown
    Comment    Start Process    /usr/bin/pkill -f node|grep appium    shell=True
    Comment    Clean_Up_DB
    Comment    Empty Directory    Template_Files/Run

Insert_Matches
    Clean_Up_DB
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date1}=    Subtract Time From Date    ${current_date}    3 days    result_format=%Y-%m-%d    #3rd date before current date
    ${date2}=    Subtract Time From Date    ${current_date}    2 days    result_format=%Y-%m-%d    #2nd date before current date
    ${date3}=    Subtract Time From Date    ${current_date}    1 days    result_format=%Y-%m-%d    #1st date before current date
    ${date5}=    Add Time To Date    ${current_date}    1 days    result_format=%Y-%m-%d    #1st date after current date
    ${date6}=    Add Time To Date    ${current_date}    2 days    result_format=%Y-%m-%d    #2nd date after current date
    ${date7}=    Add Time To Date    ${current_date}    3 days    result_format=%Y-%m-%d    #3rd date after current date
    #Update date for list match
    Copy File    Template_Files/template_post_pre_match.txt    Template_Files/Run/template_post_pre_match.txt
    ${match_list}=    Get File    Template_Files/Run/template_post_pre_match.txt
    @{list1}=    Create List    2019-05-01    2019-05-02    2019-05-03    2019-05-04    2019-05-05
    ...    2019-05-06    2019-05-07
    @{list2}=    Create List    ${date1}    ${date2}    ${date3}    ${current_date}    ${date5}
    ...    ${date6}    ${date7}
    : FOR    ${i}    IN RANGE    0    7
    \    run    sed -i’’ -e s/@{list1}[${i}]/@{list2}[${i}]/ Template_Files/Run/template_post_pre_match.txt
    ${match_list1}=    Get File    Template_Files/Run/template_post_pre_match.txt
    #Connect DB to insert match
    PostgreSQLDB.Connect To Postgresql    ${database}    ${user}    ${password}    ${host}    ${port}
    ${create_match_table}=    PostgreSQLDB.Execute Plpgsql Script    Template_Files/Create_Table_Match.sql
    ${script_insert_match}=    PostgreSQLDB.Execute Plpgsql Script    Template_Files/script_insert_match.sql
    PostgreSQLDB.Execute Plpgsql Block    CALL public.insert_match('${match_list1}')
    Remove File    Template_Files/Run/template_post_pre_match.txt

Clean_Up_DB
    Connect To Database    psycopg2    ${database}    ${user}    ${password}    ${host}    ${port}
    DatabaseLibrary.Execute Sql String    DELETE FROM "Match"
    DatabaseLibrary.Execute Sql String    DELETE FROM "LiveMatch"
    DatabaseLibrary.Execute Sql String    DELETE FROM "Timeline"
    DatabaseLibrary.Execute Sql String    DELETE FROM "MatchResult"
    DatabaseLibrary.Execute Sql String    DELETE FROM "Odds"

Update_Template_List_Event_Of_Match1
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    #Update date for list event of match 1
    Copy File    Template_Files/List_event_data_template1.txt    Template_Files/Run/List_event_data_template1.txt
    @{list_date_match1_1}=    Create List    2019-07-01
    @{list_date_match1_2}=    Create List    ${current_date}
    : FOR    ${i}    IN RANGE    0    1
    \    run    sed -i’’ -e s/@{list_date_match1_1}[${i}]/@{list_date_match1_2}[${i}]/ Template_Files/Run/List_event_data_template1.txt
    ${match1_event1}=    Get File    Template_Files/Run/List_event_data_template1.txt

Update_Template_Odds_Movement_Of_Match1_Bettype_1x2
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    #Update date for odds movement of match 1 - bettype 1x2
    Copy File    Template_Files/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt
    @{list_date_odds_1}=    Create List    2019-07-01
    @{list_date_odds_2}=    Create List    ${current_date}
    : FOR    ${i}    IN RANGE    0    1
    \    run    sed -i’’ -e s/@{list_date_odds_1}[${i}]/@{list_date_odds_2}[${i}]/ Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt
    ${match1_event1}=    Get File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt

Get date from value
    [Arguments]    ${value}
    ${ac_date}    Get Current Date    UTC    ${value} days    result_format=%d
    ${ac_month}    Get Current Date    UTC    ${value} days    result_format=%m
    [Return]    ${ac_date}    ${ac_month}

Return day month
    [Arguments]    ${day}    ${month}    # format of return value will be: DD_Month Name (07 Jul)
    @{MONTHS}    Create List    NONE    Jan    Feb    Mar    Apr
    ...    May    Jun    Jul    Aug    Sep    Oct
    ...    Nov    Dec
    ${day_monthname}    Set Variable    @{MONTHS}[${month}]
    ${day_monthname}    Set Variable    ${day}${space}${day_monthname}
    [Return]    ${day_monthname}

Push_Odds_For_PostMatch
    #Push Odds for 1x2
    Update_Template_Odds_1x2
    ${json}=    Get File    Template_Files/Run/Data_Odds_1x2_auto.json
    Post    ${Push_odds}    ${json}
    Integer    response status    200
    Sleep    3
    #Push Odds for HDP
    Update_Template_Odds_AsianHdp
    ${json_old}=    Get File    Template_Files/Run/Data_Odds_AsianHDP.json
    #Push Odds hdp to DB
    Post    ${Push_odds}    ${json_old}
    Integer    response status    200
    Sleep    3
    ${json1}=    Get File    Template_Files/Run/Data_Odds_AsianHDP_live.json
    #Push Odds hdp to DB
    Post    ${Push_odds}    ${json1}
    Integer    response status    200
    Sleep    3
    #Push Odds_OU
    Update_Template_Odds_Over_Under
    ${json2}=    Get File    Template_Files/Run/Data_Odds_Over_Under_auto.json
    #Push Odds hdp to DB
    Post    ${Push_odds}    ${json2}
    Integer    response status    200
    Sleep    3
    Update_Template_Odds_Movement_Of_Match1_Bettype_1x2
    ${file}=    Get File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt
    #Push events to insert odds
    Post    ${Push_odds}    ${file}
    Integer    response status    200

Get_Value_Number
    [Arguments]    ${json_file}    ${json_path}
    ${Return_value}=    Get Value From Json    ${json_file}    ${json_path}
    ${Return_value}    BuiltIn.Catenate    @{Return_value}
    ${Return_value}=    Builtin.Convert To Number    ${Return_value}    2
    ${Return_value}=    Convert To String    ${Return_value}
    [Return]    ${Return_value}

Update_Template_Odds_1x2
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    #Update date update_time for push odds
    Copy File    Template_Files/Data_Odds_1x2_auto.json    Template_Files/Run/Data_Odds_1x2_auto.json
    @{list_date_odds_1}=    Create List    2019-07-01
    @{list_date_odds_2}=    Create List    ${current_date}
    : FOR    ${i}    IN RANGE    0    1
    \    run    sed -i’’ -e s/@{list_date_odds_1}[${i}]/@{list_date_odds_2}[${i}]/ Template_Files/Run/Data_Odds_1x2_auto.json
    ${match1_event1}=    Get File    Template_Files/Run/Data_Odds_1x2_auto.json

GetOdds_1x2
    [Arguments]    ${json_file}    ${Bookmaker_name}
    ${path_live_Odd_1}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${Bookmaker_name}    ')].outcomes.[0].odds
    ${path_live_Odd_draw}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${Bookmaker_name}    ')].outcomes.[2].odds
    ${path_live_Odd_2}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${Bookmaker_name}    ')].outcomes.[1].odds
    ${path_open_Odd_1}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${Bookmaker_name}    ')].outcomes.[0].opening_odds
    ${path_open_Odd_draw}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${Bookmaker_name}    ')].outcomes.[2].opening_odds
    ${path_open_Odd_2}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${Bookmaker_name}    ')].outcomes.[1].opening_odds
    ${bk_live_1}=    Get_Value_Number    ${json_file}    ${path_live_Odd_1}
    ${bk_live_X}=    Get_Value_Number    ${json_file}    ${path_live_Odd_draw}
    ${bk_live_2}=    Get_Value_Number    ${json_file}    ${path_live_Odd_2}
    ${bk_open_1}=    Get_Value_Number    ${json_file}    ${path_open_Odd_1}
    ${bk_open_X}=    Get_Value_Number    ${json_file}    ${path_open_Odd_draw}
    ${bk_open_2}=    Get_Value_Number    ${json_file}    ${path_open_Odd_2}
    ${Odds_infor_1x2}=    Create List    ${Bookmaker_name}    ${bk_live_1}    ${bk_live_X}    ${bk_live_2}    ${bk_open_1}
    ...    ${bk_open_X}    ${bk_open_2}
    [Return]    ${Odds_infor_1x2}

Update_Template_Odds_AsianHdp
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    #Update date for Asian_Hdp_1st
    Copy File    Template_Files/Data_Odds_AsianHDP.json    Template_Files/Run/Data_Odds_AsianHDP.json
    @{list_date_odds_1}=    Create List    2019-07-01
    @{list_date_odds_2}=    Create List    ${current_date}
    : FOR    ${i}    IN RANGE    0    1
    \    run    sed -i’’ -e s/@{list_date_odds_1}[${i}]/@{list_date_odds_2}[${i}]/ Template_Files/Run/Data_Odds_AsianHDP.json
    ${match1_event1}=    Get File    Template_Files/Run/Data_Odds_AsianHDP.json
    #Update date for Asian_Hdp_2nd
    Copy File    Template_Files/Data_Odds_AsianHDP_live.json    Template_Files/Run/Data_Odds_AsianHDP_live.json
    : FOR    ${j}    IN RANGE    0    1
    \    run    sed -i’’ -e s/@{list_date_odds_1}[${j}]/@{list_date_odds_2}[${j}]/ Template_Files/Run/Data_Odds_AsianHDP_live.json
    ${match1_event2}=    Get File    Template_Files/Run/Data_Odds_AsianHDP_live.json

Get_Odds_HDP
    [Arguments]    ${json_file}    ${bookmaker_name}
    Comment    ${json_file}=    Load_File_Json    ${address_file}
    ${path_odd_home}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${bookmaker_name}    ')].outcomes.[0].odds
    ${path_odd_away}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${bookmaker_name}    ')].outcomes.[1].odds
    ${path_handicap}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${bookmaker_name}    ')].outcomes.[0].handicap
    ${bk_odds_home}=    Get_Value_Number    ${json_file}    ${path_odd_home}
    ${bk_odds_away}=    Get_Value_Number    ${json_file}    ${path_odd_away}
    ${bk_handicap}=    Get_Value_Number    ${json_file}    ${path_handicap}
    ${Odds_infor}=    Create List    ${bookmaker_name}    ${bk_odds_home}    ${bk_handicap}    ${bk_odds_away}
    [Return]    ${Odds_infor}

Update_Template_Odds_Over_Under
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    #Update date update_time for push odds
    Copy File    Template_Files/Data_Odds_Over_Under_auto.json    Template_Files/Run/Data_Odds_Over_Under_auto.json
    @{list_date_odds_1}=    Create List    2019-07-01
    @{list_date_odds_2}=    Create List    ${current_date}
    : FOR    ${i}    IN RANGE    0    1
    \    run    sed -i’’ -e s/@{list_date_odds_1}[${i}]/@{list_date_odds_2}[${i}]/ Template_Files/Run/Data_Odds_Over_Under_auto.json
    ${match1_event1}=    Get File    Template_Files/Run/Data_Odds_Over_Under_auto.json

GetOdds_Over_Under
    [Arguments]    ${json_file}    ${bookmaker_name_OU}
    ${path_live_over}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${bookmaker_name_OU}    ')].outcomes.[0].odds
    ${path_live_under}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${bookmaker_name_OU}    ')].outcomes.[1].odds
    ${path_live_value_OU}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${bookmaker_name_OU}    ')].outcomes.[0].total
    ${bk_live_over}=    Get_Value_Number    ${json_file}    ${path_live_over}
    ${bk_live_under}=    Get_Value_Number    ${json_file}    ${path_live_under}
    ${bk_live_value_OU}=    Get_Value_Number    ${json_file}    ${path_live_value_OU}
    ${path_opening_over}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${bookmaker_name_OU}    ')].outcomes.[0].opening_odds
    ${path_opening_under}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${bookmaker_name_OU}    ')].outcomes.[1].opening_odds
    ${path_opening_value_OU}=    BuiltIn.Catenate    SEPARATOR=    $..books[?(@.name=='    ${bookmaker_name_OU}    ')].outcomes.[0].opening_total
    ${bk_opening_over}=    Get_Value_Number    ${json_file}    ${path_opening_over}
    ${bk_opening_under}=    Get_Value_Number    ${json_file}    ${path_opening_under}
    ${bk_opening_value_OU}=    Get_Value_Number    ${json_file}    ${path_opening_value_OU}
    ${OU_Odds_infor}=    Create List    ${bookmaker_name_OU}    ${bk_live_over}    ${bk_live_value_OU}    ${bk_live_under}    ${bk_opening_over}
    ...    ${bk_opening_value_OU}    ${bk_opening_under}
    [Return]    ${OU_Odds_infor}

Push_Event
    [Arguments]    ${dir_pushfile}
    ${file}=    Get File    ${dir_pushfile}
    #Push events
    ${ac_event}    Post    ${Push_File}    ${file}
    ${event_status}=    Run Keyword and return status    Integer    response status    200
    run keyword if    '${event_status}' != 'True'    log    PUSH DATA IS FAILED AT THE FIRST TIME
    run keyword if    '${event_status}' != 'True'    Post    ${Push_File}    ${file}

Click Control
    [Arguments]    ${locator}
    [Documentation]    This action is use to click on element with waiting for element visible
    wait until element is visible    ${locator}    ${cf_timeout}    Can not found element after timeout
    click element    ${locator}

Swipe Down
    [Arguments]    ${locator}
    [Documentation]    This action is use to wipe down from locator. Support for case auto test need to refresh data by pulling down
    ${ac_element_locator_dic}    get element location    ${locator}
    ${ac_element_locator}    get dictionary values    ${ac_element_locator_dic}
    ${ac_element_y}    evaluate    ${ac_element_locator}[0]+30
    swipe by percent    ${ac_element_locator}[0]    ${ac_element_locator}[1]    ${ac_element_y}    ${ac_element_locator}[1]
