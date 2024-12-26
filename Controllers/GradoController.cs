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
    public class GradoController : Controller
    {
        // GET: Grado
        public ActionResult Index()
        {
            List<GradoProfesor_DTO> list_grados = new List<GradoProfesor_DTO>();

            using (EscuelaEntities context = new EscuelaEntities())
            {
                list_grados = (from e in context.Profesor
                               join g in context.Grado on e.ID_Profesor equals g.ID_Profesor
                               select new GradoProfesor_DTO()
                               {
                                   gradoP_ = new Grado_DTO()
                                   {
                                       ID_Grado = g.ID_Grado,
                                       Grado = g.Grado1,
                                       Grupo = g.Grupo
                                   },
                                   Profesor_ = new Profesor_DTO()
                                   {
                                       ID_Profesor = e.ID_Profesor,
                                       Nombre = e.Nombre
                                   }
                               }).ToList();

            }

            ViewBag.Titulo = "Lista de grados";

            return View(list_grados);
        }

        [HttpGet]
        public ActionResult Nuevo_Grado()
        {
            cargarDDL();
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo_Grado(Grado_DTO model)
        {
            try
            {
                if (model.Grado != "" && model.Grupo != "") 
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var grado = new Grado();
                        grado.Grado1 = model.Grado;
                        grado.Grupo = model.Grupo;
                        grado.ID_Profesor = model.ID_Profesor;
                        context.Grado.Add(grado);
                        context.SaveChanges();
                        SweetAlert("Registrado!", $"Todo correcto", NotificationType.success);
                        return RedirectToAction("Index");

                    }
                }
                else
                {
                    cargarDDL();
                    SweetAlert("Falta un dato!", $"Es necesario llenar el formulario", NotificationType.info);
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

        public ActionResult Editar_Grado(int id)
        {
            if (id > 0)
            {
                Grado_DTO grados = new Grado_DTO();
                using (EscuelaEntities context = new EscuelaEntities())
                {
                    var grad_aux = context.Grado.Where(x => x.ID_Grado == id).FirstOrDefault();

                    grados.Grado = grad_aux.Grado1;
                    grados.Grupo = grad_aux.Grupo;
                    grados.ID_Profesor = grad_aux.ID_Profesor;
                    grados.ID_Grado = id;

                    grados = (from c in context.Grado
                              where c.ID_Grado == id
                                  select new Grado_DTO()
                                  {
                                      ID_Grado = c.ID_Grado,
                                      Grado = c.Grado1,
                                      Grupo = c.Grupo,
                                      ID_Profesor= c.ID_Profesor
                                  }).FirstOrDefault();
                }
                if (grados == null) 
                {
                    return RedirectToAction("Index");
                }
                //si todo sale bien, envio a la vista con datos a editar
                ViewBag.Titulo = $"Editar grado {grados.ID_Grado}";
                cargarDDL();
                return View(grados);
            }
            else
            {
                //seet alert
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult Editar_Grado(Grado_DTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (EscuelaEntities context = new EscuelaEntities())
                    {
                        var grado = new Grado();

                        grado.ID_Grado = model.ID_Grado;
                        grado.Grupo = model.Grupo;
                        grado.Grado1 = model.Grado;
                        grado.ID_Profesor = model.ID_Profesor;

                        context.Entry(grado).State = System.Data.Entity.EntityState.Modified;

                        try
                        {
                            context.SaveChanges();
                            SweetAlert("Editado!", $"Todo correcto", NotificationType.success);
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
                            SweetAlert("Opsss...", $"Ha ocurrido un error: {ex.Message}", NotificationType.error);
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


        public ActionResult Eliminar_Grado(int id)
        {
            try
            {
                using (EscuelaEntities context = new EscuelaEntities())
                {

                    var grado = context.Grado.FirstOrDefault(x => x.ID_Grado == id);

                    if (grado == null)
                    {
                        //sweet alert
                        SweetAlert("No encontrado", $"No hemos encontradi el grado con identificador: {id}", NotificationType.info);
                        return RedirectToAction("Index");
                    }
                    //procedo a eliminra
                    context.Grado.Remove(grado);
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

        public ActionResult Confirmar_Eliminar(int id)
        {
            SweetAlert_Eliminar(id);
            return RedirectToAction("Index");
        }
        public void cargarDDL()
        {

            List<Profesor> Lista_Profesor = new List<Profesor>();

            using (EscuelaEntities contexto = new EscuelaEntities())
            {
                Lista_Profesor = (from Profesor in contexto.Profesor select Profesor).ToList();
                ViewBag.Lista_Profesor = Lista_Profesor;
            }
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
                "text: 'Estás apunto de eliminar el grado: " + id.ToString() + "'," +
                "icon: 'info'," +
                "showDenyButton: true," +
                "showCancelButton: true," +
                "confirmButtonText: 'Eliminar'," +
                "denyButtonText: 'Cancelar'" +
                "}).then((result) => {" +
                "if (result.isConfirmed) {  " +
                "window.location.href = '/Grado/Eliminar_Grado/" + id + "';" +
                "} else if (result.isDenied) {  " +
                "Swal.fire('Se ha cancelado la operación','','info');" +
                "}" +
                "});" +
                "</script>";

            TempData["sweetalert"] = script;
        }

        public enum NotificationType 
        {
            error,
            success,
            info,
            question
        }
        #endregion

    }
}