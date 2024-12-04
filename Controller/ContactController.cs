using CityPowerAndLight.Model;
using CityPowerAndLight.Service;
using CityPowerAndLight.Utils;
using Microsoft.Xrm.Sdk;
using System;

namespace CityPowerAndLight.Controller
{
    internal class ContactController
    {
        private readonly EntityService<Contact> _contactService;

        public ContactController(EntityService<Contact> contactService)
        {
            _contactService = contactService;
        }

        // Read all contacts
        internal void ReadAll()
        {
            var contacts = _contactService.GetAll();

            // Define columns and their widths
            string[] headers = { "Id", "Full Name", "Company Name", "Email Address" };
            int[] columnWidths = { 40, 30, 30, 35 };

            // Print headers
            ConsoleFormatter.PrintTableHeader(headers, columnWidths);

            // Print the contact details
            foreach (Contact contact in contacts)
            {
                string[] rowData = {
                    contact.Id.ToString(),
                    $"{contact.FirstName} {contact.LastName}".Trim(),
                    contact.Company ?? "N/A",
                    contact.EMailAddress1 ?? "N/A"
                };

                ConsoleFormatter.PrintTableRow(rowData, columnWidths);
            }
        }

        // Create a new contact
        internal Guid Create(string firstName, string lastName, string company, string email /*, string mobilePhone */)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(company))
                throw new ArgumentException("Company name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                throw new ArgumentException("Invalid email address.");

            Contact newContact = new()
            {
                FirstName = firstName,
                LastName = lastName,
                Company = company,
                EMailAddress1 = email,
                // MobilePhone = mobilePhone, // Commented out
            };

            Guid contactId = _contactService.Create(newContact);
            Console.WriteLine($"Created Contact ID: {contactId}");
            return contactId;
        }

        // Delete a contact by ID
        internal void Delete(Guid contactId)
        {
            // Validate input
            if (contactId == Guid.Empty)
                throw new ArgumentException("Invalid contact ID.");

            _contactService.Delete(contactId);
            Console.WriteLine($"Deleted Contact ID: {contactId}");
        }

        // Update an existing contact with full parameters
        internal void Update(Guid contactId, string firstName, string lastName, string company, string email /*, string mobilePhone */)
        {
            // Validate inputs
            if (contactId == Guid.Empty)
                throw new ArgumentException("Invalid contact ID.");
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(company))
                throw new ArgumentException("Company name cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                throw new ArgumentException("Invalid email address.");

            Contact updatedContact = new()
            {
                Id = contactId,
                FirstName = firstName,
                LastName = lastName,
                Company = company,
                EMailAddress1 = email,
                // MobilePhone = mobilePhone, // Commented out
            };

            _contactService.Update(updatedContact);
            Console.WriteLine($"Contact with ID {contactId} updated.");
        }

        // Overloaded Update method with minimal parameters
        internal void Update(Guid contactId, string email)
        {
            // Validate inputs
            if (contactId == Guid.Empty)
                throw new ArgumentException("Invalid contact ID.");
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
                throw new ArgumentException("Invalid email address.");

            Contact updatedContact = new()
            {
                Id = contactId,
                EMailAddress1 = email,
            };

            _contactService.Update(updatedContact);
            Console.WriteLine($"Contact with ID {contactId} updated with new email.");
        }

        // Helper method to validate email
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
