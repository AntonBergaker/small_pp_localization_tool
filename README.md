# Small Pentapop Localization tool
Small Pentapop Localization tool is an export tool originally made for [Pentapop](https://play.google.com/store/apps/details?id=com.AntonBergaker.Pentapop), but has been expanded to be usable in other GameMaker projects like [The Story Goes On](https://store.steampowered.com/app/369560/The_Story_Goes_On/) and [Wally and the FANTASTIC PREDATORS](https://store.steampowered.com/app/1077450/Wally_and_the_FANTASTIC_PREDATORS/). While made for GameMaker, the files are easy to parse and can be used in any language.

The actual localization is not done inside the tool, but instead is handled inside a Google Sheet (or anything that can export a csv). This means you can have custom formatting rules, comments and multiple collaborators in the same place.  
The tool relies on creating custom buffer formats thats made to be fast to load on phones, and seperates the languages into their own files so nothing unnecessary is loaded.

# Getting started
To use the tool, first make a copy of this Google Sheet document: [link](https://docs.google.com/spreadsheets/d/1DduV7bqzB3jAvBhMyimEUDVvGc78UI2iymK1-B5dEHA/edit?usp=sharing). You then need to publish the document as a csv file to the web. You can do this by going to: File > Publish to the web > Link. And then selecting .csv in the drop down and then hitting publish. Make a copy of the result url.

The tool is run in the command line and expects two values.  
Usage: `./small_pp_localization_tool.exe <url> <target>`  
Replace \<url\> with your url you copied, and replace \<target\> with the folder where you want your language files to end up.

That's it! I recommend making a .bat file with the command so you in the future can export your languages with the press of a button.

# Using the files in GameMaker
First make sure to grab this script file and add it to your project: [file](https://github.com/AntonBergaker/small_pp_localization_tool/blob/master/Examples/GameMakerExample/scripts/localization/localization.gml)  
This file contains all the functions you'll need.

## Importing
To import a language, use the `localization_import(folder, file_name, [default_file_name])` function.  
The first argument is what folder to look in for your files.  
The second argument is the name of language, for example "en-US" from the template.  
The third argument lets you optionally specify a file to use as a default in case a translation is missing for a specific entry. For example you might wish to have English as a fallback language, so you'd put it there.

## Using
`localize(section, key)`  
This function will return a string with the entry located in the section given by the first argument, with the key of the second. Even on failure it will return a string with the entry and key it tried to look up.

`localize_format(section, key, identifier, replacement)`  
This function works the same as localize, but will replace a part of the string with the last argument. For example your string might be: "damage: %dmg", you'd want to use `localize_format(section, key, "%dmg", global.damage)`.

`localize_format_many(section, key, ...)`  
Same as above, but has no limit on how many words you can replace

`localize_count_in_section(section)`  
Returns how many entries exist in a section. This can be useful for dynamic content. For example you might show random loading screen messages from a long list. If the messages are in their own section you can return a random entry from it if their key is a number, which lets you have a variable amount of messages in different languages. 

# Resulting file layout
The resulting files are in a buffer format and can be parsed the following way
```
section_count = buffer.read_int_32() 
repeat (section_count)
  section_name = buffer.read_string() // uf8
  entry_count = buffer.read_int_32()
  repeat (entry_count)
    key = buffer.read_string() // utf8
    entry = buffer.read_string() // utf8
    // You can consume these values as you wish here
```
