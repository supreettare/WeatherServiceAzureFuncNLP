﻿using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherServiceAzureFuncNLP.Services.Interface
{
    public interface IKernelBase
    {
        Kernel CreateKernel();
    }
}
