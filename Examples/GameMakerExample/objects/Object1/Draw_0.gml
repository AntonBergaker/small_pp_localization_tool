draw_set_font(Font1);

draw_text(10, 10, localize("second_section", "in_game_key"));
draw_text(10, 30, "Press C to change between Swedish and English");

if (keyboard_check_pressed(ord("C"))) {
	isEnglish = !isEnglish;
	if (isEnglish) {
		localization_import("Localization", "en-US", "en-US");
	} else {
		localization_import("Localization", "sv-SE", "en-US");
	}
}