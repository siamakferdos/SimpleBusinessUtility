using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using Microsoft.SqlServer.Server;

namespace Shoniz.Common.Data.DataConvertor.Mapper
{
    class DataRecordConvertorGenerator<T>
    {
        internal string MethodBodyGenerator(SqlDataRecord reader)
        {
            var statement = new StringBuilder();
            statement.AppendLine("public " + typeof(T).FullName + " Convert(SqlDataRecord reader, object o)");
            statement.AppendLine("{");

            statement.AppendLine(" var schemaTable = reader.GetSchemaTable();");

            var objectQueue = new Queue<QueueObject>();
            var o = (T)Activator.CreateInstance(typeof(T));

            objectQueue.Enqueue(new QueueObject("_" + o.GetType().Name, o));
            statement.Append("var ");


            while (objectQueue.Count > 0)
            {
                var quObj = objectQueue.Dequeue();
                var obj = quObj.InstanceOfObject;

                var currentObjName = quObj.ObjectFullSpec;
                var currentObjTypeName = obj.GetType().FullName;
                statement.Append(currentObjName + " = new " + currentObjTypeName + "();\r\n");
                statement.AppendLine(" if (schemaTable != null) ");
                statement.AppendLine("{");
                //statement.AppendLine(" dynamic a11;");
                var counter = 0;
                foreach (var prop in obj.GetType().GetProperties())
                {
                    counter++;
                    var collections = new List<Type>() { typeof(IEnumerable<>), typeof(IEnumerable) };
                    if (prop.PropertyType != typeof(string) && prop.PropertyType.GetInterfaces().Any(i => collections.Any(c => i == c)))
                        continue;

                    if (prop.PropertyType.IsValueType || prop.PropertyType == typeof(string) ||
                        prop.PropertyType.IsEnum)
                    {
                        //if data reader has object property

                        statement.AppendLine("if (schemaTable.Select(\"ColumnName='" + prop.Name + "'\").Length > 0) ");
                        statement.AppendLine("  {");

                        
                        
                        if (prop.PropertyType.IsEnum)
                            statement.AppendLine(string.Format("{0}.{1} = ({2})(reader[\"{1}\"]);",
                                currentObjName,
                                prop.Name, prop.PropertyType.FullName));
                        else if (prop.PropertyType == typeof(string))
                            statement.AppendLine(
                                string.Format(
                                    "{0}.{1} = ({2})(!reader.IsDBNull(reader.GetOrdinal(\"{1}\")) ? reader[\"{1}\"].ToString() : \"\");",
                                    currentObjName, prop.Name, prop.PropertyType.Name));
                        else
                        {
                            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                statement.AppendLine(
                               string.Format(
                                    
                                    " if(!reader.IsDBNull(reader.GetOrdinal(\"{1}\"))){{" +
                                    " {2} a{3};" +
                                    " {2}.TryParse(reader[\"{1}\"].ToString(), out a{3});" +
                                    "{0}.{1} = a{3};" +
                                    "}}" +
                                    //" {0}.{1} = {2}.TryParse(reader[\"{1}\"].ToString(), a11);" +
                                   " else {0}.{1} = null;",
                                   currentObjName, prop.Name, prop.PropertyType.GetGenericArguments()[0], counter));
                            else
                                statement.AppendLine(
                                string.Format(
                                
                                    " if(!reader.IsDBNull(reader.GetOrdinal(\"{1}\"))){{" +
                                    " {2} a{3};" +
                                    " {2}.TryParse(reader[\"{1}\"].ToString(), out a{3});" +
                                    "{0}.{1} = a{3};" +
                                    "}}" +
                                    //" {0}.{1} = {2}.TryParse(reader[\"{1}\"].ToString(), a11);" +
                                   " else {0}.{1} = default({2});",
                                    currentObjName, prop.Name, prop.PropertyType.FullName, counter));
                        }

                        statement.AppendLine("  }");










                    }
                    //else
                    //{
                    //    if (prop.PropertyType.IsEnum)
                    //        statement.AppendLine(string.Format("{0}.{1} = null",
                    //            currentObjName,
                    //            prop.Name));
                    //    else if (prop.PropertyType == typeof(string))
                    //        statement.AppendLine(
                    //            string.Format(
                    //                "{0}.{1} =  \"\";",
                    //                currentObjName, prop.Name));
                    //    else
                    //    {
                    //        if (prop.PropertyType.IsGenericType &&
                    //            prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    //            statement.AppendLine(
                    //                string.Format(
                    //                    "{0}.{1} = null;",
                    //                    currentObjName, prop.Name));
                    //        else
                    //            statement.AppendLine(
                    //                string.Format(
                    //                    "{0}.{1} =  default({2});",
                    //                    currentObjName, prop.Name, prop.PropertyType.FullName));
                    //    }
                    //}
                    //}
                    else
                    {
                        var ns = obj.GetType().Namespace;
                        if (ns != null && prop.PropertyType.IsClass && !ns.StartsWith("System") && !ns.StartsWith("mscorlib"))
                            objectQueue.Enqueue(new QueueObject(quObj.ObjectFullSpec + "." + prop.Name, Activator.CreateInstance(prop.PropertyType)));
                    }
                }
                counter = 0;
                foreach (var field in obj.GetType().GetFields())
                {
                    counter++;
                    if (field.FieldType.IsValueType || field.FieldType == typeof(string) ||
                        field.FieldType.IsEnum)
                    {
                        //if data reader has object property
                        var collections = new List<Type>() { typeof(IEnumerable<>), typeof(IEnumerable) };
                        if (field.FieldType != typeof(string) && field.FieldType.GetInterfaces().Any(i => collections.Any(c => i == c)))
                            continue;

                        statement.AppendLine("if (schemaTable.Select(\"ColumnName='" + field.Name + "'\").Length > 0) ");
                        statement.AppendLine("  {");

                        

                        if (field.FieldType.IsEnum)
                            statement.AppendLine(string.Format("{0}.{1} = ({2})(reader[\"{1}\"]);",
                                currentObjName,
                                field.Name, field.FieldType.FullName));
                        else if (field.FieldType == typeof(string))
                            statement.AppendLine(
                                string.Format(
                                    "{0}.{1} = ({2})(!reader.IsDBNull(reader.GetOrdinal(\"{1}\")) ? reader[\"{1}\"].ToString() : \"\");",
                                    currentObjName, field.Name, field.FieldType.Name));
                        else
                        {
                            if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                statement.AppendLine(
                               string.Format(
                                   " if(!reader.IsDBNull(reader.GetOrdinal(\"{1}\"))){{" +
                                   " {2} a{3};" +
                                    " {2}.TryParse(reader[\"{1}\"].ToString(), out a{3});" +
                                    "{0}.{1} = a{3};" +
                                    "}}" +
                                   " else {0}.{1} = null;",
                                   currentObjName, field.Name, field.FieldType.FullName, counter));
                            else
                                statement.AppendLine(
                                string.Format(
                                " if(!reader.IsDBNull(reader.GetOrdinal(\"{1}\"))){{" +
                                   " {2} a{3};" +
                                    " {2}.TryParse(reader[\"{1}\"].ToString(), out a{3});" +
                                    "{0}.{1} = a{3};" +
                                    "}}" +
                                   " else {0}.{1} = default({2});",
                                    currentObjName, field.Name, field.FieldType.GetGenericArguments()[0], counter));
                            //"{0}.{1} = !reader.IsDBNull(reader.GetOrdinal(\"{1}\")) ? {2}.Parse(reader[\"{1}\"].ToString()) : default({2});"
                        }

                        statement.AppendLine("  }");

                        //}
                        //else
                        //{

                        //    if (field.FieldType.IsEnum)
                        //        statement.AppendLine(string.Format("{0}.{1} = null",
                        //            currentObjName,
                        //            field.Name));
                        //    else if (field.FieldType == typeof (string))
                        //        statement.AppendLine(
                        //            string.Format(
                        //                "{0}.{1} =  \"\";",
                        //                currentObjName, field.Name));
                        //    else
                        //    {
                        //        if (field.FieldType.IsGenericType &&
                        //            field.FieldType.GetGenericTypeDefinition() == typeof (Nullable<>))
                        //            statement.AppendLine(
                        //                string.Format(
                        //                    "{0}.{1} = null;",
                        //                    currentObjName, field.Name));
                        //        else
                        //            statement.AppendLine(
                        //                string.Format(
                        //                    "{0}.{1} =  default({2});",
                        //                    currentObjName, field.Name, field.FieldType.FullName));
                        //    }

                        //}
                    }
                    else
                    {
                        var ns = obj.GetType().Namespace;
                        if (ns != null && field.FieldType.IsClass && !ns.StartsWith("System") && !ns.StartsWith("mscorlib"))
                            objectQueue.Enqueue(new QueueObject(quObj.ObjectFullSpec + "." + field.Name, Activator.CreateInstance(field.FieldType)));
                    }
                }
                statement.AppendLine("}");


            }
            statement.AppendLine("return " + "_" + o.GetType().Name + ";");
            statement.AppendLine("}");
            return statement.ToString();
        }

