[assembly: WebActivator.PostApplicationStartMethod(typeof(NotesAPI.App_Start.SimpleInjectorWebApiInitializer), "Initialize")]

namespace NotesAPI.App_Start
{
    using System.Web.Http;
    using NotesAPI.Services.AuthService;
    using NotesAPI.Services.NoteService;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using SimpleInjector.Lifestyles;
    
    public static class SimpleInjectorWebApiInitializer
    {
        /// <summary>Initialize the container and register it as Web API Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            
            InitializeContainer(container);

            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
       
            container.Verify();
            
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
     
        private static void InitializeContainer(Container container)
        {
            //Register your services here (remove this line).

            // For instance:
            // container.Register<IUserRepository, SqlUserRepository>(Lifestyle.Scoped);
            container.Register<IAuthService,AuthService > (Lifestyle.Scoped);
            container.Register<INoteService,NoteService > (Lifestyle.Scoped);
        }
    }
}