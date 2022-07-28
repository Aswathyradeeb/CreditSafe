using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework.Logging
{
    /// <summary>
    /// Defines the interface of the logging API.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Log a message object with the <see cref="System.Diagnostics.Debug"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Debug(object message);

        /// <summary>
        /// Log a message object with the <see cref="System.Diagnostics.Debug"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Debug(object message, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void DebugFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        void DebugFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);


        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        void Debug(Func<string> formattingCallback);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        /// <param name="exception">The exception to log.</param>
        void Debug(Func<string> formattingCallback, Exception exception);


        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Info(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Info(object message, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void InfoFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        void InfoFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Info"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        void Info(Func<string> formattingCallback);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        /// <param name="exception">The exception to log.</param>
        void Info(Func<string> formattingCallback, Exception exception);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Warn(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Warn(object message, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void WarnFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        void WarnFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Warn"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        void Warn(Func<string> formattingCallback);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        /// <param name="exception">The exception to log.</param>
        void Warn(Func<string> formattingCallback, Exception exception);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Error(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Error(object message, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void ErrorFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Error"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        void Error(Func<string> formattingCallback);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        /// <param name="exception">The exception to log.</param>
        void Error(Func<string> formattingCallback, Exception exception);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Fatal(object message);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        void Fatal(object message, Exception exception);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        void FatalFormat(string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        void FatalFormat(IFormatProvider formatProvider, string format, params object[] args);

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Fatal"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        void Fatal(Func<string> formattingCallback);

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        /// <param name="exception">The exception to log.</param>
        void Fatal(Func<string> formattingCallback, Exception exception);
    }
}