        public Type ClassGenerator(SqlDataReader reader)
        {
            CSharpCodeProvider c = new CSharpCodeProvider();
#pragma warning disable 618
            ICodeCompiler icc = c.CreateCompiler();
#pragma warning restore 618
            CompilerParameters cp = new CompilerParameters();

            cp.ReferencedAssemblies.Add("system.dll");
            cp.ReferencedAssemblies.Add("system.xml.dll");
            cp.ReferencedAssemblies.Add("system.data.dll");
            cp.ReferencedAssemblies.Add("mscorlib.dll");
            //cp.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            cp.ReferencedAssemblies.Add(typeof(T).Assembly.ManifestModule.FullyQualifiedName);
            cp.ReferencedAssemblies.Add(typeof(IMapper<SqlDataReader, T>).Assembly.ManifestModule.FullyQualifiedName);

            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;


            var sb = new StringBuilder("");
            sb.Append("using System; \n");
            sb.Append("using System.Xml; \n");
            sb.Append("using System.Data; \n");
            sb.Append("using System.Data.SqlClient; \n");
            //sb.Append("using Microsoft.CSharp.RuntimeBinder; \n");
            sb.Append("using Shoniz.Common.Data.DataConvertor.Mapper; \n");
            if (!IsMicrosoftType(typeof(T)))
                cp.ReferencedAssemblies.Add(typeof(T).Assembly.ManifestModule.FullyQualifiedName);

            sb.Append("namespace DynamicAssemblyInRuntime \n");
            sb.Append("{ \n");
            sb.Append("    public class " + typeof(T).FullName.Replace('.', '_') + " : IMapper<SqlDataReader, " + typeof(T).FullName + "> \n");
            sb.Append("    { \n");
            sb.Append(MethodBodyGenerator(reader));
            //sb.Append("        public string Execute() \n");
            //sb.Append("        {");
            //sb.Append("             return  \"hhhhhhhhhh\";");
            //sb.Append("            //#CSharpCodesToReturnTOutObject \n");
            //sb.Append("        } // EOF method \n");
            sb.Append("    } // EOF class \n");
            sb.Append("} // EOF namespace \n");

            //
            // Generate Code within 'EvalCode' method
            //


            var cr = icc.CompileAssemblyFromSource(cp, sb.ToString());
            if (cr.Errors.Count > 0)
            {
                throw new EvaluateException("ERROR: " + cr.Errors[0].ErrorText);
            }

            System.Reflection.Assembly a = cr.CompiledAssembly;

            Type t = a.GetType("DynamicAssemblyInRuntime." + typeof(T).FullName.Replace('.', '_'));

            //MethodInfo mi = t.GetMethod("Convert");

            return t;
        }

