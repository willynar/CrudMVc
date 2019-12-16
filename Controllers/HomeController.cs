using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CrudMVc.Models;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace CrudMVc.Controllers
{
    public class HomeController : Controller
    {
        #region vistas 
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult guardar()
        {
            return View();

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
        #region metodos
        public IActionResult Delete(string id,[FromServices]IDaoService DAO)
        {
            DataSet data = new DataSet();
            if (id == null)
            {
                //return RedirectToAction("guardar", model);            
            }
            else
            {
                SqlParameter[] parametros =
                {
                    new SqlParameter("@id",id)    
                };
                data = DAO.ejecutarProcedimiento("sp_eliminar_usuario", parametros);
            }

            return RedirectToAction("Visualizar", "Home");

        }
        public IActionResult Visualizar([FromServices]IDaoService DAO)
        {
            DataSet data = new DataSet();
            try
            {
                data = DAO.ejecutarProcedimiento("sp_consultar_usuarios", null);

                List<Usuario> datos = new List<Usuario>();
                foreach (DataRow item in data.Tables[0].Rows)
                {
                    Usuario usuario = new Usuario();
                    usuario.nombre = item["nombre"].ToString();
                    usuario.apellido = item["apellido"].ToString();
                    usuario.correo = item["correo"].ToString();
                    usuario.contraseña = item["contraseña"].ToString();
                    datos.Add(usuario);
                }

                return View(datos);

            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpPost]
        public IActionResult guardarUsuario(Usuario model, [FromServices]IDaoService DAO)
        {
            DataSet data = new DataSet();
            if (!ModelState.IsValid)
            {
                return RedirectToAction("guardar", model);
            }
            else
            {

                SqlParameter[] parametros =
                {
                    new SqlParameter("@nombre",model.nombre),
                    new SqlParameter("@apellido",model.apellido),
                    new SqlParameter("@correo",model.correo),
                    new SqlParameter("@contrasena",model.contraseña)
                };
                data = DAO.ejecutarProcedimiento("sp_crear_usuario", parametros);
            }
            return RedirectToAction("Visualizar", "Home");
        }
        #endregion

    }
}
