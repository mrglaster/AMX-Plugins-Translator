## AMX Plugins Translator
C# Utility generating Dictionary for the AMX Plugin and Replacing almost all hardcoded strings to Dictionary References

## What is this utility for?

Often when developing AMX plugins it happens that before the release it becomes necessary to add multilingual support.

This is not a difficult task, but sometimes it takes a lot of time to implement it: 
1) convert hardcoded strings to a certain form
2) create a dictionary
3) translate the entire hardcode into other languages.

This utility automates the above mentioned tasks. You just need to specify the path to the plugin, the source language of the hardcode and the list of languages ​​you want to translate the plugin into.
You can leave last argument out, then the utility will translate the plugin into all supported languages

## Example of utility work

### Source code of Plugin

```
#include <amxmodx>

#define PLUGIN "Plugin for Translator Test"
#define VERSION "1.0"
#define AUTHOR "Glaster"
public plugin_init() {
    register_clcmd("say /menu", "NewMenu");
    register_plugin(PLUGIN, VERSION, AUTHOR);
    register_clcmd("say /test_prints", "print_stuff");
}

public print_stuff(id) {
    new name[32];

    //client_print example
    get_user_name(id, name, 32);
    client_print(id, print_chat, "Hello Player with Name: %s", name);
    client_print(id, print_center, "Hello Dude!");
    client_print(id, print_console, "Test Value");

    //Hud example. Is valid also for dhud
    set_hudmessage(64, 0, 128, -1.0, 0.0, 0, 6.0, 6.0, 0.1, 0.2, -1)
    show_hudmessage(0, "Hello Player!")

}
//Menu exaple
public NewMenu(id) {
    new i_Menu = menu_create("\wOur Server Menu", "NewMenu_handler");
    menu_additem(i_Menu, "\wItem 1", "1", 0);
    menu_additem(i_Menu, "\wItem 2", "2", 0);
    menu_additem(i_Menu, "\wItem 3", "3", 0);
    menu_setprop(i_Menu, MPROP_NEXTNAME, "\rNext!");
    menu_setprop(i_Menu, MPROP_BACKNAME, "\rBack");
    menu_setprop(i_Menu, MPROP_EXITNAME, "\rExit");
    menu_display(id, i_Menu, 0)
}
public NewMenu_handler(id, menu, item) {
    if (item < 0) return PLUGIN_CONTINUE;
    new cmd[3], access, callback;
    menu_item_getinfo(menu, item, access, cmd, 2, _, _, callback);
    return PLUGIN_HANDLED;
}        
```

### Generated modified plugin

```
//Plugins was translated with AMX Plugin Translator: https://github.com/mrglaster/AMX-Plugin-Translator
#include <amxmodx>

#define PLUGIN "Plugin for Translator Test"
#define VERSION "1.0"
#define AUTHOR "Glaster"

public plugin_init() {
    register_dictionary("test_plugin.txt");

    register_clcmd("say /menu", "NewMenu"); 
    register_plugin(PLUGIN, VERSION, AUTHOR);
    register_clcmd("say /test_prints", "print_stuff");
}

public print_stuff(id) {
    new name[32];

    //client_print example
    get_user_name(id, name, 32);
    client_print(id, print_chat, "%L", LANG_PLAYER, "HELLO_PLAYER_WITH_NAME", name);
    client_print(id, print_center, "%L", LANG_PLAYER, "HELLO_DUDE");
    client_print(id, print_console, "%L", LANG_PLAYER, "TEST_VALUE");

    //Hud example. Is valid also for dhud
    set_hudmessage(64, 0, 128, -1.0, 0.0, 0, 6.0, 6.0, 0.1, 0.2, -1)
    show_hudmessage(0, "%L", LANG_PLAYER, "HELLO_PLAYER")

}

//Menu exaple
public NewMenu(id) {
    new szStringBuf[64]
    formatex(szStringBuf, charsmax(szStringBuf), "%L", LANG_PLAYER, "WOUR_SERVER_MENU");
    new i_Menu = menu_create(szStringBuf, "NewMenu_handler");
    formatex(szStringBuf, charsmax(szStringBuf), "%L", LANG_PLAYER, "WITEM_1");
    menu_additem(i_Menu, szStringBuf, "1", 0);
    formatex(szStringBuf, charsmax(szStringBuf), "%L", LANG_PLAYER, "WITEM_2");
    menu_additem(i_Menu, szStringBuf, "2", 0);
    formatex(szStringBuf, charsmax(szStringBuf), "%L", LANG_PLAYER, "WITEM_3");
    menu_additem(i_Menu, szStringBuf, "3", 0);
    menu_setprop(i_Menu, MPROP_NEXTNAME, "\rNext!");
    menu_setprop(i_Menu, MPROP_BACKNAME, "\rBack");
    menu_setprop(i_Menu, MPROP_EXITNAME, "\rExit");
    menu_display(id, i_Menu, 0)
}

public NewMenu_handler(id, menu, item) {
    if (item < 0) return PLUGIN_CONTINUE;
    new cmd[3], access, callback;
    menu_item_getinfo(menu, item, access, cmd, 2, _, _, callback);
    return PLUGIN_HANDLED;
}
```