        private bool IsMicrosoftType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.Assembly.GetName().Name.Equals("mscorlib", StringComparison.OrdinalIgnoreCase))
                return true;

            var atts = type.Assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), true);
            if (atts.Length == 0)
                return false;

            var aca = (AssemblyCompanyAttribute)atts[0];
            return aca.Company != null && aca.Company.IndexOf("Microsoft Corporation", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        //private string SetDefaultStatementGenerator(string currentObjName, PropertyInfo prop)
        //{
        //    var statement = new StringBuilder();
        //    if (prop.PropertyType.IsEnum)
        //        statement.AppendLine(string.Format("{0}.{1} = null",
        //            currentObjName,
        //            prop.Name));
        //    else if (prop.PropertyType == typeof(string))
        //        statement.AppendLine(
        //            string.Format(
        //                "{0}.{1} =  \"\";",
        //                currentObjName, prop.Name));
        //    else
        //    {
        //        if (prop.PropertyType.IsGenericType &&
        //            prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
        //            statement.AppendLine(
        //                string.Format(
        //                    "{0}.{1} = null;",
        //                    currentObjName, prop.Name));
        //        else
        //            statement.AppendLine(
        //                string.Format(
        //                    "{0}.{1} =  default({2});",
        //                    currentObjName, prop.Name, prop.PropertyType.FullName));
        //    }

        //    return statement.ToString();
        //}


    }



}
