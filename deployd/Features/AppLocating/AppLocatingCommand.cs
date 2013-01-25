﻿using System;
using System.Collections.Generic;
using System.Linq;
using deployd.Features.ClientConfiguration;
using deployd.Features.FeatureSelection;
using log4net;

namespace deployd.Features.AppLocating
{
    public class AppLocatingCommand : IFeatureCommand
    {
        private readonly ILog _log;
        private readonly List<IAppInstallationLocator> _finders;
        
        public Configuration Configuration { get; set; }
        public InstanceConfiguration InstanceConfiguration { get; set; }

        public AppLocatingCommand(IEnumerable<IAppInstallationLocator> finders, ILog log)
        {
            _log = log;
            _finders = finders.ToList();
        }

        public void Execute()
        {
            var location =
                _finders.Select(locator => locator.CanFindPackageAsObject(InstanceConfiguration.AppName))
                        .FirstOrDefault(result => result != null);

            if (location != null)
            {
                InstanceConfiguration.PackageLocation = location;
                return;
            }
            
            _log.Info("No package matching " + InstanceConfiguration.AppName + " found.");
            Environment.Exit(0);
        }
    }
}
