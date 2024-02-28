using BlazorAppUpload.Data;
using Microsoft.Extensions.Configuration;

namespace BlazorAppUpload.Tests
{
    public class AzureStorageHelperTests
    {
        private readonly IConfiguration _configuration;

        public AzureStorageHelperTests()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = configBuilder.Build();
        }

        [Fact]
        public async Task UploadAndDownloadDocxFile_Success()
        {
            // Arrange
            var azureStorageHelper = new AzureStorageHelper(_configuration);

            string containerName = _configuration["ContainerName"];
            string sourceFilePath = "C:\\Users\\masha\\Documents\\компоненти_успішної_роботи_питання.docx";
            string destFileName = "компоненти_успішної_роботи_питання.docx";

            // Act
            string uploadedFileUrl = await azureStorageHelper.UploadFile(containerName, sourceFilePath, destFileName, overWrite: true);

            // Assert
            Assert.NotNull(uploadedFileUrl);
            Assert.StartsWith(_configuration["StorageBaseUrl"], uploadedFileUrl);

            string downloadDestFilePath = "path_to_download_test.docx";
            string downloadResult = await azureStorageHelper.DownloadFile(containerName, destFileName, downloadDestFilePath);

            Assert.Equal("OK", downloadResult);
            Assert.True(File.Exists(downloadDestFilePath));

            File.Delete(downloadDestFilePath);
        }

        [Fact]
        public async Task UploadFile_Failure()
        {
            // Arrange
            var azureStorageHelper = new AzureStorageHelper(_configuration);
            string containerName = _configuration["ContainerName"];
            string sourceFilePath = "non_existing_file.docx";
            string destFileName = "test.docx";

            // Act
            string uploadedFileUrl = await azureStorageHelper.UploadFile(containerName, sourceFilePath, destFileName, overWrite: true);

            // Assert
            Assert.Null(uploadedFileUrl);
        }
    }
}
