using CityPowerAndLight.Model;
using CityPowerAndLight.Controller;
using CityPowerAndLight.Utils;

namespace CityPowerAndLight.View
{
    /// <summary>
    /// Provides methods to run a series of operations on various controllers like Account, Contact, and Case.
    /// This class demonstrates creating, reading, updating, and deleting records using the controller classes.
    /// </summary>
    internal static class OperationRunner
    {
        /// <summary>
        /// Runs the operations for managing accounts, including creating, reading, updating, and deleting an account.
        /// Each operation is logged, and errors are handled through the <see cref="ErrorHandler"/>.
        /// </summary>
        /// <param name="accountController">An instance of the <see cref="AccountController"/> used to manage accounts.</param>
        public static void RunAccountOperations(AccountController accountController)
        {
            // 1. Create a New Account
            ConsoleFormatter.PrintHeader("1. Create a New Account");
            Guid newAccountId = ErrorHandler.HandleFunction(
                () => accountController.Create("Test Create", "TestCreate@outlook.com", "555-123-4567", "Test City",
                    null),
                "Create Account"
            );
            Console.WriteLine(newAccountId != Guid.Empty
                ? $"Created Account ID: {newAccountId}"
                : "Account creation failed.");
            Console.WriteLine();

            // 2. Read All Accounts
            ConsoleFormatter.PrintHeader("2. Read All Accounts");
            ErrorHandler.HandleAction(() => accountController.ReadAll(), "Read All Accounts");
            Console.WriteLine();

            // 3. Update the Newly Created Account
            ConsoleFormatter.PrintHeader("3. Update the Newly Created Account");
            ErrorHandler.HandleAction(
                () => accountController.Update(newAccountId, "UpdatedName", "UpdatedEmail@outlook.com", "555-987-6543",
                    "Updated City", null),
                "Update Account"
            );
            Console.WriteLine($"Updated Account ID: {newAccountId}");
            Console.WriteLine();

            // 4. Read All Accounts After Update
            ConsoleFormatter.PrintHeader("4. Read All Accounts After Update");
            ErrorHandler.HandleAction(() => accountController.ReadAll(), "Read All Accounts After Update");
            Console.WriteLine();

            // 5. Delete the Account
            ConsoleFormatter.PrintHeader("5. Delete the Account");
            ErrorHandler.HandleAction(() => accountController.Delete(newAccountId), "Delete Account");
            Console.WriteLine($"Deleted Account ID: {newAccountId}");
            Console.WriteLine();

            // 6. Read All Accounts After Deletion
            ConsoleFormatter.PrintHeader("6. Read All Accounts After Deletion");
            ErrorHandler.HandleAction(() => accountController.ReadAll(), "Read All Accounts After Deletion");
            Console.WriteLine("========================================");
        }

        /// <summary>
        /// Runs the operations for managing contacts, including creating, reading, updating, and deleting a contact.
        /// Each operation is logged, and errors are handled through the <see cref="ErrorHandler"/>.
        /// </summary>
        /// <param name="contactController">An instance of the <see cref="ContactController"/> used to manage contacts.</param>
        public static void RunContactOperations(ContactController contactController)
        {
            // 1. Create a New Contact
            ConsoleFormatter.PrintHeader("1. Create a New Contact");
            Guid newContactId = ErrorHandler.HandleFunction(
                () => contactController.Create("John", "Doe", "Acme Corp", "john.doe@outlook.com", "123-456-7890"),
                "Create Contact"
            );
            Console.WriteLine(newContactId != Guid.Empty
                ? $"Created Contact ID: {newContactId}"
                : "Contact creation failed.");
            Console.WriteLine();

            // 2. Read All Contacts
            ConsoleFormatter.PrintHeader("2. Read All Contacts");
            ErrorHandler.HandleAction(() => contactController.ReadAll(), "Read All Contacts");
            Console.WriteLine();

            // 3. Update the Newly Created Contact
            ConsoleFormatter.PrintHeader("3. Update the Newly Created Contact");
            ErrorHandler.HandleAction(
                () => contactController.Update(newContactId, "Jane", "Doe", "Acme Inc", "jane.doe@outlook.com",
                    "987-654-3210"),
                "Update Contact"
            );
            Console.WriteLine($"Updated Contact ID: {newContactId}");
            Console.WriteLine();

            // 4. Read All Contacts After Update
            ConsoleFormatter.PrintHeader("4. Read All Contacts After Update");
            ErrorHandler.HandleAction(() => contactController.ReadAll(), "Read All Contacts After Update");
            Console.WriteLine();

            // 5. Delete the Contact
            ConsoleFormatter.PrintHeader("5. Delete the Contact");
            ErrorHandler.HandleAction(() => contactController.Delete(newContactId), "Delete Contact");
            Console.WriteLine($"Deleted Contact ID: {newContactId}");
            Console.WriteLine();

            // 6. Read All Contacts After Deletion
            ConsoleFormatter.PrintHeader("6. Read All Contacts After Deletion");
            ErrorHandler.HandleAction(() => contactController.ReadAll(), "Read All Contacts After Deletion");
            Console.WriteLine("========================================");
        }


