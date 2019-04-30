using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shoniz.MVCGrid
{
    public static class GridExtentions
    {
        public static string DisplayName(this System.Reflection.PropertyInfo propertyInfo)
        {
            try
            {
                return ((System.ComponentModel.DisplayNameAttribute)propertyInfo.GetCustomAttribute(
                    typeof(System.ComponentModel.DisplayNameAttribute))).DisplayName;
            }
            catch
            {
                return propertyInfo.Name;
            }
        }

        internal static GridCustomAttribute GetGridCustomAttribute(
            this System.Reflection.PropertyInfo property)
        {
            return property.GetCustomAttributes()
                .Where(a => a.TypeId.Equals(typeof(GridCustomAttribute)))
                .Cast<GridCustomAttribute>()
                .FirstOrDefault();
        }

        public static List<System.Data.DataTable> ToGridDataTable(this GridVm viewModel,
            System.Collections.Specialized.NameValueCollection form)
        {
            var dataTableList = new List<System.Data.DataTable>();
            var gridNames = form.AllKeys
                .Where(k => k.StartsWith("_Grid_*Grids"))
                .Select(key => key.Split('*')[2])
                .ToList();

            if (gridNames.Count < 1)
                return null;

            foreach (var gridName in gridNames)
            {
                var dt = new System.Data.DataTable(gridName);
                
                foreach (var fieldName in form.AllKeys.Where(k => k.StartsWith("_Grid_*Fields*" + gridName + "*")))
                    dt.Columns.Add(fieldName.Split('*')[3]);

                //the format of the keys for field is _Grid_*Value*GridName*key1=val1,key2=val2*FieldName
                var source = form.AllKeys.Where(f => f.StartsWith("_Grid_*Value")).ToList();

                source.Sort();
                
                var startField = source[0].Split('*')[0] + source[0].Split('*')[1] + source[0].Split('*')[2] + source[0].Split('*')[3];
                
                System.Data.DataRow dr = dt.NewRow();

                dr[source[0].Split('*')[4]] = form[source[0]];
                for (var i = 1; i < source.Count; i++)
                    //Ckeck that if all columns adds to table, then create new row(it will know from primary keys anf grid name)
                    if (startField != source[i].Split('*')[0] + source[i].Split('*')[1] + source[i].Split('*')[2] + source[i].Split('*')[3])
                    {
                        //before create new row,  this code will add all primary keys to columns. check i > 0 is for skip the first column
                        if (i > 1)
                        {
                            foreach (string primaryKeyValue in source[i - 1].Split('*')[3].Split(','))
                            {
                                dr[primaryKeyValue.Split('=')[0]] = primaryKeyValue.Split('=')[1];
                            }
                            if (i < source.Count - 1)
                                dt.Rows.Add(dr);
                        }
                        dr = dt.NewRow();
                        startField = source[i].Split('*')[0] + source[i].Split('*')[1] + source[i].Split('*')[2] + source[i].Split('*')[3];
                        dr[source[i].Split('*')[4]] = form[source[i]];
                    }
                    else
                        dr[source[i].Split('*')[4]] = form[source[i]];

                //for last row, primary keys will not be inserted(because there's no next row). So it will inserted here
                foreach (string primaryKeyValue in source[source.Count - 1].Split('*')[3].Split(','))
                {
                    dr[primaryKeyValue.Split('=')[0]] = primaryKeyValue.Split('=')[1];
                }

                dt.Rows.Add(dr);
                dataTableList.Add(dt);
            }
            return dataTableList;
        }

        internal static List<object> ToPageX(this IEnumerable<object> wholeList, int pageSize, int currentPage)
        {
            if (wholeList.Count() > (pageSize) * (currentPage - 1))
                return wholeList.Skip((pageSize) * (currentPage - 1)).Take(pageSize).ToList();
            else
                return wholeList.ToList();
        }

        public static List<object> ToModelViewObjectList<TModelViewType>(this IEnumerable<object> source)
        {
            var destinationList = new List<TModelViewType>();

            var modelViewProperties = typeof(TModelViewType).GetProperties();
            foreach (var sourceElement in source)
            {
                object destElement = Activator.CreateInstance<TModelViewType>();
                foreach (var sourceProperty in sourceElement.GetType().GetProperties()
                    .Where(s => modelViewProperties.Select(m => m.Name).Contains(s.Name)))
                {
                    destElement.GetType().GetProperty(sourceProperty.Name).SetValue(destElement, sourceProperty.GetValue(sourceElement, null));
                }
                destinationList.Add((TModelViewType)destElement);
            }

            return destinationList.Cast<object>().ToList();// destinationList.AsQueryable();
        }

        internal static bool HasAttribute(this PropertyInfo property, object attributeType)
        {
            return property.GetCustomAttributes()
                .FirstOrDefault(a => a.TypeId.Equals(attributeType)) != null;
        }

        public static System.Web.Mvc.MvcHtmlString ShonizGrid(this System.Web.Mvc.HtmlHelper helper, GridModel gridModel)
        {
            var gridContext = new GridContext(gridModel);
            return gridContext.InitialGrid();
        }
    }
}