using System.Web.Optimization;
using ChameleonForms;
using ChameleonForms.Templates.TwitterBootstrap3;
using Zeitgeist.Web.App_Start;

[assembly: WebActivator.PreApplicationStartMethod(typeof(ConfigureChameleonTwitterBootstrap), "Start")]

namespace Zeitgeist.Web.App_Start
{
    public static class ConfigureChameleonTwitterBootstrap
    {
        public static void Start()
        {
            FormTemplate.Default = new TwitterBootstrapFormTemplate();
            
        }
    }
}
