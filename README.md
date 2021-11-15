# Pillo SDK Package

This repository contains the Unity Package for the Pillo SDK.

## Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your manifest.json file located within your project's Packages directory, or by adding the Git URL to the Package Manager Window inside of Unity.

```json
"nl.hulan.pillo-sdk": "git+https://github.com/hulanbv/pillo-sdk-package"
```

## Documentation

The Pillo SDK provides a High-Level API for interacting with the Pillo Hardware on the following platforms:

- AppleTV running tvOS 9.0 or later

#### Getting Started

Get started by installing this package in your Unity project and Switching the build platform to Apple TV. Get started by importing the Pillo SDK Namespace.

```csharp
using Hulan.Pillo.SDK;
```

## Development

To contribute to the Pillo SDK package, create a new Unity Project and close this repository inside of the packages folder as following. This clones the Package as a development package inside of your Unity project. It will appear as a normal packages within the Unity Editor, but is editable. When any of the files within the Assembly Definition are changed, it will be recompiled and reimported.

```sh
$ cd MyUnityProject/Packages
$ git clone https://github.com/hulanbv/pillo-sdk-package pillo-sdk-package
```

#### Termology

The new Pillos communicate over BLE, this stands for Bluetooth Low Energy. The "Wokkel" developed by Vention provides an implementation which exposes various Services and Characteristics over this protocol.

In Bluetooth-speak, the device that has the data is referred to as the Peripheral, and the device that wants the data contained in the Peripheral is known as the Central. In case of a Pillo app, this means that typically the iPhone, iPad or AppleTV app we are developing will be the Central, interacting with one or more Peripherals to glean information that can then be processed, analyzed, or stored in some meaningful fashion.

An app using the Pillo SDK will act as a Central, we will want to know how to get the data we’re interested in out of the Peripheral. This is where Services and Characteristics come into play. You can think of Characteristics somewhat as being like properties of your device which and be read from and written to, and a Service is a collection of characteristics. A Peripheral can (and usually does) contain multiple services that we inspect to determine what characteristics are available with which to interact.

Before you can read from or write to values on the device, you need to Discover it. As it turns out, if a Pillo is turned on and is in range, it periodically sends out a little signal that lets interested parties know that it’s alive and kicking. This process is known as Advertising, and the time between signals is known as the “advertising interval.”

The Pillo SDK listens for those advertisements, and it will specify exactly which Services it is interested in. When going through its discovery phase, the SDK will find Pillos in the area that support the desired Services. This is very important, because the number of Bluetooth LE devices in existence, and which could potentially be in our vicinity, is constantly increasing. Therefore, it is helpful to let the framework weed out the devices that are not broadcasting the types of services we are interested in.

On iOS and TVOS the Core Bluetooth framework provides the classes needed for your apps to communicate with Bluetooth-equipped low energy (LE) and Basic Rate / Enhanced Data Rate (BR/EDR) wireless technology.

#### Pillo BLE GATT profiles

###### BAS (battery)

Generic service for monitoring battery state.

Service_UUID: 180f
Battery level: 2a19
1 Byte, Read & Notify

- uint8_t battery_level:
  - 0-100

#### Structure

TODO ...

## Creating Releases

Mark releases by pushing version tags.

```sh
$ npm version [<newversion> | major | minor | patch]
$ git push origin <tag_name>
```

## Distribution

The repository is distributed automatically via Git.
