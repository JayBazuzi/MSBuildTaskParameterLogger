﻿using System;
using Microsoft.Build.Construction;

namespace MSBuildTaskParameterLogger.Extensions
{
    public static class ProjectRootElementExtensions
    {
        public static void AddUsingTask<TTask>(this ProjectRootElement projectRootElement)
        {
            Type taskType = typeof (TTask);
            projectRootElement
                .AddUsingTask(taskType.Name, taskType.Assembly.Location, null);
        }
    }
}