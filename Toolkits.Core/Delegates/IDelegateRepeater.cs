using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toolkits.Core;

public interface IDelegateRepeater
{
    /// <summary>
    /// Unregister all delegate callback from subscriber by subscribeToken
    /// </summary>
    /// <param name="unregisterToken"></param>
    void Unregister(string unregisterToken);

    /// <summary>
    ///  execute a delegate callback  by subscribeToken and other delegate parameters
    /// </summary>
    /// <param name="publishToken"></param>
    /// <param name="delegateParamters"></param>
    void Publish(string publishToken, params object[] delegateParamters);

    /// <summary>
    ///  async execute a delegate callback  by subscribeToken and other delegate parameters
    /// </summary>
    /// <param name="publishToken"></param>
    /// <param name="delegateParamters"></param>
    /// <returns></returns>
    Task PublishAsync(string publishToken, params object[] delegateParamters);

    /// <summary>
    ///  execute a delegate callback  by subscribeToken and other delegate parameters
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="publishToken"></param>
    /// <param name="delegateParamters"></param>
    /// <returns><typeparamref name="TResult"/></returns>
    TResult Publish<TResult>(string publishToken, params object[] delegateParamters);

    /// <summary>
    ///  async execute a delegate callback  by subscribeToken and other delegate parameters
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="publishToken"></param>
    /// <param name="delegateParamters"></param>
    /// <returns><typeparamref name="TResult"/></returns>
    Task<TResult> PublishAsync<TResult>(string publishToken, params object[] delegateParamters);

    #region 0

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TResult>(string subscribeToken, Func<TResult> subscribeDelegate);

    /// <summary>
    /// subscribe function
    /// </summary>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe(string subscribeToken, Action subscribeDelegate);

    #endregion

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1>(string subscribeToken, Action<TMessage1> subscribeDelegate);

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TResult>(string subscribeToken, Func<TMessage1, TResult> subscribeDelegate);

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2>(string subscribeToken, Action<TMessage1, TMessage2> subscribeDelegate);

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TResult>(string subscribeToken, Func<TMessage1, TMessage2, TResult> subscribeDelegate);

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3>(string subscribeToken, Action<TMessage1, TMessage2, TMessage3> subscribeDelegate);

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TResult>(string subscribeToken, Func<TMessage1, TMessage2, TMessage3, TResult> subscribeDelegate);

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4>(
        string subscribeToken,
        Action<TMessage1, TMessage2, TMessage3, TMessage4> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TResult>(
        string subscribeToken,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TResult> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5>(
        string subscribeToken,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TResult>(
        string subscribeToken,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TResult> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6>(
        string subscribeToken,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TResult>(
        string subscribeToken,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TResult> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7>(
        string subscribeToken,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TResult>(
        string subscribeToken,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TResult> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8>(
        string subscribeToken,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TResult>(
        string subscribeToken,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TResult> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9>(
        string subscribeToken,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TResult>(
        string subscribeToken,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TResult> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10>(
        string subscribeToken,
        Action<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10, TResult>(
        string subscribeToken,
        Func<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10, TResult> subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<TMessage1, TMessage2, TMessage3, TMessage4, TMessage5, TMessage6, TMessage7, TMessage8, TMessage9, TMessage10, TMessage11>(
        string subscribeToken,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TResult
    >(
        string subscribeToken,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TResult
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12
    >(
        string subscribeToken,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TResult
    >(
        string subscribeToken,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TResult
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13
    >(
        string subscribeToken,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TResult
    >(
        string subscribeToken,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TResult
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14
    >(
        string subscribeToken,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14,
        TResult
    >(
        string subscribeToken,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14,
            TResult
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>
    /// <typeparam name="TMessage15">parameter 15</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14,
        TMessage15
    >(
        string subscribeToken,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14,
            TMessage15
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>
    /// <typeparam name="TMessage15">parameter 15</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14,
        TMessage15,
        TResult
    >(
        string subscribeToken,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14,
            TMessage15,
            TResult
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>
    /// <typeparam name="TMessage15">parameter 15</typeparam>
    /// <typeparam name="TMessage16">parameter 16</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14,
        TMessage15,
        TMessage16
    >(
        string subscribeToken,
        Action<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14,
            TMessage15,
            TMessage16
        > subscribeDelegate
    );

    /// <summary>
    /// subscribe function
    /// </summary>
    /// <typeparam name="TMessage1">parameter 1</typeparam>
    /// <typeparam name="TMessage2">parameter 2</typeparam>
    /// <typeparam name="TMessage3">parameter 3</typeparam>
    /// <typeparam name="TMessage4">parameter 4</typeparam>
    /// <typeparam name="TMessage5">parameter 5</typeparam>
    /// <typeparam name="TMessage6">parameter 6</typeparam>
    /// <typeparam name="TMessage7">parameter 7</typeparam>
    /// <typeparam name="TMessage8">parameter 8</typeparam>
    /// <typeparam name="TMessage9">parameter 9</typeparam>
    /// <typeparam name="TMessage10">parameter 10</typeparam>
    /// <typeparam name="TMessage11">parameter 11</typeparam>
    /// <typeparam name="TMessage12">parameter 12</typeparam>
    /// <typeparam name="TMessage13">parameter 13</typeparam>
    /// <typeparam name="TMessage14">parameter 14</typeparam>
    /// <typeparam name="TMessage15">parameter 15</typeparam>
    /// <typeparam name="TMessage16">parameter 16</typeparam>
    /// <typeparam name="TResult">return value type</typeparam>

    /// <param name="subscribeToken">subscribe token</param>
    /// <param name="subscribeDelegate">subscribe callback</param>
    void Subscribe<
        TMessage1,
        TMessage2,
        TMessage3,
        TMessage4,
        TMessage5,
        TMessage6,
        TMessage7,
        TMessage8,
        TMessage9,
        TMessage10,
        TMessage11,
        TMessage12,
        TMessage13,
        TMessage14,
        TMessage15,
        TMessage16,
        TResult
    >(
        string subscribeToken,
        Func<
            TMessage1,
            TMessage2,
            TMessage3,
            TMessage4,
            TMessage5,
            TMessage6,
            TMessage7,
            TMessage8,
            TMessage9,
            TMessage10,
            TMessage11,
            TMessage12,
            TMessage13,
            TMessage14,
            TMessage15,
            TMessage16,
            TResult
        > subscribeDelegate
    );
}
