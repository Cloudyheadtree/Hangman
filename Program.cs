using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HangmanConsole
{
    internal class Program
    {
        static bool gameOver = false;
        static bool guessedSuccessfully = false;
        static int totalNumberOfGuesses = 5;
        static bool keepPlaying = false;
        static Dictionary<string, List<string>> wordBank = new Dictionary<string, List<string>> {
            { "Food", new List<string>() { "Banana", "Cherry", "Grapes", "Cake" } },
            { "Animals", new List<string>() { "Lions", "Tigers", "Bears", "Human" } },
            { "Sports", new List<string>() { "Archery", "Skiing", "Boxing", "Swimming" } },
            { "Colors", new List<string>() { "Perrywinkle", "Red", "Lilac", "Indigo" } },
        };
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("Playing Hangman! The following are a list of the available categories");

                ShowCategoriesForSelection();
                int selectedCategoryIndex = GetUserSelectedCategory();
                string hangmanWord = GetRandomWordFromSelectedCategory(selectedCategoryIndex);
                PlayHangmanWithWord(hangmanWord);
                Console.Write("Would you like to play again? Y/n: ");
                string playAgain = Console.ReadKey().KeyChar.ToString();

                if (playAgain.ToLower() == "y")
                {
                    keepPlaying = true;
                    Console.Clear();

                }
            } while (keepPlaying);
            
        }

        private static void ShowCategoriesForSelection()
        {
            int keyIndex = 0;
            foreach (string key in wordBank.Keys)
            {
                Console.WriteLine(keyIndex + ": " + key);
                keyIndex++;
            }
        }

        private static int GetUserSelectedCategory()
        {

            int selectedValue = -1;

            while (selectedValue < 0 || selectedValue > wordBank.Count - 1)
            {
                Console.Write(Environment.NewLine + "Please choose a valid category by number: ");
                char selectedCategoryIndex = Console.ReadKey().KeyChar;
                selectedValue = (int)Char.GetNumericValue(selectedCategoryIndex);
            }

            return selectedValue;
        } 

        private static string GetRandomWordFromSelectedCategory(int selectedIndex)
        {
            Random rnd = new Random();
            string selectedWord = wordBank[wordBank.Keys.ElementAt(selectedIndex)].ElementAt(rnd.Next(wordBank.Count - 1));
            return selectedWord;
        }

        private static void PlayHangmanWithWord(string selectedWord)
        {
            List<string> invalidEntries = new List<string>();
            List<string> validEntries = new List<string>();
            while (!gameOver)
            {
                string maskedWord = GetMaskedWord(selectedWord, validEntries);
                if (maskedWord.IndexOf("_") == -1)
                {
                    guessedSuccessfully = true;
                    break;
                }
                
                Console.WriteLine(Environment.NewLine + maskedWord);
                Console.WriteLine("Incorrect guesses: " + string.Join(", ", invalidEntries));
                Console.WriteLine("Guesses left: " + (totalNumberOfGuesses - invalidEntries.Count));
                Console.Write("Would you like to guess another letter? ");

                string selectedCharacter = Console.ReadKey().KeyChar.ToString();
                if (Regex.IsMatch(selectedCharacter, @"^[a-zA-Z]+$"))
                {
                    if (selectedWord.ToLower().IndexOf(selectedCharacter.ToLower()) != -1)
                    {
                        if (validEntries.FirstOrDefault(a => a.ToLower() == selectedCharacter.ToLower()) == null)
                            validEntries.Add(selectedCharacter);
                    }
                    else
                    {
                        if (invalidEntries.FirstOrDefault(a => a.ToLower() == selectedCharacter.ToLower()) == null)
                            invalidEntries.Add(selectedCharacter);
                    }

                    gameOver = (totalNumberOfGuesses - invalidEntries.Count) == 0;
                    Console.Write(Environment.NewLine + "------------------------------------------------------------------------------");
                }
               
            }

            Console.WriteLine(Environment.NewLine + (guessedSuccessfully ? "Congratulations! You guessed the word!" : "Sorry. Better luck next time!"));
        }

        private static string GetMaskedWord(string selectedWord, List<string> validCharacters)
        {
            StringBuilder maskedString = new StringBuilder();
            string[] characterList = selectedWord.ToCharArray().Select(c => c.ToString()).ToArray();
            foreach (string character in characterList)
            {
                maskedString.Append(validCharacters.FirstOrDefault(a => a.ToLower() == character.ToLower()) != null ? character : "_");
            }
            return maskedString.ToString();
        }

    }
}
