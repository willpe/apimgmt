// --------------------------------------------------------------------------
//  <copyright file="Config.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------

namespace MS.Azure.ApiManagement.Tests
{
    public partial class Config
    {
        private static readonly Config instance = new Config();

        public static string ConnectionString
        {
            get { return instance.GetConnectionString(); }
        }
    }
}