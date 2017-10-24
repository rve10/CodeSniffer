﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSniffer.Interfaces
{
    public interface IMetric
    {
        string Name { get; set; }
        double Value { get; set; }
        double Calculate();
    }
}
