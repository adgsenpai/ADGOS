﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Sys = Cosmos.System;

namespace ADGOS
{
    public class Kernel : Sys.Kernel
    {
        /* SNAKE VARS */
        public int[] matrix;
        public List<int[]> commands;
        public List<int[]> snake;
        public List<int> food;
        public int randomNumber;
        Random rnd = new Random();

        private static Sys.FileSystem.CosmosVFS FS;
        public static string file;


        String version = "0.0.1";
        protected override void BeforeRun()
        {
            FS = new Sys.FileSystem.CosmosVFS(); 
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(FS);
            FS.Initialize(true);
            Console.WriteLine("Welcome to ADGOS!");
        }
        public string getSnakeScore()
        {
            if (snake.Count < 10)
            {
                return snake.Count + "   ";
            }
            else if (snake.Count < 100)
            {
                return snake.Count + "  ";
            }
            else if (snake.Count < 1000)
            {
                return snake.Count + " ";
            }
            else
            {
                return snake.Count + "";
            }
        }

        public void updatePosotion()
        {
            List<int[]> tmp = new List<int[]>();

            foreach (int[] point in snake)
            {
                switch (point[1])
                {
                    case 1:
                        point[0] = point[0] - 1;
                        break;
                    case 2:
                        point[0] = point[0] + 80;
                        break;
                    case 3:
                        point[0] = point[0] + 1;
                        break;
                    case 4:
                        point[0] = point[0] - 80;
                        break;
                    default:
                        break;
                }
                tmp.Add(point);
            }
            snake = tmp;
        }

        public void changeArray()
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = 0;
            }

            foreach (int[] point in snake)
            {
                matrix[point[0]] = 3;
            }

            foreach (int point in food)
            {
                matrix[point] = 2;
            }

