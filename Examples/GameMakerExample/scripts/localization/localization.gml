global.__localization_entries = ds_map_create(); ///@is {ds_map<string, string>}
global.__localization_sections = ds_map_create(); ///@is {ds_map<string, int>}

///@param section
///@param key
function localize(section/*:string*/, key/*:string*/) /*-> string*/ {
	var _val = global.__localization_entries[? section + ";" + key];
	
	// Error if it can't find it
	if (_val == undefined) {
		return "[" + section + "]: " + key;
	}

	return _val;	
}

///@description Returns how many entries are in a section
///@param section
function localize_count_in_section(section/*:string*/) /*-> int*/ {
	var _val = global.__localization_sections[? section];
	if (_val == undefined) {
		return -1;	
	}
	return _val;
}

///@param key
///@param section
///@param identifier
///@param replacement
function localize_format(key/*:string*/, section/*:string*/, identifier/*:string*/, replacement/*:string*/) /*-> string*/ {
	return string_replace(localize(key, section), identifier, string(replacement));
}

///@param key:string
///@param section:string
///@param ...identifiersAndReplacements:string
function localize_format_many() /*-> string*/ {
	var _str = localize(argument[0], argument[1]);
	for (var i=2; i < argument_count; i+=2) {
		_str = string_replace(_str, argument[i], string(argument[i+1]));
	}
	return _str;
	
}

///@param raw_identifier
function localize_raw(raw_identifier/*:string*/) /*-> string*/ {
	var _val = global.__localization_entries[? raw_identifier];
	if (_val == undefined) {
		return raw_identifier;	
	}

	return _val;
}

/// @param folder:string
/// @param file_name:string
/// @param ?default_file_name:string
function localization_import() {

	var _folderPath = argument[0];
	if (_folderPath != "") {
		_folderPath = _folderPath + "/";	
	}
	var _filePath = argument[1];
	var _combinedPath = _folderPath + _filePath; 
	var _defaultFilePath = argument_count == 3 ? argument[2] : undefined;
	

	ds_map_clear(global.__localization_entries);
	ds_map_clear(global.__localization_sections);

	
	var import_file = function(_filePath) {
		var _map = global.__localization_entries;
		var _sectionMap = global.__localization_sections;

		var _buff = buffer_load(_filePath + ".lang");
		var _categories = buffer_read(_buff, buffer_s32) /*#as int*/;
		var _parent = "";

		repeat (_categories) {
			var _category = buffer_read(_buff, buffer_string) /*#as string*/;
			var _count = buffer_read(_buff, buffer_s32) /*#as int*/;
			ds_map_add(_sectionMap, _category, _count);
		
			// Add the seperator
			_category += ";";
		
			repeat (_count) {
				var _key = buffer_read(_buff, buffer_string) /*#as string*/;
				var _field = buffer_read(_buff, buffer_string) /*#as string*/;

				_key = _category + _key;
				if (_key == "meta;parent") {
					_parent = _field;
				}
				
				// Ignore if it's already added
				if (ds_map_exists(_map, _key)) {
					continue;	
				}
				_map[? _key] = _field;
			}
		}

		buffer_delete(_buff);
		return _parent;
	}
	
	var _parent = import_file(_combinedPath);
	var _recursion = 0;
	// Imports the parent
	while (_parent != "") {
		_recursion += 1;
		if (_recursion > 32) {
			show_message("Recursion detected");
			return;
		}
		_parent = import_file(_folderPath + _parent);	
	}
	if (_defaultFilePath != undefined && _defaultFilePath != "") {
		import_file(_folderPath + _defaultFilePath);
	}
}
