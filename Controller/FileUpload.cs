using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.CreateUploadSession;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.Security;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using static Microsoft.Graph.Constants;

namespace Tessenger.Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUpload : ControllerBase
    {
        private readonly IConfiguration configuration;
        private GraphServiceClient graphServiceClient;
        public static FileUpload Instance { get; private set; }

        public FileUpload(IConfiguration configuration)
        {


            var clientId = configuration.GetValue<string>("AzureAuthenticationData:ClientId");
            var clientSecret = configuration.GetValue<string>("AzureAuthenticationData:ClientSecret");
            var tenantId = configuration.GetValue<string>("AzureAuthenticationData:TenantId");
            string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
            var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            graphServiceClient = new GraphServiceClient(clientSecretCredential);
            this.configuration = configuration;
        }

        [HttpGet("CreateUploadSession/{OnedrivePath},{FilePath}")]
        public async Task<ActionResult<string>> CreateUploadSession(string OnedrivePath, string FilePath)
        {
            try
            {
                OnedrivePath = OnedrivePath.Replace("`", "/");
                FilePath = FilePath.Replace("`", "/");
                var userId = configuration.GetValue<string>("AzureAuthenticationData:AzureUserId");
                var driveid = await graphServiceClient.Users[userId].Drive.GetAsync();
                var uploadSessionRequestBody = new DriveItemUploadableProperties
                {
                    Name = Path.GetFileName(FilePath),
                    FileSystemInfo = new Microsoft.Graph.Models.FileSystemInfo
                    {
                        CreatedDateTime = DateTime.Now,
                        LastModifiedDateTime = DateTime.Now
                    }
                };
                var requestbody = new CreateUploadSessionPostRequestBody() { Item = uploadSessionRequestBody };
                var uploadSession = await graphServiceClient.Drives[driveid.Id].Items["root"].ItemWithPath(OnedrivePath).CreateUploadSession.PostAsync(requestbody);
                return Ok(uploadSession.UploadUrl);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error Creating UploadSession: {ex.Message}");
            }
        }

        [HttpGet("GetFileDownloadUrlFromFilePath/{path}")]
        public async Task<string> GetFileDownloadUrlFromFilePath(string path)
        {
            var userId = configuration.GetValue<string>("AzureAuthenticationData:AzureUserId");
            var driveid = await graphServiceClient.Users[userId].Drive.GetAsync();
            var file = await graphServiceClient.Drives[driveid.Id].Items["root"].ItemWithPath(path).GetAsync();
            return (string)file.AdditionalData["@microsoft.graph.downloadUrl"];
        }

       

    }
}
