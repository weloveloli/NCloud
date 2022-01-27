// -----------------------------------------------------------------------
// <copyright file="AssemblyExtension.cs" company="Weloveloli">
//    Copyright (c) 2021 weloveloli. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace NCloud.FileProviders.Support
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public static class AssemblyExtension
    {
        /// <summary>
        /// GetProviderAssembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetProviderAssembly(this Assembly assembly)
        {
            var path = Path.GetDirectoryName(assembly.Location);
            var list = new List<Assembly>(); 
            foreach (var dll in Directory.GetFiles(path, "NCloud.FileProviders.*.dll"))
            {
                list.Add(Assembly.LoadFrom(dll));
            }
            return list;
        }
    }
}
