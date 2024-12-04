using CityPowerAndLight.Model;
using CityPowerAndLight.Service;
using CityPowerAndLight.Utils;
using Microsoft.Xrm.Sdk;
using System;
using System.Text.RegularExpressions;

namespace CityPowerAndLight.Controller
{
    internal class AccountController
    {
        private readonly EntityService<Account> _accountService;

        public AccountController(EntityService<Account> accountService)
        {
            _accountService = accountService;
        }

        // Read all accounts
        internal void ReadAll()
        {
            var accounts = _accountService.GetAll();

            // Define columns and their widths
            string[] headers = { "Id", "Name", "Email", "Phone Number" };
            int[] columnWidths = { 40, 30, 30, 15 };

            // Print headers
            ConsoleFormatter.PrintTableHeader(headers, columnWidths);

            // Print the account details
            foreach (Account account in accounts)
            {
                string[] rowData = {
                    account.Id.ToString(),
                    account.Name,
                    account.EMailAddress1 ?? "N/A",
                    account.Telephone1 ?? "N/A"
                };

                ConsoleFormatter.PrintTableRow(rowData, columnWidths);
            }
        }

        // Create a new account
        internal Guid Create(string name, string email, string phoneNumber)
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

            // Call the service to create the account and return the account Id
            return _accountService.Create(newAccount);
        }

        // Delete an account by ID
        internal void Delete(Guid accountId)
        {
            if (accountId == Guid.Empty)
                throw new ArgumentException("Invalid account ID.");
            
            _accountService.Delete(accountId);
        }

        // Update an existing account
        internal void Update(Guid accountId, string name, string email, string phoneNumber, int numberOfEmployees, decimal revenue, account_statuscode statusCode)
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

        // Update an existing account (overloaded method)
        internal void Update(Guid accountId, string name, string email, string phoneNumber)
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

        // Helper method to validate email format
        private bool IsValidEmail(string email)
        {
            // Basic regex for validating email format
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
