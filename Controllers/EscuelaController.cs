using DTO;
using EscuelaMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;

namespace EscuelaMVC.Controllers
{
    public class EscuelaController : Controller
    {
        // GET: Escuela
        public ActionResult Index()
        {
            List<Escuela> list_escuela = new List<Escuela>();

            using (EscuelaEntities context = new EscuelaEntities())
            {
                //lleno mi lista directamente usando linq
                list_escuela = (from Escuela in context.Escuela select Escuela).ToList();
            }

            ViewBag.Titulo = "Lista de escuelas";

            return View(list_escuela);
        }

        [HttpGet]
        public ActionResult Nueva_Escuela()
        {
            return View();
        }

        //POST: Nuevo_Asignatura
        [HttpPost]
        public ActionResult Nueva_Escuela(Escuela_DTO model)
        {
            try
            {
                if (ModelState.IsValid)//checar si si es necesario validar eso
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var escuela = new Escuela();
                        escuela.Nombre = model.Nombre;
                        escuela.Clave = model.Clave;
                        escuela.Telefono = model.Telefono;
                        escuela.Nivel = model.Nivel;
                        escuela.Direccion = model.Direccion;
                        escuela.FechaFundacion = model.FechaFundacion;
                        context.Escuela.Add(escuela);
                        context.SaveChanges();
                        return RedirectToAction("Index");

                    }
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        //GET:Editar_Asignatura/{id}
        public ActionResult Editar_Escuela(int id)
        {
            if (id > 0)
            {
                Escuela_DTO escuela = new Escuela_DTO();
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var escuela_aux = context.Escuela.Where(x => x.ID_Escuela == id).FirstOrDefault();

                    escuela.Nombre = escuela.Nombre;

                    escuela = (from c in context.Escuela
                                  where c.ID_Escuela == id
                                  select new Escuela_DTO()
                                  {
                                      ID_Escuela = c.ID_Escuela,
                                      Nombre = c.Nombre,
                                      Clave = c.Clave,
                                      Telefono = c.Telefono,
                                      Nivel = c.Nivel,
                                      Direccion = c.Direccion,
                                      FechaFundacion = c.FechaFundacion
                                  }).FirstOrDefault();
                }
                if (escuela == null) 
                {
                    //sweet alert
                    return RedirectToAction("Index");
                }
                //si todo sale bien, envio a la vista con datos a editar
                ViewBag.Titulo = $"Editar escuela {escuela.ID_Escuela}";
                return View(escuela);
            }
            else
            {
                //seet alert
                return RedirectToAction("Index");
            }
        }

        //POST: Editar_Escuela
        [HttpPost]
        public ActionResult Editar_Escuela(Escuela_DTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var escuela = new Escuela();

                        escuela.ID_Escuela = model.ID_Escuela;
                        escuela.Nombre = model.Nombre;
                        escuela.Clave = model.Clave;
                        escuela.Telefono = model.Telefono;
                        escuela.Nivel = model.Nivel;
                        escuela.Direccion = model.Direccion;
                        escuela.FechaFundacion = model.FechaFundacion;

                        context.Entry(escuela).State = System.Data.Entity.EntityState.Modified;
                        try
                        {
                            context.SaveChanges();
                        }
                        //agregar using desde 'using System.Data.Entity.Validation;'
                        catch (DbEntityValidationException ex)
                        {
                            string resp = "";
                            //recorro todos los posibles errores de la Entidad Referencial
                            foreach (var error in ex.EntityValidationErrors)
                            {
                                //recorro los detalles de cada error
                                foreach (var validationError in error.ValidationErrors)
                                {
                                    resp += "Error en la Entidad: " + error.Entry.Entity.GetType().Name;
                                    resp += validationError.PropertyName;
                                    resp += validationError.ErrorMessage;
                                }
                            }
                            //Sweet Alert
                        }
                        //Sweet Alert
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    //Sweet Alert
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //Sweet Alert
                return View(model);
            }
        }

        //GET: Eliminar_Asignatura/{ID}
        public ActionResult Eliminar_Escuela(int id)
        {
            try
            {
                using (EscuelaEntities context = new EscuelaEntities())
                {

                    var escuela = context.Escuela.FirstOrDefault(x => x.ID_Escuela == id);
                    
                    if (escuela == null)
                    {
                        //sweet alert
                        SweetAlert("No encontrado", $"No hemos encontradi la asignatura con identificador: {id}", NotificationType.info);
                        return RedirectToAction("Index");
                    }
                    //procedo a eliminra
                    context.Escuela.Remove(escuela);
                    context.SaveChanges();
                    //sweetalert
                    SweetAlert("Eliminado", $"Ha ocurrido un error: ", NotificationType.success);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                //sweetalert
                SweetAlert("Opsss...", $"Ha ocurrido un error: {ex.Message}", NotificationType.error);
                return RedirectToAction("Index");
            }
        }

        //GET: Confirmar ELiminar
        public ActionResult Confirmar_Eliminar(int id)
        {
            SweetAlert_Eliminar(id);
            return RedirectToAction("Index");
        }

        #region Sweet Alert
        private void SweetAlert(string title, string msg, NotificationType type)
        {
            var script = "<script languaje='javascript'> " +
                         "Swal.fire({" +
                         "title: '" + title + "'," +
                         "text: '" + msg + "'," +
                         "icon: '" + type + "'" +
                         "});" +
                         "</script>";
            TempData["sweetalert"] = script;
        }
        private void SweetAlert_Eliminar(int id)
        {
            var script = "<script languaje='javascript'>" +
                "Swal.fire({" +
                "title: '¿Estás Seguro?'," +
                "text: 'Estás apunto de eliminar la escuela: " + id.ToString() + "'," +
                "icon: 'info'," +
                "showDenyButton: true," +
                "showCancelButton: true," +
                "confirmButtonText: 'Eliminar'," +
                "denyButtonText: 'Cancelar'" +
                "}).then((result) => {" +
                "if (result.isConfirmed) {  " +
                "window.location.href = '/Escuela/Eliminar_Escuela/" + id + "';" +
                "} else if (result.isDenied) {  " +
                "Swal.fire('Se ha cancelado la operación','','info');" +
                "}" +
                "});" +
                "</script>";

            TempData["sweetalert"] = script;
        }

        public enum NotificationType //el enum es el que limita los datos, en eset caso limita los tipos de notificaiones para el sweet alert
        {
            error,
            success,
            info,
            question
        }
        #endregion

    }
}