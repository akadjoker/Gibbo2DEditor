﻿#region Copyrights
/*
Gibbo2D - Copyright - 2013 Gibbo2D Team
Founders - Joao Alves <joao.cpp.sca@gmail.com> and Luis Fernandes <luisapidcloud@hotmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. 
*/
#endregion
#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#endregion

namespace Gibbo.Engine.Windows
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {        
        static MyTextWriter textWritter;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                textWritter = new MyTextWriter();
                //Console.SetOut(textWritter);
                textWritter.ConsoleOutput += textWritter_ConsoleOutput;
                
                using (var game = new Game1())
                {
                    if (args.Length > 0)
                    {
                        Console.WriteLine("arg count: " + args.Length);

                        if (args.Length >= 2)
                        {
                            int px = int.Parse(args[0]);
                            int py = int.Parse(args[1]);
                            game.preferredPositionX = px;
                            game.preferredPositionY = py;
                        }

                        int cnt = 0;
                        foreach (var arg in args)
                        {
                            cnt++;
                            Console.WriteLine("ARG[" + cnt + "]: " + arg);
                        }
                    }

                    Gibbo.Library.SceneManager.GameArgs = args;

                    game.Run();
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "error");
                Console.ReadLine();
            }

            //Console.WriteLine("DEBUG MODE, PRESS ANY KEY TO EXIT...");
            //Console.ReadLine();
        }

        static void textWritter_ConsoleOutput(object sender, EventArgs e)
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                w.WriteLine((e as MyTextWriterArgs).Text);
            }
        }
    }
#endif
}
