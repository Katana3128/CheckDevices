# Check Devices

Inventura Webex zařízení a připojených periferií.

## Nastavení:
V `App.config` klíč `token` je potřeba zadat Personal Access Token pro komunikaci s webexem pomocí API.

## Výstup do txt souboru WbxDeviceAPeriferie.txt:
- Název zařízení
  - Product: Typ
  - SN: Seriové číslo
  - Periferie:
	- Type: Typ periferie, eg. TouchPanel/Kamera
	- SN: Seriové číslo periferie
	- V případě více periferií se Type a SN opakují