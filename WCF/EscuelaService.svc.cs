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
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "EscuelaService" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione EscuelaService.svc o EscuelaService.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class EscuelaService : IEscuelaService
    {
        //creo un instancia de mi contexto que sea visible y alcanzable para toods los metodos
        private readonly EscuelaEntities _context;

        //creo un constructor que inicialice el contexto para poder usarlo
        public EscuelaService()
        {
            _context = new EscuelaEntities();
        }
        public string create_Escuela(string Nombre, string Clave, string Telefono, string Nivel, string Direccion, DateTime FechaFundacion)
        {
            //preparo una respuesta
            string respuesta = "";
            try
            {
                //creo un objto del modelo original para asignarle los valores del exterior
                Escuela _escuela = new Escuela();
                _escuela.Nombre = Nombre;
                _escuela.Clave = Clave;
                _escuela.Telefono = Telefono;
                _escuela.Nivel = Nivel;
                _escuela.Direccion = Direccion;
                _escuela.FechaFundacion = FechaFundacion;

                //añado e objeto al contexto
                _context.Escuela.Add(_escuela);
                //impacto la bd
                _context.SaveChanges();
                //respuesta
                return respuesta = "Escuela registrada con exito";
            }
            catch (Exception ex)
            {
                return respuesta = "Error: " + ex.Message;
            }

        }

        public string delete_Escuela(int id)
        {
            //preparo una espuesta
            string respuesta = "";
            try
            {
                //busco el camion (el modelo original)
                Escuela _escuela = _context.Escuela.Find(id);
                //elimino el objeto del contexto
                _context.Escuela.Remove(_escuela);
                //impacto a la bd
                _context.SaveChanges();
                //respondo
                return respuesta = $"Escuela {id} eliminado con exito";
            }
            catch (Exception ex)
            {
                return respuesta = "Error: " + ex.Message;
            }
        }

        public List<Escuela_DTO> list_escuela(int id)
        {
            //creo y lleno una lista de camiones_DTO utilizando LinQ
            List<Escuela_DTO> list = new List<Escuela_DTO>();
            try
            {
                list = (from c in _context.Escuela
                        where (id == 0 || c.ID_Escuela == id)
                        select new Escuela_DTO()
                        {
                            ID_Escuela = c.ID_Escuela,
                            Nombre = c.Nombre,
                            Clave = c.Clave,
                            Telefono = c.Telefono,
                            Nivel = c.Nivel,
                            Direccion = c.Direccion,
                            FechaFundacion = c.FechaFundacion
                        }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return list;
        }

        public string update_escuela(int id, string Nombre, string Clave, string Telefono, string Nivel, string Direccion, DateTime FechaFundacion)
        {
            string respuesta = "";
            try
            {
                //creo un objto del modelo original para asignarle los valores del exterior
                Escuela _escuela = new Escuela();
                _escuela.ID_Escuela = id;
                _escuela.Nombre = Nombre;
                _escuela.Clave = Clave;
                _escuela.Telefono = Telefono;
                _escuela.Nivel = Nivel;
                _escuela.Direccion = Direccion;
                _escuela.FechaFundacion = FechaFundacion;

                //modifico el estado en el contexto
                _context.Entry(_escuela).State = System.Data.Entity.EntityState.Modified;
                //impcto la bd
                _context.SaveChanges();
                //respondo
                return respuesta = "Escuela actualizada con exito";
            }
            catch (Exception ex)
            {
                return respuesta = "Error: " + ex.Message;
            }

        }
    }
}
