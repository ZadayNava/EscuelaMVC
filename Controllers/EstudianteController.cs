using DTO;
using EscuelaMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EscuelaMVC.Controllers
{
    public class EstudianteController : Controller
    {
        public ActionResult Index()
        {
            List<EstudianteGrado_DTO> list_estudiante = new List<EstudianteGrado_DTO>();

            using (EscuelaEntities context = new EscuelaEntities())
            {
                list_estudiante = (from e in context.Estudiante
                                   join g in context.Grado on e.ID_Grado equals g.ID_Grado
                                   select new EstudianteGrado_DTO()
                                   {
                                       Grado_ = new Grado_DTO()
                                       {
                                           ID_Grado = g.ID_Grado,
                                           Grado = g.Grado1
                                       },
                                       Estudiante_ = new Estudiante_DTO()
                                       {
                                           ID_Estudiante = e.ID_Estudiante,
                                           Nombre = e.Nombre,
                                           AMaterno = e.AMaterno,
                                           APaterno = e.APaterno,
                                           CURP = e.CURP,
                                           Sexo = e.Sexo,
                                          Telefono = e.Telefono,
                                          Direccion = e.Direccion,
                                          FechaNacimiento = e.FechaNacimiento
                                       }
                                   }).ToList();
            }

            ViewBag.Titulo = "Lista de estudiantes";

            return View(list_estudiante);
        }

        [HttpGet]
        public ActionResult Nuevo_Estudiante()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo_Estudiante(Estudiante_DTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var estudiante = new Estudiante();

                        estudiante.Nombre = model.Nombre;
                        estudiante.APaterno = model.APaterno;
                        estudiante.AMaterno = model.AMaterno;
                        estudiante.CURP = model.CURP;
                        estudiante.Sexo = model.Sexo;
                        estudiante.Telefono = model.Telefono;
                        estudiante.Direccion = model.Direccion;
                        estudiante.FechaNacimiento = model.FechaNacimiento;
                        estudiante.ID_Grado = model.ID_Grado;
                        context.Estudiante.Add(estudiante);
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

        public ActionResult Editar_Estudiante(int id)
        {
            if (id > 0)
            {
                Estudiante_DTO estudiante = new Estudiante_DTO();
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var estudiante_aux = context.Estudiante.Where(x => x.ID_Estudiante == id).FirstOrDefault();
                    estudiante.ID_Estudiante = estudiante.ID_Estudiante;
                    estudiante.Nombre = estudiante.Nombre;
                    estudiante.APaterno = estudiante.APaterno;
                    estudiante.AMaterno = estudiante.AMaterno;
                    estudiante.CURP = estudiante.CURP;
                    estudiante.Sexo = estudiante.Sexo;
                    estudiante.Telefono = estudiante.Telefono;
                    estudiante.Direccion = estudiante.Direccion;
                    estudiante.FechaNacimiento = estudiante.FechaNacimiento;
                    estudiante.ID_Grado = estudiante.ID_Grado;

                    estudiante = (from c in context.Estudiante
                               where c.ID_Estudiante == id
                               select new Estudiante_DTO()
                               {
                                   ID_Estudiante = c.ID_Estudiante,
                                   Nombre = c.Nombre,
                                   APaterno = c.APaterno,
                                   AMaterno = c.AMaterno,
                                   CURP = c.CURP,
                                   Sexo = c.Sexo,
                                   Telefono = c.Telefono,
                                   Direccion = c.Direccion,
                                   FechaNacimiento = c.FechaNacimiento,
                                   ID_Grado = c.ID_Grado
                               }).FirstOrDefault();
                }
                if (estudiante == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Titulo = $"Editar estudiante {estudiante.ID_Estudiante}";
                return View(estudiante);
            }
            else
            {
                //seet alert
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Editar_Estudiante(Estudiante_DTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var estudiante = new Estudiante();

                        estudiante.ID_Estudiante = model.ID_Estudiante;
                        estudiante.Nombre = model.Nombre;
                        estudiante.APaterno = model.APaterno;
                        estudiante.AMaterno = model.AMaterno;
                        estudiante.CURP = model.CURP;
                        estudiante.Sexo = model.Sexo;
                        estudiante.Telefono = model.Telefono;
                        estudiante.Direccion = model.Direccion;
                        estudiante.FechaNacimiento = model.FechaNacimiento;
                        estudiante.ID_Grado = model.ID_Grado;

                        context.Entry(estudiante).State = System.Data.Entity.EntityState.Modified;
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            string resp = "";
                            foreach (var error in ex.EntityValidationErrors)
                            {
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

        public ActionResult Eliminar_Estudiante(int id)
        {
            try
            {
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var estudiante = context.Estudiante.FirstOrDefault(x => x.ID_Estudiante == id);
                    if (estudiante == null)
                    {
                        //sweet alert
                        SweetAlert("No encontrado", $"No hemos encontradi el estudiante con identificador: {id}", NotificationType.info);
                        return RedirectToAction("Index");
                    }
                    //procedo a eliminra
                    context.Estudiante.Remove(estudiante);
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
                "text: 'Estás apunto de eliminar al estudiante: " + id.ToString() + "'," +
                "icon: 'info'," +
                "showDenyButton: true," +
                "showCancelButton: true," +
                "confirmButtonText: 'Eliminar'," +
                "denyButtonText: 'Cancelar'" +
                "}).then((result) => {" +
                "if (result.isConfirmed) {  " +
                "window.location.href = '/Estudiante/Eliminar_Estudiante/" + id + "';" +
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