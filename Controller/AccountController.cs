using CityPowerAndLight.Model;
using CityPowerAndLight.Service;
using CityPowerAndLight.Utils;
using Microsoft.Xrm.Sdk;
using System.Text.RegularExpressions;

namespace CityPowerAndLight.Controller
{
    /// <summary>
    /// Controller responsible for handling account-related operations such as Create, Read, Update, and Delete (CRUD).
    /// </summary>
    internal class AccountController
    {
        private readonly EntityService<Account> _accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="accountService">An instance of <see cref="EntityService{Account}"/> used to interact with the account data.</param>
        public AccountController(EntityService<Account> accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Reads all accounts and prints them in a table format.
        /// </summary>
        internal void ReadAll()
        {
            try
            {
                var accounts = _accountService.GetAll();

                // Define columns and their widths
                string[] headers = { "Id", "Name", "Email", "Phone Number" };
                int[] columnWidths = { 40, 30, 35, 15 };

                // Print headers
                ConsoleFormatter.PrintTableHeader(headers, columnWidths);

                // Print the account details
                foreach (Account account in accounts)
                {
                    string[] rowData = {
                        account.Id.ToString(),
                        account.Name ?? "N/A", // Default to "N/A" if Name is null
                        account.EMailAddress1 ?? "N/A", // Default to "N/A" if Email is null
                        account.Telephone1 ?? "N/A" // Default to "N/A" if PhoneNumber is null
                    };

                    ConsoleFormatter.PrintTableRow(rowData, columnWidths);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading accounts: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new account with the provided details.
        /// </summary>
        /// <param name="name">The name of the account.</param>
        /// <param name="email">The email address associated with the account.</param>
        /// <param name="phoneNumber">The phone number associated with the account.</param>
        /// <returns>The unique identifier (GUID) of the created account.</returns>
        /// <exception cref="ArgumentException">Thrown if any of the input parameters are invalid.</exception>
        internal Guid Create(string name, string email, string phoneNumber)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be null or empty.");
                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                    throw new ArgumentException("Invalid email address.");
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    throw new ArgumentException("Phone number cannot be null or empty.");

                // Create a new Account with the provided variables
                Account newAccount = new()
                {
                    Name = name,
                    EMailAddress1 = email,         // Set Email
                    Telephone1 = phoneNumber,      // Set Phone Number
                    CreditOnHold = false,
                    LastOnHoldTime = new DateTime(2023, 1, 1),
                    Address1_Latitude = 47.642311,
                    Address1_Longitude = -122.136841,
                    NumberOfEmployees = 500,
                    Revenue = new Money(new decimal(5000000.00)),
                    AccountCategoryCode = account_accountcategorycode.PreferredCustomer,
                    StatusCode = account_statuscode.Active,
                };
                
                return _accountService.Create(newAccount);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
                throw;  // Re-throw the exception for further handling if needed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the account: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes an account by its unique identifier (GUID).
        /// </summary>
        /// <param name="accountId">The unique identifier (GUID) of the account to be deleted.</param>
        /// <exception cref="ArgumentException">Thrown if the provided account ID is invalid.</exception>
        internal void Delete(Guid accountId)
        {
            try
            {
                if (accountId == Guid.Empty)
                    throw new ArgumentException("Invalid account ID.");
                
                _accountService.Delete(accountId);
                Console.WriteLine($"Account with ID {accountId} deleted.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the account: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing account with the specified details.
        /// </summary>
        /// <param name="accountId">The unique identifier (GUID) of the account to be updated.</param>
        /// <param name="name">The updated name of the account.</param>
        /// <param name="email">The updated email address of the account.</param>
        /// <param name="phoneNumber">The updated phone number of the account.</param>
        /// <param name="numberOfEmployees">The updated number of employees associated with the account.</param>
        /// <param name="revenue">The updated revenue associated with the account.</param>
        /// <param name="statusCode">The updated status code of the account.</param>
        /// <exception cref="ArgumentException">Thrown if any of the input parameters are invalid.</exception>
        internal void Update(Guid accountId, string name, string email, string phoneNumber, int numberOfEmployees, decimal revenue, account_statuscode statusCode)
        {
            try
            {
                // Validate inputs
                if (accountId == Guid.Empty)
                    throw new ArgumentException("Invalid account ID.");
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be null or empty.");
                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                    throw new ArgumentException("Invalid email address.");
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    throw new ArgumentException("Phone number cannot be null or empty.");
                if (numberOfEmployees <= 0)
                    throw new ArgumentException("Number of employees must be greater than zero.");
                if (revenue <= 0)
                    throw new ArgumentException("Revenue must be greater than zero.");

                // Fetch the account first (you may need to retrieve it from GetAll or use a specific query)
                Account updatedAccount = new()
                {
                    Id = accountId, // Ensure that we are updating the existing account
                    Name = name, // Update name with the provided parameter
                    EMailAddress1 = email, // Update email with the provided parameter
                    Telephone1 = phoneNumber, // Update phone number with the provided parameter
                    NumberOfEmployees = numberOfEmployees, // Update number of employees
                    Revenue = new Money(revenue), // Update revenue
                    StatusCode = statusCode // Update status code with the provided parameter
                };

                // Update the account in Dataverse
                _accountService.Update(updatedAccount);
                Console.WriteLine($"Account with ID {accountId} updated.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the account: {ex.Message}");
            }
        }

        /// <summary>
        /// Overloaded method that updates an account with the provided basic details.
        /// </summary>
        /// <param name="accountId">The unique identifier (GUID) of the account to be updated.</param>
        /// <param name="name">The updated name of the account.</param>
        /// <param name="email">The updated email address of the account.</param>
        /// <param name="phoneNumber">The updated phone number of the account.</param>
        /// <exception cref="ArgumentException">Thrown if any of the input parameters are invalid.</exception>
        internal void Update(Guid accountId, string name, string email, string phoneNumber)
        {
            try
            {
                // Validate inputs
                if (accountId == Guid.Empty)
                    throw new ArgumentException("Invalid account ID.");
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be null or empty.");
                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                    throw new ArgumentException("Invalid email address.");
                if (string.IsNullOrWhiteSpace(phoneNumber))
                    throw new ArgumentException("Phone number cannot be null or empty.");

                // Fetch the account first (you may need to retrieve it from GetAll or use a specific query)
                Account updatedAccount = new()
                {
                    Id = accountId, // Ensure that we are updating the existing account
                    Name = name, // Update name with the provided parameter
                    EMailAddress1 = email, // Update email with the provided parameter
                    Telephone1 = phoneNumber, // Update phone number with the provided parameter
                    NumberOfEmployees = 500, // Update number of employees
                    Revenue = new Money(new decimal(5000000.00)), // Update revenue
                    StatusCode = account_statuscode.Active, // Update status code with the provided parameter
                };

                // Update the account in Dataverse
                _accountService.Update(updatedAccount);
                Console.WriteLine($"Account with ID {accountId} updated.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the account: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates the format of an email address.
        /// </summary>
        /// <param name="email">The email address to be validated.</param>
        /// <returns><c>true</c> if the email format is valid; otherwise, <c>false</c>.</returns>
        private bool IsValidEmail(string email)
        {
            // Basic regex for validating email format
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
