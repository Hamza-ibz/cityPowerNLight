using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CityPowerAndLight.Service
{
    /// <summary>
    /// Provides common CRUD operations for managing entities in Microsoft Dynamics CRM.
    /// </summary>
    /// <typeparam name="T">The type of entity that this service will manage, which must be a subclass of <see cref="Entity"/>.</typeparam>
    internal class EntityService<T> where T : Entity
    {
        private readonly IOrganizationService _organisationService;
        private readonly string _entityLogicalName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityService{T}"/> class.
        /// </summary>
        /// <param name="organisationService">The service used to interact with Microsoft Dynamics CRM.</param>
        /// <param name="entityLogicalName">The logical name of the entity that the service will manage.</param>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="organisationService"/> or <paramref name="entityLogicalName"/> is null.</exception>
        public EntityService(IOrganizationService organisationService, string entityLogicalName)
        {
            _organisationService = organisationService ?? throw new ArgumentNullException(nameof(organisationService), "Organisation service cannot be null.");
            _entityLogicalName = entityLogicalName ?? throw new ArgumentNullException(nameof(entityLogicalName), "Entity logical name cannot be null.");
        }

        /// <summary>
        /// Creates a new entity in Microsoft Dynamics CRM.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>The ID of the newly created entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="entity"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs during the creation process.</exception>
        public Guid Create(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "The entity to create cannot be null.");

                Guid entityId = _organisationService.Create(entity);
                return entityId;
            }
            catch (Exception ex)
            {
                // Log or handle the exception (logging mechanism not shown)
                Console.WriteLine($"Error occurred while creating the entity: {ex.Message}");
                throw new InvalidOperationException("Failed to create the entity.", ex);
            }
        }

        /// <summary>
        /// Retrieves all entities of the specified type from Microsoft Dynamics CRM.
        /// </summary>
        /// <returns>A list of all entities of type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs while retrieving the entities.</exception>
        public IEnumerable<T> GetAll()
        {
            try
            {
                QueryExpression query = new QueryExpression(_entityLogicalName)
                {
                    ColumnSet = new ColumnSet(true),
                    PageInfo = new PagingInfo() { Count = 5000, PageNumber = 1 }
                };

                List<T> allRecords = new List<T>();
                EntityCollection results;

                do
                {
                    results = _organisationService.RetrieveMultiple(query);

                    foreach (Entity record in results.Entities)
                    {
                        T entity = (T)record;
                        allRecords.Add(entity);
                    }
                    query.PageInfo.PageNumber++;
                } while (results.MoreRecords);

                return allRecords;
            }
            catch (Exception ex)
            {
                // Log or handle the exception (logging mechanism not shown)
                Console.WriteLine($"Error occurred while retrieving all entities: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve entities.", ex);
            }
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="entityId">The ID of the entity to delete.</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="entityId"/> is an empty GUID.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs during the deletion process.</exception>
        public void Delete(Guid entityId)
        {
            try
            {
                if (entityId == Guid.Empty)
                    throw new ArgumentException("The entity ID must be valid.", nameof(entityId));

                _organisationService.Delete(_entityLogicalName, entityId);
            }
            catch (Exception ex)
            {
                // Log or handle the exception (logging mechanism not shown)
                Console.WriteLine($"Error occurred while deleting the entity: {ex.Message}");
                throw new InvalidOperationException("Failed to delete the entity.", ex);
            }
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="entity"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="entity"/> has an invalid ID.</exception>
        /// <exception cref="InvalidOperationException">Thrown when an error occurs during the update process.</exception>
        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "The entity to update cannot be null.");

                if (entity.Id == Guid.Empty)
                    throw new ArgumentException("The entity must have a valid ID before updating.", nameof(entity));

                _organisationService.Update(entity);
            }
            catch (Exception ex)
            {
                // Log or handle the exception (logging mechanism not shown)
                Console.WriteLine($"Error occurred while updating the entity: {ex.Message}");
                throw new InvalidOperationException("Failed to update the entity.", ex);
            }
        }
    }
}
