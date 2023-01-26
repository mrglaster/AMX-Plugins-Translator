#include <amxmodx>
                       
#define PLUGIN "Plugin for Translator Test" 
#define VERSION "1.0"                    
#define AUTHOR "Glaster"          

public plugin_init() {
register_clcmd("say /menu", "NewMenu"); //Команда вызова меню: /menu в чат
    register_plugin(PLUGIN, VERSION, AUTHOR);
    register_clcmd("say /test_prints", "print_stuff");
}
                                                  
public print_stuff(id){       
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
    if( item < 0 ) return PLUGIN_CONTINUE; 
    new cmd[3], access, callback;
    menu_item_getinfo(menu, item, access, cmd,2,_,_, callback); 
    return PLUGIN_HANDLED;                                         
}                                                             
