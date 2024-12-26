using DTO;
using EscuelaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace EscuelaMVC.WCF
{
    public class AsignaturaService : IAsignaturaService
    {

        //creo un instancia de mi contexto que sea visible y alcanzable para toods los metodos
        private readonly EscuelaEntities _context;

        //creo un constructor que inicialice el contexto para poder usarlo
        public AsignaturaService()
        {
            _context = new EscuelaEntities();
        }
        public string create_Asignatura(string Nombre)
        {
            //preparo una respuesta
            string respuesta = "";
            try
            {
                //creo un objto del modelo original para asignarle los valores del exterior
                Asignatura _asignatura = new Asignatura();
                _asignatura.Nombre = Nombre;

                //añado e objeto al contexto
                _context.Asignatura.Add(_asignatura);
                //impacto la bd
                _context.SaveChanges();
                //respuesta
                return respuesta = "Asignatura registrada con exito";
            }
            catch (Exception ex)
            {
                return respuesta = "Error: " + ex.Message;
            }

        }

        public string delete_Asignatura(int id)
        {
            string respuesta = "";
            try
            {
                Asignatura _asignatura = _context.Asignatura.Find(id);
                _context.Asignatura.Remove(_asignatura);
                _context.SaveChanges();
                return respuesta = $"Asignatura {id} eliminado con exito";
            }
            catch (Exception ex)
            {
                return respuesta = "Error: " + ex.Message;
            }
        }

        public List<Asignatura_DTO> list_asignatura(int id)
        {
            //creo y lleno una lista de camiones_DTO utilizando LinQ
            List<Asignatura_DTO> list = new List<Asignatura_DTO>();
            try
            {
                list = (from c in _context.Asignatura
                        where (id == 0 || c.ID_Asignatura == id)
                        select new Asignatura_DTO()
                        {
                            ID_Asignatura = c.ID_Asignatura,
                            Nombre = c.Nombre
                        }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return list;
        }

        public string update_Asignatura(int id, string Nombre)
        {
            //preparo una respuesta
            //preparo una respuesta
            string respuesta = "";
            try
            {
                //creo un objto del modelo original para asignarle los valores del exterior
                Asignatura _asignatura = new Asignatura();
                _asignatura.Nombre = Nombre;

                //modifico el estado en el contexto
                _context.Entry(_asignatura).State = System.Data.Entity.EntityState.Modified;
                //impcto la bd
                _context.SaveChanges();
                //respondo
                return respuesta = "Asignatura actualizado con exito";
            }
            catch (Exception ex)
            {
                return respuesta = "Error: " + ex.Message;
            }

        }

    }
}
