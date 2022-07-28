using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsApp.Framework.Logging
{
    /// <summary>
    /// Represents the Logging API implementation. This class is a wrapper around Log4net logging library.
    /// </summary>
    public class Logger : ILogger
    {
        #region field

        private readonly log4net.ILog _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a <code>Log</code> object
        /// </summary>
        /// <param name="logger"><code>log4net.ILog</code> to wrap.</param>
        internal Logger(log4net.ILog logger)
        {
            //Contract.Requires<ArgumentNullException>(logger != null); 

            _logger = logger;
        }

        #endregion
        

        #region ILog Members

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        public void Debug(object message)
        {
            _logger.Debug(message);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        public void Debug(object message, Exception exception)
        {
            _logger.Debug(message, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args">the list of format arguments</param>
        public void DebugFormat(string format, params object[] args)
        {
            if (_logger.IsDebugEnabled)
                _logger.DebugFormat(format, args);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args">the list of format arguments</param>
        public void DebugFormat(string format, Exception exception, params object[] args)
        {
            if (_logger.IsDebugEnabled)
                _logger.DebugFormat(format, args, exception);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="args"></param>
        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (_logger.IsDebugEnabled)
                _logger.DebugFormat(formatProvider, format, args);
        }

        /// <summary>
        /// Log a message with the <see cref="LogLevel.Debug"/> level.
        /// </summary>
        /// <param name="formatProvider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="format">The format of the message object to log.<see cref="string.Format(string,object[])"/> </param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="args"></param>
        public void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (_logger.IsDebugEnabled)
                _logger.DebugFormat(formatProvider, format, args, exception);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        public void Debug(Func<string> formattingCallback)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(formattingCallback());
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Debug"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        /// <param name="exception">The exception to log.</param>
        public void Debug(Func<string> formattingCallback, Exception exception)
        {
            if (_logger.IsDebugEnabled)
                _logger.Debug(formattingCallback(), exception);
        }

        public void Info(object message)
        {
            _logger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            _logger.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (_logger.IsInfoEnabled)
                _logger.InfoFormat(format, args);
        }

        public void InfoFormat(string format, Exception exception, params object[] args)
        {
            if (_logger.IsInfoEnabled)
                _logger.InfoFormat(format, args, exception);
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (_logger.IsInfoEnabled)
                _logger.InfoFormat(formatProvider, format, args);
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (_logger.IsInfoEnabled)
                _logger.InfoFormat(formatProvider, format, args, exception);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        public void Info(Func<string> formattingCallback)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(formattingCallback());
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Info"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        /// <param name="exception">The exception to log.</param>
        public void Info(Func<string> formattingCallback, Exception exception)
        {
            if (_logger.IsInfoEnabled)
                _logger.Info(formattingCallback(), exception);
        }

        public void Warn(object message)
        {
            _logger.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            _logger.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (_logger.IsWarnEnabled)
                _logger.WarnFormat(format, args);
        }

        public void WarnFormat(string format, Exception exception, params object[] args)
        {
            if (_logger.IsWarnEnabled)
                _logger.WarnFormat(format, args, exception);
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (_logger.IsWarnEnabled)
                _logger.WarnFormat(formatProvider, format, args);
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (_logger.IsWarnEnabled)
                _logger.WarnFormat(formatProvider, format, args, exception);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        public void Warn(Func<string> formattingCallback)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(formattingCallback());
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Warn"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        /// <param name="exception">The exception to log.</param>
        public void Warn(Func<string> formattingCallback, Exception exception)
        {
            if (_logger.IsWarnEnabled)
                _logger.Warn(formattingCallback(), exception);
        }

        public void Error(object message)
        {
            _logger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (_logger.IsErrorEnabled)
                _logger.ErrorFormat(format, args);
        }

        public void ErrorFormat(string format, Exception exception, params object[] args)
        {
            if (_logger.IsErrorEnabled)
                _logger.ErrorFormat(format, args, exception);
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (_logger.IsErrorEnabled)
                _logger.ErrorFormat(formatProvider, format, args);
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (_logger.IsErrorEnabled)
                _logger.ErrorFormat(formatProvider, format, args, exception);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        public void Error(Func<string> formattingCallback)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(formattingCallback());
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Error"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        /// <param name="exception">The exception to log.</param>
        public void Error(Func<string> formattingCallback, Exception exception)
        {
            if (_logger.IsErrorEnabled)
                _logger.Error(formattingCallback(), exception);
        }

        public void Fatal(object message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            _logger.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (_logger.IsFatalEnabled)
                _logger.FatalFormat(format, args);
        }

        public void FatalFormat(string format, Exception exception, params object[] args)
        {
            if (_logger.IsFatalEnabled)
                _logger.FatalFormat(format, args, exception);
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (_logger.IsFatalEnabled)
                _logger.FatalFormat(formatProvider, format, args);
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (_logger.IsFatalEnabled)
                _logger.FatalFormat(formatProvider, format, args, exception);
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        public void Fatal(Func<string> formattingCallback)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(formattingCallback());
        }

        /// <summary>
        /// Log a message object with the <see cref="LogLevel.Fatal"/> level including
        /// the stack trace of the <see cref="Exception"/> passed
        /// as a parameter.
        /// </summary>
        /// <param name="formattingCallback">Callback method that allows defering the formatting 
        /// of message to be performed when needed</param>
        /// <param name="exception">The exception to log.</param>
        public void Fatal(Func<string> formattingCallback, Exception exception)
        {
            if (_logger.IsFatalEnabled)
                _logger.Fatal(formattingCallback(), exception);
        }

        public bool IsDebugEnabled
        {
            get { return _logger.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return _logger.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return _logger.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _logger.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return _logger.IsWarnEnabled; }
        }
        #endregion

    }
}
