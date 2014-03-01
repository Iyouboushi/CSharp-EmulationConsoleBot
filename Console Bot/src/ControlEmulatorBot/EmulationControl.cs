///////////////////////////////////////////////
///// Emulation Control Bot version 1.2 ///////
////// By James "Iyouboushi" of esper.net /////
///////////////////////////////////////////////
// In order for this program to work, your 
// emulator keys have to be set as follows:
//  
// ALL EMULATORS:
//   UP: up arrow
//   DOWN: down arrow
//   LEFT/RIGHT: left and right arrows
//   A: HOME
//   B: END
//   START: PAGE UP
//   SELECT: PAGE DOWN
//
// GBA:
//   L: INSERT
//   R: DELETE 
//
// SNES:
//   X: P
//   Y: O
// SEGA:
//   C: I
//
// PSX:
//   L1: V
//   R1: B
//   R2: T
//   L2: E
//   TRIANGLE: D
//   SQUARE: S
//   CIRCLE: X
//   X: P
//
///////////////////////////////////////////////
// For VisualBoyAdvance make sure the follow
// is set right:
// Options -> Emulator -> Show Speed -> None
///////////////////////////////////////////////
// TO DO: 
// * Add more emulator options.
///////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using InputManager;
using System.Windows.Forms;
using System.Collections;
using System.IO;


namespace ControlEmulatorBot
{
    class botControl
    {
        
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int SendMessage(
        uint hWnd, // handle to destination window
        uint Msg, // message
        uint wParam, // first message parameter
        uint lParam // second message parameter
        );

        String windowHandle = "";
        IntPtr handle;

        private Thread botRun;

        private Random randnum;

        private String emulatorRunning;
        private String customEmulatorName;
        private String emulatorControlSetting;
        private String profileName;

        private bool useStartButton;
        private bool botRunning = true;

        private int rndNum = 0;

        private ArrayList emulationControls = new ArrayList();

        public botControl(string emRunning, string emControlSetting, string profile)
        {
            Console.WriteLine("Bot created..\r");

            //Adding keyboard event handlers and installing the hook
            KeyboardHook.KeyDown += new KeyboardHook.KeyDownEventHandler(KeyboardHook_KeyDown);
            KeyboardHook.KeyUp += new KeyboardHook.KeyUpEventHandler(KeyboardHook_KeyUp);
            KeyboardHook.InstallHook();

            // Initalize random
            randnum = new Random();

            this.emulatorRunning = emRunning;
            this.emulatorControlSetting = emControlSetting;
            this.profileName = profile;

            if (this.emulatorControlSetting == "NES-NOSTART")
            {
                useStartButton = false;
                this.emulatorControlSetting = "NES";
            }
            if (this.emulatorControlSetting == "NES-START")
            {
                useStartButton = true;
                this.emulatorControlSetting = "NES";
            }


            if (this.emulatorRunning == "custom")
                this.customEmulatorName = emControlSetting;

            Console.WriteLine("emulator running: " + this.emulatorRunning + "\r");
            Console.WriteLine("emulator control setting: " + this.emulatorControlSetting + "\r");

            buildEmuControlList();
            getEmulatorHandle();

            Console.WriteLine("Handle: " + handle);


            botRun = new Thread(new ThreadStart(botCommandRun));

            // Begin the control.
            botRun.Start();
        }

#region getEmulatorWindowHandle
        // getEmulatorHandle finds the windows handle ID necessary for this program to work based on which emulator is selected.
        // TODO: Make it so it can find handles of any window instead of only specific emulators.
       private void getEmulatorHandle()
        {

            if (this.emulatorRunning == "VisualBoyAdvance") { windowHandle = "VisualBoyAdvance"; }
            if (this.emulatorRunning == "BoycottAdvance") { windowHandle = "BoycottAdvance"; }
            if (this.emulatorRunning == "nester") { windowHandle = "nester"; }
            if (this.emulatorRunning == "ZSNES") { windowHandle = "ZSNES"; }

            if (this.emulatorRunning == "custom") { windowHandle = customEmulatorName; }


            handle = FindWindow(null, windowHandle);

            if (handle.Equals(IntPtr.Zero))
                handle = findWindowHandle();
        }
#endregion

       // This function attempts to find the window handle if it fails in the above section.
       // This function should make it possible to use any emulator.
#region FindWindowHandle
       IntPtr findWindowHandle()
       {

           IntPtr hWnd = IntPtr.Zero;
           foreach (Process pList in Process.GetProcesses())
           {
               if (pList.MainWindowTitle.Contains(windowHandle))
               {
                   hWnd = pList.MainWindowHandle;
               }
           }
           return hWnd;

       }
#endregion


