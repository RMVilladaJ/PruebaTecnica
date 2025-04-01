using System;
using System.IO;
using System.Threading.Tasks;

namespace PruebaTecnica.Shared.Interfaces
{
    public interface IS3Service
    {
        /// <summary>
        /// Sube un archivo a un bucket de S3.
        /// </summary>
        /// <param name="fileStream">Flujo de datos del archivo a subir.</param>
        /// <param name="fileName">Nombre con el que se guardará en S3.</param>
        /// <returns>La URL pública del archivo subido.</returns>
        Task<string> UploadFileAsync(Stream fileStream, string fileName);

        /// <summary>
        /// Elimina un archivo de un bucket de S3.
        /// </summary>
        /// <param name="fileName">Nombre del archivo a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, de lo contrario False.</returns>
        Task<bool> DeleteFileAsync(string fileName);

        /// <summary>
        /// Obtiene la URL de un archivo almacenado en S3.
        /// </summary>
        /// <param name="fileName">Nombre del archivo.</param>
        /// <returns>La URL del archivo.</returns>
        Task<string> GetFileUrlAsync(string fileName);
    }
}


