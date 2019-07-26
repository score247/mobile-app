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

*** Variables ***
${database}       Score247.AutomationTest
${user}           postgres
${password}       1234aa
${host}           10.18.200.110
${port}           5444
${Push_File}      https://api.nexdev.net/V4/api/Mock/PushMatchEvents
${Push_odds}      https://api.nexdev.net/V4/api/Mock/InsertOdds?forceInsert=true

*** Keywords ***
Start Appium Server
    Start Process    /usr/local/bin/appium --session-override    shell=True
    Sleep    10s

Open Application On Real Ios Device
    [Arguments]    ${deviceName}    ${udid}
    Open Application    http://0.0.0.0:4723/wd/hub    platformName=iOS    platformVersion=12.2    deviceName=${deviceName}    bundleId=Score247.LiveScore    udid=${udid}
    ...    xcodeOrgId=FFHZ4F8L88    xcodeSigningId=iPhone Developer    newCommandTimeout=1500    usePrebuiltWDA=false    newCommandTimeout=120    #
    ...    # ${EMPTY}    # FFHZ4F8L88    iPhone Developer"    updatedWDABundleId=WebDriverAgentRunner.WebDriverAgentRunner
    sleep    3s    \    #    deviceName=Iphone6    udid=34a775db8a3839d4651f0f066d28675b6756623a

Open Application On Simulator
    [Arguments]    ${deviceName}
    Open Application    http://0.0.0.0:4725/wd/hub    platformName=iOS    platformVersion=12.2    deviceName=${deviceName}    bundleId=Score247.LiveScore    newCommandTimeout=120
    sleep    3s

Init_Simulator
    [Arguments]    ${simulator_name}
    Comment    Start Appium Server
    Insert_Matches
    Open Application On Simulator    ${simulator_name}

Init_Real Device
    [Arguments]    ${device_name}    ${udid}
    Start Appium Server
    Open Application On Real Ios Device    ${device_name}    ${udid}

Suite TearDown
    Comment    Start Process    /usr/bin/pkill -f node|grep appium    shell=True
    Comment    Clean_Up_DB
    Empty Directory    Template_Files/Run

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
