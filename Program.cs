using System;
using CSharpExercisesTemplate;

namespace CSharpExercisesTemplate
{
    public class Program
    {
        private static ExercisesApp? _exercisesApp;

        static void Main(string[] args)
        {
            _exercisesApp = new ExercisesApp();
            _exercisesApp.Run();
        }
    }
}
