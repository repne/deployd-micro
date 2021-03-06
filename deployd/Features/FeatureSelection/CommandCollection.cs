﻿using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using deployd.Features.AppLocating;
using log4net;

namespace deployd.Features.FeatureSelection
{
    public class CommandCollection : List<IFeatureCommand>
    {
        private readonly ILog _log;
        private readonly InstanceConfiguration _instanceConfiguration;
        private readonly IFileSystem _fs;

        public CommandCollection(ILog log, InstanceConfiguration instanceConfiguration, IFileSystem fs)
        {
            _log = log;
            _instanceConfiguration = instanceConfiguration;
            _fs = fs;
        }

        public void RunAll()
        {
            try
            {
                foreach (var command in this)
                {
                    command.Execute();
                }
            }
            catch (NoPackageFoundException ex)
            {
                _log.Info(ex.Message);
                Environment.Exit(-2);
            }
            catch (Exception ex)
            {
                _log.Fatal(ex.ToString());
                Environment.Exit(-1);
            }
            finally
            {
                UnlockAppInstallation();
            }
        }

        private void UnlockAppInstallation()
        {
            if (_instanceConfiguration.ApplicationMap != null
                && _instanceConfiguration.ApplicationMap.Lock != null)
            {
                _instanceConfiguration.ApplicationMap.Lock.Close();
                _instanceConfiguration.ApplicationMap.Lock.Dispose();
                _fs.File.Delete(_instanceConfiguration.ApplicationMap.Lockfile);
            }
        }
    }
}