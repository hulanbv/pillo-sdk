# Pillo SDK

The Pillo SDK a series of packages for interacting with the Pillo Hardware using the Unity engine. The SDK is supported on the following build platforms:

- AppleTV running tvOS 9.0 or later
- Apple iPhone running iOS 5.0 or later
- Apple iPad running iPadOS 5.0 or later

## Pillo SDK Framework

The Pillo SDK Framework provides the core functionality for interacting with the Pillo Hardware.

### Installation

Install the latest stable release using the Unity Package Manager by adding the following line to your manifest.json file located within your project's Packages directory, or by adding the Git URL to the Package Manager Window inside of Unity. To make sure you're using a stable version, use a tag at the end of the Git URL in order to install a specific release.

```json
{
  "dependencies": {
    "nl.hulan.pillo-sdk.framework": "git+https://github.com/hulanbv/pillo-sdk-package.git?path=/Framework#v1.0.0"
  }
}
```

## Development

To start developing on the Pillo SDK, clone the repository as if it were the Unity's Packages directory by checking it out in your Unity project's base directory and renaming it to `Packages`. This will allow you to develop on the SDK and test it in your Unity project as if all the packages were installed using the Unity Package Manager.
