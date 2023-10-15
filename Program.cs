using System;
using System.Collections.Generic;
using System.Reflection;
using CSharpExercisesTemplate.Interfaces;
using CSharpExercisesTemplate.Exercises;

namespace CSharpExercisesTemplate
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true) 
            {
                // Load exercises dynamically
                List<IExercise> exercises = LoadExercises();

                // Clear the console
                Console.Clear();

                // Display menu
                ShowMenuOptions(exercises);

                int choice = 0;

                // Get user choice
                try 
                {
                    choice = GetUserChoice(exercises);
                }
                catch (NullReferenceException ex)
                {
                    // Show error
                    Console.WriteLine($"Error: {ex.Message}\nCannot use white-space as option, please enter a value.");
                    // Pause the execution
                    Pause();
                    // Jump to next iteration
                    continue;
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}\nPlease enter a value from the options.");
                    Pause();
                    continue;
                }
                
                // Exit point
                if (choice == 0)
                    break;

                // Execute the selected exercise
                exercises[choice - 1].Execute();

                // Pause the execution
                Pause();
            }
        }

        /*
            They need to be static unless you want to instantiate the Program class in the Main method.

            This is just a loop that prints the options based on the exercise List.
        */ 
        static void ShowMenuOptions(List<IExercise> exercises)
        {
            Console.WriteLine("Select an exercise to run:\n");
            for (int i = 0; i < exercises.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] - {exercises[i].GetType().Name}");
            }
            Console.WriteLine($"[0] - Exit");
        }

        /*
            Just an input read with exception handling, 
            i have to avoid the user entering shit other thant the number of the exercises.

            This returns the valid integer values or throws exceptions.
        */
        static int GetUserChoice(List<IExercise> exercises, string msg = "\nEnter your option: ")
        {
            Console.Write(msg);
            string choice = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("");

            if (choice == string.Empty)
            {
                throw new NullReferenceException("Empty value.");
            }

            int choice_number = int.Parse(choice);

            if (choice_number < 0 || choice_number > exercises.Count)
            {
                throw new IndexOutOfRangeException("Out of range value.");
            }

            return choice_number;
        }


        /*
            Uses reflection and search the assemblies to get the exercise files,
            makes a list and returns them.
        */
        static List<IExercise> LoadExercises()
        {
            List<IExercise> exercises = new List<IExercise>();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (typeof(IExercise).IsAssignableFrom(type) && !type.IsInterface)
                {
                    IExercise exercise = (IExercise)Activator.CreateInstance(type);
                    exercises.Add(exercise);
                }
            }

            return exercises;
        }

        /*
            Typical stop of the program execution
        */
        static void Pause()
        {
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
