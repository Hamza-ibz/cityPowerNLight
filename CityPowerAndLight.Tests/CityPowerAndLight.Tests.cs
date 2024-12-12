using Moq;
using System;
using System.Collections.Generic;
using Xunit;
using CityPowerAndLight.Model;
using CityPowerAndLight.Service;
using CityPowerAndLight.Controller;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CityPowerAndLight.Tests
{
    public class CaseControllerTests
    {
        private readonly Mock<IOrganizationService> _mockOrganizationService;
        private readonly EntityService<Incident> _entityService;
        private readonly CaseController _controller;

        public CaseControllerTests()
        {
            // Mock the IOrganizationService
            _mockOrganizationService = new Mock<IOrganizationService>();

            // Create the EntityService with the mocked IOrganizationService
            _entityService = new EntityService<Incident>(_mockOrganizationService.Object, "incident");

            // Create the controller, passing the mocked service
            _controller = new CaseController(_entityService);
        }

        // Test for Create functionality
        [Fact]
        public void Create_ShouldReturn_IncidentId_WhenValidInputIsProvided()
        {
            // Arrange
            string title = "Test Incident";
            string description = "Test Description";
            Guid customerId = Guid.NewGuid(); // Random GUID for customer ID

            var expectedIncidentId = Guid.NewGuid(); // Random GUID for the incident ID
            _mockOrganizationService.Setup(service => service.Create(It.IsAny<Entity>())).Returns(expectedIncidentId);

            // Act
            var result = _controller.Create(title, description, customerId);

            // Assert
            Assert.Equal(expectedIncidentId, result); // Verify the returned incident ID matches the expected
        }

        // Test for Create functionality with invalid inputs (empty title)
        [Fact]
        public void Create_ShouldThrow_ArgumentException_WhenInvalidTitleIsProvided()
        {
            // Arrange
            string title = ""; // Invalid empty title
            string description = "Test Description";
            Guid customerId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _controller.Create(title, description, customerId)); // Expect an exception due to invalid title
        }

        // Test for Create functionality with invalid inputs (null title)
        [Fact]
        public void Create_ShouldThrow_ArgumentException_WhenNullTitleIsProvided()
        {
            // Arrange
            string title = null; // Invalid null title
            string description = "Test Description";
            Guid customerId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _controller.Create(title, description, customerId)); // Expect an exception due to invalid title
        }

        // Test for ReadAll functionality
        [Fact]
        public void ReadAll_ShouldCall_RetrieveMultiple_WhenCalled()
        {
            // Arrange
            _mockOrganizationService.Setup(service => service.RetrieveMultiple(It.IsAny<QueryExpression>())).Returns(new EntityCollection());

            // Act
            _controller.ReadAll();

            // Assert
            _mockOrganizationService.Verify(service => service.RetrieveMultiple(It.IsAny<QueryExpression>()), Times.Once); // Verify RetrieveMultiple is called once
        }

        // Test for ReadAll when no incidents are available
        [Fact]
        public void ReadAll_ShouldHandle_EmptyResult_WhenNoIncidentsAreFound()
        {
            // Arrange
            _mockOrganizationService.Setup(service => service.RetrieveMultiple(It.IsAny<QueryExpression>())).Returns(new EntityCollection());

            // Act & Assert
            var exception = Record.Exception(() => _controller.ReadAll());
            Assert.Null(exception); // Ensure no exception occurs
        }

        // Test for Delete functionality with valid case ID
        [Fact]
        public void Delete_ShouldCall_Delete_WithValidCaseId()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            _mockOrganizationService.Setup(service => service.Delete(It.IsAny<string>(), It.IsAny<Guid>()));

            // Act
            _controller.Delete(caseId);

            // Assert
            _mockOrganizationService.Verify(service => service.Delete(It.IsAny<string>(), caseId), Times.Once); // Verify Delete is called with the correct ID
        }
        

        // Test for Update functionality with valid parameters
        [Fact]
        public void Update_ShouldCall_Update_WithValidParameters()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            string title = "Updated Title";
            string description = "Updated Description";
            var statusCode = incident_statuscode.InProgress;
            var priorityCode = incident_prioritycode.High;

            var incident = new Incident { Id = caseId, Title = title, Description = description }; // Create an incident object

            _mockOrganizationService.Setup(service => service.Update(It.IsAny<Entity>()));

            // Act
            _controller.Update(caseId, title, description, statusCode, priorityCode);

            // Assert
            _mockOrganizationService.Verify(service => service.Update(It.IsAny<Entity>()), Times.Once); // Verify Update is called once
        }
        

        // Test for Update functionality with only title and status
        [Fact]
        public void Update_ShouldCall_Update_WithTitleAndStatusOnly()
        {
            // Arrange
            var caseId = Guid.NewGuid();
            string title = "Updated Title";
            var statusCode = incident_statuscode.InProgress;

            _mockOrganizationService.Setup(service => service.Update(It.IsAny<Entity>()));

            // Act
            _controller.Update(caseId, title, statusCode);

            // Assert
            _mockOrganizationService.Verify(service => service.Update(It.IsAny<Entity>()), Times.Once); // Verify Update is called once
        }
        
    }
}
