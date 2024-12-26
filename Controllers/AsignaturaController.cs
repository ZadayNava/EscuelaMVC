using Antlr.Runtime.Misc;
using DTO;
using EscuelaMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EscuelaMVC.Controllers
{
    public class AsignaturaController : Controller
    {
        // GET: Asignatura
        public ActionResult Index()
        {
            List<Asignatura> list_asignaturas = new List<Asignatura>();

            using (EscuelaEntities context = new EscuelaEntities())
            {
                list_asignaturas = (from Asignatura in context.Asignatura select Asignatura).ToList();
            }
            ViewBag.Titulo = "Lista de asignaturas";
            return View(list_asignaturas);
        }

        [HttpGet]
        public ActionResult Nueva_Asignatura()
        {
            return View();
        }

        //POST: Nuevo_Asignatura
        [HttpPost]
        public ActionResult Nueva_Asignatura(Asignatura_DTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var asignatura = new Asignatura();
                        asignatura.Nombre = model.Nombre;
                        context.Asignatura.Add(asignatura);
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
        public ActionResult Editar_Asignatura(int id)
        {
            if (id > 0)
            {
               Asignatura_DTO asignatura = new Asignatura_DTO();
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var asignatura_aux = context.Asignatura.Where(x => x.ID_Asignatura == id).FirstOrDefault();

                    asignatura.Nombre = asignatura_aux.Nombre;

                    asignatura = (from c in context.Asignatura
                              where c.ID_Asignatura == id
                              select new Asignatura_DTO()
                              {
                                  ID_Asignatura = c.ID_Asignatura,
                                  Nombre = c.Nombre
                              }).FirstOrDefault();
                }
                if (asignatura == null) 
                {
                    //sweet alert
                    return RedirectToAction("Index");
                }

                ViewBag.Titulo = $"Editar asignatura {asignatura.ID_Asignatura}";
                return View(asignatura);
            }
            else
            {
                //seet alert
                return RedirectToAction("Index");
            }
        }

        //POST: Editar_Asignatura
        [HttpPost]
        public ActionResult Editar_Asignatura(Asignatura_DTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var asignatura = new Asignatura();

                        asignatura.ID_Asignatura = model.ID_Asignatura;
                        asignatura.Nombre = model.Nombre;
                        context.Entry(asignatura).State = System.Data.Entity.EntityState.Modified;
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            string resp = "";
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
        public ActionResult Eliminar_Asignatura(int id)
        {
            try
            {
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var asignatura = context.Asignatura.FirstOrDefault(x => x.ID_Asignatura == id);
                    if (asignatura == null)
                    {
                        //sweet alert
                        SweetAlert("No encontrado", $"No hemos encontradi la asignatura con identificador: {id}", NotificationType.info);
                        return RedirectToAction("Index");
                    }
                    context.Asignatura.Remove(asignatura);
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
        //declaraciones de un htmlhelper personalizado: Digase que alquel metodo auxiliar que me permite construir codigo html/s en tiempo real basado en las acciones del razor/controller
        private void SweetAlert(string title, string msg, NotificationType type)
        {
            var script = "<script languaje='javascript'> " +
                         "Swal.fire({" +
                         "title: '" + title + "'," +
                         "text: '" + msg + "'," +
                         "icon: '" + type + "'" +
                         "});" +
                         "</script>";
            //TempData funciona como ViewBag, pasando informacion del controlador a cualquier parte de mi proyecto, sinedo este, mas observable y con un tiempo de vida mayoe
            TempData["sweetalert"] = script;
        }
        private void SweetAlert_Eliminar(int id)
        {
            var script = "<script languaje='javascript'>" +
                "Swal.fire({" +
                "title: '¿Estás Seguro?'," +
                "text: 'Estás apunto de eliminar la asignatura: " + id.ToString() + "'," +
                "icon: 'info'," +
                "showDenyButton: true," +
                "showCancelButton: true," +
                "confirmButtonText: 'Eliminar'," +
                "denyButtonText: 'Cancelar'" +
                "}).then((result) => {" +
                "if (result.isConfirmed) {  " +
                "window.location.href = '/Asignatura/Eliminar_Asignatura/" + id + "';" +
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