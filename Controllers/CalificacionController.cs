using DTO;
using EscuelaMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Mvc;

namespace EscuelaMVC.Controllers
{
    public class CalificacionController : Controller
    {
        // GET: Calificacion
        public ActionResult Index()
        {
            List<CalificacionesConNombre_DTO> list_calificacion = new List<CalificacionesConNombre_DTO>();

            using (EscuelaEntities context = new EscuelaEntities())
            {
                list_calificacion = (from c in context.Calificacion
                                     join e in context.Estudiante on c.ID_Estudiante equals e.ID_Estudiante
                                     join a in context.Asignatura on c.ID_Asignatura equals a.ID_Asignatura
                                     select new CalificacionesConNombre_DTO()
                                     {
                                         Calificacion_ = new Calificacion_DTO()
                                         {
                                             ID_Calificacion = c.ID_Calificacion,
                                             Calificacion = c.Calificacion1
                                         },
                                         Asignatura_ = new Asignatura_DTO()
                                         {
                                             ID_Asignatura = a.ID_Asignatura,
                                             Nombre = a.Nombre
                                         },
                                         Estudiante_ = new Estudiante_DTO()
                                         {
                                             ID_Estudiante = e.ID_Estudiante,
                                             Nombre = e.Nombre
                                         }
                                     }).OrderByDescending(x => x.Estudiante_.Nombre).ToList();
            }

            ViewBag.Titulo = "Lista de calificacion";                
            return View(list_calificacion);
        }

        [HttpGet]
        public ActionResult Nueva_Calificacion()
        {
            cargarDDL();
            return View();
        }

        [HttpPost]
        public ActionResult Nueva_Calificacion(Calificacion_DTO model)
        {
            try
            {
                if (model.Calificacion > 0)
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var calificaciones = new Calificacion();
                        calificaciones.Calificacion1 = model.Calificacion;
                        calificaciones.ID_Estudiante = model.ID_Estudiante;
                        calificaciones.ID_Asignatura = model.ID_Asignatura;
                        context.Calificacion.Add(calificaciones);
                        context.SaveChanges();
                        SweetAlert("Registrado!", $"Todo correcto", NotificationType.success);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    cargarDDL();
                    SweetAlert("No es valido", $"Ha ocurrido un error: ", NotificationType.error);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                cargarDDL();
                SweetAlert("Opsss...", $"Ha ocurrido un error: {ex.Message}", NotificationType.error);
                return View(model);
            }
        }

        //GET:Editar_Calficacion/{id}
        public ActionResult Editar_Calificacion(int id)
        {
            cargarDDL();
            if (id > 0)
            {
                Calificacion_DTO cal = new Calificacion_DTO();

                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var cal_aux = context.Calificacion.Where(x => x.ID_Calificacion == id).FirstOrDefault();

                    cal.ID_Calificacion = cal_aux.ID_Calificacion;
                    cal.Calificacion = cal_aux.Calificacion1;
                    cal.ID_Estudiante = cal_aux.ID_Estudiante;
                    cal.ID_Asignatura = cal_aux.ID_Asignatura;


                }//cierre el "using(context)"
                if (cal == null) //valido si realmente recuperé los datos de la bd
                {
                    //sweet alert
                    return RedirectToAction("Index");
                }
                //si todo sale bien, envio a la vista con datos a editar
                ViewBag.Titulo = $"Editar calificacion {cal.ID_Calificacion}";
                return View(cal);
            }
            else
            {
                //seet alert
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Editar_Calificacion(Calificacion_DTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var cali = new Calificacion();

                        cali.ID_Calificacion = model.ID_Calificacion;
                        cali.Calificacion1 = model.Calificacion;
                        cali.ID_Estudiante = model.ID_Estudiante;
                        cali.ID_Asignatura = model.ID_Asignatura;

                        context.Entry(cali).State = System.Data.Entity.EntityState.Modified;
                        //impactamos la BD
                        try
                        {
                            context.SaveChanges();
                            SweetAlert("Editado!", $"Todo correcto", NotificationType.success);
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
                    SweetAlert("No es valido", $"Ha ocurrido un error: ", NotificationType.error);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                //Sweet Alert
                SweetAlert("Opsss...", $"Ha ocurrido un error: {ex.Message}", NotificationType.error);
                return View(model);
            }
        }

        public ActionResult Eliminar_Calificacion(int id)
        {
            try
            {
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var cali = context.Calificacion.FirstOrDefault(x => x.ID_Calificacion == id);
                    if (cali == null)
                    {
                        //sweet alert
                        SweetAlert("No encontrado", $"No hemos encontradi la calificacion con identificador: {id}", NotificationType.info);
                        return RedirectToAction("Index");
                    }
                    //procedo a eliminra
                    context.Calificacion.Remove(cali);
                    context.SaveChanges();
                    //sweetalert
                    SweetAlert("Eliminado", $"Todo correcto", NotificationType.success);
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


        public void cargarDDL()
        {
            
            List<Estudiante> Lista_Estudiantes = new List<Estudiante>();

            List<Asignatura> Lista_Asignatura = new List<Asignatura>();

            using (EscuelaEntities contexto = new EscuelaEntities())
            {
                Lista_Estudiantes = (from Estudiante in contexto.Estudiante select Estudiante).ToList();
                ViewBag.Lista_Estudiantes = Lista_Estudiantes;

                Lista_Asignatura = (from Asignatura in contexto.Asignatura select Asignatura).ToList();
                ViewBag.Lista_Asignatura = Lista_Asignatura;
            }
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
                "text: 'Estás apunto de eliminar la calificacion: " + id.ToString() + "'," +
                "icon: 'info'," +
                "showDenyButton: true," +
                "showCancelButton: true," +
                "confirmButtonText: 'Eliminar'," +
                "denyButtonText: 'Cancelar'" +
                "}).then((result) => {" +
                "if (result.isConfirmed) {  " +
                "window.location.href = '/Calificacion/Eliminar_Calificacion/" + id + "';" +
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