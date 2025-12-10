/***********************************************************
 * (C) Copyright 2022 HP Development Company, L.P.
 * All rights reserved.
 ***********************************************************/
using System;

namespace OXPd2ExamplesHost.Models
{
    public class LogItem
    {
        public dynamic Data  { get; }

        public string Type { get; }
       
        public String TimeStamp { get; }

        public LogItem(string type, dynamic data)
        {
            this.Type = type;
            this.Data = data;
            this.TimeStamp = DateTime.Now.ToString();
        }

        public LogItem(dynamic data)
        {
            this.Data = data;
            this.TimeStamp = DateTime.Now.ToString();
        }
    }
    public class AuthenticationLogItem : LogItem
    {
        public const string PostPromptResultType = "PostPromptResult";
        public const string PrePromptResultType = "PrePromptResult";
        public const string PromptType = "Prompt";
        public const string SignoutNotificationType = "SignoutNotification";

        public AuthenticationLogItem(string Type, object AuthApiRequest, object AuthResult) : base(Type, new { AuthApiRequest, AuthResult })
        {
        }
    }

    public enum AuthenticationLogItemType
    {
        PostPromptResult, PrePromptResult, Prompt, SignoutNotification
    }
}
