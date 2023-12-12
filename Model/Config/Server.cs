﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProgrammaticallyBuilding.Model.Config
{
    public class Server : Device
    {
        public List<Drive> Drives { get; set; } = new List<Drive>();
        public List<MemoryModule> MemoryModules { get; set; } = new List<MemoryModule>();
        public List<CPU> CPUs { get; set; } = new List<CPU>();

    }
}
