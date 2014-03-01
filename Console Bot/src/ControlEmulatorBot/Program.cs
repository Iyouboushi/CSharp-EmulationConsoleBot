using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ControlEmulatorBot
{
    partial class Program
    {
        static void Main(string[] args)
        {
            bool userWantsToExit = false;
            botControl botControl;

            Console.WriteLine("===================================");
            Console.WriteLine("Emulation Control Bot version 1.2");
            Console.WriteLine("Written by James Iyouboushi");
            Console.WriteLine("===================================");

            Console.WriteLine("Turn your emulator on and then");
            Console.WriteLine("pick the emulator to bot in:");
            Console.WriteLine("   ");

            Console.WriteLine("Gameboy Advance: ");
            Console.WriteLine(" 1. VisualBoyAdvance");
            Console.WriteLine(" 2. BoycottAdvance");
            Console.WriteLine("   ");

            Console.WriteLine("Nintendo:");
            Console.WriteLine(" 3. nester");
            Console.WriteLine("   ");

            Console.WriteLine("Super Nintendo:");
            Console.WriteLine(" 4. ZSNES");
            Console.WriteLine("   ");

            Console.WriteLine("Other:");
            Console.WriteLine(" 5. Another Emu Not on the List");
            Console.WriteLine("   ");


            Console.WriteLine("===================================");

            String m = Console.ReadLine();
            String startbutton = "";

            while ((((((m != "1") && (m != "2") && (m != "3") && (m != "4") && (m != "5") && (m.ToUpper() != "QUIT"))))))
            {
                Console.WriteLine("Invalid number. Please try again or type QUIT to quit");
                m = Console.ReadLine();
            }

            if (m == "1")
            {
                Console.WriteLine("Starting VisualBoyAdvance bot..");
                Console.WriteLine("--------");

                Console.WriteLine("Type in the name of a game control profile; include file extension");
                Console.WriteLine("For default GBA, leave blank. For GB put gameboy.cfg");

                string profile = Console.ReadLine();
                if (profile == "") { profile = "defaultGBA.cfg"; }

                botControl = new botControl("VisualBoyAdvance", "GBA", profile);
            }

            if (m == "2")
            {
                Console.WriteLine("Starting BoycottAdvance bot..");
                Console.WriteLine("--------");

                Console.WriteLine("Type in the name of a game control profile; include file extension");
                Console.WriteLine("For default GBA, leave blank.");

                string profile = Console.ReadLine();
                if (profile == "") { profile = "defaultGBA.cfg"; }

                botControl = new botControl("BoycottAdvance", "GBA", profile);
            }

            if (m == "3")
            {
                Console.WriteLine("Starting nester bot..");
                Console.WriteLine("--------");

                Console.WriteLine("Type in the name of a game control profile or leave blank for default");
                Console.WriteLine("please include the file extension");

                string profile = Console.ReadLine();
                if (profile == "") { profile = "defaultNES.cfg"; }

                if (profile == "defaultNES.cfg")
                {

                    Console.WriteLine("--------");
                    Console.WriteLine("Do you want to enable the START button? Y or N?");
                    startbutton = Console.ReadLine();
                }

                if (startbutton == "") { startbutton = "Y"; } 

                if ((startbutton.ToUpper() == "Y") || (startbutton.ToUpper() == "YES")) { botControl = new botControl("nester", "NES-START", profile); }
                else { botControl = new botControl("nester", "NES-NOSTART", profile); }
            }

            if (m == "4")
            {
                
                Console.WriteLine("Starting ZSNES bot..");
                Console.WriteLine("--------");

                Console.WriteLine("Type in the name of a game control profile; include file extension");
                Console.WriteLine("For default SNES, leave blank.");

                string profile = Console.ReadLine();
                if (profile == "") { profile = "defaultSNES.cfg"; }

                botControl = new botControl("ZSNES", "SNES", profile);
                
            }

            if (m == "5")
            {
                Console.WriteLine("** This is still experimental and may not work properly");
                Console.WriteLine("*** The keyboard keys still need to be set the same as other emulators");
                String customEmulator = "";
                do
                {
                    Console.WriteLine("Please input the name of the emulator");
                    customEmulator = Console.ReadLine();
                } while (customEmulator == "");

                Console.WriteLine("Starting " + customEmulator + "bot..");
                Console.WriteLine("--------");

                Console.WriteLine("Type in the name of a game control profile; include file extension");
                Console.WriteLine("For default use defaultCONSOLE.cfg like defaultNES.cfg or defaultGBA.cfg");

                string profile = Console.ReadLine();
                if (profile == "") { profile = "defaultNES.cfg"; }

                botControl = new botControl("custom", customEmulator, profile);
            }

            if (m.ToUpper() == "QUIT")
            {
                userWantsToExit = true;
            }

        }

    }
}
