using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuildTaskParameterLogger
{
    class StringLogger : ILogger
    {
        private readonly StringBuilder _highImportanceStringBuilder = new StringBuilder();
        private readonly StringBuilder _normalImportanceStringBuilder = new StringBuilder();
        private readonly StringBuilder _lowImportanceStringBuilder = new StringBuilder();
        private readonly StringBuilder _errorStringBuilder = new StringBuilder();

        public void Initialize(IEventSource eventSource)
        {
            eventSource.MessageRaised += eventSource_MessageRaised;
            eventSource.ErrorRaised += eventSource_ErrorRaised;
        }

        void eventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            _errorStringBuilder.AppendLine(e.Message);
        }

        void eventSource_MessageRaised(object sender, BuildMessageEventArgs e)
        {
            var sb = SelectStringBuilder(e.Importance);
            sb.AppendLine(e.Message);
        }

        private StringBuilder SelectStringBuilder(MessageImportance messageImportance)
        {
            switch (messageImportance)
            {
                case MessageImportance.High: return this._highImportanceStringBuilder;
                case MessageImportance.Normal: return this._normalImportanceStringBuilder;
                case MessageImportance.Low: return this._lowImportanceStringBuilder;
                default: throw new ArgumentOutOfRangeException("messageImportance");
            }
        }

        public string Parameters { get; set; }

        public void Shutdown() { }

        public LoggerVerbosity Verbosity { get; set; }

        public override string ToString()
        {
            return this._highImportanceStringBuilder.ToString();
        }

        public string HighImportance
        {
            get { return _highImportanceStringBuilder.ToString(); }
        }

        public string Error
        {
            get { return _errorStringBuilder.ToString(); }
        }
    }
}
