using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Ardalis.SmartEnum.JsonNet;
using Ardalis.SmartEnum;

public class MessagingHelper
{
    public bool Success { get; set; }
    public string Message { get; set; } = "";

    [JsonConverter(typeof(SmartEnumValueConverter<ErrorType, string>))]
    public ErrorType? ErrorType { get; set; }

    public void SetMessage(string message)
    {
        Message = message;
    }

    public void SetMessageAndWriteToLog(string message, LogTypes logType, string? extraMessageToLog = "", [System.Runtime.CompilerServices.CallerMemberName] string methodName = "")
    {
        Message = message;

        StackTrace stackTrace = new StackTrace();
        var callingMethod = stackTrace.GetFrame(1)?.GetMethod();
        var methodNamespace = callingMethod?.DeclaringType?.Namespace ?? "Unknown";

        string messageToLog = $"{message} {extraMessageToLog}";
        Utils.WriteToLog(logType, messageToLog, methodName, methodNamespace);
    }

    // Método estático para mensagens de sucesso (para a versão não genérica)
    public static MessagingHelper SuccessMessage(string message)
    {
        return new MessagingHelper
        {
            Success = true,
            Message = message
        };
    }

    // Método estático para mensagens de erro (para a versão não genérica)
    public static MessagingHelper ErrorMessage(string message, ErrorType errorType)
    {
        return new MessagingHelper
        {
            Success = false,
            Message = message,
            ErrorType = errorType
        };
    }
}

public class MessagingHelper<T> : MessagingHelper
{
    public T? Obj { get; set; }

    // Método estático para mensagens de sucesso (para a versão genérica)
    public static MessagingHelper<T> SuccessMessage(T obj, string message = "Operation succeeded")
    {
        return new MessagingHelper<T>
        {
            Success = true,
            Obj = obj,
            Message = message
        };
    }

    // Método estático para mensagens de erro (para a versão genérica)
    public static MessagingHelper<T> ErrorMessage(string message, ErrorType errorType)
    {
        return new MessagingHelper<T>
        {
            Success = false,
            Message = message,
            ErrorType = errorType
        };
    }

    // Método estático para mensagens de erro (para a versão genérica sem ErrorType)
    public static MessagingHelper<T> ErrorMessage(string message)
    {
        return new MessagingHelper<T>
        {
            Success = false,
            Message = message
        };
    }
}

public class ErrorType : SmartEnum<ErrorType, string>
{
    public static readonly ErrorType DataHasChanged = new ErrorType("DataHasChanged");
    public static readonly ErrorType NotFound = new ErrorType("NotFound");

    protected ErrorType(string name) : base(name, name) { }
}

public enum LogTypes
{
    Info,
    Warning,
    Error
}

public static class Utils
{
    public static void WriteToLog(LogTypes logType, string message, string methodName, string methodNamespace)
    {
        Console.WriteLine($"[{logType}] {methodNamespace}.{methodName}: {message}");
    }
}