       // Outputs the command that is being sent and sends the command along.
        private void controlEmulator(string m)
        {
            switch (m.ToUpper())
            {
                case "UP":
                    Console.WriteLine("COMMAND SENT: \u2191 \r");
                    doCommand(m.ToUpper());
                    break;
                case "DOWN":
                    Console.WriteLine("COMMAND SENT: \u2193 \r");
                    doCommand(m.ToUpper());
                    break;
                case "LEFT":
                    Console.WriteLine("COMMAND SENT: \u2190 \r");
                    doCommand(m.ToUpper());
                    break;
                case "LEFT2":
                    Console.WriteLine("COMMAND SENT: \u2190 \u2190 \r");
                    doCommand(m.ToUpper());
                    break;
                case "RIGHT":
                    Console.WriteLine("COMMAND SENT: \u2192 \r");
                    doCommand(m.ToUpper());
                    break;
                case "RIGHT2":
                    Console.WriteLine("COMMAND SENT: \u2192 \u2192 \r");
                    doCommand(m.ToUpper());
                    break;
                case "DOWNA":
                    Console.WriteLine("COMMAND SENT: \u2193 A \r");
                    doCommand(m.ToUpper());
                    break;
                case "DOWNB":
                    Console.WriteLine("COMMAND SENT: \u2193 B \r");
                    doCommand(m.ToUpper());
                    break;
                case "O":
                case "CIRCLE":
                    Console.WriteLine("COMMAND SENT: O \r");
                    doCommand(m.ToUpper());
                    break;
                case "TRIANGLE":
                    Console.WriteLine("COMMAND SENT: ▲ \r");
                    doCommand(m.ToUpper());
                    break;
                case "SQUARE":
                    Console.WriteLine("COMMAND SENT: [] \r");
                    doCommand(m.ToUpper());
                    break;
                default:
                    Console.WriteLine("COMMAND SENT: " + m + "\r");
                    doCommand(m.ToUpper());
                    break;

            }
        }

#region DoTheCommand
        private void doCommand(string command)
        {
            SetForegroundWindow(handle);
            SetFocus(handle);

            if (!handle.Equals(IntPtr.Zero))
            {

                int attempts = 0;
                do
                {
                    SetForegroundWindow(handle);
                    attempts++;
                } while ((!SetForegroundWindow(handle) || (attempts <= 3)));


                if (SetForegroundWindow(handle))
                {
                    Thread.Sleep(10);

                    switch (command.ToUpper())
                    {
                        case "UP":
                            Keyboard.KeyDown(Keys.Up);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.Up);
                            break;

                        case "DOWN":
                            Keyboard.KeyDown(Keys.Down);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.Down);
                            break;

                        case "LEFT":
                            Keyboard.KeyDown(Keys.Left);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.Left);
                            break;

                        case "RIGHT":
                            Keyboard.KeyDown(Keys.Right);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.Right);
                            break;

                        case "A":
                            Keyboard.KeyDown(Keys.Home);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.Home);
                            break;

                        case "A BUTTON":
                            Keyboard.KeyDown(Keys.Home);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.Home);
                            break;

                        case "B":
                            Keyboard.KeyDown(Keys.End);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.End);
                            break;

                        case "B BUTTON":
                            Keyboard.KeyDown(Keys.End);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.End);
                            break;

                        case "C":
                            Keyboard.KeyDown(Keys.I);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.I);
                            break;

                        case "AB":
                            Keyboard.KeyDown(Keys.Home);
                            Thread.Sleep(10);
                            Keyboard.KeyDown(Keys.End);
                            Thread.Sleep(300);
                            Keyboard.KeyUp(Keys.End);
                            Keyboard.KeyUp(Keys.Home);
                            Thread.Sleep(300);
                            break;

                        case "DOWNA":
                            Thread.Sleep(100);
                            Keyboard.KeyDown(Keys.Down);
                            Thread.Sleep(10);
                            Keyboard.KeyDown(Keys.Home);
                            Thread.Sleep(300);
                            Keyboard.KeyUp(Keys.Down);
                            Keyboard.KeyUp(Keys.Home);
                            break;

                        case "DOWNB":
                            Thread.Sleep(100);
                            Keyboard.KeyDown(Keys.Down);
                            Thread.Sleep(10);
                            Keyboard.KeyDown(Keys.End);
                            Thread.Sleep(300);
                            Keyboard.KeyUp(Keys.Down);
                            Keyboard.KeyUp(Keys.Home);
                            break;

                        case "L":
                            Keyboard.KeyDown(Keys.Insert);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.Insert);
                            break;

                        case "L1":
                            Keyboard.KeyDown(Keys.V);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.V);
                            break;

                        case "L BUTTON":
                            Keyboard.KeyDown(Keys.Insert);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.Insert);
                            break;

                        case "R":
                            Keyboard.KeyDown(Keys.Delete);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.Delete);
                            break;

                        case "R1":
                            Keyboard.KeyDown(Keys.B);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.B);
                            break;

                        case "R BUTTON":
                            Keyboard.KeyDown(Keys.Delete);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.Delete);
                            break;

                        case "X":
                            Keyboard.KeyDown(Keys.P);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.P);
                            break;

                        case "Y":
                            Keyboard.KeyDown(Keys.O);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.O);
                            break;

                        case "R2":
                            Keyboard.KeyDown(Keys.T);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.T);
                            break;

                        case "L2":
                            Keyboard.KeyDown(Keys.E);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.E);
                            break;

                        case "TRIANGLE":
                            Keyboard.KeyDown(Keys.D);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.D);
                            break;

                        case "SQUARE":
                            Keyboard.KeyDown(Keys.S);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.S);
                            break;

                        case "O":
                        case "CIRCLE":
                            Keyboard.KeyDown(Keys.X);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.X);
                            break;

                        case "START":
                            Keyboard.KeyDown(Keys.PageUp);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.PageUp);
                            break;

                        case "SELECT":
                            Keyboard.KeyDown(Keys.PageDown);
                            Thread.Sleep(200);
                            Keyboard.KeyUp(Keys.PageDown);
                            break;

                    }

                }


            }
            else
            {
                Console.WriteLine("The emulator is not on or found. Attempting to find it.\r\n");
                getEmulatorHandle();
            }

        }
