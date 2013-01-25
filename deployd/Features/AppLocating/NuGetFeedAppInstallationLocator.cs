﻿using System;
using System.Linq;
using NuGet;
using deployd.Features.ClientConfiguration;
using log4net;
using deployd.Infrastructure;

namespace deployd.Features.AppLocating
{
    public class NuGetFeedAppInstallationLocator : IAppInstallationLocator
    {
        private readonly ILog _log;
        private readonly IPackageRepository _packageRepository;

        public NuGetFeedAppInstallationLocator(ILog log, Configuration clientConfig, IPackageRepositoryFactory packageRepositoryFactory)
        {
            _log = log;
            var repoLocation = clientConfig.PackageSource.ToAbsolutePath();
            _packageRepository = packageRepositoryFactory.CreateRepository(repoLocation);
        }

        public PackageLocation CanFindPackage(string appName)
        {
            try
            {
                var all = _packageRepository.GetPackages()
                                            .Where(x => x.Id == appName && x.IsLatestVersion)
                                            .Reverse()
                                            .ToList();

                var latestPackage = all.FirstOrDefault();

                if (latestPackage != null)
                {
                    return new PackageLocation {NuGetPackage = latestPackage};
                }
            }
            catch (Exception ex)
            {
                _log.Error("Could not get packages", ex);
            }

            return null;
        }
    }
}