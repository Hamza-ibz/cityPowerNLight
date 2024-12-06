using CityPowerAndLight.Model;
using CityPowerAndLight.Service;
using CityPowerAndLight.Utils;
using Microsoft.Xrm.Sdk;
using System;

namespace CityPowerAndLight.Controller
{
    /// <summary>
    /// Controller responsible for managing incident (case) related operations such as Create, Read, Update, and Delete (CRUD).
    /// </summary>
    internal class CaseController
    {
        private readonly EntityService<Incident> _caseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaseController"/> class.
        /// </summary>
        /// <param name="caseService">An instance of <see cref="EntityService{Incident}"/> used to interact with the incident data.</param>
        public CaseController(EntityService<Incident> caseService)
        {
            _caseService = caseService;
        }

        /// <summary>
        /// Reads all incidents and prints them in a table format.
        /// </summary>
        internal void ReadAll()
        {
            try
            {
                var incidents = _caseService.GetAll();

                // Define columns and their widths
                string[] headers =
                {
                    "ID", "Case Title", "Case Number", "Priority", "Origin", "Customer", "Status Reason", "Created On"
                };
                int[] columnWidths = { 40, 62, 20, 15, 15, 35, 20, 20 };

                // Print headers
                ConsoleFormatter.PrintTableHeader(headers, columnWidths);

                // Print the incident details
                foreach (Incident incident in incidents)
                {
                    string[] rowData =
                    {
                        incident.Id.ToString(),
                        incident.Title ?? "N/A", // Case Title
                        incident.TicketNumber ?? "N/A", // Case Number
                        incident.PriorityCode?.ToString() ?? "N/A", // Priority
                        incident.CaseOriginCode?.ToString() ?? "N/A", // Origin
                        incident.CustomerId?.Name ?? "N/A", // Customer
                        incident.StatusCode?.ToString() ?? "N/A", // Status Reason
                        incident.CreatedOn?.ToString("yyyy-MM-dd") ?? "N/A" // Created On
                    };

                    ConsoleFormatter.PrintTableRow(rowData, columnWidths);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading incidents: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new incident with the specified title, description, and customer ID.
        /// </summary>
        /// <param name="title">The title of the incident.</param>
        /// <param name="description">The description of the incident.</param>
        /// <param name="customerId">The unique identifier (GUID) of the customer associated with the incident.</param>
        /// <returns>The unique identifier (GUID) of the created incident.</returns>
        /// <exception cref="ArgumentException">Thrown when any input parameter is invalid.</exception>
        internal Guid Create(string title, string description, Guid customerId)
        {
            try
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
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
                throw; // Re-throw the exception for further handling if needed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the incident: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Deletes an incident by its unique identifier (GUID).
        /// </summary>
        /// <param name="caseId">The unique identifier (GUID) of the incident to be deleted.</param>
        /// <exception cref="ArgumentException">Thrown if the provided case ID is invalid.</exception>
        internal void Delete(Guid caseId)
        {
            try
            {
                if (caseId == Guid.Empty)
                    throw new ArgumentException("Invalid case ID.");

                _caseService.Delete(caseId);
                Console.WriteLine($"Deleted Case ID: {caseId}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the incident: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing incident with the specified title, description, status, and priority.
        /// </summary>
        /// <param name="caseId">The unique identifier (GUID) of the incident to be updated.</param>
        /// <param name="title">The updated title of the incident.</param>
        /// <param name="description">The updated description of the incident.</param>
        /// <param name="statusCode">The updated status of the incident.</param>
        /// <param name="priorityCode">The updated priority of the incident.</param>
        /// <exception cref="ArgumentException">Thrown if any of the input parameters are invalid.</exception>
        internal void Update(Guid caseId, string title, string description, incident_statuscode statusCode,
            incident_prioritycode priorityCode)
        {
            try
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
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the incident: {ex.Message}");
            }
        }

        /// <summary>
        /// Overloaded method to update an existing incident with the specified title and status.
        /// </summary>
        /// <param name="caseId">The unique identifier (GUID) of the incident to be updated.</param>
        /// <param name="title">The updated title of the incident.</param>
        /// <param name="statusCode">The updated status of the incident.</param>
        /// <exception cref="ArgumentException">Thrown if any of the input parameters are invalid.</exception>
        internal void Update(Guid caseId, string title, incident_statuscode statusCode)
        {
            try
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
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the incident: {ex.Message}");
            }
        }
    }
}