        /// <summary>
        /// Runs the operations for managing cases, including creating, reading, updating, and deleting a case.
        /// It also demonstrates how to link a case to a contact. Each operation is logged, and errors are handled through the <see cref="ErrorHandler"/>.
        /// </summary>
        /// <param name="caseController">An instance of the <see cref="CaseController"/> used to manage cases.</param>
        /// <param name="contactController">An instance of the <see cref="ContactController"/> used to manage contacts.</param>
        public static void RunCaseOperations(CaseController caseController, ContactController contactController)
        {
            try
            {
                // 1. Create a Contact for the Case
                ConsoleFormatter.PrintHeader("Creating a Contact for the Case");
                Guid newContactId = ErrorHandler.HandleFunction(
                    () => contactController.Create("Case", "Customer", "CC", "cc@outlook.com",
                        "123-456-7890"), // Added phone number as 5th argument
                    "Create Contact for Case"
                );
                Console.WriteLine();

                // 2. Create a New Case
                ConsoleFormatter.PrintHeader("1. Create a New Case");
                Guid newCaseId = ErrorHandler.HandleFunction(
                    () => caseController.Create("TestCase", "This is a test case",
                        newContactId), // Pass newContactId as customerId
                    "Create Case"
                );
                Console.WriteLine($"Created Case ID: {newCaseId}\n");

                // 3. Read All Cases
                ConsoleFormatter.PrintHeader("2. Read All Cases");
                ErrorHandler.HandleAction(() => caseController.ReadAll(), "Read All Cases");
                Console.WriteLine();

                // 4. Update the Newly Created Case
                ConsoleFormatter.PrintHeader("3. Update the Newly Created Case");
                ErrorHandler.HandleAction(
                    () => caseController.Update(newCaseId, "Updated TestCase", "Updated description",
                        incident_statuscode.InProgress, incident_prioritycode.Low),
                    "Update Case"
                );
                Console.WriteLine($"Updated Case ID: {newCaseId}\n");

                // 5. Read All Cases After Update
                ConsoleFormatter.PrintHeader("4. Read All Cases After Update");
                ErrorHandler.HandleAction(() => caseController.ReadAll(), "Read All Cases After Update");
                Console.WriteLine();

                // 6. Delete the Case
                ConsoleFormatter.PrintHeader("5. Delete the Case");
                ErrorHandler.HandleAction(() => caseController.Delete(newCaseId), "Delete Case");
                Console.WriteLine($"Deleted Case ID: {newCaseId}\n");

                // 7. Delete Associated Contact
                ConsoleFormatter.PrintHeader("Deleting Associated Contact");
                ErrorHandler.HandleAction(() => contactController.Delete(newContactId), "Delete Contact");
                Console.WriteLine();

                // 8. Read All Cases After Deletion
                ConsoleFormatter.PrintHeader("6. Read All Cases After Deletion");
                ErrorHandler.HandleAction(() => caseController.ReadAll(), "Read All Cases After Deletion");
                Console.WriteLine("========================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during case operations: {ex.Message}");
            }
        }
    }
}