﻿using System.Diagnostics;
using Acolyte.Assertions;

namespace AlgorithmAnalysis.Common
{
    public static class TraceHelper
    {
        public static void SetTraceListener(string traceLogFilename, string listenerName,
            bool autoFlush)
        {
            traceLogFilename.ThrowIfNullOrWhiteSpace(nameof(traceLogFilename));
            listenerName.ThrowIfNullOrEmpty(nameof(listenerName));

            Trace.Listeners.Add(new TextWriterTraceListener(traceLogFilename, listenerName));
            Trace.AutoFlush = autoFlush;
        }

        public static void SetTraceListener(string traceLogFilename, string listenerName)
        {
            SetTraceListener(
                traceLogFilename: traceLogFilename,
                listenerName: listenerName,
                autoFlush: true
            );
        }

        public static void SetTraceListener(string traceLogFilePath)
        {
            SetTraceListener(
                traceLogFilename: traceLogFilePath,
                listenerName: CommonConstants.TextLogTraceListenerName, 
                autoFlush: true
            );
        }
    }
}