#endregion

#region RunTheBot
        private void botCommandRun()
        {

            // Random bot mode. Will randomly pick a command and send it to the emulator.
            string command = " ";

            while (botRunning)
            {

                Thread.Sleep(50);

                int numberOfCommands = emulationControls.Count;
                rndNum = randnum.Next(numberOfCommands);

                command = (string)emulationControls[rndNum];
                controlEmulator(command);

                Thread.Sleep(30);
            }
        }

#endregion

#region KeyBoardHooks
        void KeyboardHook_KeyUp(int vkCode)
        {
            //Everytime the users releases a certain key up,
            //your application will go to this line
            //Use the vKCode argument to determine which key has been released
        }

        void KeyboardHook_KeyDown(int vkCode)
        {
            //Everytime the users holds a certain key down,
            //your application will go to this line
            //Use the vKCode argument to determine which key is held down
        }
#endregion

#region buildEmulationControlList
        void buildEmuControlList()
        {

            String line;

            // See if the config file exists.  If not, load the default.

            if (!File.Exists(profileName))
            {
                Console.WriteLine("building default commands");

                // Build the default config file.

                /// NES CONTROLS
                if (emulatorControlSetting == "NES")
                {
                    emulationControls.Add("UP");
                    emulationControls.Add("DOWN");
                    emulationControls.Add("LEFT");
                    emulationControls.Add("RIGHT");
                    emulationControls.Add("A");
                    emulationControls.Add("B");
                    emulationControls.Add("SELECT");
                    if (useStartButton == true) { emulationControls.Add("START"); }
                }

                /// GBA CONTROLS
                if (emulatorControlSetting == "GBA")
                {
                    emulationControls.Add("UP");
                    emulationControls.Add("DOWN");
                    emulationControls.Add("LEFT");
                    emulationControls.Add("RIGHT");
                    emulationControls.Add("A");
                    emulationControls.Add("B");
                    emulationControls.Add("L");
                    emulationControls.Add("R");
                    emulationControls.Add("SELECT");
                    emulationControls.Add("START"); 
                }

                /// SNES CONTROLS
                if (emulatorControlSetting == "SNES")
                {
                    emulationControls.Add("UP");
                    emulationControls.Add("DOWN");
                    emulationControls.Add("LEFT");
                    emulationControls.Add("RIGHT");
                    emulationControls.Add("A");
                    emulationControls.Add("B");
                    emulationControls.Add("X");
                    emulationControls.Add("Y");
                    emulationControls.Add("L");
                    emulationControls.Add("R");
                    emulationControls.Add("SELECT");
                    emulationControls.Add("START"); 
                }
            }

            else
            {
                Console.WriteLine("building profile commands for: " + profileName);

                // Build a list of commands from the file.
                using (StreamReader emuCommandsBuild = File.OpenText(profileName))
                {
                    while ((line = emuCommandsBuild.ReadLine()) != null)
                    {
                        emulationControls.Add(line.ToUpper());
                    }
                   emuCommandsBuild.Close();
                }
            }
#endregion



        }


    }

}