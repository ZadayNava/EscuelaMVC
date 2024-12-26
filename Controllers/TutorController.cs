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
    public class TutorController : Controller
    {
        // GET: Tutor
        public ActionResult Index()
        {
            List<TutorEstudiante_DTO> list_tutor = new List<TutorEstudiante_DTO>();

            using (EscuelaEntities context = new EscuelaEntities())
            {
                list_tutor = (from e in context.Tutor
                                join g in context.Estudiante on e.ID_Estudiante equals g.ID_Estudiante
                                select new TutorEstudiante_DTO()
                                {
                                    Tutor_ = new Tutor_DTO()
                                    {
                                        ID_Estudiante = e.ID_Estudiante,
                                        ID_Tutor = e.ID_Tutor,
                                        Nombre = e.Nombre,
                                        AMaterno = e.AMaterno,
                                        APaterno = e.APaterno,
                                        CURP = e.CURP,
                                        Sexo = e.Sexo,
                                        Telefono = e.Telefono,
                                        Parentesco = e.Parentesco,
                                        Direccion = e.Direccion,
                                        FechaNacimiento = e.FechaNacimiento
                                    },
                                    estudiante_ = new Estudiante_DTO()
                                    {
                                        ID_Estudiante = g.ID_Estudiante,
                                        Nombre = g.Nombre
                                    }
                                }).ToList();
            }

            ViewBag.Titulo = "Lista de tutores";

            return View(list_tutor);
        }

        [HttpGet]
        public ActionResult Nuevo_Tutor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo_Tutor(Tutor_DTO model)
        {
            try
            {
                if (ModelState.IsValid)//checar si si es necesario validar eso
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var tutor = new Tutor();

                        tutor.Nombre = model.Nombre;
                        tutor.APaterno = model.APaterno;
                        tutor.AMaterno = model.AMaterno;
                        tutor.CURP = model.CURP;
                        tutor.Sexo = model.Sexo;
                        tutor.Telefono = model.Telefono;
                        tutor.Parentesco = model.Parentesco;
                        tutor.Direccion = model.Direccion;
                        tutor.FechaNacimiento = model.FechaNacimiento;
                        tutor.ID_Estudiante = model.ID_Estudiante;
                        context.Tutor.Add(tutor);
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


        public ActionResult Editar_Tutor(int id)
        {
            if (id > 0)
            {
                Tutor_DTO tutor = new Tutor_DTO();
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var tutor_aux = context.Tutor.Where(x => x.ID_Tutor == id).FirstOrDefault();
                    tutor.ID_Tutor = tutor.ID_Tutor;
                    tutor.Nombre = tutor.Nombre;
                    tutor.APaterno = tutor.APaterno;
                    tutor.AMaterno = tutor.AMaterno;
                    tutor.CURP = tutor.CURP;
                    tutor.Sexo = tutor.Sexo;
                    tutor.Telefono = tutor.Telefono;
                    tutor.Parentesco = tutor.Parentesco;
                    tutor.Direccion = tutor.Direccion;
                    tutor.FechaNacimiento = tutor.FechaNacimiento;
                    tutor.ID_Estudiante = tutor.ID_Estudiante;

                    tutor = (from c in context.Tutor
                                  where c.ID_Tutor == id
                                  select new Tutor_DTO()
                                  {
                                      ID_Tutor = c.ID_Tutor,
                                      Nombre = c.Nombre,
                                      APaterno = c.APaterno,
                                      AMaterno = c.AMaterno,
                                      CURP = c.CURP,
                                      Sexo = c.Sexo,
                                      Telefono = c.Telefono,
                                      Parentesco = c.Parentesco,
                                      Direccion = c.Direccion,
                                      FechaNacimiento = c.FechaNacimiento,
                                      ID_Estudiante = c.ID_Estudiante
                                  }).FirstOrDefault();
                }
                if (tutor == null)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Titulo = $"Editar tutor {tutor.ID_Tutor}";
                return View(tutor);
            }
            else
            {
                //seet alert
                return RedirectToAction("Index");
            }
        }

        //POST: Editar_Escuela
        [HttpPost]
        public ActionResult Editar_Tutor(Tutor_DTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var tutor = new Tutor();

                        tutor.ID_Tutor = model.ID_Tutor;
                        tutor.APaterno = model.APaterno;
                        tutor.AMaterno = model.AMaterno;
                        tutor.CURP = model.CURP;
                        tutor.Sexo = model.Sexo;
                        tutor.Telefono = model.Telefono;
                        tutor.Parentesco = model.Parentesco;
                        tutor.Direccion = model.Direccion;
                        tutor.FechaNacimiento = model.FechaNacimiento;
                        tutor.ID_Estudiante = model.ID_Estudiante;

                        context.Entry(tutor).State = System.Data.Entity.EntityState.Modified;
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

        public ActionResult Eliminar_Tutor(int id)
        {
            try
            {
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var tutor = context.Tutor.FirstOrDefault(x => x.ID_Tutor == id);
                    if (tutor == null)
                    {
                        //sweet alert
                        SweetAlert("No encontrado", $"No hemos encontradi el tutor con identificador: {id}", NotificationType.info);
                        return RedirectToAction("Index");
                    }
                    //procedo a eliminra
                    context.Tutor.Remove(tutor);
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
                "text: 'Estás apunto de eliminar al tutor: " + id.ToString() + "'," +
                "icon: 'info'," +
                "showDenyButton: true," +
                "showCancelButton: true," +
                "confirmButtonText: 'Eliminar'," +
                "denyButtonText: 'Cancelar'" +
                "}).then((result) => {" +
                "if (result.isConfirmed) {  " +
                "window.location.href = '/Tutor/Eliminar_Tutor/" + id + "';" +
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