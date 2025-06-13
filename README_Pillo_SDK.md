# Pillo SDK Mono Repository

![readme banner](https://github.com/hulanbv/pillo-sdk-mono/blob/master/.github/wiki/readme-banner.png?raw=true)

De **Pillo SDK Mono Repository** bevat de broncode van verschillende Pillo SDK componenten die samen de volledige SDK vormen. Hiermee ontwikkel je eenvoudig interactieve Unity-projecten met ondersteuning voor de Pillo hardware, zowel fysiek als gesimuleerd.  
Deze SDK is ontwikkeld en onderhouden door **Hulan B.V.**, met alle rechten voorbehouden.

---

## Inhoud

Deze repository bevat de volgende pakketten:

- [`UnityDeviceManager`](#unity-device-manager)
- [`UnitySimulator`](#unity-simulator)
- [`UnityBuildTools`](#unity-build-tools)
- [`UnityDebugger`](#unity-debugger)

---

## Installatie-instructies

Voeg onderstaande regels toe aan het `manifest.json` bestand binnen de `Packages` map van je Unity project, of gebruik de GIT URL direct via de Unity Package Manager UI.

### Unity Device Manager

Bevat de kernfunctionaliteit om te communiceren met de Pillo hardware.

```json
"nl.hulan.pillo-sdk.device-manager": "https://github.com/hulanbv/pillo-sdk-mono.git?path=/UnityDeviceManager"
```

#### Compatibiliteit

- Unity voor **AppleTV (tvOS 9.0+)**
- Unity voor **iPhone (iOS 5.0+)**
- Unity voor **iPad (iPadOS 5.0+)**

### Unity Simulator

Simuleer de Pillo hardware binnen de Unity Editor. Ideaal voor testen zonder fysiek apparaat.

```json
"nl.hulan.pillo-sdk.simulator": "https://github.com/hulanbv/pillo-sdk-mono.git?path=/UnitySimulator"
```

### Unity Build Tools

Handige editor scripts die helpen bij het configureren en bouwen van projecten met Pillo SDK-integratie.

```json
"nl.hulan.pillo-sdk.build-tools": "https://github.com/hulanbv/pillo-sdk-mono.git?path=/UnityBuildTools"
```

### Unity Debugger

Ondersteunt debugging van de SDK, zowel binnen de Unity Editor als standalone builds.

```json
"nl.hulan.pillo-sdk.debugger": "https://github.com/hulanbv/pillo-sdk-mono.git?path=/UnityDebugger"
```
