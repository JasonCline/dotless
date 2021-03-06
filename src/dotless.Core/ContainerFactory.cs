/* Copyright 2009 dotless project, http://www.dotlesscss.com
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *     
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License. */

namespace dotless.Core
{
    using Abstractions;
    using configuration;
    using engine;
    using engine.Pipeline;
    using Microsoft.Practices.ServiceLocation;
    using Pandora;

    public class ContainerFactory
    {
        private static PandoraContainer _container;
        private static readonly object LockObject = new object();

        public IServiceLocator GetContainer()
        {
            if (_container == null)
            {
                lock (LockObject)
                {
                    if (_container == null)
                    {
                        DotlessConfiguration configuration = new WebConfigConfigurationLoader().GetConfiguration();
                        _container = CreateContainer(configuration);
                    }
                }
            }

            return new CommonServiceLocatorAdapter(_container);
        }

        public IServiceLocator GetCoreContainer(DotlessConfiguration configuration)
        {
            return new CommonServiceLocatorAdapter(RegisterCoreServices(new PandoraContainer(), configuration));
        }

        
        private PandoraContainer RegisterCoreServices(PandoraContainer container, DotlessConfiguration configuration)
        {
            container.Register(p =>
                                   {
                                       if (configuration.MinifyOutput)
                                       {
                                           p.Service<ILessEngine>()
                                               .Implementor<MinifierDecorator>();
                                       }
                                       p.Service<ILessEngine>()
                                           .Implementor<ExtensibleEngine>();
                                       p.Service<ILessSource>()
                                           .Implementor<FileSource>();
                                   });
            return container;
        }

        /// <summary>
        /// Used to create the Container for the HttpHandler. Not used by Console Compiler and T4CSS
        /// </summary>
        /// <param name="configuration">Configuration for the HttpHandler</param>
        /// <returns></returns>
        private PandoraContainer CreateContainer(DotlessConfiguration configuration)
        {
            var container = new PandoraContainer();
            container.Register(p =>
                                   {
                                       p.Service<ILessSource>()
                                           .Implementor(configuration.LessSource);
                                       p.Service<ICache>()
                                           .Implementor<CssCache>();
                                       p.Service<IRequest>()
                                           .Implementor<Request>();

                                       if (!configuration.CacheEnabled)
                                       {
                                           p.Service<IResponse>()
                                               .Implementor<CssResponse>();
                                       }
                                       else
                                       {
                                           p.Service<IResponse>()
                                               .Implementor<CachedCssResponse>();
                                           p.Service<ILessEngine>()
                                               .Implementor<AspCacheDecorator>();
                                       }
                                   });

            return RegisterCoreServices(container, configuration);
        }
    }
}