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
    public class ProesorController : Controller
    {
        // GET: Proesor
        public ActionResult Index()
        {
            List<Profesor> list_prof = new List<Profesor>();

            using (EscuelaEntities context = new EscuelaEntities())
            {
                //lleno mi lista directamente usando linq
                list_prof = (from Profesor in context.Profesor select Profesor).ToList();
            }

            ViewBag.Titulo = "Lista de Profesores";

            return View(list_prof);
        }

        [HttpGet]
        public ActionResult Nuevo_Profesor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo_Profesor(Profesor_DTO model)
        {
            try
            {
                if (ModelState.IsValid)//checar si si es necesario validar eso
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var profesor = new Profesor();

                        profesor.Nombre = model.Nombre;
                        profesor.APaterno = model.APaterno;
                        profesor.AMaterno = model.AMaterno;
                        profesor.CURP = model.CURP;
                        profesor.Sexo = model.Sexo;
                        profesor.Telefono = model.Telefono;
                        profesor.FechaNacimiento = model.FechaNacimiento;
                        context.Profesor.Add(profesor);
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

        public ActionResult Editar_Profesor(int id)
        {
            if (id > 0)
            {
                Profesor_DTO profe = new Profesor_DTO();
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var profe_aux = context.Profesor.Where(x => x.ID_Profesor == id).FirstOrDefault();
                    profe.ID_Profesor = profe.ID_Profesor;
                    profe.Nombre = profe.Nombre;
                    profe.APaterno = profe.APaterno;
                    profe.AMaterno = profe.AMaterno;
                    profe.CURP = profe.CURP;
                    profe.Sexo = profe.Sexo;
                    profe.Telefono = profe.Telefono;
                    profe.FechaNacimiento = profe.FechaNacimiento;

                    profe = (from c in context.Profesor
                             where c.ID_Profesor == id
                                  select new Profesor_DTO()
                                  {
                                      ID_Profesor = c.ID_Profesor,
                                      Nombre = c.Nombre,
                                      APaterno = c.APaterno,
                                      AMaterno = c.AMaterno,
                                      CURP = c.CURP,
                                      Sexo = c.Sexo,
                                      Telefono = c.Telefono,
                                      FechaNacimiento = c.FechaNacimiento
                                  }).FirstOrDefault();
                }
                if (profe == null)
                {
                    return RedirectToAction("Index");
                }
                //si todo sale bien, envio a la vista con datos a editar
                ViewBag.Titulo = $"Editar profesor: {profe.ID_Profesor}";
                return View(profe);
            }
            else
            {
                //seet alert
                return RedirectToAction("Index");
            }
        }

        //POST: Editar_Escuela
        [HttpPost]
        public ActionResult Editar_Profesor(Profesor_DTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var proff = new Profesor();

                        proff.ID_Profesor = model.ID_Profesor;
                        proff.Nombre = model.Nombre;
                        proff.APaterno = model.APaterno;
                        proff.AMaterno = model.AMaterno;
                        proff.CURP = model.CURP;
                        proff.Sexo = model.Sexo;
                        proff.Telefono = model.Telefono;
                        proff.FechaNacimiento = model.FechaNacimiento;

                        context.Entry(proff).State = System.Data.Entity.EntityState.Modified;
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

        public ActionResult Eliminar_Profesor(int id)
        {
            try
            {
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var profe = context.Profesor.FirstOrDefault(x => x.ID_Profesor == id);
                    if (profe == null)
                    {
                        //sweet alert
                        SweetAlert("No encontrado", $"No hemos encontradi el profesor con identificador: {id}", NotificationType.info);
                        return RedirectToAction("Index");
                    }
                    //procedo a eliminra
                    context.Profesor.Remove(profe);
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
                "text: 'Estás apunto de eliminar al profesor: " + id.ToString() + "'," +
                "icon: 'info'," +
                "showDenyButton: true," +
                "showCancelButton: true," +
                "confirmButtonText: 'Eliminar'," +
                "denyButtonText: 'Cancelar'" +
                "}).then((result) => {" +
                "if (result.isConfirmed) {  " +
                "window.location.href = '/Proesor/Eliminar_Profesor/" + id + "';" +
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