﻿using System.IO.Abstractions;
using System.Linq;
using deployd.Features.FeatureSelection;

namespace deployd.Features.PurgeOldBackups
{
    public class PurgeOldBackupsCommand : IFeatureCommand
    {
        private readonly IFileSystem _fs;
        private readonly InstanceConfiguration _config;
        private const int TotalBackupsToKeep = 10;

        public PurgeOldBackupsCommand(IFileSystem fs, InstanceConfiguration config)
        {
            _fs = fs;
            _config = config;
        }

        public void Execute()
        {
            var backups = _fs.Directory.GetDirectories(_config.ApplicationMap.FullPath);
            if (backups.Length <= 10)
            {
                return;
            }

            var oldestFirst = backups.Reverse().ToArray();
            var itemsToRemove = oldestFirst.Skip(TotalBackupsToKeep + 1).ToList();

            foreach (var item in itemsToRemove)
            {
                _fs.Directory.Delete(item, true);
            }
        }
    }
}
