# UlConnect
<img src="https://i.ibb.co/Fn6VLC3/main.png" alt="main" border="0">

## How to use program
- You <bold>need</bold> ESP32 / ESP8266
- Port 8266 must be open if you want access your board from the internet
### 1. Click on + to add connection
### 2. Enter ws://yourBoardIpAddress:8266 in the textbox "Board Address" 
### 3. Enter password of your board in the textbox "Password"
### 4. Save connection info and click on "Connect"
### 5. Click on "Turn on/off" to turn your PC on or off
## Building from source
### NOTE
Currently if you compile windows .exe on Linux or Mac and run app, a useless console will also appear.

There's temporary solution - compile it on Windows.
### 1. Requirements
- .NET Core 3.0-3.1
- Git
### 2a. Build the project for Windows

```
git clone https://github.com/arxflay/ulconnect.git
cd ulconnect
dotnet restore 
dotnet publish -r win-x64
```
### 2b. Build the project for Linux

```
git clone https://github.com/arxflay/ulconnect.git
cd ulconnect
dotnet restore 
dotnet build 
```

### 3. (optional) Install additional languages
#### For windows version
in linux:

```
cp -a /additional_languages /bin/Debug/netcoreapp3.0/win-x64/publish/lang
```
or if you run windows:

```
copy \additional_languages\*.* \bin\Debug\netcoreapp3.0\win-x64\publish\lang
```
#### For linux version
in linux:

```
cp -a /additional_languages /bin/Debug/netcoreapp3.0/lang
```
or if you run windows:

```
copy \additional_languages\*.* \bin\Debug\netcoreapp3.0\lang
```
### 4a Run windows binary
in the project folder nagivate to "\bin\Debug\netcoreapp3.0\win-x64\publish\" and run UlConnect.exe
### 4b Run linux binary

```
cd /bin/Debug/netcoreapp3.0
./UlConnect
```
