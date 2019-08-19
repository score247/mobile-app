*** Settings ***
Library           AppiumLibrary
Library           Process
Library           Collections
Library           DateTime
Library           JSONLibrary
Library           OperatingSystem
Library           String
Library           REST
Library           RequestsLibrary

*** Variables ***
${Push_File}      http://ha.nexdev.net:7206/V3/api/soccer-xt3/other/stream/events/PushEvents
${Push_odds}      http://ha.nexdev.net:7206/V3/ScheduleOdds
${cf_timeout}     10s

*** Keywords ***
Start Appium Server
    Start Process    /usr/local/bin/appium --session-override    shell=True
    Sleep    10s

Open Application On Real Ios Device
    [Arguments]    ${deviceName}    ${udid}
    Open Application    http://0.0.0.0:4723/wd/hub    platformName=iOS    platformVersion=12.4    deviceName=${deviceName}    bundleId=Score247.LiveScore    udid=${udid}
    ...    xcodeOrgId=FFHZ4F8L88    xcodeSigningId=iPhone Developer    newCommandTimeout=1500    usePrebuiltWDA=false    newCommandTimeout=120
    sleep    3s

Open Application On Simulator
    [Arguments]    ${deviceName}
    Open Application    http://0.0.0.0:4723/wd/hub    platformName=iOS    platformVersion=12.4    deviceName=${deviceName}    bundleId=Score247.LiveScore    newCommandTimeout=120
    sleep    3s

Init_Simulator
    [Arguments]    ${simulator_name}
    Start Appium Server
    Insert_Matches
    ###    Post data for tcs SP3-SP4
    Update_Template_List_Event_Of_Match1
    Push_Event    ${EXECDIR}/Template_Files/Run/List_event_data_template1.txt
    Push_Event    ${EXECDIR}/Template_Files/List_event_data_template2.txt
    Push_Event    ${EXECDIR}/Template_Files/List_event_data_template3.txt
    Push_Event    ${EXECDIR}/Template_Files/List_event_data_template4.txt
    Push_Odds_For_PostMatch
    ###    Complete push data test
    Open Application On Simulator    ${simulator_name}

Init_Real Device
    [Arguments]    ${device_name}    ${udid}
    Start Appium Server
    Open Application On Real Ios Device    ${device_name}    ${udid}

Suite TearDown
    Comment    Start Process    /usr/bin/pkill -f node|grep appium    shell=True
    Comment    Clean_Up_DB

Insert_Matches
    # Post file to insert matches
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    ${date1}=    Subtract Time From Date    ${current_date}    3 days    result_format=%Y-%m-%d    #3rd date before current date
    ${date2}=    Subtract Time From Date    ${current_date}    2 days    result_format=%Y-%m-%d    #2nd date before current date
    ${date3}=    Subtract Time From Date    ${current_date}    1 days    result_format=%Y-%m-%d    #1st date before current date
    ${date5}=    Add Time To Date    ${current_date}    1 days    result_format=%Y-%m-%d    #1st date after current date
    ${date6}=    Add Time To Date    ${current_date}    2 days    result_format=%Y-%m-%d    #2nd date after current date
    ${date7}=    Add Time To Date    ${current_date}    3 days    result_format=%Y-%m-%d    #3rd date after current date
    ${result_pre_match}=    Load JSON From File    Template_Files/Template_pre_match_final.txt
    ${match_date5}    Get Value From Json    ${result_pre_match}    $[0]
    REST.Post    http://ha.nexdev.net:7206/V3/DailySchedule?date=${date5}    @{match_date5}
    Integer    response status    200
    ${match_date6}    Get Value From Json    ${result_pre_match}    $[1]
    REST.Post    http://ha.nexdev.net:7206/V3/DailySchedule?date=${date6}    @{match_date6}
    Integer    response status    200
    ${match_date7}    Get Value From Json    ${result_pre_match}    $[2]
    REST.Post    http://ha.nexdev.net:7206/V3/DailySchedule?date=${date7}    @{match_date7}
    Integer    response status    200
    ${match_current_date}    Get Value From Json    ${result_pre_match}    $[3]
    REST.Post    http://ha.nexdev.net:7206/V3/DailySchedule?date=${current_date}    @{match_current_date}
    Integer    response status    200
    ${result_post_match}=    Load JSON From File    Template_Files/Template_post_match_final.txt
    ${match_date1}    Get Value From Json    ${result_post_match}    $[0]
    REST.Post    http://ha.nexdev.net:7206/V3/DailyResults?date=${date1}    @{match_date1}
    Integer    response status    200
    ${match_date2}    Get Value From Json    ${result_post_match}    $[1]
    REST.Post    http://ha.nexdev.net:7206/V3/DailyResults?date=${date2}    @{match_date2}
    Integer    response status    200
    ${match_date3}    Get Value From Json    ${result_post_match}    $[2]
    REST.Post    http://ha.nexdev.net:7206/V3/DailyResults?date=${date3}    @{match_date3}
    Output
    Integer    response status    200
    Sleep    5
    # Fetch jobs to have matches in DB
    ${headers}    create dictionary    Content-Type=charset=UTF-8    Content-Type=application/x-www-form-urlencoded
    Create session    AA    http://ha.nexdev.net:7874
    ${data1}    create dictionary    jobs[]=FetchPostMatch
    ${b1}    Post request    AA    /hangfire/recurring/trigger    headers=${headers}    data=${data1}
    ${data2}    create dictionary    jobs[]=FetchPreMatch
    ${b2}    Post request    AA    /hangfire/recurring/trigger    headers=${headers}    data=${data2}

