# LORIOT Azure Functions Library

This repository contains useful Azure functions for LORIOT users

## Scheduled Import Device

Time triggered function, imports devices from LORIOT network server to Azure IoT Hub. It will ensure no duplicates are created.

Parameters:
* *AZURE_IOT_HUB_OWNER_CONNECTION_STRING*: IoT Hub connection string—primary key for iothubowner
* *LORIOT_SERVER*: LORIOT network server url
* *LORIOT_APPS*: Array of LORIOT application IDs
* *LORIOT_API_KEY*: LORIOT user API key