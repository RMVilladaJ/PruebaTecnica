using System.Net;

namespace PruebaTecnica.Web.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Error = error;
            Response = response;
            HttpResponseMessage = httpResponseMessage;
        }

        public HttpResponseWrapper(T response, bool error, HttpResponseMessage httpResponseMessage, string errorMessage = null)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
            ErrorMessage = errorMessage;
        }


        public bool Error { get; set; }

        public T? Response { get; set; }

        public HttpResponseMessage HttpResponseMessage { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<string?> GetErrorMessageAsync()
        {
            if (!Error)
            {
                return null;
            }

            var codigoEstatus = HttpResponseMessage.StatusCode;
            if (codigoEstatus == HttpStatusCode.NotFound)//404
            {
                return "Recurso no encontrado";
            }
            else if (codigoEstatus == HttpStatusCode.BadRequest)//400
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            else if (codigoEstatus == HttpStatusCode.Unauthorized)//401
            {
                return " Debes loguearte para realizar esta acción";
            }
            else if (codigoEstatus == HttpStatusCode.Forbidden)//403  Prohibido
            {
                return " No tienes permisos para ejecutar esta acción";
            }

            return "Ha ocurrido un error inesperado";
        }
    }


}
