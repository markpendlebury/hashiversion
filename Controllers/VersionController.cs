using System.Net;
using System.Text;
using hashiversion.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Hashiversion.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionController : ControllerBase
{

    private readonly ILogger<VersionController> logger;

    public VersionController(ILogger<VersionController> logger)
    {
        this.logger = logger;
    }
    string[] applicationList = { "terraform", "vault", "consul", "nomad", "packer" };

    /// <summary>
    /// Get request, accepts one of the items from applicationList
    /// and returns the version number for that application from 
    /// hashicorps github tags
    /// </summary>
    /// <param name="applicationName"></param>
    /// <returns>string</returns>
    [HttpGet("/{applicationName}")]
    public async Task<IActionResult> GetByName(string applicationName)
    {
        logger.LogInformation($"Request recieved for {applicationName}");
        try
        {
            logger.LogInformation($"Found {applicationName} in applicationList");

            // create an empty html string
            string html = string.Empty;

            // Create the base url for the github api request
            string url = $"https://api.github.com/repos/hashicorp/{applicationName}/tags";

            logger.LogInformation($"Attempting get request to: {url}");

            // Create a http web request from the above url
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            // This header is required by githubs api
            request.Headers["User-Agent"] = "request";
            // Possibly not needed? cba testing without, feel fre to remove
            request.AutomaticDecompression = DecompressionMethods.GZip;

            logger.LogInformation("Creating stream reader...");

            // Get the response
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                logger.LogInformation($"Response: {response.StatusCode}");
                // Create a response stream
                using (Stream stream = response.GetResponseStream())
                // Read the response stream
                using (StreamReader reader = new StreamReader(stream))
                {
                    // Stream the contents of the response stream into
                    // the html string
                    html = reader.ReadToEnd();
                }
            }
            logger.LogInformation("Deserialising response into responseModel");
            // Deserialise the html response string into our github model
            // Order by descending and cast to a list
            var responseModel = JsonConvert.DeserializeObject<List<GithubModel>>(html).OrderByDescending(r => r.name).ToList();
            // Replace the conents of responseModel with
            // all instances that don't include alpha and 
            // beta in the name property
            responseModel = responseModel.Where(r => !r.name.ToLower().Contains("alpha") && !r.name.ToLower().Contains("beta") && !r.name.ToLower().Contains("dev") && !r.name.ToLower().Contains("rc")).ToList();
            logger.LogInformation("Returning the name of the first result");

            // Return the first (latest) name value (v#.###)
            return Ok(responseModel[0].name);
        }
        catch (Exception ex)
        {
            // Something went wrong
            logger.LogCritical(ex.ToString());
            // Let the user know something went wrong
            return StatusCode(500, "Something unexpected happened, i've been made aware");
        }
    }

}