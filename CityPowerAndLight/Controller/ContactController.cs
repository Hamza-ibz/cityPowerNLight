using CityPowerAndLight.Model;
using CityPowerAndLight.Service;
using CityPowerAndLight.Utils;

namespace CityPowerAndLight.Controller
{
    /// <summary>
    /// Provides CRUD operations for managing contacts.
    /// </summary>
    internal class ContactController
    {
        private readonly EntityService<Contact> _contactService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactController"/> class.
        /// </summary>
        /// <param name="contactService">The service used to interact with the contact entity.</param>
        public ContactController(EntityService<Contact> contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Reads and displays all contacts.
        /// </summary>
        internal void ReadAll()
        {
            try
            {
                var contacts = _contactService.GetAll();

                // Define columns and their widths
                string[] headers = { "Id", "Full Name", "Email Address", "Company Name", "Business Phone" };
                int[] columnWidths = { 40, 30, 35, 30, 20 };

                // Print headers
                ConsoleFormatter.PrintTableHeader(headers, columnWidths);

                // Print the contact details
                foreach (Contact contact in contacts)
                {
                    string[] rowData =
                    {
                        contact.Id.ToString(),
                        $"{contact.FirstName} {contact.LastName}".Trim(),
                        contact.EMailAddress1 ?? "N/A",
                        contact.Company ?? "N/A",
                        contact.MobilePhone ?? "N/A"
                    };

                    ConsoleFormatter.PrintTableRow(rowData, columnWidths);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading contacts: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new contact with the specified information.
        /// </summary>
        /// <param name="firstName">The first name of the contact.</param>
        /// <param name="lastName">The last name of the contact.</param>
        /// <param name="company">The company name of the contact.</param>
        /// <param name="email">The email address of the contact.</param>
        /// <param name="businessPhone">The business phone number of the contact.</param>
        /// <returns>The ID of the newly created contact.</returns>
        internal Guid Create(string firstName, string lastName, string company, string email, string businessPhone)
        {
            try
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
                    MobilePhone = businessPhone
                };

                Guid contactId = _contactService.Create(newContact);
                Console.WriteLine($"Created Contact ID: {contactId}");
                return contactId;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the contact: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes a contact by ID.
        /// </summary>
        /// <param name="contactId">The ID of the contact to delete.</param>
        internal void Delete(Guid contactId)
        {
            try
            {
                // Validate input
                if (contactId == Guid.Empty)
                    throw new ArgumentException("Invalid contact ID.");

                _contactService.Delete(contactId);
                Console.WriteLine($"Deleted Contact ID: {contactId}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the contact: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing contact with full parameters.
        /// </summary>
        /// <param name="contactId">The ID of the contact to update.</param>
        /// <param name="firstName">The first name of the contact.</param>
        /// <param name="lastName">The last name of the contact.</param>
        /// <param name="company">The company name of the contact.</param>
        /// <param name="email">The email address of the contact.</param>
        /// <param name="businessPhone">The business phone of the contact.</param>
        internal void Update(Guid contactId, string firstName, string lastName, string company, string email,
            string businessPhone)
        {
            try
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
                    MobilePhone = businessPhone
                };

                _contactService.Update(updatedContact);
                Console.WriteLine($"Contact with ID {contactId} updated.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the contact: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing contact's email address.
        /// </summary>
        /// <param name="contactId">The ID of the contact to update.</param>
        /// <param name="email">The new email address of the contact.</param>
        internal void Update(Guid contactId, string email)
        {
            try
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
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the contact: {ex.Message}");
            }
        }

        /// <summary>
        /// Validates if the provided email address is in a valid format.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns><c>true</c> if the email is valid; otherwise, <c>false</c>.</returns>
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