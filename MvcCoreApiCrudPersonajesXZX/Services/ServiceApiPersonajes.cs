using MvcCoreApiCrudPersonajesXZX.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcCoreApiCrudPersonajesXZX.Services
{
    public class ServiceApiPersonajes
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;

        public ServiceApiPersonajes(IConfiguration configuration)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi = configuration.GetValue<string>
                ("ApiUrls:ApiCrudPersonajes");
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            string request = "/api/Personajes";
            List<Personaje> personajes =
                await this.CallApiAsync<List<Personaje>>(request);
            return personajes;
        }

        public async Task<Personaje> FindPersonajeAsync(int idpersonaje)
        {
            string request = "/api/Personajes/" + idpersonaje;
            Personaje personaje =
                await this.CallApiAsync<Personaje>(request);
            return personaje;
        }

        public async Task DeletePersonajeAsync(int idpersonaje)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Personajes/" + idpersonaje;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();

                HttpResponseMessage response =
                    await client.DeleteAsync(request);

            }
        }

        public async Task InsertPersonajeAsync
            (int idpersonaje, string personaje, string imagen, int idserie)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Personajes";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Personaje pj = new Personaje();
                pj.IdPersonaje = idpersonaje;
                pj.Personajes = personaje;
                pj.Imagen = imagen;
                pj.IdSerie = idserie;

                string json = JsonConvert.SerializeObject(pj);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);

            }
        }

        public async Task UpdatePersonajeAsync
            (int idpersonaje, string personaje, string imagen, int idserie)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Personajes";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Personaje pj =
                    new Personaje
                    {
                        IdPersonaje = idpersonaje,
                        Personajes = personaje,
                        Imagen = imagen,    
                        IdSerie = idserie
                    };

                string json = JsonConvert.SerializeObject(pj);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");

                await client.PutAsync(request, content);
            }
        }



    }
}
