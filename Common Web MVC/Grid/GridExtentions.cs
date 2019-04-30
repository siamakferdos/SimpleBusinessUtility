using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Shoniz.Common.Core;

namespace Shoniz.Common.Web.MVC.Grid
{
    public static class GridExtentions
    {
        public static string DisplayName(this PropertyInfo propertyInfo)
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

        public static List<System.Data.DataTable> ToGridDataTable(this System.Web.HttpRequestBase request,
            System.Collections.Specialized.NameValueCollection form)
        {
            var dataTableList = new List<System.Data.DataTable>();
            var gridNames = form.AllKeys
                .Where(k => k.StartsWith("Grid*primarykey"))
                .Select(key => key.Split('*')[2]).Distinct()
                .ToList();

            if (gridNames.Count < 1)
                return null;

            foreach (var gridName in gridNames)
            {
                var dt = new System.Data.DataTable(gridName);
                var primaryKeyList = new List<string>();
                var FieldList = new List<String>();

                if (form.AllKeys.Any(k => k.StartsWith("Grid*primarykey*" + gridName + "*")))
                {
                    foreach (var primary in form.AllKeys.Where(k => k.StartsWith("Grid*primarykey*" + gridName + "*")))
                    {
                        if (dt.Columns.Contains(primary.Split('*')[3]))
                            break;
                        dt.Columns.Add(primary.Split('*')[3]);
                        primaryKeyList.Add(primary.Split('*')[3]);
                    }
                }
                foreach (var fieldName in form.AllKeys.Where(k => k.StartsWith("Grid*input*" + gridName + "*")))
                {
                    if (dt.Columns.Contains(fieldName.Split('*')[3]))
                        break;
                    dt.Columns.Add(fieldName.Split('*')[3]);
                    FieldList.Add(fieldName.Split('*')[3]);
                }
                if(!FieldList.Any())
                    break;
                var indexList = new List<int>();
                if (form.AllKeys.Any(k => k.StartsWith("Grid*input*" + gridName + "*" + FieldList[0] + "*")))
                    indexList =
                        form.AllKeys.Where(k => k.StartsWith("Grid*input*" + gridName + "*" + FieldList[0] + "*")).Select(ind => (ind.Split('*')[4]).ToInt()).ToList();
                else if (form.AllKeys.Any(k => k.StartsWith("Grid*primarykey*" + gridName + "*" + primaryKeyList[0] + "*")))
                    indexList =
                        form.AllKeys.Where(k => k.StartsWith("Grid*primarykey*" + gridName + "*" + primaryKeyList[0] + "*")).Select(ind => (ind.Split('*')[4]).ToInt()).ToList();

                foreach (var i in indexList)
                {
                    var newRow = dt.NewRow();
                    foreach (var primary in primaryKeyList)
                    {
                        newRow[primary] = form["Grid*primarykey*" + gridName + "*" + primary + "*" + i];
                    }
                    foreach (var field in FieldList)
                    {
                        newRow[field] = form["Grid*input*" + gridName + "*" + field + "*" + i];
                    }

                    dt.Rows.Add(newRow);
                }
                dataTableList.Add(dt);
            }

            return dataTableList;
        }

        internal static List<T> ToPageX<T>(this IEnumerable<T> wholeList, int pageSize, int currentPage)
        {
            if (wholeList.Count() > (pageSize) * (currentPage - 1))
                return wholeList.Skip((pageSize) * (currentPage - 1)).Take(pageSize).ToList();
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

        public static System.Web.Mvc.MvcHtmlString ShonizGrid<T>(this System.Web.Mvc.HtmlHelper helper, GridModel<T> gridModel)
        {
            var gridContext = new GridContext<T>(gridModel);
            return gridContext.InitialGrid();
        }


        public static string ToPKGroupJson(this System.Collections.Specialized.NameValueCollection form, string gridName)
        {
            var keysList = form.AllKeys.Where(k => k.StartsWith("Grid*primarykey*" + gridName));
            if (!keysList.Any()) return null;
            string json = "";

            foreach (var k in keysList)
            {
                if (json != "")
                    json += ",";
                json += form[k];
            }


            return json;
        }
    }
}