Update_Template_List_Event_Of_Match1
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    #Update date for list event of match 1
    Copy File    Template_Files/List_event_data_template1.txt    Template_Files/Run/List_event_data_template1.txt
    @{list_date_match1_1}=    Create List    2019-07-01
    @{list_date_match1_2}=    Create List    ${current_date}
    : FOR    ${i}    IN RANGE    0    1
    \    run    sed -i’’ -e s/@{list_date_match1_1}[${i}]/@{list_date_match1_2}[${i}]/ Template_Files/Run/List_event_data_template1.txt
    ${match1_event1}=    Get File    Template_Files/Run/List_event_data_template1.txt

Update_Template_Odds_Movement_Of_Match1
    ${current_date}=    Get Current Date    result_format=%Y-%m-%d    #current date
    #Update date for odds movement of match 1 - bettype 1x2
    Copy File    Template_Files/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt
    @{list_date_odds_1}=    Create List    2019-07-01
    @{list_date_odds_2}=    Create List    ${current_date}
    : FOR    ${i}    IN RANGE    0    1
    \    run    sed -i’’ -e s/@{list_date_odds_1}[${i}]/@{list_date_odds_2}[${i}]/ Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt
    ${match1_event1}=    Get File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt
    #Update date for odds movement of match 1 - bettype asian_handicap
    Copy File    Template_Files/Template_Odds_Movement_Of_Match1_Bettype_asian_handicap.txt    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_asian_handicap.txt
    : FOR    ${i}    IN RANGE    0    1
    \    run    sed -i’’ -e s/@{list_date_odds_1}[${i}]/@{list_date_odds_2}[${i}]/ Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_asian_handicap.txt
    ${match1_event2}=    Get File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_asian_handicap.txt
    #Update date for odds movement of match 1 - bettype over_under
    Copy File    Template_Files/Template_Odds_Movement_Of_Match1_Bettype_over_under.txt    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_over_under.txt
    : FOR    ${i}    IN RANGE    0    1
    \    run    sed -i’’ -e s/@{list_date_odds_1}[${i}]/@{list_date_odds_2}[${i}]/ Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_over_under.txt
    ${match1_event3}=    Get File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_over_under.txt

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
    REST.Post    ${Push_odds}    ${json}
    Integer    response status    200
    Sleep    3
    #Push Odds for HDP
    Update_Template_Odds_AsianHdp
    ${json_old}=    Get File    Template_Files/Run/Data_Odds_AsianHDP.json
    #Push Odds hdp to DB
    REST.Post    ${Push_odds}    ${json_old}
    Integer    response status    200
    Sleep    3
    ${json1}=    Get File    Template_Files/Run/Data_Odds_AsianHDP_live.json
    #Push Odds hdp to DB
    REST.Post    ${Push_odds}    ${json1}
    Integer    response status    200
    Sleep    3
    #Push Odds_OU
    Update_Template_Odds_Over_Under
    ${json2}=    Get File    Template_Files/Run/Data_Odds_Over_Under_auto.json
    #Push Odds OU to DB
    REST.Post    ${Push_odds}    ${json2}
    Integer    response status    200
    Sleep    3
    Update_Template_Odds_Movement_Of_Match1
    ${file1}=    Get File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_1x2.txt
    ${file2}=    Get File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_asian_handicap.txt
    ${file3}=    Get File    Template_Files/Run/Template_Odds_Movement_Of_Match1_Bettype_over_under.txt
    #Push events to insert odds for bettype 1x2
    REST.Post    ${Push_odds}    ${file1}
    Integer    response status    200
    #Push events to insert odds for bettype asian_handicap
    REST.Post    ${Push_odds}    ${file2}
    Integer    response status    200
    #Push events to insert odds for bettype over/under
    REST.Post    ${Push_odds}    ${file3}
    Integer    response status    200
    # Fetch jobs to have odds in DB
    ${headers}    create dictionary    Content-Type=charset=UTF-8    Content-Type=application/x-www-form-urlencoded
    Create session    AA    http://ha.nexdev.net:7874
    ${data}    create dictionary    jobs[]=FetchOdds
    ${b}    Post request    AA    /hangfire/recurring/trigger    headers=${headers}    data=${data}

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
    ${ac_event}    REST.Post    ${Push_File}    ${file}
    ${event_status}=    Run Keyword and return status    Integer    response status    200
    run keyword if    '${event_status}' != 'True'    log    PUSH DATA IS FAILED AT THE FIRST TIME
    run keyword if    '${event_status}' != 'True'    REST.Post    ${Push_File}    ${file}

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