            for (int i = 0; i < matrix.Length; i++)
            {
                if (i <= 79 && i >= 0)
                {
                    matrix[i] = 1;
                }
                else if (i <= 1760 && i >= 1679)
                {
                    matrix[i] = 1;
                }
                else if (i % 80 == 0)
                {
                    matrix[i] = 1;
                }

                else if (i % 80 == 79)
                {
                    matrix[i] = 1;
                }
            }
        }

        public Boolean gameover()
        {
            int head = snake[0][0];
            for (int i = 1; i < snake.Count; i++)
            {
                if (head == snake[i][0])
                {
                    return true;
                }
            }
            if (head % 80 == 79 || head % 80 == 0 || head <= 1760 && head >= 1679 || head <= 79 && head >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void printGame()
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                if (gameover() && i == 585)
                {
                    Console.Write("###############################");
                    i = i + 30;
                }
                else if (gameover() && i == 665)
                {
                    Console.Write("###############################");
                    i = i + 30;
                }
                else if (gameover() && i == 745)
                {
                    Console.Write("####                       ####");
                    i = i + 30;
                }
                else if (gameover() && i == 825)
                {
                    Console.Write("####       GAMEOVER        ####");
                    i = i + 30;
                }
                else if (gameover() && i == 905)
                {
                    Console.Write("####      Score: " + getSnakeScore() + "      ####");
                    i = i + 30;
                }
                else if (gameover() && i == 985)
                {
                    Console.Write("####                       ####");
                    i = i + 30;
                }
                else if (gameover() && i == 1065)
                {
                    Console.Write("###############################");
                    i = i + 30;
                }
                else if (gameover() && i == 1145)
                {
                    Console.Write("###############################");
                    i = i + 30;
                }
                else if (matrix[i] == 1)
                {
                    Console.Write("#");
                }
                else if (matrix[i] == 2)
                {
                    Console.Write("$");
                }
                else if (matrix[i] == 3)
                {
                    Console.Write("*");
                }
                else
                {
                    Console.Write(" ");
                }
            }

            Console.Write("#  Current score: " + getSnakeScore() + "      Exit game: ESC button      Restart game: R button  #");
            Console.Write("################################################################################");
        }

        public void delay(int time)
        {
            for (int i = 0; i < time; i++) ;
        }

        public int xy2p(int x, int y)
        {
            return y * 80 + x;
        }

        public int randomFood()
        {
            int rand = rnd.Next(81, 1700);
            if (rand != randomNumber)
            {
                randomNumber = rand;
                return rand;
            }
            else
            {
                return 1400;
            }
        }

        public void configSnake()
        {
            matrix = new int[1760];
            commands = new List<int[]>();
            snake = new List<int[]>();
            food = new List<int>();
            randomNumber = 0;
            snake.Add(new int[2] { xy2p(10, 10), 3 });
            changeArray();
            food.Add(randomFood());
        }

        public void updateDirections()
        {
            List<int[]> tmp = new List<int[]>();
            foreach (int[] com in commands)
            {
                if (com[1] < snake.Count)
                {
                    snake[com[1]][1] = com[0];
                    com[1] = com[1] + 1;
                    tmp.Add(com);
                }
            }
            commands = tmp;
        }

        public void checkIfTouchFood()
        {
            List<int> foodtmp = new List<int>();
            foreach (int pos in food)
            {
                if (snake[0][0] == pos)
                {
                    foodtmp.Add(randomFood());
                    int tmp1 = snake[snake.Count - 1][0];
                    int tmp2 = snake[snake.Count - 1][1];
                    if (tmp2 == 1)
                    {
                        tmp1 = tmp1 + 1;
                    }
                    else if (tmp2 == 2)
                    {
                        tmp1 = tmp1 - 80;
                    }
                    else if (tmp2 == 3)
                    {
                        tmp1 = tmp1 - 1;
                    }
                    else if (tmp2 == 4)
                    {
                        tmp1 = tmp1 + 80;
                    }
                    snake.Add(new int[] { tmp1, tmp2 });
                }
                else
                {
                    foodtmp.Add(pos);
                }
            }
            food = foodtmp;
        }
        void MainMenu()
        {
            // print current directory
            Console.Write("ADGOS - "+  Directory.GetCurrentDirectory() + ":");
            var input = Console.ReadLine();
            if (input == "cls")
            {
                Console.Clear();
            }
            // if input contains echo
            else if (input == "echo")
            {
                Console.Write("Text: ");
                var text = Console.ReadLine();
                Console.WriteLine(text);
            }
            else if (input == "shutdown")
            {
                Cosmos.System.Power.Shutdown();
            }
            else if (input == "reboot")
            {
                Cosmos.System.Power.Reboot();
            }
            else if (input == "version")
            {
                Console.WriteLine("ADGOS Version: " + version);
            }
            else if (input == "time")
            {
                Console.WriteLine("Time: " + DateTime.Now.ToString("HH:mm:ss"));
            }
            else if (input == "date")
            {
                Console.WriteLine("Date: " + DateTime.Now.ToString("dd/MM/yyyy"));
            }
            else if (input == "calc")
            {
                Console.WriteLine("Opening Calculator...");
            }
            else if (input == "notepad")
            {
                MIV.StartMIV();
            }
            else if (input == "snake")
            {
                Console.WriteLine("Opening Snake...");
                configSnake();
                ConsoleKey x;
                while (true)
                {
                    while (gameover())
                    {
                        printGame();
                        Boolean endGame = false;
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.R:
                                configSnake();
                                break;
                            case ConsoleKey.Escape:
                                endGame = true;
                                //clear console
                                Console.Clear();

                                break;
                        }

                        if (endGame)
                        {
                            break;
                        }
                    }
                    while (!Console.KeyAvailable && !gameover())
                    {

                        updateDirections();

                        updatePosotion();

                        checkIfTouchFood();

                        Console.Clear();
                        changeArray();
                        printGame();
                        delay(10000000);
                    }

                    x = Console.ReadKey(true).Key;

                    if (x == ConsoleKey.LeftArrow)
                    {
                        if (snake[0][1] != 3)
                        {
                            commands.Add(new int[2] { 1, 0 });
                        }
                    }
                    else if (x == ConsoleKey.UpArrow)
                    {
                        if (snake[0][1] != 2)
                        {
                            commands.Add(new int[2] { 4, 0 });
                        }
                    }
                    else if (x == ConsoleKey.RightArrow)
                    {
                        if (snake[0][1] != 1)
                        {
                            commands.Add(new int[2] { 3, 0 });
                        }
                    }
                    else if (x == ConsoleKey.DownArrow)
                    {
                        if (snake[0][1] != 4)
                        {
                            commands.Add(new int[2] { 2, 0 });
                        }
                    }
                    else if (x == ConsoleKey.Escape)
                    {
                        break;
                    }
                    else if (x == ConsoleKey.R)
                    {
                        configSnake();
                    }
                }
            }
            else if (input == "tetris")
            {
                Console.WriteLine("Opening Tetris...");

            }
            else if (input == "help")
            {
                Help();
            }
            else if (input == "cd")
            {
                cd();
            }
            else if (input == "ls")
            {
                ls();
            }
            else if (input == "specs")
            {
                specs();
            }
            else
            {
                Console.WriteLine("Command not found!");
            }
        }

        void Help()
        {
            Console.WriteLine("=========================");
            Console.WriteLine("ADGOS - Commands");
            Console.WriteLine("=========================");
            Console.WriteLine("cls - clears the screen");
            Console.WriteLine("echo - echos the text");
            Console.WriteLine("shutdown - shuts down the computer");
            Console.WriteLine("reboot - reboots the computer");
            Console.WriteLine("version - shows the version of ADGOS");
            Console.WriteLine("time - shows the time");
            Console.WriteLine("date - shows the date");
            Console.WriteLine("calc - opens the calculator");
            Console.WriteLine("notepad - opens notepad");
            Console.WriteLine("specs - shows the specs of the computer");
            Console.WriteLine("snake - opens snake");
            Console.WriteLine("tetris - opens tetris");
            Console.WriteLine("ls- shows the files in the current directory");
            Console.WriteLine("cd - changes the current directory");
        }

        void ls()
        {
            // list files in current directory
            Console.WriteLine("Files in current directory:");
            foreach (var file in Directory.GetFiles(@"0:\"))
            {
                Console.WriteLine(file);
            }
        }


        void cd()
        {
            // input directory
            Console.Write("Directory: ");
            var dir = Console.ReadLine();
    
            //if dir exists
            if (Directory.Exists(dir))
            {
                // change directory
                Directory.SetCurrentDirectory(dir);
            }
            else
            {
                Console.WriteLine("Directory does not exist!");
            }
        }


        void specs()
        {
            Console.WriteLine("CPU: " + Cosmos.Core.CPU.GetCPUBrandString());
            Console.WriteLine("RAM: " + Cosmos.Core.CPU.GetAmountOfRAM() + "MB");
        }

        protected override void Run()
        {
            MainMenu();

        }


    }
}
