using CityPowerAndLight.Model;
using CityPowerAndLight.Service;
using CityPowerAndLight.Utils;
using Microsoft.Xrm.Sdk;
using System;

namespace CityPowerAndLight.Controller
{
    internal class CaseController
    {
        private readonly EntityService<Incident> _caseService;

        public CaseController(EntityService<Incident> caseService)
        {
            _caseService = caseService;
        }

        // Read all incidents (cases)
        internal void ReadAll()
        {
            var incidents = _caseService.GetAll();

            // Define columns and their widths
            string[] headers = { "Id", "Title", "Customer Name", "Priority", "Case Number", "Status" };
            int[] columnWidths = { 40, 62, 35, 10, 20, 15 };

            // Print headers
            ConsoleFormatter.PrintTableHeader(headers, columnWidths);

            // Print the incident details
            foreach (Incident incident in incidents)
            {
                string[] rowData = {
                    incident.Id.ToString(),
                    incident.Title ?? "N/A",
                    incident.CustomerId?.Name ?? "N/A",
                    incident.PriorityCode.ToString(),
                    incident.TicketNumber ?? "N/A",
                    incident.StatusCode.ToString()
                };

                ConsoleFormatter.PrintTableRow(rowData, columnWidths);
            }
        }

        // Create a new incident (case)
        internal Guid Create(string title, string description, Guid customerId)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be null or empty.");
            if (customerId == Guid.Empty)
                throw new ArgumentException("Invalid customer ID.");

            // Create a new Incident
            Incident newIncident = new()
            {
                Title = title,
                Description = description,
                CustomerId = new EntityReference(Contact.EntityLogicalName, customerId),
                StatusCode = incident_statuscode.InProgress,
                PriorityCode = incident_prioritycode.High,
            };

            // Call the service to create the incident and return the case ID
            Guid incidentId = _caseService.Create(newIncident);
            Console.WriteLine($"Created Case ID: {incidentId}");
            return incidentId;
        }

        // Delete an incident (case) by ID
        internal void Delete(Guid caseId)
        {
            // Validate input
            if (caseId == Guid.Empty)
                throw new ArgumentException("Invalid case ID.");

            _caseService.Delete(caseId);
            Console.WriteLine($"Deleted Case ID: {caseId}");
        }

        // Update an existing incident (case) with full parameters
        internal void Update(Guid caseId, string title, string description, incident_statuscode statusCode, incident_prioritycode priorityCode)
        {
            // Validate inputs
            if (caseId == Guid.Empty)
                throw new ArgumentException("Invalid case ID.");
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be null or empty.");

            // Create updated Incident
            Incident updatedIncident = new()
            {
                Id = caseId,
                Title = title,
                Description = description,
                StatusCode = statusCode,
                PriorityCode = priorityCode,
            };

            _caseService.Update(updatedIncident);
            Console.WriteLine($"Case with ID {caseId} updated.");
        }

        // Overloaded Update method with minimal parameters
        internal void Update(Guid caseId, string title, incident_statuscode statusCode)
        {
            // Validate inputs
            if (caseId == Guid.Empty)
                throw new ArgumentException("Invalid case ID.");
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.");

            // Create updated Incident
            Incident updatedIncident = new()
            {
                Id = caseId,
                Title = title,
                StatusCode = statusCode,
            };

            _caseService.Update(updatedIncident);
            Console.WriteLine($"Case with ID {caseId} updated.");
        }
    }
}
