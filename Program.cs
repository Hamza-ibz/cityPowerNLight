namespace CityPowerAndLight;

using CityPowerAndLight.Config;
using CityPowerAndLight.Controller;
using CityPowerAndLight.Utils;
using CityPowerAndLight.Model;
using CityPowerAndLight.Service;
using Microsoft.Xrm.Sdk;

class Program
{
    static void Main()
    {
        InitializeEnvironment();

        IOrganizationService service = OrganisationServiceConnector.Connect(
            Environment.GetEnvironmentVariable("SERVICE_URL") ?? "",
            Environment.GetEnvironmentVariable("APP_ID") ?? "",
            Environment.GetEnvironmentVariable("CLIENT_SECRET") ?? ""
        );

        // Initialize controllers
        var accountController = InitializeAccountController(service);
        var contactController = InitializeContactController(service);
        var caseController = InitializeCaseController(service);

        // Run operations
        RunAccountOperations(accountController);
        RunContactOperations(contactController);
        RunCaseOperations(caseController, contactController);
    }

    static void InitializeEnvironment()
    {
        // ConsoleFormatter.PrintHeader("Initializing Environment");
        string configPath = "Config/environmentVariables.json";
        AppConfig.ParseAndSetEnvironmentVariables(configPath);
        // ConsoleFormatter.PrintFooter();
    }

    static AccountController InitializeAccountController(IOrganizationService service)
    {
        return new AccountController(
            new EntityService<Account>(service, Account.EntityLogicalName)
        );
    }

    static ContactController InitializeContactController(IOrganizationService service)
    {
        return new ContactController(
            new EntityService<Contact>(service, Contact.EntityLogicalName)
        );
    }

    static CaseController InitializeCaseController(IOrganizationService service)
    {
        return new CaseController(
            new EntityService<Incident>(service, Incident.EntityLogicalName)
        );
    }

    static void RunAccountOperations(AccountController accountController)
    {
        // 1. Create a New Account
        ConsoleFormatter.PrintHeader("1. Create a New Account");
        Guid newAccountId = accountController.Create("TestCreate", "TestCreate@outlook.com", "555-123-4567");
        Console.WriteLine($"Created Account ID: {newAccountId}");
        Console.WriteLine();

        // 2. Read All Accounts
        ConsoleFormatter.PrintHeader("2. Read All Accounts");
        accountController.ReadAll();
        Console.WriteLine();

        // 3. Update the Newly Created Account
        ConsoleFormatter.PrintHeader("3. Update the Newly Created Account");
        accountController.Update(
            newAccountId,
            "UpdatedName",
            "UpdatedEmail@outlook.com",
            "555-987-6543"
        );
        Console.WriteLine($"Updated Account ID: {newAccountId}");
        Console.WriteLine();

        // 4. Read All Accounts After Update
        ConsoleFormatter.PrintHeader("4. Read All Accounts After Update");
        accountController.ReadAll();
        Console.WriteLine();

        // 5. Delete the Account
        ConsoleFormatter.PrintHeader("5. Delete the Account");
        accountController.Delete(newAccountId);
        Console.WriteLine($"Deleted Account ID: {newAccountId}");
        Console.WriteLine();

        // 6. Read All Accounts After Deletion
        ConsoleFormatter.PrintHeader("6. Read All Accounts After Deletion");
        accountController.ReadAll();
        Console.WriteLine("========================================");
    }


    static void RunContactOperations(ContactController contactController)
    {
        // 1. Create a New Contact
        ConsoleFormatter.PrintHeader("1. Create a New Contact");
        Guid newContactId = contactController.Create(
            "John", 
            "Doe", 
            "Acme Corp", 
            "john.doe@outlook.com"
        );
        // Console.WriteLine($"Created Contact ID: {newContactId}");
        Console.WriteLine();

        // 2. Read All Contacts
        ConsoleFormatter.PrintHeader("2. Read All Contacts");
        contactController.ReadAll();
        Console.WriteLine();

        // 3. Update the Newly Created Contact
        ConsoleFormatter.PrintHeader("3. Update the Newly Created Contact");
        contactController.Update(
            newContactId,
            "Jane",
            "Doe",
            "Acme Inc",
            "jane.doe@outlook.com"
        );
        // Console.WriteLine($"Updated Contact ID: {newContactId}");
        Console.WriteLine();

        // 4. Read All Contacts After Update
        ConsoleFormatter.PrintHeader("4. Read All Contacts After Update");
        contactController.ReadAll();
        Console.WriteLine();

        // 5. Delete the Contact
        ConsoleFormatter.PrintHeader("5. Delete the Contact");
        contactController.Delete(newContactId);
        // Console.WriteLine($"Deleted Contact ID: {newContactId}");
        Console.WriteLine();

        // 6. Read All Contacts After Deletion
        ConsoleFormatter.PrintHeader("6. Read All Contacts After Deletion");
        contactController.ReadAll();
        Console.WriteLine("========================================");
    }

    static void RunCaseOperations(CaseController caseController, ContactController contactController)
    {
        // Create a Contact for the Case
        Guid newContactId = contactController.Create("Case", "Customer", "CC", "cc@outlook.com");

        // 1. Create a New Case
        ConsoleFormatter.PrintHeader("1. Create a New Case");
        Guid newCaseId = caseController.Create(
            "TestCase", 
            "This is a test case", 
            newContactId
        );
        // Console.WriteLine($"Created Case ID: {newCaseId}");
        Console.WriteLine();

        // 2. Read All Cases
        ConsoleFormatter.PrintHeader("2. Read All Cases");
        caseController.ReadAll();
        Console.WriteLine();

        // 3. Update the Newly Created Case
        ConsoleFormatter.PrintHeader("3. Update the Newly Created Case");
        caseController.Update(
            newCaseId,
            "Updated TestCase",
            "Updated description",
            incident_statuscode.InProgress,
            incident_prioritycode.Low
        );
        // Console.WriteLine($"Updated Case ID: {newCaseId}");
        Console.WriteLine();

        // 4. Read All Cases After Update
        ConsoleFormatter.PrintHeader("4. Read All Cases After Update");
        caseController.ReadAll();
        Console.WriteLine();

        // 5. Delete the Case
        ConsoleFormatter.PrintHeader("5. Delete the Case");
        caseController.Delete(newCaseId);
        // Console.WriteLine($"Deleted Case ID: {newCaseId}");
        Console.WriteLine();

        // Delete Associated Contact
        contactController.Delete(newContactId);

        // 6. Read All Cases After Deletion
        ConsoleFormatter.PrintHeader("6. Read All Cases After Deletion");
        caseController.ReadAll();
        Console.WriteLine("========================================");
    }

}
