# Small Pentapop Localization tool
Tiny localization export tool originally made for [Pentapop](https://play.google.com/store/apps/details?id=com.AntonBergaker.Pentapop), but has been expanded to be usable in other GameMaker projects like [The Story Goes On](https://store.steampowered.com/app/369560/The_Story_Goes_On/) and [Wally and the FANTASTIC PREDATORS](https://store.steampowered.com/app/1077450/Wally_and_the_FANTASTIC_PREDATORS/).

The actual localization is not done inside the tool, but instead is handled inside a Google Sheet (or anything that can export a csv). This means you can have custom formatting rules, comments and multiple collaborators in the same place.  
The tool relies on creating custom buffer formats thats made to be fast to load on phones, and seperates the languages into their own files so nothing unnecessary is loaded.

# Getting started
To use the tool, first make a copy of this Google Sheet document: [link](https://docs.google.com/spreadsheets/d/1DduV7bqzB3jAvBhMyimEUDVvGc78UI2iymK1-B5dEHA/edit?usp=sharing). You then need to publish the document as a csv file to the web. You can do this by going to: File > Publish to the web > Link. And then selecting .csv in the drop down and then hitting publish. Make a copy of the result url.

The tool is run in the command line and expects two values.  
Usage: `./small_pp_localization_tool.exe <url> <target>`  
Replace \<url\> with your url you copied, and replace \<target\> with the folder where you want your language files to end up.

That's it! I recommend making a .bat file with the command so you in the future can export your languages with the press of a button.

# Using the files in GameMaker
I'll publish my scripts in a bit, need to convert to 2.3. 
