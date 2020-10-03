# UlConnect
<img src="https://i.ibb.co/S5Zx0HP/App-view.png" alt="App-view" border="0">
<h2>How to use program</h2>
- You <bold>require</bold> our solution to work with this program</br>
- Port 8266 must be open if you want access to your board from internet</br>
<h3>1. Click on + to add connection 
<h3>2. In textbox "Board Address" enter ws://yourBoardIpAddress:8266</h3>
<h3>3. In textbox "Password" enter password of your board </h3>
<h3>4. Save connection info and click on "Connect"</h3>
<h3>5. Click on "Turn on/off" to turn your PC on or off</h3>
<h2>Bulding from source</h2>
<h3>1. Requirements</h2>
- .NET Core >=3.0</br>
- Git
<h3>2a. Build the project for Windows</h3>

```
git clone https://github.com/arxflay/ulconnect.git
cd ulconnect
dotnet restore 
dotnet publish -r win-x64
```
<h3>2b. Build the project for Linux></h3>

```
git clone https://github.com/arxflay/ulconnect.git
cd ulconnect
dotnet restore 
dotnet build 
```

<h3>3. (optional) Add more languages</h3>
<h4>For windows version</h4>
in linux:

```
cp -a /additional_languages /bin/Debug/netcoreapp3.0/win-x64/publish/lang
```
or if you run windows:

```
copy \additional_languages\*.* \bin\Debug\netcoreapp3.0\win-x64\publish\lang
```
<h4>For linux version</h4>
in linux:

```
cp -a /additional_languages /bin/Debug/netcoreapp3.0/lang
```
or if you run windows:

```
copy \additional_languages\*.* \bin\Debug\netcoreapp3.0\lang
```
<h3>4a Run windows binary</h3>
in the project folder nagivate to "\bin\Debug\netcoreapp3.0\win-x64\publish\" and run UlConnect.exe
<h3>4b Run linux binary</h3>

```
cd /bin/Debug/netcoreapp3.0
./UlConnect
```