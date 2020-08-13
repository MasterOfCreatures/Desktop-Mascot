Forked : https://github.com/TCPHijinks/Desktop-Mascot
Thx btw great work

# Changes to this repo
- Gif animations are played while idle etc
- Added a cthulhu gif to data and linked in animations json for testing (https://giphy.com/gifs/cthulhu-lovecraft-lovecraftian-5k01Xbfhom6zRfgYEN)
- manipulated gravitation (because, why not)
- Added Contex menu with a nice Robot icon to reset buddy, enter and exit console, and exit application(https://pixabay.com/vectors/robot-icon-flat-flat-design-2192617/)
- Supports Multiple Screens/Monitors now (has some bugs caused by resolution calculations)

# What does it do?
It's simple software that gives you a desktop buddy to play with and enjoy! Upon completion it will also include sound effects. All of which in addition to the animations will be easily customizable.

# Visual Studio Build Errors.
This mascot requires that you place the "Data" folder/directory inside of the built application folder. So for a normal Debug build, take the "Data" folder from the main folder and place it into:
  
Desktop-Mascot > Desktop-Actor Code > Desktop Actor > bin > Debug (PLACE IT HERE)

# Features
- Actor character with physics to throw around.
- Easily replace actor with custom one.
- Basic animation state machine.


### Latest update
**2.0**
- Finished complete rewrite.
- Basic animation state machine.
- Added context menu to allow exiting and enabling/disabling some features.
- Modified physics to lessen deceleration and consequently increase slide.


# Next Planned Features.
- Fix boundaries so actor can't be gliched outside of the play area.
- Improve animation state machine to accept animations with multiple frames.
- Sound effects! Ability to easily edit via a json file.

 
**NOTE:** All art is placeholder. I've also changed the project scope to be smaller but still allowing another developer to easily expand on what I've done so far. 
