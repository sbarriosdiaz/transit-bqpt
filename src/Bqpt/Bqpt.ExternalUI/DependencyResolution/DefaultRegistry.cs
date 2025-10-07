////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced"> Copyright 2012 Web Advanced
// (www.webadvanced.com) Licensed under the Apache License, Version 2.0 (the "License"); you may not
// use this file except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software distributed under the License
// is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
// or implied. See the License for the specific language governing permissions and limitations under
// the License. </copyright> --------------------------------------------------------------------------------------------------------------------

namespace Bqpt.ExternalUI.DependencyResolution
{
    using BC.Identity.Kernel;
    using Bqpt.Application;
    using Bqpt.Infrastructure;
    using Bqpt.Persistence;
    using FluentValidation;
    using MediatR;
    using StructureMap;
    using StructureMap.Pipeline;

    public class DefaultRegistry : Registry
    {
        #region Constructors and Destructors

        public DefaultRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<ExternalDashBoardQuery>();
                    scan.WithDefaultConventions();
                    scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                    scan.AddAllTypesOf(typeof(IValidator<>));
                    scan.With(new ControllerConvention());
                });

            // MediatR
            For<ServiceFactory>().Use<ServiceFactory>(ctx => ctx.GetInstance);
            For<IMediator>().Use<Mediator>();

            // Services
            For<IIdentityApiClient>().Use<IdentityApiClient>();
            For<IKernelSecurity>().Use<KernelSecurity>();
            For<IFilesConnectedServices>().Use<FilesConnectedServices>();

            For<ICurrentUserService>().Use<CurrentUserService>();

            // DbContext IoC
            For<IDbService>().Use(ctx => DbContextFactory()).LifecycleIs<TransientLifecycle>();
        }

        public AppDbContext DbContextFactory()
        {
            var context = new AppDbContext();

            // standard configuration for our DB Context
            context.Configuration.ProxyCreationEnabled = false;
            context.Configuration.LazyLoadingEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;

            return context;
        }

        #endregion Constructors and Destructors
    }
}