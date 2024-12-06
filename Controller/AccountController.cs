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
                string[] headers = { "Id", "Account Name", "Main Phone", "Address1: City", "Primary Contact" };
                int[] columnWidths = { 40, 30, 20, 25, 30 };

                // Print headers
                ConsoleFormatter.PrintTableHeader(headers, columnWidths);

                // Print the account details
                foreach (Account account in accounts)
                {
                    string[] rowData =
                    {
                        account.Id.ToString(),
                        account.Name ?? "N/A", // Default to "N/A" if Name is null
                        account.Telephone1 ?? "N/A", // Default to "N/A" if PhoneNumber is null
                        account.Address1_City ?? "N/A", // Default to "N/A" if Address1_City is null
                        account.PrimaryContactId?.ToString() ?? "N/A" // Default to "N/A" if Primary Contact is null
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
        /// <param name="city">The city associated with the account's address.</param>
        /// <param name="primaryContactId">The GUID of the primary contact associated with the account.</param>
        /// <returns>The unique identifier (GUID) of the created account.</returns>
        /// <exception cref="ArgumentException">Thrown if any of the input parameters are invalid.</exception>
        internal Guid Create(string name, string email, string phoneNumber, string city, Guid? primaryContactId)
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
                if (string.IsNullOrWhiteSpace(city))
                    throw new ArgumentException("City cannot be null or empty.");

                // Create a new Account with the provided variables
                Account newAccount = new()
                {
                    Name = name,
                    EMailAddress1 = email, // Set Email
                    Telephone1 = phoneNumber, // Set Phone Number
                    Address1_City = city, // Set Address1: City
                    PrimaryContactId = primaryContactId.HasValue
                        ? new EntityReference("contact", primaryContactId.Value)
                        : null, // If no primary contact, set it to null
                };

                return _accountService.Create(newAccount);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
                throw; // Re-throw the exception for further handling if needed
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
        /// <param name="city">The updated city for the account's address.</param>
        /// <param name="primaryContactId">The updated primary contact ID for the account.</param>
        /// <param name="numberOfEmployees">The updated number of employees associated with the account.</param>
        /// <param name="revenue">The updated revenue associated with the account.</param>
        /// <param name="statusCode">The updated status code of the account.</param>
        /// <exception cref="ArgumentException">Thrown if any of the input parameters are invalid.</exception>
        internal void Update(Guid accountId, string name, string email, string phoneNumber, string city,
            Guid? primaryContactId)
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
                if (string.IsNullOrWhiteSpace(city))
                    throw new ArgumentException("City cannot be null or empty.");

                // Fetch the account first (you may need to retrieve it from GetAll or use a specific query)
                Account updatedAccount = new()
                {
                    Id = accountId, // Ensure that we are updating the existing account
                    Name = name, // Update name with the provided parameter
                    EMailAddress1 = email, // Update email with the provided parameter
                    Telephone1 = phoneNumber, // Update phone number with the provided parameter
                    Address1_City = city, // Update Address1: City
                    PrimaryContactId = primaryContactId.HasValue
                        ? new EntityReference("contact", primaryContactId.Value)
                        : null, // If no primary contact, set it to null
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
        /// Validates if the provided email address is in a correct format.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>true if the email is valid, false otherwise.</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return emailRegex.IsMatch(email);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}