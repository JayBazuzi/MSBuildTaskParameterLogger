using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuildTaskParameterLogger.Extensions
{
    public static class LoggingTaskExtensions
    {
        public static void LogInputs(this Task task, MessageImportance importance)
        {
            task.Log.LogMessage(importance, "Inputs:");

            foreach (var propertyInfo in GetInputProperties(task))
            {
                LogProperty(propertyInfo, importance, task);
            }
        }

        private static IEnumerable<PropertyInfo> GetInputProperties(Task task)
        {
            var propertyInfos = from propertyInfo in task.GetType().GetProperties()
                                where propertyInfo.HasAttribute<RequiredAttribute>()
                                select propertyInfo;

            return propertyInfos;
        }

        private static void LogProperty(PropertyInfo propertyInfo, MessageImportance importance, Task task)
        {
            string name = propertyInfo.Name;
            var value = propertyInfo.GetValue(task);
            if (value is ITaskItem[])
            {
                LogTaskItems(value as ITaskItem[], name, importance, task);
            }
            else
            {
                task.Log.LogMessage(importance, "  {0} = {1}", name, value);
            }
        }

        private static void LogTaskItems(ITaskItem[] taskItems, string itemGroup, MessageImportance importance, Task task)
        {
            task.Log.LogMessage(importance, "  {0} =", itemGroup);

            foreach (var item in taskItems)
            {
                LogTaskItem(task, item, importance);
            }
        }

        private static void LogTaskItem(Task task, ITaskItem item, MessageImportance importance)
        {
            task.Log.LogMessage(importance, "    {0}", item.ItemSpec);
            LogItemMetadata(task, item, importance);
        }

        private static void LogItemMetadata(Task task, ITaskItem item, MessageImportance importance)
        {
            var metadata = item.CloneCustomMetadata();
            foreach (var name in metadata.Keys.Cast<string>())
            {
                LogMetadata(task, name, (string)metadata[name], importance);
            }
        }

        private static void LogMetadata(Task task, string metadataName, string value, MessageImportance importance)
        {
            task.Log.LogMessage(importance, "      {0}={1}", metadataName, value);
        }
    }
}