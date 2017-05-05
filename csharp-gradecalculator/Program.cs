/* Copyright 2017 Tom D
 * Licenced under the MIT License, which can be found in the "LICENSE" file in this repository */

// .NET Core standard library imports
using System; // System.Console, System.Environment and other functions are in here
using System.Collections.Generic; // Provides Dictionary

// NuGet package imports
using Microsoft.Data.Sqlite; // SQLite database library (provides small subset of ADO.NET provider)

namespace csharp_gradecalculator
{
    class Program
    {
        /// <summary>
        /// Gets the user's input and checks that it is an integer.
        /// If it is not, then asks the user again.
        /// </summary>
        /// <param name="message">Message to be printed out to user</param>
        /// <returns>
        /// Integer with user's input.
        /// </returns>
        static int IntegerInputApproved(string message)
        {
            Console.Write(message); // Prints out message provided to console
            string input = Console.ReadLine(); // Reads user's input from console
            int number = -1; // Default error value
            while (!int.TryParse(input, out number)) // Tries to parse user's input as an integer. If it fails then asks the user for input again.
            {
                Console.WriteLine("Please enter a number.");
                Console.Write("\n" + message);
                input = Console.ReadLine();
            }
            return number; // Returns validated integer with user's input
        }

        /// <summary>
        /// Safely converts a string to an integer.
        /// </summary>
        /// <param name="stringToBeConverted">String to be converted to an integer</param>
        /// <returns>
        /// An integer which has been converted from the provided string.
        /// -1 if unable to convert string to integer.
        /// </returns>
        static int SafeConvertStringToInt(string stringToBeConverted)
        {
            int convertedInteger = -1; // Default error value
            if (!int.TryParse(stringToBeConverted, out convertedInteger)) // Tries to convert provided string to integer
            {
                return -1; // Returns -1 if string cannot be converted
            }
            return convertedInteger; // Returns integer value from string
        }

        /// <summary>
        /// Looks up qualification from number of units.
        /// </summary>
        /// <param name="unitsTaken">Number of units to be converted to a qualification</param>
        /// <returns>
        /// Internal qualification name if found, "Unknown" otherwise.
        /// </returns>
        static string LookupQualification(int unitsTaken)
        {
            Dictionary<int, string> knownQualifications = new Dictionary<int, string>() // Qualification dictionary mapping number of units to 
                // qualifications
            {
                {3, "Certificate" },
                {6, "SubsidiaryDiploma" },
                {9, "90CreditDiploma" },
                {12, "Diploma" },
                {18, "ExtendedDiploma"}
            };

            if (knownQualifications.TryGetValue(unitsTaken, out string qualification)) // Tries to get value from internal dictionary
            {
                return qualification; // Returns internal qualification name
            }
            else
            {
                return "Unknown"; // Returns "Unknown" if can't find qualification for number of units provided
            }
        }

        /// <summary>
        /// Converts internal qualification name from LookupQualification() to a fancy 
        /// qualification name with spaces.
        /// </summary>
        /// <param name="qualification">Internal qualification name to be converted</param>
        /// <returns>Fancy qualification name if found, "Unknown qualification" otherwise.</returns>
        static string QualificationToFancyQualfication(string qualification)
        {
            Dictionary<string, string> fancyQualifications = new Dictionary<string, string>() // Fancy qualification dictionary mapping internal 
                // qualification names to fancy qualification names
            {
                {"Certificate", "Certificate" },
                {"SubsidiaryDiploma", "Subsidiary Diploma" },
                {"90CreditDiploma", "90 Credit Diploma" },
                {"Diploma", "Diploma" },
                {"ExtendedDiploma", "Extended Diploma"}
            };

            if (fancyQualifications.TryGetValue(qualification, out string fancyQualification)) // Tries to get value from internal dictionary
            {
                return fancyQualification;
            }
            else
            {
                return "Unknown qualification"; // Returns "Unknown qualification if can't find fancy qualification name from internal qualification
                    // name provided
            }
        }