### Generated Dictionary

```
[en]
HELLO_PLAYER_WITH_NAME = Hello Player with Name: %s
HELLO_DUDE = Hello Dude!
TEST_VALUE = Test Value
HELLO_PLAYER = Hello Player!
WOUR_SERVER_MENU = Our Server Menu
WITEM_1 = Item 1
WITEM_2 = Item 2
WITEM_3 = Item 3

 
[ru]
HELLO_PLAYER_WITH_NAME = Привет, игрок с именем: %s 
HELLO_DUDE = Привет, чувак! 
TEST_VALUE = Тестовое значение 
HELLO_PLAYER = Привет Игрок! 
WOUR_SERVER_MENU = Меню нашего сервера 
WITEM_1 = Пункт 1 
WITEM_2 = Пункт 2 
WITEM_3 = Пункт 3 

 
[de]
HELLO_PLAYER_WITH_NAME = Hallo Spieler mit Namen: %s 
HELLO_DUDE = Hallo Alter! 
TEST_VALUE = Testwert 
HELLO_PLAYER = Hallo Spieler! 
WOUR_SERVER_MENU = Unser Servermenü 
WITEM_1 = Punkt 1 
WITEM_2 = Punkt 2 
WITEM_3 = Punkt 3 

```

### In-game example

![alt text](https://github.com/mrglaster/AMX-Plugin-Translator/blob/main/readme_images/example_print_center_new.png)

![alt text](https://github.com/mrglaster/AMX-Plugin-Translator/blob/main/readme_images/example_menu.png)

## How to use the utility?

The utility receives 2 or 3 arguments as input. The first one is the path to the AMX script in .sma format, the second one is the original hardcode language of the plugin, the third one (optional) is the list of languages into which the plugin will be translated.

Examples of using :

```
AMX-Plugins-Translator.exe myplugin.sma en
```

or

```
AMX-Plugins-Translator.exe myplugin.sma en "ru, de, fr"
```
### Arguments Description

| Argument Name    | Type   | Required | Description                                                                                                        |
|------------------|--------|----------|--------------------------------------------------------------------------------------------------------------------|
| Path to Plugin   | String | TRUE     | Path to your .sma AMX Plugin                                                                                       |
| Source Language  | String | TRUE     | Source language of plugin's hardcode. Example: en, de, other languages from the "Supported Languages" table        |
| Output Languages | String | FALSE    | String containing output languages in format "de, ru, ua" (check the "Supported Languages" table for the full list |

## Supported Languages

| №  | Abbreviation used in the utility | Language             |
|----|----------------------------------|----------------------|
| 1  | en                               | English              |
| 2  | de                               | German               |
| 3  | sr                               | Serbian              |
| 4  | tr                               | Turkish              |
| 5  | fr                               | French               |
| 6  | sv                               | Swedish              |
| 7  | da                               | Danish               |
| 8  | pl                               | Polish               |
| 9  | nl                               | Dutch                |
| 10 | es                               | Spanish              |
| 11 | bp                               | Brazilian Portuguese |
| 12 | cz                               | Czech                |
| 13 | fi                               | Finnish              |
| 14 | bg                               | Bulgarian            |
| 15 | ro                               | Romanian             |
| 16 | hu                               | Hungarian            |
| 17 | lt                               | Lithuanian           |
| 18 | sk                               | Slovak               |
| 19 | mk                               | Macedonian           |
| 20 | ru                               | Russian              |
| 21 | hr                               | Croatian             |
| 22 | bs                               | Bosnian              |
| 23 | cn                               | Chinese              |
| 24 | al                               | Albanian             |
| 25 | ua                               | Ukrainian            |
| 26 | lv                               | Latvian              |


### Used Libraries

1) [GTranslate: A collection of free translation APIs](https://github.com/d4n3436/GTranslate)
