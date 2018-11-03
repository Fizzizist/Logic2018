/*
Main class for Logic 2018 - a program for solving logical derivations
-Problem sets taken from Logic 2010 https://logiclx.humnet.ucla.edu/
Author: Peter Vlasveld
*/
using System;
using System.Collections.Generic;
using System.IO;

namespace Logic2018
{
	//main class
    class Program
    {
        
        static void Main(string[] args)
        {
		//variables
            string userID = null;
            var stillRunning = true;
            var saveCloud = new SaveCloud();
            var mainInventory = new List<Premise>();
            var writer = new Reader();
	        Console.OutputEncoding = System.Text.Encoding.UTF8;
            //Testing
            /*Console.WriteLine("Premise:");
            var testingCommand = Console.ReadLine();
            problemConstructor = new ProblemConstructor();
            var testPremise = problemConstructor.MakeCustom(testingCommand);
            Console.WriteLine(testPremise.GetPremise());*/
            
		//check connection to database
	    if (saveCloud.CheckConnection())
            {
                
            }
            else
            {
                Console.WriteLine("It appears that there is a problem with the internet connection.");
                Console.WriteLine("This program uses the internet to generate problem sets.");
                Console.WriteLine("Please reconnect to the internet before playing.");
                stillRunning = false;
                goto MainMenu;
            }
            
		//user loop
            InitialLoop:
            var initialInt = 0;

            //user menu
            Console.WriteLine("Choose from the following options:");
            Console.WriteLine("1. New User");
            Console.WriteLine("2. Existing User");
            Console.Write("Input:");

		//error checking
            try
            {
                initialInt = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("That's not a number. Try again.");
                goto InitialLoop;
            }

		//select input
            switch (initialInt)
            {
                case 1:
                    saveCloud.CreateNewUser();
                    userID = saveCloud.GetUserID();
                    break;
                case 2:
                    Console.Write("User ID:");
                    userID = Console.ReadLine();
                    System.Console.Write("password: ");
                    string password = null;
                    while (true)
                    {
                        var key = System.Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                            //Console.WriteLine(' ');
                            break;
                        password += key.KeyChar;
                        Console.Write('*');
                    }
                    if (saveCloud.UserAuthenticate(userID, password))
                    {
                        saveCloud.UserTableCheck(userID);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("UserID and password did not match what is in the system. Please try again.");
                        goto InitialLoop;
                    }
                default:
                    Console.WriteLine("Invalid response. Try again.");
                    goto InitialLoop;
            }
            writer.AddTenBlankLines();
            writer.ReadWholeFile("textFiles/Intro.txt");

		//main menu
            MainMenu:
            while (stillRunning)
            {
		//main menu
                Console.WriteLine("Choose from the following menu options:");
                Console.WriteLine("1. Tutorials");
                Console.WriteLine("2. Problem set 1 (Working with Conditionals)");
                Console.WriteLine("3. Problem Set 2 (Intro to Theorems)");
                Console.WriteLine("4. Problem Set 3 (Final Conditional Battle)");
                Console.WriteLine("5. Problem Set 4 (Welcome to the AND, OR and Biconditional)");
                Console.WriteLine("6. Problem Set 5 (Practicing Makes Perfect)");
                int mainChoice = 0;
                var mainInput = "";
                
		//error checking
		try
                {
                    mainInput = Console.ReadLine();
                    mainChoice = Convert.ToInt32(mainInput);
                }
                catch (Exception)
                {
                    if (mainInput == "exit") 
                    {
                        stillRunning = false;
                        goto MainMenu;
                    }
                    /*else if (mainInput == "fill-table")  //for updating argument_display tables.
                    {
                        Console.Write("Which table?");
                        var tableInput = Convert.ToInt32(Console.ReadLine());
                        saveCloud.FillArgumentDisplayTable(tableInput);
                    }*/
                    else 
                    {
                        Console.WriteLine("That is not a valid choice. Try Again");
                        goto MainMenu;
                    }
                }
		//select choice
                switch (mainChoice)
                {
                    case 1:
                        TutorialMenu:
                        Console.WriteLine("Choose a tutorial, or type 'exit' to return to the main menu:");
                        Console.WriteLine("1. The Very Basics (Direct Derivations)");
                        Console.WriteLine("2. Indirect Derivations");
                        Console.WriteLine("3. Conditional Derivations and extra Show statements");
                        Console.Write("Choice:");
                        var tutorialChoice = Console.ReadLine();
                        var tutorialChoiceInt = 0;
                        Tutorial tutorial;
                        try
                        {
                            tutorialChoiceInt = Convert.ToInt32(tutorialChoice);
                        }
                        catch (Exception)
                        {
                            if (tutorialChoice=="exit") goto MainMenu;
                            else 
                            {
                                Console.WriteLine("That is not even a number. Try Again, or type 'exit' to go to main menu.");
                                goto TutorialMenu;
                            }
                        }
                        switch (tutorialChoiceInt)
                        {
                            case 1:
                                tutorial = new Tutorial(1,userID);
                                goto MainMenu;
                            case 2:
                                tutorial = new Tutorial(2,userID);
                                goto MainMenu;
                            case 3:
                                tutorial = new Tutorial(3,userID);
                                goto MainMenu;
                            default:
                                Console.WriteLine("That is not a valid choice. Try Again or type 'exit' to go to main menu.");
                                goto TutorialMenu;

                        }
                    case 2: case 3: case 4: case 5: case 6:
                        var problemSet = new ProblemSet((mainChoice-1), userID);
                        goto MainMenu;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        goto MainMenu;
                }
            }
        }
    }
}