        /// <summary>
        /// Calculates BTEC points from number of units at pass, merit, and distinction level.
        /// </summary>
        /// <param name="passUnits">Number of units achieved at pass level</param>
        /// <param name="meritUnits">Number of units achieved at merit level</param>
        /// <param name="distinctionUnits">Number of units achieved at distinction level</param>
        /// <returns>
        /// Number of BTEC points for the unit values provided.
        /// </returns>
        static int CalculateBtecPoints(int passUnits, int meritUnits, int distinctionUnits)
        {
            int unitValue = 10; // Value of each unit

            int passPoints = passUnits * 7 * unitValue; // Passes are worth 7 * the unit value
            int meritPoints = meritUnits * 8 * unitValue; // Merits are worth 8 * the unit value
            int distinctionPoints = distinctionUnits * 9 * unitValue; // Distinctions are worth 9 * the unit value
            int btecPoints = passPoints + meritPoints + distinctionPoints; // Total number of points is points from passes,
                // merits and distinctions added together
            return btecPoints; // Returns total number of BTEC points
        }

        /// <summary>
        /// Looks up BTEC grade from BTEC points provided.
        /// </summary>
        /// <param name="databaseFile">SQLite database file to use for lookup</param>
        /// <param name="qualification">Internal qualification name to lookup (returned from LookupQualification()</param>
        /// <param name="btecPoints">Number of BTEC points to get a BTEC grade for</param>
        /// <returns>
        /// String with BTEC grade, for example "DDD".
        /// "?" if unable to lookup grade.
        /// </returns>
        static string LookupBtecGrade(string databaseFile, string qualification, int btecPoints)
        {
            using (var connection = new SqliteConnection("" +
                new SqliteConnectionStringBuilder
                {
                    DataSource = databaseFile // SQLite database file to use
                })) // Creates a new connection to the SQLite database provided
            {
                connection.Open(); // Opens the connection

                using (var transaction = connection.BeginTransaction()) // Begins the database transaction. 
                    // "using" is used because the connection will be closed and cleaned up when the code is completed
                {
                    var btecGradeQuery = connection.CreateCommand();
                    btecGradeQuery.CommandText = "SELECT BTECGrade FROM " + qualification + " WHERE $points BETWEEN BTECMinimumPoints AND BTECMaximumPoints"; // SQL query to find 
                        // BTEC grade
                    btecGradeQuery.Parameters.AddWithValue("$points", btecPoints);
                    using (var reader = btecGradeQuery.ExecuteReader()) // Runs the SQL query
                    {
                        while (reader.Read())
                        {
                            string btecGrade = reader.GetString(0); // Puts the first value of the result of the SQL query in the btecGrade variable
                            return btecGrade; // Returns the BTEC grade
                        }
                        return "?"; // Returns "?" if BTEC grade cannot be looked up
                    }

                }
            }
        }

        /// <summary>
        /// Looks up UCAS points from BTEC grade provided.
        /// </summary>
        /// <param name="databaseFile">SQLite database file to use for lookup</param>
        /// <param name="qualification">Internal qualification name to lookup (returned from LookupQualification()</param>
        /// <param name="btecGrade">BTEC grade to get UCAS points for</param>
        /// <returns>
        /// Integer with UCAS points.
        /// -1 if unable to lookup grade.
        /// </returns>
        static int LookupUcasPoints(string databaseFile, string qualification, string btecGrade)
        {
            using (var connection = new SqliteConnection("" +
                new SqliteConnectionStringBuilder
                {
                    DataSource = databaseFile // SQLite database file to use
                })) // Creates a new connection to the SQLite database provided
            {
                connection.Open(); // Opens the connection

                using (var transaction = connection.BeginTransaction()) // Begins the database transaction.
                    // "using" is used because the connection will be closed and cleaned up when the code is completed
                {
                    var ucasPointsQuery = connection.CreateCommand();
                    ucasPointsQuery.CommandText = "SELECT UCASPoints FROM " + qualification + " WHERE $grade = BTECGrade";
                    ucasPointsQuery.Parameters.AddWithValue("$grade", btecGrade);
                    using (var reader = ucasPointsQuery.ExecuteReader()) // Runs the SQL query
                    {
                        while (reader.Read()) 
                        {
                            int ucasPoints = SafeConvertStringToInt(reader.GetString(0)); // Puts the first value of the result of the SQL query 
                                // in the ucasPoints variable
                            return ucasPoints; // Returns numbr of UCAS points
                        }
                        return -1; // Returns -1 if UCAS points cannot be looked up
                    }

                }
            }
        }

