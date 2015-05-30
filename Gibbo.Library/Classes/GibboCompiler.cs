﻿#region Copyrights
/*
Gibbo2D - Copyright - 2013 Gibbo2D Team
Founders - Joao Alves <joao.cpp.sca@gmail.com> and Luis Fernandes <luisapidcloud@hotmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. 
*/
#endregion
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.Reflection;

namespace Gibbo.Library
{
#if WINDOWS
    /// <summary>
    /// Another way of compilling scripts.
    /// This is not how the editor compiles the scripts, this should be used if you want to compile scripts manually.
    /// </summary>
    public static class GibboCompiler
    {
        /// <summary>
        /// Compiles a source file with the given parameters and source
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        private static CompilerResults Compile(CompilerParameters parameters, string source)
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.Location.Contains("Microsoft.Xna") || asm.Location.Contains("Gibbo.Library")
                    || asm.Location.Contains("System"))
                {
                    parameters.ReferencedAssemblies.Add(asm.Location);
                }
            }

            //parameters.ReferencedAssemblies.Add(@"C:\Users\Inspire\Documents\Gibbo\scripts_sds\DefaultGameScripts.dll");

            CodeDomProvider compiler = CSharpCodeProvider.CreateProvider("CSharp");
            return compiler.CompileAssemblyFromSource(parameters, source);
        }

        /// <summary>
        /// Compiles the input string and saves it to an output.dll
        /// </summary>
        /// <param name="source">Source Code</param>
        /// <param name="referencedAssemblies">The assembly names to be referenciated</param>
        /// <param name="output">Filename for the output .dll</param>
        public static void CompileScripts(string source, List<string> referencedAssemblies, string output)
        {
            CompilerParameters parameters = new CompilerParameters();

            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = false;
            parameters.IncludeDebugInformation = false;
            parameters.OutputAssembly = output;

            //Add the required assemblies
            foreach(string reference in referencedAssemblies)
                parameters.ReferencedAssemblies.Add(SceneManager.GameProject.ProjectPath + "\\libs\\" + reference);

            Compile(parameters, source);
        }

        /// <summary>
        /// Compiles the input string and saves it in memory
        /// </summary>
        /// <param name="source"></param>
        /// <param name="referencedAssemblies"></param>
        /// <returns></returns>
        public static CompilerResults LoadScriptsToMemory(string source, List<string> referencedAssemblies)
        {
            CompilerParameters parameters = new CompilerParameters();

            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            parameters.IncludeDebugInformation = false;

            //Add the required assemblies
            foreach (string reference in referencedAssemblies)
            {
                parameters.ReferencedAssemblies.Add(SceneManager.GameProject.ProjectPath + "\\libs\\" + reference);
            }
            
            return Compile(parameters, source);
        }
    }
#endif
}
