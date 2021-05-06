# LORIOT Azure Functions Library

This repository contains useful Azure functions for LORIOT users:
- [Device Provisioning](#device-provisioning)
- [Routing](#routing)

## Device Provisioning
Import devices from LORIOT to Azure IoT Hub:
* **ImportDevices**: Http triggered function
* **ScheduledImportDevices**: Time triggered function to ensure regular syncing of new devicesÇ
![ImportDevices](https://user-images.githubusercontent.com/6308233/117285396-42792700-ae68-11eb-9b15-b422938e6a57.jpg)
![ScheduledImportDevices](https://user-images.githubusercontent.com/6308233/117285402-4442ea80-ae68-11eb-8885-226411a3b08f.jpg)

### Environment parameters:
* **AZURE_IOT_HUB_OWNER_CONNECTION_STRING**: IoT Hub connection string—primary key for iothubowner
  > HostName=myiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=1nMDaC6ArtFRJVG/DYlbZnxJ+gMHRv9nr8KUSoem0t0=
* **LORIOT_SERVER**: LORIOT network server url
  > eu1.loriot.io
* **LORIOT_API_KEY**: LORIOT user API key
  > AAAANgXyM8bYn71AbFBjAgd08-pkBiE50rcdb_fF8kADixHi0
* **IMPORT_DEVICES_SCHEDULE_EXP**: Schedule expression for ScheduledImportDevices time trigger
  > 0 0 0 * * *
* **LORIOT_APPS (optional)**: List of LORIOT application IDs (comma separated). If not defined, all devices from all applications will be imported.
  > BE7A2148,BE7A25C0

## Routing
Route incoming LORIOT messages
* **Routing**: IoT Hub triggered function that parses every device uplink
![Routing](https://user-images.githubusercontent.com/6308233/117285417-49079e80-ae68-11eb-8678-1b9601b2f37c.jpg)

### Environment parameters:
* **IOT_HUB_ENDPOINT**: IoT Hub Event Hub-compatible endpoint
  > Endpoint=sb://ihsuprodamres999dednamespace.servicebus.windows.net/;SharedAccessKeyName=iothubowner;SharedAccessKey=1nSEaB6AftSRJVG/DYlbZnxJ+gMHRa9nr8KUSoem0t0=;EntityPath=iothub-ehub-loriot-iot-9127777-ff7bd7b7d8