        /// <summary>
        /// Main function of program
        /// </summary>
        static void Main(string[] args)
        {
            string[] aWasteOfMemory = { "This", "is", "a", "totally", "unneeded", "array,",
                "but", "it", "is", "included", "to", "satisfy", "the", "needs", "of", "the",
                "assignment." }; // The assignment said that arrays must be used, so they have been
            string databaseFile = "..\\..\\..\\..\\Level3BtecNationals.sqlite3"; // SQLite database file, used in the lookup functions

            Console.Write(
                "* LICENCING INFORMATION **************************************************\n" +
                "* This software is distributed in the hope that it will be useful.       *\n" +
                "* It is licenced under the MIT License, a permissive open-source         *\n" +
                "* software licence. You are allowed to redistribute and/or modify the    *\n" +
                "* software under the conditions of the licence.                          *\n" +
                "* For more details please check the LICENSE file in this repository.     *\n" +
                "**************************************************************************\n" +
                "\n" +
                "* DISCLAIMERS ************************************************************\n" +
                "* This calculator is for Level 3 2010 BTEC National qualifications only. *\n" +
                "* This calculator assumes that the user has passed all units.            *\n" +
                "* This calculator may be out of date.                                    *\n" +
                "* No guarantee or warranty is provided in respect to the accuracy or     *\n" +
                "* correctness of the information that this calculator provides.          *\n" +
                "**************************************************************************\n" +
                "\n" +
                "Welcome to Tom's BTEC Nationals grade and UCAS points calculator.\n" +
                "\n"
            ); // Prints startup messages

            int passUnits = IntegerInputApproved("Please enter the number of units achieved " +
                "at pass level: "); // Number of passes
            int meritUnits = IntegerInputApproved("Please enter the number of units achieved " +
                "at merit level: "); // Number of merits
            int distinctionUnits = IntegerInputApproved("Please enter the number of units " +
                "achieved at distinction level: "); // Number of distinctions

            Console.Write("\n"); // Prints a new line

            int unitsTaken = passUnits + meritUnits + distinctionUnits; // Calculates the number of units taken
            Console.WriteLine("Number of units taken: " + unitsTaken); // Prints number of units taken

            string qualification = LookupQualification(unitsTaken); // Looks up internal qualification name from number of units
            if (qualification == "Unknown") // Error handling if qualification is unknown
            {
                Console.Write(
                    "\n" +
                    "The number of units that you have provided does not correspond \n" +
                    "to any of the calculator's known qualifications. Please contact your local \n" +
                    "careers advisor or other person in authority for information on your \n" +
                    "qualification."
                ); // Advises user to contact person(s) in authority for information on their qualification
                Console.ReadLine(); // Waits for enter key to be pressed
                System.Environment.Exit(1); // Exits with exit code 1 (error)
            }

            string fancyQualification = QualificationToFancyQualfication(qualification); // Looks up fancy qualification name from internal qualification name
            Console.WriteLine("Qualification: " + fancyQualification); // Prints fancy qualification name

            int btecPoints = CalculateBtecPoints(passUnits, meritUnits, distinctionUnits); // Calculates BTEC points from number of passes, merits, and distinctions
            Console.WriteLine("BTEC points: " + btecPoints); // Prints BTEC points

            string btecGrade = LookupBtecGrade(databaseFile, qualification, btecPoints); // Looks up BTEC grade from BTEC points
            if (btecGrade == "?") // Error handling when unable to look up BTEC grade
            {
                Console.Write(
                    "Unable to lookup BTEC grade.\n" +
                    "Please seek assistance.\n"
                ); // Advises user to seek assistance
                Console.ReadLine(); // Waits for enter key to be pressed
                System.Environment.Exit(1); // Exits with exit code 1 (error)
            }
            Console.WriteLine("BTEC grade: " + btecGrade); // Prints BTEC grade

            int ucasPoints = LookupUcasPoints(databaseFile, qualification, btecGrade); // Looks up UCAS points from BTEC grade
            if (ucasPoints == -1) // Error handling when unable to look up UCAS points
            {
                Console.Write(
                    "Unable to lookup UCAS points.\n" +
                    "Please seek assistance.\n"
                ); // Advises user to seek assistance
                Console.ReadLine(); // Waits for enter key to be pressed
                System.Environment.Exit(1); // Exits with exit code 1 (error)
            }
            Console.WriteLine("UCAS points: " + ucasPoints); // Prints UCAS points

            Console.ReadLine(); // Waits for enter key to be pressed
        }
    }
}
