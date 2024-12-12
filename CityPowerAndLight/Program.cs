namespace CityPowerAndLight
{
    using Utils;
    using View;
    using Service;

    class Program
    {
        /// <summary>
        /// The main method that serves as the entry point for the application.
        /// It initializes environment variables, connects to the CRM service, 
        /// initializes controllers, and executes the operations on accounts, contacts, and cases.
        /// </summary>
        static void Main()
        {
            // Initialize environment variables
            InitializationHelper.InitializeEnvironment();
            
            // Console.WriteLine(Environment.GetEnvironmentVariable("SERVICE_URL"));

            // Connect to the CRM service using environment variables
            var service = OrganisationServiceConnector.Connect(
                Environment.GetEnvironmentVariable("SERVICE_URL") ?? "",
                Environment.GetEnvironmentVariable("APP_ID") ?? "",
                Environment.GetEnvironmentVariable("CLIENT_SECRET") ?? ""
            );
            
            // Initialize controllers
            var accountController = InitializationHelper.InitializeAccountController(service);
            var contactController = InitializationHelper.InitializeContactController(service);
            var caseController = InitializationHelper.InitializeCaseController(service);
            
            // Run operations
            OperationRunner.RunAccountOperations(accountController);
            OperationRunner.RunContactOperations(contactController);
            OperationRunner.RunCaseOperations(caseController, contactController);
        }
    }
}