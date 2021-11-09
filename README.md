# Pillo SDK Package

This repository contains the Unity Package for the Pillo SDK.

## Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your manifest.json file located within your project's Packages directory, or by adding the Git URL to the Package Manager Window inside of Unity.

```json
"nl.hulan.pillo-sdk": "git+https://github.com/hulanbv/pillo-sdk-package"
```

## Usage Documentation

The Pillo SDK includes a Framework which allows the developer to interact with the Pillo hardware on an AppleTV using a Unity project. Get started by installing this package in your Unity project and Switching the build platform to Apple TV. Get started by importing the Pillo SDK Namespace.

```csharp
using Hulan.Pillo.SDK;
```

## Development Usage

...

## Creating Releases

Mark releases by pushing version tags.

```sh
$ npm version [<newversion> | major | minor | patch]
$ git push origin <tag_name>
```

## Distribution

The repository is distributed automatically via Git.
