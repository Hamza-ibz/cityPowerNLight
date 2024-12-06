namespace CityPowerAndLight.Utils
{
    /// <summary>
    /// Provides utility methods for handling exceptions during the execution of actions and functions.
    /// This class ensures that exceptions are caught and logged in a standardized way.
    /// </summary>
    public static class ErrorHandler
    {
        /// <summary>
        /// Executes an <see cref="Action"/> and handles exceptions in a standardized way.
        /// Catches <see cref="ArgumentException"/> and general <see cref="Exception"/> types, logging appropriate messages.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to execute.</param>
        /// <param name="operationName">A descriptive name of the operation being executed, used for logging purposes.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="action"/> or <paramref name="operationName"/> is <c>null</c>.</exception>
        public static void HandleAction(Action action, string operationName)
        {
            if (action == null) 
                throw new ArgumentNullException(nameof(action), "Action cannot be null.");
            if (string.IsNullOrWhiteSpace(operationName)) 
                throw new ArgumentNullException(nameof(operationName), "Operation name cannot be null or empty.");

            try
            {
                action.Invoke();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation error during {operationName}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during {operationName}: {ex.Message}");
            }
        }

        /// <summary>
        /// Executes a <see cref="Func{T}"/> and handles exceptions, returning a default value if an exception occurs.
        /// Catches <see cref="ArgumentException"/> and general <see cref="Exception"/> types, logging appropriate messages.
        /// </summary>
        /// <typeparam name="T">The return type of the function.</typeparam>
        /// <param name="func">The <see cref="Func{T}"/> to execute.</param>
        /// <param name="operationName">A descriptive name of the operation being executed, used for logging purposes.</param>
        /// <param name="defaultValue">The default value to return if an exception occurs. The default is the default value of type <typeparamref name="T"/>.</param>
        /// <returns>The result of the function or the default value if an exception occurs.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="func"/> or <paramref name="operationName"/> is <c>null</c>.</exception>
        public static T HandleFunction<T>(Func<T> func, string operationName, T defaultValue = default) where T : struct
        {
            if (func == null) 
                throw new ArgumentNullException(nameof(func), "Function cannot be null.");
            if (string.IsNullOrWhiteSpace(operationName)) 
                throw new ArgumentNullException(nameof(operationName), "Operation name cannot be null or empty.");

            try
            {
                return func.Invoke();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation error during {operationName}: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during {operationName}: {ex.Message}");
            }
            return defaultValue;
        }
    }
}
