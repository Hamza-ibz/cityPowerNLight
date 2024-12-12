using Newtonsoft.Json;



namespace CityPowerAndLight.Config;

public static class AppConfig
{
    public static void ParseAndSetEnvironmentVariables(string environmentVariablesJsonPath)
    {
        try
        {
            using StreamReader reader = new(environmentVariablesJsonPath);
            var json = reader.ReadToEnd();
            var vars = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (vars is null) return;

            foreach ((string key, string value) in vars)
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
        catch (FileNotFoundException ex)
        {
            throw new FileNotFoundException($"FATAL: File not found: {ex.FileName}", ex);
        }
        catch (JsonReaderException ex)
        {
            throw new JsonReaderException($"ERROR: {environmentVariablesJsonPath} contains invalid JSON", ex);
        }
    }

}