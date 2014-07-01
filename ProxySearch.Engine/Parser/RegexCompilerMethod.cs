using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CSharp;
using ProxySearch.Engine.Proxies;

namespace ProxySearch.Engine.Parser
{
    public class RegexCompilerMethod : IParseMethod
    {
        private Regex regex;
        private dynamic instance;

        public RegexCompilerMethod(ParseDetails details)
        {
            regex = new Regex(details.RegularExpression);

            string classString = string.Format(EmbeddedResource.ReadToEnd("ProxySearch.Engine.Resources.ProxyParserRuntime._cs"), details.Code);

            CompilerParameters parameters = new CompilerParameters
            {
                GenerateInMemory = true,
            };

            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Core.dll");
            parameters.ReferencedAssemblies.Add("System.Web.dll");
            parameters.ReferencedAssemblies.Add("ProxySearch.Engine.dll");

            CompilerResults compilerResult = new CSharpCodeProvider().CompileAssemblyFromSource(parameters, classString);

            if (compilerResult.Errors.Count > 0)
                throw new InvalidOperationException(string.Join(Environment.NewLine,
                                                                compilerResult.Errors
                                                                              .Cast<CompilerError>()
                                                                              .Select(error => error.ErrorText)));

            instance = compilerResult.CompiledAssembly.CreateInstance("ProxySearch.Engine.Resources.ProxyParserRuntime");
        }

        public IEnumerable<Proxy> Parse(string document)
        {
            return regex.Matches(document)
                        .Cast<Match>()
                        .Select(match => (Proxy)instance.GetProxyOrNull(document, match))
                        .Where(proxy => proxy != null);
        }
    }
}
