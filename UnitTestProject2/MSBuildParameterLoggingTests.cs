using Microsoft.Build.Construction;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MSBuildTaskParameterLogger;
using MSBuildTaskParameterLogger.Extensions;

namespace MSBuildTaskParameterLogger
{
    [TestClass]
    public class MSBuildParameterLoggingTests
    {
        [TestMethod]
        public void RequiredInputGetsLogged()
        {
            var buildTarget = CreateBuildTarget();

            buildTarget.AddTask<TaskWithRequiredInput>().SetParameter("Foo", "hi");

            const string expected = @"Inputs:
  Foo = hi
";
            AssertTargetOutput(expected, buildTarget);
        }

        public class TaskWithRequiredInput : Task
        {
            [Required]
            public string Foo { get; set; }

            public override bool Execute()
            {
                this.LogInputs(MessageImportance.High);
                return true;
            }
        }

        [TestMethod]
        public void TaskItemsGetLogged()
        {
            var buildTarget = CreateBuildTarget();
            var itemGroup = buildTarget.AddItemGroup();
            itemGroup.AddItem("Foo", "a");
            itemGroup.AddItem("Foo", "b");
            buildTarget.AddTask<TaskWithItems>().SetParameter("Foo", "@(Foo)");

            const string expected = @"Inputs:
  Foo =
    a
    b
";
            AssertTargetOutput(expected, buildTarget);
        }

        [TestMethod]
        public void TaskItemMetaDataGetsLogged()
        {
            var buildTarget = CreateBuildTarget();
            var itemGroup = buildTarget.AddItemGroup();
            itemGroup.AddItem("Foo", "a")
                .AddMetadata("b", "c");
            buildTarget.AddTask<TaskWithItems>().SetParameter("Foo", "@(Foo)");

            const string expected = @"Inputs:
  Foo =
    a
      b=c
";
            AssertTargetOutput(expected, buildTarget);
        }
        public class TaskWithItems : Task
        {
            [Required]
            public ITaskItem[] Foo { get; set; }

            public override bool Execute()
            {
                this.LogInputs(MessageImportance.High);
                return true;
            }
        }

        private ProjectTargetElement CreateBuildTarget()
        {
            var buildTarget = ProjectRootElement.Create().AddTarget("Build");
            return buildTarget;
        }

        private static void AssertTargetOutput(string expected, ProjectTargetElement projectTargetElement)
        {
            var stringLogger = new StringLogger();

            var success = BuildTarget(projectTargetElement, stringLogger);
            Assert.IsTrue(success, stringLogger.Error);

            Assert.AreEqual(expected, stringLogger.HighImportance);
        }

        private static bool BuildTarget(ProjectTargetElement projectTargetElement, StringLogger stringLogger)
        {
            var success = new ProjectInstance(projectTargetElement.ContainingProject).Build(new[] { stringLogger });
            return success;
        }
    }
}
