using System.Linq;
using Microsoft.Build.Construction;

namespace MSBuildTaskParameterLogger.Extensions
{
    static class ProjectTargetElementExtensions
    {
        public static ProjectTaskElement AddTask<TTask>(this ProjectTargetElement projectTargetElement)
        {
            string taskName = typeof(TTask).Name;
            ProjectRootElement projectRootElement = projectTargetElement.ContainingProject;

            if (!projectRootElement.UsingTasks.Any(ut => ut.TaskName == taskName))
            {
                projectRootElement.AddUsingTask<TTask>();
            }

            return projectTargetElement.AddTask(taskName);
        }
    }
}