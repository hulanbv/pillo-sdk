![readme banner](https://github.com/hulanbv/pillo-sdk-mono/blob/master/.github/wiki/readme-banner.png?raw=true)

The Pillo SDK Mono Repository contains the source for various Pillo SDK packages and libraries. Everything you'll need to get working on and working with the Pillo SDK is included in this repository. The Pillo SDK Mono Repository is licensed by Hulan BV where all rights are reserved.

## Compatibility

The Pillo SDK Framework is compatible with the following platforms:

- Unity Engine for AppleTV running tvOS 9.0 or later
- Unity Engine for Apple iPhone running iOS 5.0 or later
- Unity Engine for Apple iPad running iPadOS 5.0 or later

## Development and Contribution

### Working on the Unity Packages

To start development on the Unity packages of the Pillo SDK, clone the entirety of this repository in your Unity project's `Assets` directory as if it were a normal asset. This will allow you to make changes to the packages and test them in your Unity project without having them to be installed via the Unity Package Manager.

### Working on the Native Libraries

To start development on the native libraries of the Pillo SDK, clone the entirety of this repository in any directory on your computer. Open this XCode project which can be found in the `Plugin` directory within the Pillo SDK Framework.

To test the native libraries in an actual Unity environment, a build must be made using the first step. This will generate an XCode project which can be opened and run on a physical device or simulator.
