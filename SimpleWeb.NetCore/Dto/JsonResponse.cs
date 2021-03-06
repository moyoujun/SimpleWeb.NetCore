﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleWeb.NetCore.Dto
{
    public class JsonResponseModel
    {
        /// <summary>
        /// 消息 
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 状态 
        /// </summary>
        public JsonResponseStatus Status { get; set; }
    }

    /// <summary>
    /// JsonResponseModel for Generic Types 
    /// </summary>
    /// <typeparam name="T"> Type </typeparam>
    public class JsonResponseModel<T> : JsonResponseModel
    {
        /// <summary>
        /// Data 
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// JsonResponseStatus 
    /// </summary>
    public enum JsonResponseStatus
    {
        /// <summary>
        /// Success 
        /// </summary>
        Success = 0,

        /// <summary>
        /// RequestError 
        /// </summary>
        RequestError = 1,

        /// <summary>
        /// AuthFail 
        /// </summary>
        AuthFail = 2,

        /// <summary>
        /// ProcessFail 
        /// </summary>
        ProcessFail = 3,

        /// <summary>
        /// IDNotFound
        /// </summary>
        IDNotFound = 4,

        AccountError = 5,
    }
}
