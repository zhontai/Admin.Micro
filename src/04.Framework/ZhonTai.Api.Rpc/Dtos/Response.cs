﻿using ZhonTai.Api.Rpc.Exceptions;

namespace ZhonTai.Api.Rpc.Dtos;

/// <summary>
/// 结果输出
/// </summary>
public class Response<T> : IResponse<T>
{
    /// <summary>
    /// 是否成功标记
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Msg { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public T Data { get; set; }

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="msg">消息</param>
    public Response<T> Ok(T data, string msg = null)
    {
        Success = true;
        Data = data;
        Msg = msg;

        return this;
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="msg">消息</param>
    /// <param name="data">数据</param>
    /// <returns></returns>
    public Response<T> NotOk(string msg = null, T data = default)
    {
        Success = false;
        Msg = msg;
        Data = data;

        return this;
    }
}

/// <summary>
/// 静态结果输出
/// </summary>
public static partial class Response
{
    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    public static IResponse<T> Ok<T>(T data = default(T), string msg = null)
    {
        return new Response<T>().Ok(data, msg);
    }

    /// <summary>
    /// 成功
    /// </summary>
    /// <returns></returns>
    public static IResponse<string> Ok()
    {
        return Ok<string>();
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="msg">消息</param>
    /// <param name="data">数据</param>
    /// <returns></returns>
    public static IResponse<T> NotOk<T>(string msg = null, T data = default)
    {
        return new Response<T>().NotOk(msg, data);
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    public static IResponse<string> NotOk(string msg = null)
    {
        return new Response<string>().NotOk(msg);
    }

    /// <summary>
    /// 根据布尔值返回结果
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static IResponse<T> Result<T>(bool success)
    {
        return success ? Ok<T>() : NotOk<T>();
    }

    /// <summary>
    /// 根据布尔值返回结果
    /// </summary>
    /// <param name="success"></param>
    /// <returns></returns>
    public static IResponse<string> Result(bool success)
    {
        return success ? Ok() : NotOk();
    }

    /// <summary>
    /// 系统异常
    /// </summary>
    /// <param name="msg">消息</param>
    /// <param name="code">编码</param>
    /// <returns></returns>
    public static AppException Exception(string msg = null, string code = null)
    {
        return new AppException(msg, code);
    }
}