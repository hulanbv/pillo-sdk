# Pillo SDK Package

This repository contains the Unity Package for the Pillo SDK.

## Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your manifest.json file located within your project's Packages directory, or by adding the Git URL to the Package Manager Window inside of Unity.

```json
"nl.hulan.pillo-sdk": "git+https://github.com/hulanbv/pillo-sdk-package"
```

## Usage Documentation

The Pillo SDK includes a Framework which allows the developer to interact with the Pillo hardware on an AppleTV using a Unity project.

#### Getting Started

Get started by installing this package in your Unity project and Switching the build platform to Apple TV. Get started by importing the Pillo SDK Namespace.

```csharp
using Hulan.Pillo.SDK;
```

## Development Usage

To contribute to the Pillo SDK package, create a new Unity Project and close this repository inside of the packages folder as following. This clones the Package as a development package inside of your Unity project. It will appear as a normal packages within the Unity Editor, but is editable. When any of the files within the Assembly Definition are changed, it will be recompiled and reimported.

```sh
$ cd MyUnityProject/Packages
$ git clone https://github.com/hulanbv/pillo-sdk-package pillo-sdk-package
```

#### Structure

...

## Creating Releases

Mark releases by pushing version tags.

```sh
$ npm version [<newversion> | major | minor | patch]
$ git push origin <tag_name>
```

## Distribution

The repository is distributed automatically via Git.
