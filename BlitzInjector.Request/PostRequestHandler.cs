﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BlitzInjector.Request
{
    public class PostRequestHandler : RequestHandler
    {

        /// <summary>
        /// Dictionary like Data but those paramters aint gonna change
        /// </summary>
        public Dictionary<string, string> Data { get; set; }

        public string Key { get; set; }

        public PostRequestHandler(Uri url,
                                  Dictionary<string, string> data,
                                  string key,
                                  Dictionary<HttpRequestHeader, string> headers = null)
            : base(url, headers)
        {
            Data = data;

            Key = key;

            if(!data.ContainsKey(key))
                throw new Exception("key does not exists");

        }

        public override string Fetch()
        {

            var byteArrayData = Encoding.UTF8.GetBytes(BuildDataString());

            RequestInstance.ContentLength = byteArrayData.Length;

            RequestInstance.Method = "POST";

            using (var dataStream = RequestInstance.GetRequestStream())
            {
                dataStream.Write(byteArrayData, 0, byteArrayData.Length);
            }

            var response = (HttpWebResponse)RequestInstance.GetResponse();

            return new StreamReader(response.GetResponseStream()).ReadToEnd();

        }

        public override void RefreshVariable(string value)
        {
            Data[Key] = value;
        }

        private string BuildDataString()
        {
            var stringBuilder = "";

            foreach (var parameter in Data)
            {
                if (stringBuilder.Length == 0)
                    stringBuilder = String.Format("{0}={1}", parameter.Key, parameter.Value);
                else
                    stringBuilder += String.Format("&{0}={1}", parameter.Key, parameter.Value);
            }

            foreach (var parameter in Data)
            {
                if (stringBuilder.Length == 0)
                    stringBuilder = String.Format("{0}={1}", parameter.Key, parameter.Value);
                else
                    stringBuilder += String.Format("&{0}={1}", parameter.Key, parameter.Value);
            }

            return stringBuilder;
        }
    }
}
