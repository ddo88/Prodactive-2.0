﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using FluentMongo.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoModels;
using Zeitgeist.Appsco.Web.Api;
using Zeitgeist.Appsco.Web.Api.Model;
using Zeitgeist.Appsco.Web.Helpers;
using Zeitgeist.Appsco.Web.Models;
using Zeitgeist.Appsco.Web.Properties;
using MailClass = ZeitGeist.Tools.MailClass;

namespace Zeitgeist.Appsco.Web.App_Start
{
    public class Manager
    {

        public static Manager _instance;
        public static object _mutex = new object();
        public MongoDatabase Database;
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Manager));
        public static Manager Instance
        {
			get {
			
				if (_instance == null)
				{
					lock(_mutex)
					{
						if (_instance == null)
						{
							_instance= new Manager();
						}
					}
				}
				return _instance;
			}
            
        }
        
        public Manager()
        {
            
            var client = new MongoClient(Settings.Default.ConnectionString);
            var server = client.GetServer();
            Database   = server.GetDatabase(Settings.Default.Bd);
        }
        
        public MongoCollection<T> GetCollection<T>(string collection)
        {
            if (HttpRuntime.Cache[collection] == null)
            {
                var a = Database.GetCollection<T>(collection);
                insert(collection,a);
                return a;
            }
            
            return HttpRuntime.Cache[collection] as MongoCollection<T>;

        }

        public void insert(string key, object obj)
        {
            HttpRuntime.Cache.Insert(
                /* key */ key,
                /* value */ obj,
                /* dependencies */ null,
                /* absoluteExpiration */ Cache.NoAbsoluteExpiration,
                /* slidingExpiration */ Cache.NoSlidingExpiration,
                /* priority */ CacheItemPriority.NotRemovable,
                /* onRemoveCallback */ null);
        }
        public void RemoveCache(string key)
        {
            if (HttpRuntime.Cache[key] != null)
                HttpRuntime.Cache.Remove(key);
        }

        public bool Save<T>             (T item,string collection)
        {
            var wc = GetCollection<T>(collection).Save(item);
            if (wc.Ok)
                return true;

            return false;
        }
        public bool SaveClient          (LandingData item)
        {
            return Save(item,Settings.Default.ClientCollection);
        }
        public bool SavePersona         (Persona p)
        {
            return Save(p, Settings.Default.ColeccionPersona);
        }
        public bool SaveLiga            (Liga liga)
        {
            return Save(liga, Settings.Default.ColeccionLiga);
        }
        public bool SaveEquipo          (Equipo equipo)
        {
            return Save(equipo, Settings.Default.CollectionEquipos);
        }
        public bool SaveDivision        (Division division)
        {
            return Save(division, Settings.Default.ColeccionDivision);
        }
        public bool SaveRegistroProgreso(RequestLogEjercicio reg)
        {
            return Save(reg.ToLogEjercicio(), Settings.Default.CollectionLogEjercicio);
        }
        public bool SaveInvitacion      (Invitacion invitacion)
        {
            return Save(invitacion, Settings.Default.ColeccionInvitaciones);
        }
        public bool SaveReto            (Reto r)
        {
            return Save(r, Settings.Default.ColeccionRetos);
        }
        public bool UpdateReto          (Reto reto)
        {
            var wr = GetCollection<Reto>(Settings.Default.ColeccionRetos)
                .Update(Query.EQ("_id", new ObjectId(reto.Id)), Update.Replace(reto));
            if (wr.Ok)
            {
                RemoveCache(Settings.Default.ColeccionRetos);
                return true;
            }

            return false;
        }
        
        public bool DeleteLiga(Liga liga)
        {
            WriteConcernResult wr = GetCollection<Liga>(Settings.Default.ColeccionLiga).Remove(Query.EQ("_id", new ObjectId(liga.Id)));
            if (wr.Ok)
                return true;
            return false;
        }

        public bool AddUserToleague(Invitacion invitacion)
        {
            try
            {
                
                Task<Persona> tp = Task.Factory.StartNew(() => { return GetDatosUsuarioByMail(invitacion.Mail); });
                Liga l = GetLigaById(invitacion.Id);
                
                Persona p = tp.Result;
                var a = p.Cuentas.Where(x => x.Value == invitacion.Mail).Select(x => x.Key).First();

                if (l.Usuarios == null)
                    l.Usuarios = new Dictionary<string, string>();

                l.Usuarios.Add(a, invitacion.Mail);
                WriteConcernResult wcr = GetCollection<Liga>(Settings.Default.ColeccionLiga).Update(
                    Query.EQ("_id", new ObjectId(l.Id)),
                    Update.Replace(l));
                if (wcr.Ok)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool AddUserToleague(string idLiga, string user)
        {
            /*
             * añadir persona a un liga, tengo el mail, pero necesito el usuario, 
             * tengo el usuario y lo guardo en la lista de usuarios pero no tengo el area
             */
            try
            {

                Persona p = GetDatosUsuario(user);
                Task<Liga> t = Task.Factory.StartNew(() => { return GetLigaById(idLiga); });
                Task r = Task.Factory.StartNew(() =>
                {
                    return GetCollection<Invitacion>(Settings.Default.ColeccionInvitaciones)
                        .Update(Query.EQ("Cuentas.k", user), Update.Set("Estado", true));
                });

                Liga l = t.Result;
                if (l.Usuarios == null)
                    l.Usuarios = new Dictionary<string, string>();

                l.Usuarios.Add(user, p.Cuentas[user]);
                WriteConcernResult wcr = GetCollection<Liga>(Settings.Default.ColeccionLiga).Update(
                    Query.EQ("_id", new ObjectId(l.Id)),
                    Update.Replace(l));

                Task.WaitAll(new Task[] {r});
                if (wcr.Ok)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CorreoDisponible(string mail)
        {

            var a = GetCollection<Persona>(Settings.Default.ColeccionPersona).Find(Query.EQ("Cuentas.v", mail)).Count();
            if (a == 0)
                return true;
            return false;
        }
        
        public Persona  GetDatosUsuario(string user)
        {
            Persona p= new Persona();
            try
            {
                //var wr2 = GetCollection<Persona>(Settings.Default.ColeccionPersona).FindAll();
                var wr =GetCollection<Persona>(Settings.Default.ColeccionPersona).Find(Query.EQ("Cuentas.k", user));

                if (wr != null)
                    p= wr.First();
            }
            catch (Exception ex)
            {
            }
            return p;
        }
        public Persona  GetDatosUsuarioByMail(string mail)
        {
            Persona p = new Persona();
            try
            {
                //var wr2 = GetCollection<Persona>(Settings.Default.ColeccionPersona).FindAll();
                var wr = GetCollection<Persona>(Settings.Default.ColeccionPersona).Find(Query.EQ("Cuentas.v", mail));

                if (wr != null)
                    p = wr.First();
            }
            catch (Exception ex)
            {
                log.Error("Error al Buscar Datos de Usuario por Email",ex);
            }
            return p;
        }
        public Reto     GetReto()
        {
            var wr = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().FirstOrDefault();
            if (wr != null)
                return wr;
            return new Reto();
        }
        public Liga     GetLigaById(string id)
        {
            return GetCollection<Liga>(Settings.Default.ColeccionLiga).AsQueryable().First(x => x.Id==id);
        }
        public Division GetDivisionById(string division)
        {
            return GetCollection<Division>(Settings.Default.ColeccionDivision)
                .Find(Query.EQ("_id", new ObjectId(division)))
                .First();
        }
        public Reto     GetRetoById(string id)
        {
            var wr = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().Where(x => x.Id == id).FirstOrDefault();
            if (wr != null)
                return wr;
            return new Reto();
        }
        public Reto     GetReto(string user)
        {
            List<Reto> retos = GetRetosActivos();
            if (retos.Count == 0)
                retos = GetUltimosReto();
            foreach (var reto in retos)
            {
               List<Division> d= GetCollection<Division>(Settings.Default.ColeccionDivision).Find(Query.And(new[]
                {
                    Query.EQ("_id", new ObjectId(reto.Division)),
                    Query.EQ("Equipos.Miembros", user)
                })).ToList();
                if (d.Count>0)
                    return reto;
            }
            return new Reto();
        }

        public      List<Liga>         GetLigas(string p)
        {
            return GetCollection<Liga>(Settings.Default.ColeccionLiga).AsQueryable().Where(x => x.Entrenador == p).ToList();
        }
        public      List<Division>     GetDivisiones(string user)
        {
            List<Liga> lst = GetLigas(user).ToList();
            var res = lst.Select(x => x.Divisiones.ToList());
            int j = 0;
            //List<Division> divisiones=GetCollection<Division>(Settings.Default.ColeccionDivision)
            //    .AsQueryable()
            //    .Where(x => lst.Contains(y=> y.Division.co))
            //    .ToList();
            //if(divisiones.Count==0)


            return new List<Division>();

            //return divisiones;
        }
        public      List<Reto>         GetRetos(string user)
        {
            List<Reto> l = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().Where(x => x.Entrenador == user).ToList();
            if (l.Count == 0)
                return new List<Reto>();
            return l;
        }
        public      List<Reto>         GetRetosByLiga(string idLiga)
        {
            List<Reto> l = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().Where(x => x.Liga == idLiga).ToList();
            if (l.Count == 0)
                return new List<Reto>();
            return l;
        }
        public List<Reto> GetRetosActivosByLiga(string idLiga)
        {
            List<Reto> l = GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable().Where(x => x.Liga == idLiga && x.IsActivo).ToList();
            
            if (l.Count == 0)
                return new List<Reto>();
            return l;
        }
        public      List<Reto>         GetUltimosReto()
        {
            var query =
                (from a in GetCollection<Reto>(Settings.Default.ColeccionRetos).AsQueryable()
                    orderby a.FechaFin descending
                    select a).Take(50);
            return query.ToList();
        }
        public      List<Reto>         GetRetosActivos()
        {
            List<Reto> retos = GetCollection<Reto>(Settings.Default.ColeccionRetos).Find(Query.EQ("IsActivo", true)).ToList();
            if (retos.Count > 0)
                return retos;
            return new List<Reto>();
        }
        public      List<Liga>         GetLeagueUserRegistered(string user)
        {
            try
            {
                var r=GetCollection<Liga>(Settings.Default.ColeccionLiga).Find(Query.EQ("Usuarios.k", user));
                return r.ToList();
            }
            catch (Exception ex)
            {

                log.Error("GetLeagueUserRegistered", ex);
                throw;
            }
        }
        public      List<Reto>         GetRetosByIdLiga(string id)
        {
            try
            {
                return GetCollection<Reto>(Settings.Default.ColeccionRetos).Find(Query.EQ("Liga", id)).ToList();
            }
            catch (Exception ex)
            {
                log.Error("GetRetosByIdLiga", ex);
                throw;
            }
        }
        public      List<LogEjercicio> GetDatosRetoEquipo(ICollection<string> equipos, string usuario,Reto reto)
        {
                var r=GetCollection<Equipo>(Settings.Default.CollectionEquipos).Find(Query.And(new []
                {
                    Query.In("_id",new BsonArray(equipos.Select(x => new ObjectId(x)).ToList())),
                    Query.EQ("Miembros", usuario)
                })).ToList();
                Equipo p= new Equipo();
                if (r.Count > 0)
                {
                    p = r.First();
                    return GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new[]
                                                                                            { Query.In("Usuario", new BsonArray(p.Miembros)),
                                                                                              Query.GTE("FechaHora", reto.FechaInicio),
                                                                                              Query.LTE("FechaHora", reto.FechaFin)
                                                                                             })).ToList();
                }
            return new List<LogEjercicio>();
        }

        //REV: para eliminar este metodo
        public      List<LogEjercicio> GetDatosRetoEquipo(Reto reto)
        {
            List<LogEjercicio> lst = new List<LogEjercicio>();
            var r = GetEquipos(reto.Equipos);
            Parallel.ForEach(r, (e) =>
            {
                var l = GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new[]
                                                                                            { Query.In("Usuario", new BsonArray(e.Miembros)),
                                                                                              Query.GTE("FechaHora", reto.FechaInicio),
                                                                                              Query.LTE("FechaHora", reto.FechaFin)
                                                                                             })).ToList();
                lst.AddRange(l);
            });

            
            return lst;
        }
        
        public      List<LogEjercicio> GetDatosRetoUsuario(Reto reto,string user)
        {
            List<LogEjercicio> lst = new List<LogEjercicio>();
            var r = GetEquipos(reto.Equipos);
            Parallel.ForEach(r, (e) =>
            {
                var l = GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new[]
                                                                                            { Query.EQ("Usuario", user),
                                                                                              Query.GTE("FechaHora", reto.FechaInicio),
                                                                                              Query.LTE("FechaHora", reto.FechaFin)
                                                                                             })).ToList();
                lst.AddRange(l);
            });

            return lst;
        }

        public List<LogDetalleUsuario> GetDatosRetoEquipo2(Reto reto)
        {
            List<LogDetalleUsuario> lst = new List<LogDetalleUsuario>();
            var r = GetEquipos(reto.Equipos);
           
            Parallel.ForEach(r, (e) =>
            {

                IMongoGroupBy group = new GroupByBuilder(new[] { "FechaHora","Usuario" });
                var key = "FechaHora, Usuario";
                var initial = new BsonDocument("SumConteo", 0);
                var reduce = @"function(obj, prev) { prev.SumConteo = prev.SumConteo + obj.Conteo - 0; }";
                var query = Query.And(new[]
                {
                    Query.In("Usuario", new BsonArray(e.Miembros)),
                    Query.GTE("FechaHora", reto.FechaInicio),
                    Query.LTE("FechaHora", reto.FechaFin)
                });
                var l = GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio)
                    .Group(query, group, initial, reduce, null)
                    .ToList();

                foreach (var a in l)
                {
                    lst.Add(BsonSerializer.Deserialize<LogDetalleUsuario>(a));
                    Console.WriteLine("{0}",a.ToString());    
                }
            });

            return lst;

        }
        public List<LogDetalleUsuario> GetDatosRetoUsuario2(Reto reto, string user)
        {

            List<LogDetalleUsuario> lst = new List<LogDetalleUsuario>();

            var key     = "FechaHora";
            var initial = new BsonDocument("SumConteo", 0);
            var reduce  = @"function(obj, prev) { prev.SumConteo = prev.SumConteo + obj.Conteo - 0; }";
            var query   = Query.And(new[]
                    {
                        Query.EQ("Usuario", user),
                        Query.GTE("FechaHora", reto.FechaInicio),
                        Query.LTE("FechaHora", reto.FechaFin)
                    });
            var l =  GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio)
                    .Group(query, key, initial, reduce, null)
                    .ToList();
            
            
            foreach (var a in l)
            {
                lst.Add(BsonSerializer.Deserialize<LogDetalleUsuario>(a));
            }
            
            return lst;
        }


        public List<LogEjercicio> GetDatosRetoEquipoByDay(Reto reto,DateTime fecha)
        {
            List<LogEjercicio> lst = new List<LogEjercicio>();
            var r = GetEquipos(reto.Equipos);
            DateTime inicio = new DateTime(fecha.Year, fecha.Month, fecha.Day,  0,  0,  0);
            DateTime fin    = new DateTime(fecha.Year, fecha.Month, fecha.Day, 23, 59, 59);
            Parallel.ForEach(r, (e) =>
            {
                var l = GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio)
                    .Find(Query.And(new[]
                                    { Query.In("Usuario", new BsonArray(e.Miembros)),
                                      Query.GTE("FechaHora", inicio),
                                      Query.LTE("FechaHora", fin)
                                    })).ToList();
                lst.AddRange(l);
            });
            return lst;
        }
        public      List<LogEjercicio> GetLogEjercicioByIdReto(string id,string user)
        {
            Reto reto = GetRetoById(id);
            var l = GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new[]
                                                                                            { Query.EQ("Usuario",user),
                                                                                              Query.GTE("FechaHora", reto.FechaInicio),
                                                                                              Query.LTE("FechaHora", reto.FechaFin)
                                                                                             })).ToList();
            return l;
        }
        public      List<LogEjercicio> GetLogEjercicioByUserAndDates(string user, DateTime inicio, DateTime fin)
        {
            var l = GetCollection<LogEjercicio>(Settings.Default.CollectionLogEjercicio).Find(Query.And(new[]
                                                                                            { Query.EQ("Usuario",user),
                                                                                              Query.GTE("FechaHora", inicio),
                                                                                              Query.LTE("FechaHora", fin)
                                                                                             })).ToList();
            return l;
        }
        
        public      List<Equipo>       GetEquipos(ICollection<string> equipos)
        {
            return GetCollection<Equipo>(Settings.Default.CollectionEquipos).Find(Query.In("_id", new BsonArray(equipos.Select(x => new ObjectId(x)).ToList()))).ToList();
        }
        public      List<Persona>      GetMiembrosEquipo(ICollection<string> miembros)
        {
            return GetCollection<Persona>(Settings.Default.ColeccionPersona).Find(Query.In("Cuentas.k",new BsonArray(miembros))).ToList();
        }
        public      List<DetalleReto>  GetDetallesRetosByLiga(string id,string user)
        {
            List<DetalleReto> lst = new List<DetalleReto>();
            List<Reto> retos = GetRetosByIdLiga(id);
            //Puntos Individuales, Puntos Equipos, Puntos Lider o Meta

            foreach (var a in retos)
            {
                GetDetalleReto(user, a, lst);
            }
            return lst;
        }

        public void GetDetalleReto(string user, Reto a, List<DetalleReto> lst, bool isReporte = false)
        {
            Task<List<Equipo>> t = Task.Factory.StartNew(() => GetEquipos(a.Equipos));

            List<LogEjercicio> datos = GetDatosRetoEquipo(a);
            List<Equipo> equipos = t.Result;
            int totalPersonal = 0;
            Equipo miEquipo = new Equipo();
            List<DetalleEquipo> team_detail = new List<DetalleEquipo>();
            if (datos.Count > 0)
            {
                Parallel.ForEach(equipos, (equipo) =>
                {
                    DetalleEquipo detall = new DetalleEquipo();
                    foreach (var miembro in equipo.Miembros)
                    {
                        //mis detalles personales
                        if (user == miembro)
                        {
                            detall.Total += totalPersonal = datos.Where(x => x.Usuario == miembro).Sum(x => x.Conteo);
                            detall.MiEquipo = true;
                        }
                        else
                        {
                            detall.Total += datos.Where(x => x.Usuario == miembro).Sum(x => x.Conteo);
                        }
                    }
                    detall.Equipo = equipo.Id;
                    team_detail.Add(detall);
                });
                int pos = 1;
                foreach (var detalleEquipo in team_detail.OrderByDescending(x => x.Total))
                {
                    detalleEquipo.Posicion = pos++;
                }
                if (a.Tipo == TipoReto.Superando)
                {
                    if (!isReporte)
                    {
                        DetalleReto dr = new DetalleReto()
                        {
                            IdReto = a.Id,
                            Name = a.Name,
                            TotalEquipo = team_detail.Where(x => x.MiEquipo).Sum(x => x.Total),
                            PosicionEquipo = team_detail.Where(x => x.MiEquipo).Select(x => x.Posicion).First(),
                            TotalReto = team_detail.Where(x => x.Posicion == 1).Sum(x => x.Total),
                            TotalUsuario = totalPersonal,
                        };
                        lst.Add(dr);
                    }
                    else
                    {
                        lst.AddRange(team_detail.Select(x=>new DetalleReto()
                        {
                            IdReto = a.Id,Name=a.Name,TotalEquipo = x.Total,PosicionEquipo = x.Posicion, TotalReto = x.Total,NombreEquipo = equipos.Where(y=>y.Id==x.Equipo).Select(y=>y.Name).First()
                        }));
                    }
                    
                }
                else
                {

                    if (!isReporte)
                    {
                        DetalleReto dr = new DetalleReto()
                        {
                            IdReto = a.Id,
                            Name = a.Name,
                            TotalEquipo = team_detail.Where(x => x.MiEquipo).Sum(x => x.Total),
                            PosicionEquipo = team_detail.Where(x => x.MiEquipo).Select(x => x.Posicion).First(),
                            TotalReto = a.Meta,
                            TotalUsuario = totalPersonal,
                        };
                        lst.Add(dr);
                    }
                    else
                    {
                        lst.AddRange(team_detail.Select(x => new DetalleReto()
                        {
                            IdReto = a.Id,
                            Name = a.Name,
                            TotalEquipo = x.Total,
                            PosicionEquipo = x.Posicion,
                            TotalReto = a.Meta,
                            NombreEquipo = equipos.Where(y=>y.Id==x.Equipo).Select(y=>y.Name).First()
                        }));
                    }
                }
            }
        }

        public List<Equipo> GetEquiposByUser(string user)
        {
            if(!String.IsNullOrEmpty(user))
                return GetCollection<Equipo>(Settings.Default.CollectionEquipos).Find(Query.EQ("Miembros", user)).ToList();

            return new List<Equipo>();
        }

        public bool SaveLogLogrosDiarios(LogLogrosDiarios ld)
        {
            return Save(ld, Settings.Default.CollectionLogLogroDiario);
        }

        internal bool SaveChat(ChatElement c)
        {
            return Save(c, Settings.Default.CollectionChat);
        }

        internal List<ChatElement> GetLastMessages(string idLiga)
        {
            return GetCollection<ChatElement>(Settings.Default.CollectionChat)
                .Find(Query.EQ("Liga", idLiga))
                .OrderByDescending(x => x.Fecha)
                .Take(5)
                .ToList();
        }

        public List<Tips> GetRandomTips()
        {
            return GetCollection<Tips>(Settings.Default.CollectionTips).AsQueryable().Take(6).ToList();
        }
        public List<Tips> GetTipsDeporte()
        {
            return GetCollection<Tips>(Settings.Default.CollectionTips).AsQueryable().Where(x=>x.Tipo==TipoTips.Deporte).ToList();
        }

        public List<Tips> GetTipsSalud()
        {
            return GetCollection<Tips>(Settings.Default.CollectionTips).AsQueryable().Where(x => x.Tipo == TipoTips.Salud).ToList();
        }

        public List<Tips> GetTipsAlimentacion()
        {
            return GetCollection<Tips>(Settings.Default.CollectionTips).AsQueryable().Where(x => x.Tipo == TipoTips.Alimentacion).ToList();
        }


        public bool SendMail(string destino,string asunto, string htmlBody)
        {
            //ZeitGeist.Tools.MailClass mc = new MailClass(Settings.Default.mail, Settings.Default.pass, "smtp.gmail.com", 587);
            ZeitGeist.Tools.MailClass mc = new MailClass("livingdeathD@gmail.com", "p3p1t0c0", "smtp.gmail.com", 587);
            return mc.Send(destino, asunto, htmlBody);
        }

        public void GenerarReporteFinalReto(Reto reto)
        {
            //Task.Factory.StartNew(() =>
            //{
                List<DetalleReto> lst= new List<DetalleReto>();
                GetDetalleReto("",reto,lst,true);
                Console.WriteLine("dsdsd");
                SendMail("ddo88@hotmail.com", "prueba", lst.GetHtmlBody());
            //});

        }
    }


    public static class ExtensionMethods
    {
        public static string GetHtmlBody(this List<DetalleReto> lst)
        {
            StringBuilder sb= new StringBuilder();
            sb.Append("<table>");
            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("Equipo");
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append("Posicion Equipo");
            sb.Append("</td>");
            sb.Append("<td>");
            sb.Append("Total Equipo");
            sb.Append("</td>");
            sb.Append("</tr>");
            foreach (var a in lst.OrderBy(x=>x.PosicionEquipo))
            {
                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append(a.NombreEquipo);
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(a.PosicionEquipo);
                sb.Append("</td>");
                sb.Append("<td>");
                sb.Append(a.TotalEquipo);
                sb.Append("</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }
    }


}