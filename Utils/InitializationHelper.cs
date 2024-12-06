using Microsoft.Xrm.Sdk;
using DotNetEnv;
using CityPowerAndLight.Service;
using CityPowerAndLight.Controller;
using CityPowerAndLight.Model;

namespace CityPowerAndLight.Utils
{
    /// <summary>
    /// Provides utility methods for initializing the application environment and controllers.
    /// This class helps to load environment variables and instantiate the controllers for different entities.
    /// </summary>
    internal static class InitializationHelper
    {
        /// <summary>
        /// Loads environment variables from the .env file using the DotNetEnv library.
        /// This is typically used to load configuration settings such as API keys, database connections, etc.
        /// </summary>
        /// <remarks>
        /// This method will output a message to the console indicating that the environment variables have been loaded.
        /// </remarks>
        public static void InitializeEnvironment()
        {
            // Load environment variables
            Env.Load();
            Console.WriteLine("Environment variables loaded from .env file.");
        }

        /// <summary>
        /// Initializes the <see cref="AccountController"/> with the provided <see cref="IOrganizationService"/>.
        /// The controller is responsible for handling operations related to <see cref="Account"/> entities.
        /// </summary>
        /// <param name="service">An instance of <see cref="IOrganizationService"/> used to interact with the CRM system.</param>
        /// <returns>An instance of <see cref="AccountController"/> initialized with the given service.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="service"/> is <c>null</c>.</exception>
        public static AccountController InitializeAccountController(IOrganizationService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service), "IOrganizationService cannot be null.");

            return new AccountController(
                new EntityService<Account>(service, Account.EntityLogicalName)
            );
        }

        /// <summary>
        /// Initializes the <see cref="ContactController"/> with the provided <see cref="IOrganizationService"/>.
        /// The controller is responsible for handling operations related to <see cref="Contact"/> entities.
        /// </summary>
        /// <param name="service">An instance of <see cref="IOrganizationService"/> used to interact with the CRM system.</param>
        /// <returns>An instance of <see cref="ContactController"/> initialized with the given service.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="service"/> is <c>null</c>.</exception>
        public static ContactController InitializeContactController(IOrganizationService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service), "IOrganizationService cannot be null.");

            return new ContactController(
                new EntityService<Contact>(service, Contact.EntityLogicalName)
            );
        }

        /// <summary>
        /// Initializes the <see cref="CaseController"/> with the provided <see cref="IOrganizationService"/>.
        /// The controller is responsible for handling operations related to <see cref="Incident"/> entities.
        /// </summary>
        /// <param name="service">An instance of <see cref="IOrganizationService"/> used to interact with the CRM system.</param>
        /// <returns>An instance of <see cref="CaseController"/> initialized with the given service.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="service"/> is <c>null</c>.</exception>
        public static CaseController InitializeCaseController(IOrganizationService service)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service), "IOrganizationService cannot be null.");

            return new CaseController(
                new EntityService<Incident>(service, Incident.EntityLogicalName)
            );
        }
    }
}