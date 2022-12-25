using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using System.Text;
using JWT.DependencyInjection;

namespace JWT.Bootstrapper
{
    public class ModuleLoader : IDisposable
    {
        private readonly string _path;
        private readonly string _pattern;
        private bool disposed;
        private AggregateCatalog aggregateCatalog;
        private CompositionContainer compositionContainer;

        public ModuleLoader(string path, string pattern)
        {
            _path = path;
            _pattern = pattern;
        }

        private List<Export> exports;

        public IEnumerable<Export> GetExports()
        {
            if (exports != null)
                return exports;

            var directoryCatalog = new DirectoryCatalog(_path, _pattern);
            var importDef = BuildImportDefinition;

            aggregateCatalog = new AggregateCatalog();
            aggregateCatalog.Catalogs.Add(directoryCatalog);

            compositionContainer = new CompositionContainer(aggregateCatalog);
            exports = compositionContainer.GetExports(importDef)
                .ToList();

            return exports;
        }

        /// <summary>
        /// Registers modules based on type.
        /// </summary>
        /// <exception cref="TypeLoadException"></exception>
        public void Load<TRegistrar>(TRegistrar registrar)
            where TRegistrar : class
        {
            var modules = GetExports()
                .Select(export => export.Value as IModule<TRegistrar>)
                .Where(x => x != null);

            try
            {
                foreach (var module in modules)
                {
                    module.Initialize(registrar);
                }
            }
            catch (ReflectionTypeLoadException typeLoadException)
            {
                var builder = new StringBuilder();
                foreach (var loaderException in typeLoadException.LoaderExceptions)
                {
                    builder.AppendFormat("{0}.\n", loaderException.Message);
                }

                throw new TypeLoadException(builder.ToString(), typeLoadException);
            }
        }

        private ImportDefinition BuildImportDefinition => new ImportDefinition(
                def => true, typeof(IExport).FullName, ImportCardinality.ZeroOrMore, false, false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing || !disposed)
            {
                aggregateCatalog.Dispose();
                compositionContainer.Dispose();
            }

            disposed = true;
        }

        ~ModuleLoader()
        {
            Dispose(false);
        }
    }
}