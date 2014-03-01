///////////////////////////////////////////////
///// Emulation Control Bot version 1.2 ///////
////// By James "Iyouboushi" of esper.net /////
///////////////////////////////////////////////

Not too long ago an anonymous programmer created a way for Twitch users to interact using the Twitch chat and they've been trying to collectively play the first Pokemon game (to some funny results).  You can google it to learn more about that.

After seeing that I said "I bet I can do that in C#!" and set out on a frustrating journey to get it to work.  However, I'm happy to report that thanks to the Input Manager Library (http://www.codeproject.com/Articles/117657/InputManager-library-Track-user-input-and-simulate), I've managed to get it to work.

First things first, you need to set the emulator's controls.  The keys in the emulators should be set correctly with the Gameboy Advance and NES emulators I'm including but you want to make sure the keys are set up like this:

ALL EMULATORS:
  UP: up arrow
  DOWN: down arrow
  LEFT/RIGHT: left and right arrows
  A: HOME
  B: END
  START: PAGE UP
  SELECT: PAGE DOWN

GBA:
  L: INSERT
  R: DELETE 

SNES:
  X: P
  Y: O

SEGA:
  C: I

PSX:
  L1: INSERT
  R1: DELETE
  R2: T
  L2: E
  TRIANGLE: D
  SQUARE: S
  CIRCLE: X
  X: P


If you're wanting to use VisualBoyAdvance there's another thing you have to make sure is turned off:

Options -> Emulator -> Show Speed -> None


HOW TO WORK THIS CONSOLE BOT:
First things first, make sure the emulator is turned on and is at the part of the game where you want the bot to start.  Run ControlEmulatorBot-v1.2 and you will see the menu and their options.

Pick the # that corresponds to the emulator you have running and the bot will ask you if you want to load a game control profile.  You can learn more about gaming profiles later on in this text file, but the gist is that you can disable certain keys for certain games (such as START if all start does is pause).  If you don't have one or don't care about it, just hit enter to use the default system config which will give the game access to every control it can have.  The exception is for VisualBoyAdvance. If you're playing a regular GB or GBC game rather than GBA, then put defaultGB.cfg as the profile name. 

The default NES profile will ask you if you want to include the START button or not.

And that's it. Once you've selected a profile the bot will now attempt to play the game you have running.

To exit this version just rapidly hit the X a bunch (it will try to keep the emulator in focus, so until I figure out a better way of making the bot actually stop this is the only way to really kill it).


HOW TO CREATE GAME CONTROL CONFIG FILES
Starting with version 1.0 it is now possible to tell the bot that you want only certain keys to be used when playing the game of your choice.  To do this, create a new plain text file and name it whatever you want (I suggest naming it something like gamename.cfg or gamename.txt).  

Inside you list one key per line. For example:

UP
DOWN
LEFT
RIGHT
A
B
X
Y
L
R
AB
DOWNA
DOWNB


In this example the bot will not have access to START or SELECT while playing the game if this config is loaded when prompted.  

It is also possible to list the same key multiple times in order to increase the chance that the bot will pick that key more than not.  For example:


UP
UP
UP
DOWN
LEFT
RIGHT
A
A
A
B
B


In this example the bot will try to select UP and A more than any other key.  It doesn't stop the bot from picking the other keys, it just weighs heavier towards the ones that are repeated.


Once you have the config saved, start the bot, pick your emulator and then when prompted it will tell you to type in the name of the config file including the extension.  If you named it tetrisnes.cfg you have to type tetrisnes.cfg   It is also important to make sure the config file is in the same folder as the .exe.

There are several configs provided for example.