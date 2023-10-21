using System;
using System.Collections.Generic;
using System.Reflection;
using CSharpExercisesTemplate.Interfaces;
using CSharpExercisesTemplate.Exercises;

namespace CSharpExercisesTemplate
{
    public class ExercisesApp
    {

        private int _choice = 0;
        private List<IExercise>? _exercises;

        /*
            Using the constructor to inicialize the properties
         */
        public ExercisesApp()
        {
            // Loading the exercises
            _exercises = GetExercises();
        }

        public void Run()
        {
            while (true) 
            {
                Console.Clear();
                // Display menu
                ShowMenuOptions();

                // Get user choice
                try 
                {
                    _choice = GetUserChoice();
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"[ERROR]: {ex.Message} Please enter a number from the options.");
                    // Pause the execution
                    Pause();
                    // Jump to next iteration
                    continue;
                }
                catch (IndexOutOfRangeException ex)
                {
                    Console.WriteLine($"[ERROR]: {ex.Message} Select a valid number from the options.");
                    Pause();
                    continue;
                }
                
                // Exit point
                if (_choice == 0)
                    break;

                // Execute the selected exercise
                _exercises[_choice - 1].Execute();

                // Pause the execution
                Pause();
            }
        }

        /*
            They need to be static unless you want to instantiate the Program class in the Main method.

            This is just a loop that prints the options based on the exercise List.
         */ 
        void ShowMenuOptions()
        {
            Console.WriteLine("Select an exercise to run:\n");

            for (int i = 0; i < _exercises.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] - {_exercises[i].GetType().Name}");
            }

            Console.WriteLine($"[0] - Exit");
        }

        /*
            Just an input read with exception handling, 
            i have to avoid the user entering shit other thant the number of the exercises.

            This returns the valid integer values or throws exceptions.
         */
        int GetUserChoice(string msg = "\nEnter your option: ")
        {
            int choiceNumber;
            string answer;

            Console.Write(msg);

            answer = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("");

            bool isValidType = int.TryParse(answer, out choiceNumber);

            if (!isValidType)
                throw new FormatException("Invalid value type.");

            if (choiceNumber < 0 || choiceNumber > _exercises.Count)
                throw new IndexOutOfRangeException("Out of range value.");

            return choiceNumber;
        }

        /*
            Uses reflection and search the assemblies to get the exercise files,
            makes a list and returns them.
         */
        List<IExercise> GetExercises()
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
        void Pause()
        {
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}