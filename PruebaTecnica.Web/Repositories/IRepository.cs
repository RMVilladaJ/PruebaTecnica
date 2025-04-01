namespace PruebaTecnica.Web.Repositories
{
    public interface IRepository
    {
        Task<HttpResponseWrapper<T>> GetAsync<T>(string url);
        Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model);
        Task<HttpResponseWrapper<TResponse>> PostAsync<T, TResponse>(string url, T model);

        Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model); // los de object se encargan de insertar el objeto en la base de datos. 
        Task<HttpResponseWrapper<TResponse>> PutAsync<T, TResponse>(string url, T model); // Tiene la respuesta depues de enviar la petición

        Task<HttpResponseWrapper<object>> DeleteAsync<T>(string url);

        Task<HttpResponseWrapper<object>> Get(string url);

        Task<HttpResponseWrapper<TResponse>> Put<T, TResponse>(string url, T model);

        Task<HttpResponseWrapper<TResponse>> PostMultipartAsync<TResponse>(string url, MultipartFormDataContent formData);

    }
}

