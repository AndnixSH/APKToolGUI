# APK Tool GUI
![](Assets/Image.png)

[![](https://img.shields.io/github/downloads/AndnixSH/APKToolGUI/total?style=for-the-badge)](https://github.com/AndnixSH/APKToolGUI/releases) [![](https://img.shields.io/github/v/release/andnixsh/APKToolGUI?style=for-the-badge)](https://github.com/AndnixSH/APKToolGUI/releases)

GUI for apktool, signapk, zipalign and baksmali utilities.

It is a tool for reverse engineering 3rd party, closed, binary Android apps. It can decode resources to nearly original form and rebuild them after making some modifications; it makes possible to debug smali code step by step. Also it makes working with app easier because of project-like files structure and automation of some repetitive tasks like building apk, etc.

# Download links
https://github.com/AndnixSH/APKToolGUI/releases

Any alert from your antivirus regarding this application is a **false positive**, caused by how the application operates. To resolve this, manually add **APKToolGUI.exe** to your antivirus’s whitelist or exclusion list.

If you remain unsure, you can compile the app yourself or refrain from using it until you confirm its safety with your antivirus provider.

# Requirements
- Windows 7 32-bit/64-bit and above
- [Java](https://www.java.com/en/) or [JDK](https://www.oracle.com/java/technologies/downloads/) 8 or above. Using latest JDK is not really necessary. Java 8 and 17 (long-term support release) is enough. Use 64-bit version if your system is 64-bit
- [.NET Framework 4.8](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net48) (Windows 8 and above already have it preinstalled)

# Features
- Decompile & compile APK using Apktool
- Decompile APK/XAPK/APKS/ZIP/APKM & compile APK using APKEditor.jar. Automatically detect if APK was decompiled using Apktool or APKEditor
- Merge XAPK/APKS/ZIP/APKM using APKEditor.jar
- Sign APK
- Zipalign APK
- ADB
- Framework
- Drag and drop
- APK info
- Log output
- All other features from [Apktool](https://ibotpeaches.github.io/Apktool/documentation/).
- Fix some Apktool errors, such as removing DUMMY_APKTOOL, clear framework before decompiling
- Multi language
- Right-click context menu
- Create unsigned APK for Core Patch
- High DPI support
- Long path support (Windows 10 and above) 

# Languages
- English
- Brazilian Portuguese (Incomplete)
- Hungary (Incomplete)
- Russian (Incomplete)
- Chinese Simplified (Incomplete)
- German

### Translation
If you like to help translate with your native language, I recommend using Visual Studio 2022. You don't need any programming knowledge, you just edit the GUI and the `.resx` files

Create new `.resx` file under `Languages` folder, and `Forms/FormXXX.cs/`. To create a new `.resx` under `Forms/FormXXX.cs/`, open the form and select the form to show the properties. In the properties, scroll down until you see Language, and select any language you would like to translate.

<details>

<summary>Video tutorial</summary>

https://user-images.githubusercontent.com/40742924/227244601-6f796141-ac2d-4c60-b5cd-7f6872604cd7.mp4
  
</details>

If you can't use Visual Studio, you can use translator tools like [resxtranslator](https://github.com/HakanL/resxtranslator) or [RESX Translator automat](https://www.utilstudio.com/en/resx-translator/), but make sure the `.resx` file is added to [APKToolGUI.csproj](https://github.com/AndnixSH/APKToolGUI/blob/master/APKToolGUI/APKToolGUI.csproj), otherwise Visual Studio would not be able to detect it

3rd party tools can't be translated.

Make pull request when you are done

# Issues
Issues with Apktool? Please do cursory search of the issue tracker of [Apktool](https://github.com/iBotPeaches/Apktool/issues?q=is%3Aissue) and [Baksmali](https://github.com/JesusFreke/smali/issues?q=is%3Aissue), see if your question may be answered

For Framework/system apps related issues, check on [XDA Forum](https://forum.xda-developers.com/)

**Some apps may been protected or obfuscated for content protection purposes or against cheaters. Bypassing of such is beyond the scope of this tool, so please DO NOT post an issue regarding to it. This tool does not support bypassing such protections, and never will be!**

Anything else, [create a new issue](https://github.com/AndnixSH/APKToolGUI/issues)

# FAQ
##### Can it be ported to macOS or Linux?
Microsoft has not made it easy to port, and .NET Core remains impractical for this purpose. While I could use Avalonia UI as an alternative, I currently don’t have the time to port the entire project. Pull requests are welcome!

In the meantime, you can use [WineHQ](https://www.winehq.org/) or similar tools to run the .exe file.

##### How to update Apktool?
Download [Apktool.jar](https://github.com/iBotPeaches/Apktool/releases) and replace it on Resources folder

##### How to update Baksmali/Smali?
Download [Baksmali.jar/Smali.jar](https://bitbucket.org/JesusFreke/smali/downloads/) and replace it on Resources folder

##### How to reset?
Simply delete the config.xml file from the directory of the executeable

##### Can you help me with modding APKs in general?
No, I do not provide support for APK modding.

##### Can you make an automatic APK injecting tool?
No, I'm not interested.

##### Can you implement APK to AAB conversion?
No, APKTool is designed for decompiling and recompiling APKs, not for converting between formats. AAB (Android App Bundle) is used exclusively for distributing apps on the Google Play Store and is not directly installable on devices.

If you need to convert an APK to AAB, you can try using a tool like [APK2AAB](https://github.com/sensei-z/APK2AAB)

##### Can you implement features to protect, pack, or obfuscate APKs?
No, APK protection is beyond the scope of this tool.

##### Can you implement features to deobfuscate, unpack APKs, or bypass anti-cheat/security?
No, these features are also beyond the scope of this tool. I do not support or condone bypassing such protections.

# Development
This project is written in C#

Use Visual Studio 2019 and above. NET Framework 4.8 SDK is required

# Credits
- AndnixSH
- INF1NUM (Original author of [APKToolGUI](https://github.com/INF1NUM/APKToolGUI))
- iBotPeaches ([Apktool CLI](https://ibotpeaches.github.io/Apktool/))
- REAndroid ([APKEditor](https://github.com/REAndroid/APKEditor))

# Disclaimer
Same as Apktool by iBotPeaches, It is NOT intended for piracy and other non-legal uses. It could be used for localizing, adding some features or support for custom platforms and other GOOD purposes. Just try to be fair with authors of an app, that you use and probably like.

# License
The original repo does not have a license but I have permission from the author to use ane distribute this project as "open source"... I don't know what license to use so I'll just add license as unlicensed
