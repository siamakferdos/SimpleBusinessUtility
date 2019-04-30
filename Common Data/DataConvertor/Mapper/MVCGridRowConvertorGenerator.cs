//using System;
//using System.CodeDom.Compiler;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using Microsoft.CSharp;

//namespace Shoniz.Common.Data.DataConvertor.Mapper
//{
//    class MVCGridRowConvertorGenerator<T>
//    {
//        internal string MethodBodyGenerator(T gridObj)
//        {
//            var statement = new StringBuilder();
//            statement.AppendLine("public string Convert(" + typeof(T).FullName + " gridObj, string rowTemplate)");
//            statement.AppendLine("{");

//            var o = (T)Activator.CreateInstance(typeof(T));
//            statement.Append("var ");


//            statement.AppendLine("}");


            
//            statement.AppendLine("return " + "_" + o.GetType().Name + ";");
//            statement.AppendLine("}");
//            return statement.ToString();
//        }

//        public Type ClassGenerator(SqlDataReader reader)
//        {
//            CSharpCodeProvider c = new CSharpCodeProvider();
//#pragma warning disable 618
//            ICodeCompiler icc = c.CreateCompiler();
//#pragma warning restore 618
//            CompilerParameters cp = new CompilerParameters();

//            cp.ReferencedAssemblies.Add("system.dll");
//            cp.ReferencedAssemblies.Add("system.xml.dll");
//            cp.ReferencedAssemblies.Add("system.data.dll");
//            cp.ReferencedAssemblies.Add("mscorlib.dll");
//            cp.ReferencedAssemblies.Add(typeof(T).Assembly.ManifestModule.FullyQualifiedName);
//            cp.ReferencedAssemblies.Add(typeof(IMapper<SqlDataReader, T>).Assembly.ManifestModule.FullyQualifiedName);

//            cp.CompilerOptions = "/t:library";
//            cp.GenerateInMemory = true;


//            var sb = new StringBuilder("");
//            sb.Append("using System; \n");
//            sb.Append("using System.Xml; \n");
//            sb.Append("using System.Data; \n");
//            sb.Append("using System.Data.SqlClient; \n");
//            sb.Append("using Shoniz.Common.Data.DataConvertor.Mapper; \n");
//            if (!IsMicrosoftType(typeof(T)))
//                cp.ReferencedAssemblies.Add(typeof(T).Assembly.ManifestModule.FullyQualifiedName);

//            sb.Append("namespace DynamicAssemblyInRuntime \n");
//            sb.Append("{ \n");
//            sb.Append("    public class " + typeof(T).FullName.Replace('.', '_') + " : IMapper<SqlDataReader, " + typeof(T).FullName + "> \n");
//            sb.Append("    { \n");
//            sb.Append(MethodBodyGenerator(reader));
//            //sb.Append("        public string Execute() \n");
//            //sb.Append("        {");
//            //sb.Append("             return  \"hhhhhhhhhh\";");
//            //sb.Append("            //#CSharpCodesToReturnTOutObject \n");
//            //sb.Append("        } // EOF method \n");
//            sb.Append("    } // EOF class \n");
//            sb.Append("} // EOF namespace \n");

//            //
//            // Generate Code within 'EvalCode' method
//            //


//            var cr = icc.CompileAssemblyFromSource(cp, sb.ToString());
//            if (cr.Errors.Count > 0)
//            {
//                throw new EvaluateException("ERROR: " + cr.Errors[0].ErrorText);
//            }

//            System.Reflection.Assembly a = cr.CompiledAssembly;

//            Type t = a.GetType("DynamicAssemblyInRuntime." + typeof(T).FullName.Replace('.', '_'));

//            //MethodInfo mi = t.GetMethod("Convert");

//            return t;
//        }

//        private bool IsMicrosoftType(Type type)
//        {
//            if (type == null)
//                throw new ArgumentNullException("type");

//            if (type.Assembly.GetName().Name.Equals("mscorlib", StringComparison.OrdinalIgnoreCase))
//                return true;

//            var atts = type.Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
//            if (atts.Length == 0)
//                return false;

//            var aca = (AssemblyCompanyAttribute)atts[0];
//            return aca.Company != null && aca.Company.IndexOf("Microsoft Corporation", StringComparison.OrdinalIgnoreCase) >= 0;
//        }
//    }



//}
