using Amazon.S3;
using Amazon.S3.Model;
using Amazon;
using System;
using System.IO;
using System.Threading.Tasks;
using PruebaTecnica.Shared.Interfaces;
using Amazon.Runtime;

namespace PruebaTecnica.API.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "rosa.selene";

        private readonly IConfiguration _configuration;

        public S3Service(IConfiguration configuration)
        {
            _configuration = configuration;

            // Obtener las credenciales desde el archivo de configuración
            string accessKey = _configuration["S3Conf:AccessKeyID"];
            string secretAccessKey = _configuration["S3Conf:SecretAccessKey"];

            // Crear las credenciales con BasicAWSCredentials
            var credentials = new BasicAWSCredentials(accessKey, secretAccessKey);

            // Inicializar el cliente S3 con las credenciales y la región
            _s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);

        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = fileStream
                };

                var response = await _s3Client.PutObjectAsync(putRequest);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return fileName;
                }
                else
                {
                    throw new Exception("No se pudo cargar el archivo a S3.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al subir el archivo a S3: {ex.Message}");
            }
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            try
            {
                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName
                };

                var response = await _s3Client.DeleteObjectAsync(deleteRequest);

                return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el archivo de S3: {ex.Message}");
            }
        }

        public async Task<string> GetFileUrlAsync(string fileName)
        {
            try
            {
                var request = new GetPreSignedUrlRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    Expires = DateTime.Now.AddMinutes(15)
                };

                string url = _s3Client.GetPreSignedURL(request);
                return url;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener la URL del archivo: {ex.Message}");
            }
        }
    }
}