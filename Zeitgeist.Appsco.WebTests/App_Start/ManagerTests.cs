using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoModels;
using Zeitgeist.Appsco.Web.App_Start;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Zeitgeist.Appsco.Web.App_Start.Tests
{
    [TestClass()]
    public class ManagerTests
    {

        [TestMethod()]
        public void ManagerTest()
        {

        }

        [TestMethod()]
        public void GetCollectionTest()
        {

        }

        [TestMethod()]
        public void insertTest()
        {

        }

        [TestMethod()]
        public void RemoveCacheTest()
        {

        }

        [TestMethod()]
        public void SaveTest()
        {

        }

        [TestMethod()]
        public void SaveClientTest()
        {

        }

        [TestMethod()]
        public void SavePersonaTest()
        {

        }

        [TestMethod()]
        public void SaveLigaTest()
        {

        }

        [TestMethod()]
        public void SaveEquipoTest()
        {

        }

        [TestMethod()]
        public void SaveDivisionTest()
        {

        }

        [TestMethod()]
        public void SaveRegistroProgresoTest()
        {

        }

        [TestMethod()]
        public void SaveInvitacionTest()
        {

        }

        [TestMethod()]
        public void SaveRetoTest()
        {

        }

        [TestMethod()]
        public void UpdateRetoTest()
        {

        }

        [TestMethod()]
        public void DeleteLigaTest()
        {

        }

        [TestMethod()]
        public void AddUserToleagueTest()
        {

        }

        [TestMethod()]
        public void AddUserToleagueTest1()
        {

        }

        [TestMethod()]
        public void CorreoDisponibleTest()
        {

        }

        [TestMethod()]
        public void GetDatosUsuarioTest()
        {

        }

        [TestMethod()]
        public void GetDatosUsuarioByMailTest()
        {

        }

        [TestMethod()]
        public void GetRetoTest()
        {

        }

        [TestMethod()]
        public void GetLigaByIdTest()
        {

        }

        [TestMethod()]
        public void GetDivisionByIdTest()
        {

        }

        [TestMethod()]
        public void GetRetoByIdTest()
        {

        }

        [TestMethod()]
        public void GetRetoTest1()
        {

        }

        [TestMethod()]
        public void GetLigasTest()
        {

        }

        [TestMethod()]
        public void GetDivisionesTest()
        {

        }

        [TestMethod()]
        public void GetRetosTest()
        {

        }

        [TestMethod()]
        public void GetRetosByLigaTest()
        {

        }

        [TestMethod()]
        public void GetRetosActivosByLigaTest()
        {

        }

        [TestMethod()]
        public void GetUltimosRetoTest()
        {

        }

        [TestMethod()]
        public void GetRetosActivosTest()
        {

        }

        [TestMethod()]
        public void GetLeagueUserRegisteredTest()
        {

        }

        [TestMethod()]
        public void GetRetosByIdLigaTest()
        {

        }

        [TestMethod()]
        public void GetDatosRetoEquipoTest()
        {

        }

        [TestMethod()]
        public void GetDatosRetoEquipoTest1()
        {

        }

        [TestMethod()]
        public void GetDatosRetoUsuarioTest()
        {

        }

        [TestMethod()]
        public void GetDatosRetoEquipo2Test()
        {

        }

        [TestMethod()]
        public void GetDatosRetoUsuario2Test()
        {

        }

        [TestMethod()]
        public void GetDatosRetoEquipoByDayTest()
        {

        }

        [TestMethod()]
        public void GetLogEjercicioByIdRetoTest()
        {

        }

        [TestMethod()]
        public void GetLogEjercicioByUserAndDatesTest()
        {

        }

        [TestMethod()]
        public void GetEquiposTest()
        {

        }

        [TestMethod()]
        public void GetMiembrosEquipoTest()
        {

        }

        [TestMethod()]
        public void GetDetallesRetosByLigaTest()
        {

        }

        [TestMethod()]
        public void GetDetalleRetoTest()
        {
            var m = Manager.Instance;
            List<DetalleReto> lst= new List<DetalleReto>();
            m.GetDetalleReto("",new Reto(),lst);
            Assert.AreEqual(lst.Count, 0);
            Reto r=m.GetRetoById("53f5fa98f623e86b6884b6f1");
            m.GetDetalleReto("", r, lst,true);
            Assert.IsTrue(lst.Count > 0);
            try
            {
                m.GetDetalleReto("", r, lst);
            }
            catch (Exception)
            {
                Assert.IsTrue(true);
            }
            
            
            
        }

        [TestMethod()]
        public void GetEquiposByUserTest()
        {

        }

        [TestMethod()]
        public void SaveLogLogrosDiariosTest()
        {

        }

        [TestMethod()]
        public void GetRandomTipsTest()
        {

        }

        [TestMethod()]
        public void GetTipsDeporteTest()
        {

        }

        [TestMethod()]
        public void GetTipsSaludTest()
        {

        }

        [TestMethod()]
        public void GetTipsAlimentacionTest()
        {

        }

        [TestMethod()]
        public void SendMailTest()
        {

        }

        [TestMethod()]
        public void GenerarReporteFinalRetoTest()
        {
            Manager m = Manager.Instance;
            Reto r = m.GetRetoById("53f5fa98f623e86b6884b6f1");

            m.GenerarReporteFinalReto(r);

        }
    }
}
