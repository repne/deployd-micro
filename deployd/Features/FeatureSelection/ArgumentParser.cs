﻿using System.Collections.Generic;
using System.Linq;
using NDesk.Options;

namespace deployd.Features.FeatureSelection
{
    public class ArgumentParser : IArgumentParser
    {
        public InstanceConfiguration Parse(IList<string> args)
        {
            var cfg = new InstanceConfiguration
                {
                    AppName = string.Empty,
                    Help = false,
                    Verbose = false,
                    ExtraParams = new List<string>()
                };

            var p = new OptionSet
                {
                    {"app=", v => cfg.AppName = v},
                    {"i|install", v => cfg.Install = v != null},
                    {"v|verbose", v => cfg.Verbose = v != null},
                    {"h|?|help", v => cfg.Help = v != null},
                };
            cfg.OptionSet = p;

            if (args == null || !args.Any())
            {
                cfg.Help = true;
                return cfg;
            }

            cfg.ExtraParams = p.Parse(args);
            cfg.AppName = cfg.AppName.Trim('"', '\'');
            return cfg;
        }
    }
}
