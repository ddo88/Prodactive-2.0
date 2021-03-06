﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeitgeist.Core.Data.Interfaces;
using Zeitgeist.Web.Domain;
using Zeitgeist.Web.Tools;

namespace Zeitgeist.Web.Services
{
    public class RetoService
    {
        private readonly IRepository<Reto> _retoRepository;
        private readonly WorkContext _workContextService;

        public RetoService(IRepository<Reto> retoRepository,WorkContext workContextService)
        {
            _retoRepository = retoRepository;
            _workContextService = workContextService;
        }

        public List<Reto> GetChallengesWithUserRelated(int userId)
        {
            try
            {
                var r =
                    _retoRepository.Table.Where(
                        x =>
                            x.IsActivo || x.CoachId == userId || x.OwnerId == userId ||
                            x.RetoEquipoMapping.Any(y => y.Equipo.MiembrosEquipoMapping.Any(z => z.UserId == userId)))
                        .ToList();
                return r;
            }
            catch (Exception ex)
            {
                //rev
            }
            return new List<Reto>();
        }

        public int GetStepsForChallenge(int idChallenge)
        {
            var challenge=_retoRepository.GetById(idChallenge);
            return _workContextService
                .GetAuthenticatedUser()
                .User
                .LogEjercicios.Where(x => challenge.FechaInicio <= x.Fecha &&
                                          challenge.FechaFin >= x.Fecha)
                .Sum(x => x.Conteo);
        }

        public List<Tuple<string,int>> GetStepsForLastXDaysChallenge(int idChallenge,int days)
        {

            List<Tuple<string,int>> list= new List<Tuple<string, int>>();
            DateTime now = DateTime.Now;
            for (int i = 0; i <= days; i++)
            {
                var seq=now.AddDays(-days + i);
                DateTime inicio  = new DateTime(seq.Year,seq.Month,seq.Day,0,0,0);
                DateTime fin     = new DateTime(seq.Year, seq.Month, seq.Day, 23,59,59);

                var result=_workContextService
                                .GetAuthenticatedUser().User
                                .LogEjercicios.Where(x => inicio <= x.Fecha &&
                                                          fin    >= x.Fecha)
                                .Sum(x => x.Conteo);
                list.Add(new Tuple<string, int>(seq.ToString("yyyy-MM-dd"),result)); 
            }
            return list;
        }

        public List<Reto> GetChallengesWithIdLeague(int idLeague)
        {
            try
            {
                var r = _retoRepository.Table.Where(x=>x.LigaId==idLeague).ToList();
                return r;
            }
            catch (Exception ex)
            {
                //Rev
            }
            return new List<Reto>();
        }
    }
}
