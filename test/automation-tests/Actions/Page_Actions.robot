*** Settings ***
Library           AppiumLibrary
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
Resource          UI/Date_Bar.robot
Resource          UI/Leagues_Content.robot
Resource          Actions/Page_Actions.robot
Resource          DB/DB_Test.robot

*** Keywords ***
