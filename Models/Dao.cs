using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CrudMVc.Models
{
    public class Dao : IDaoService
    {
        public IConfigurationRoot obtenerConfiguracion()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            return builder.Build();
        }
        public DataSet ejecutarProcedimiento(string procedimiento, params SqlParameter[] parameters)
        {
            var configuracion = obtenerConfiguracion();
            DataSet data = new DataSet();
            DataTable tabla = new DataTable();
            try
            {
                //using (var conection = new SqlConnection(ConnfigurationManager.ConnectionStrings["conexion"].ToString())
                using (var conection = new SqlConnection(configuracion.GetSection("ConnectionStrings").GetSection("conecxion").Value))
                {
                    using (var command = new SqlCommand(procedimiento, conection))
                    {
                        using (var adapter = new SqlDataAdapter(command))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            if (parameters != null)
                                foreach (var item in parameters)
                                    command.Parameters.Add(item);
                            conection.Open();
                            adapter.Fill(data);
                            command.Parameters.Clear();
                            conection.Close();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string mensaje = e.Message;
                tabla.Columns.Add("Error");
                tabla.Rows.Add(mensaje);
                data.Tables.Add(tabla);

            }
            return data;

        }

        public string Send<T>(string url, T objectRequest, string method = "GET")
        {
            string result;
            string Token = "ProcampAutorization";
            try
            {
                //serializamos el objeto
                string json = JsonConvert.SerializeObject(objectRequest);

                //peticion
                WebRequest request = WebRequest.Create(url);
                //headers
                request.Method = method;
                request.PreAuthenticate = true;
                request.ContentType = "application/json;charset=utf-8'";
                //request.Timeout = 10000; //esto es opcional
                request.Headers.Add("Token", Token);
                request.Headers.Add("Authorization", "Token " + Token);

                if (objectRequest != null && method != "GET")
                {
                    //se escribre el json en estructura para el body
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                }

                //SearchOption realiza el envio y se recibe una respuesta de  el servicio
                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;

        }
    }
}
