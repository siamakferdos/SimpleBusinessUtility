using System;
using System.Collections.Generic;
using System.Text;

namespace Shoniz.MVCGrid
{
    public class GridContext
    {
        readonly StringBuilder _gridHtml = new StringBuilder();
        readonly GridModel _model;
        public GridContext(GridModel model)
        {
            this._model = model;
        }

        public System.Web.Mvc.MvcHtmlString InitialGrid()
        {
            var rowNum = _model.CurrentPageIndex * _model.PageRecordCount - _model.PageRecordCount;
            var list = _model.GridDataSource;

            //EditingMode has override SelectingMode in some case
            if (_model.EditingMode == ShonizGridEditMode.All)
            {
                _model.SelectingMode = ShonizGridSelectMode.None;
            }

            var primaryKeyFields = new List<string>();

            if (list != null && list.Count > 0)
            {
                var props = list[0].GetType().GetProperties();
                _gridHtml.Append("<link href=\"/Content/GridStyle.css\" rel=\"stylesheet\" />");
                _gridHtml.Append(string.Format("<div id='GridContainer{0}' class='webGridContainer'>", _model.GridName));

                //_gridHtml.Append(string.Format("<div data-grid-name='{0}' data-grid-actionname='{1}'" +
                //                 " data-grid-controller='{2}' data-grid-currentpageindex='{3}'" +
                //                 " data-grid-deleteaction='{4}' data-grid-deletecontroller='{5}'" +
                //                 " data-grid-editaction='{6} ' data-grid-editcontroller='{7}'" +
                //                 " data-grid-editingmode='{8} ' data-grid-filters='{9}'" +
                //                 " data-grid-orderfield='{10}' data-grid-pagerecordcount='{11}' " +
                //                 " data-grid-recordcount='{12}'data-grid-selectingmode='{13}'></div>",
                //                 _model.GridName, _model.ActionName, _model.Controller, _model.CurrentPageIndex, _model.DeleteAction
                //                 , _model.DeleteController, _model.EditAction, _model.EditController, _model.EditingMode, _model.Filters
                //                 , _model.OrderField, _model.PageRecordCount, _model.RecordCount, _model.SelectingMode));

                _gridHtml.Append(string.Format("<table class='webgrid {0} table {1}' id='Grid{0}'>" +
                                               "<input type=hidden id='_Grid_*Grids*{0}' />" +
                                               "<thead class='{0}Header'><tr>",
                    _model.GridName, _model.Styles.TableAdditionalClasses));
                if (_model.HasRowCounter)
                {
                    _gridHtml.Append(string.Format("<th>{0}</th>", _model.RowCounterTitle));
                }

                foreach (var p in props)
                {
                    if ((p.HasAttribute(typeof(GridCustomAttribute)) && !p.GetGridCustomAttribute().Excluded) ||
                        !p.HasAttribute(typeof(GridCustomAttribute)))
                    {
                        string columnHeaderDisplayStyle = "";

                        if (_model.HiddenFieldList.Contains(p.Name))
                        {
                            columnHeaderDisplayStyle = "display:none;";
                        }

                        if (p.GetGridCustomAttribute() != null)
                        {
                            if (p.GetGridCustomAttribute().Hidden)
                            {
                                columnHeaderDisplayStyle = "display:none;";
                            }
                        }
                        _gridHtml.Append("<th data-lab-name='" + p.Name + "' style='" + columnHeaderDisplayStyle + "'>");
                        _gridHtml.Append(p.DisplayName());

                        GridCustomAttribute attr = p.GetGridCustomAttribute();
                        if (attr != null)
                        {
                            if (attr.PrimaryKey || attr.CanEdit)
                            {
                                if (attr.PrimaryKey)
                                {
                                    _gridHtml.Append(string.Format("<input type=hidden id='_Grid_*PrimaryKeys*{0}*{1} />",
                                        _model.GridName, p.Name));
                                    primaryKeyFields.Add(p.Name);
                                }
                                _gridHtml.Append(string.Format("<input type=hidden id='_Grid_*Fields*{0}*{1} />", _model.GridName, p.Name));
                            }
                        }

                        _gridHtml.Append("</th>");
                    }
                }
                if (_model.EditingMode == ShonizGridEditMode.Single)
                {
                    _gridHtml.Append("<th></th>");
                }
                if (_model.AllowDelete)
                {
                    _gridHtml.Append("<th></th>");
                }
                _gridHtml.Append("</tr></thead>");

                _gridHtml.Append(string.Format("<tbody class='{0}Body {0}AlternativeRow'>", _model.GridName));
                foreach (var r in list)
                {
                    rowNum++;
                    props = r.GetType().GetProperties();
                    _gridHtml.Append("<tr>");
                    if (_model.HasRowCounter)
                    {
                        _gridHtml.Append("<td>" + rowNum + "</td>");
                    }
                    foreach (var p in props)
                    {
                        if ((!p.HasAttribute(typeof(GridCustomAttribute)) || p.GetGridCustomAttribute().Excluded) &&
                            p.HasAttribute(typeof(GridCustomAttribute))) continue;
                        var tdStyle = "";
                        if (_model.HiddenFieldList.Contains(p.Name))
                        {
                            tdStyle = "display:none";
                        }
                        var customTd = "<td data-lab-name=\"" + p.Name + "\" ";

                        if (_model.Styles.GetStyledColumns().Contains(p.Name))
                        {
                            customTd += " class= '" + _model.GridName + p.Name + "' ";
                        }

                        var customValue = "";
                        try
                        {
                            customValue = p.GetValue(r).ToString();
                        }
                        catch
                        {
                        }
                        if (String.Equals(customValue.Trim(), "False", StringComparison.CurrentCultureIgnoreCase))
                        {
                            customValue = "<i class='fa fa-times'></i>";
                        }
                        else if (String.Equals(customValue.Trim(), "True", StringComparison.CurrentCultureIgnoreCase))
                        {
                            customValue = "<i class='fa fa-check'></i>";
                        }
                        if (p.HasAttribute(typeof(GridCustomAttribute)))
                        {
                            var attr = p.GetGridCustomAttribute();
                            if (attr.Hidden && tdStyle == "")
                            {
                                tdStyle += "display:None";
                            }
                            if (attr.PrimaryKey)
                            {
                                customTd += " isPrimary='true' ";

                            }
                            if (attr.CanEdit)
                            {
                                customTd += " canEdit='true' ";
                                if (_model.EditingMode == ShonizGridEditMode.All)
                                {
                                    var primaryKeyString = "";
                                    for (var i = 0; i < primaryKeyFields.Count; i++)
                                    {
                                        primaryKeyString += r.GetType().GetProperty(primaryKeyFields[i]).Name + "=" +
                                                            r.GetType()
                                                                .GetProperty(primaryKeyFields[i])
                                                                .GetValue(r);
                                        if (i < primaryKeyFields.Count - 1)
                                        {
                                            primaryKeyString += ",";
                                        }
                                    }


                                    if (("FalseTrue").ToUpper().Contains(p.GetValue(r).ToString().Trim().ToUpper()))
                                    {
                                        var selectedValue = "";
                                        if (customValue == "True")
                                        {
                                            selectedValue = "Selected";
                                        }

                                        customTd += "value=" + p.GetValue(r).ToString() +
                                                    "><input type=\"checkbox\" " + selectedValue +
                                                    " data-lab-name =\"_Grid_*Value*" + _model.GridName + "*" +
                                                    primaryKeyString + "*" + p.Name + "\" " +
                                                    "  id =\"GridTextbox" + "-" + rowNum + "-" + p.Name +
                                                    "\" style='" + tdStyle + "'></td>";
                                    }
                                    else
                                    {
                                        customTd += "><input type=\"text\" value=\"" + customValue +
                                                    "\" data-lab-name =\"_Grid_*Value*" + _model.GridName + "*" +
                                                    primaryKeyString + "*" + p.Name + "\" " +
                                                    "  id =\"GridTextbox" + "-" + rowNum + "-" + p.Name +
                                                    "\" style='" + tdStyle + "'></td>";
                                    }
                                }
                                else
                                {
                                    customTd += " style='" + tdStyle + "'>" + customValue + "</td>";
                                }

                            }
                            else
                            {
                                if (("FalseTrue").ToUpper().Contains(p.GetValue(r).ToString().Trim().ToUpper()))
                                {
                                    customTd += "value=" + p.GetValue(r).ToString() + "  style='" + tdStyle + "'>" +
                                                customValue + "</td>";
                                }
                                else
                                {
                                    customTd += "  style='" + tdStyle + "'>" + customValue + "</td>";
                                }
                            }
                        }
                        else
                        {
                            if (("FalseTrue").ToUpper().Contains(p.GetValue(r).ToString().Trim().ToUpper()))
                            {
                                customTd += "value=" + p.GetValue(r) + "  style='" + tdStyle + "'>" +
                                            customValue + "</td>";
                            }
                            else
                            {
                                customTd += "  style='" + tdStyle + "'>" + customValue + "</td>";
                            }
                        }
                        _gridHtml.Append(customTd);
                    }
                    if (_model.EditingMode == ShonizGridEditMode.Single)
                    {
                        _gridHtml.Append(
                            "<td data-lab-name='edit'><i class='fa  fa-pencil'></i>" + _model.EditLinkCaption + "</td>");
                    }
                    if (_model.AllowDelete)
                    {
                        _gridHtml.Append("<td data-lab-name='delete'");
                        if (_model.ActOnClient)
                        {
                            _gridHtml.Append("data-lab-actonclient='true'");
                        }

                        _gridHtml.Append("><i class='fa  fa-trash-o'></i>" + _model.DeleteLinkCaption + "</td>");
                    }
                }
                _gridHtml.Append("</tr>");
            }
            _gridHtml.Append("</tbody>");
            _gridHtml.Append("<tfoot class='" + _model.GridName + "GridFooter'></tfoot></table>");

            _gridHtml.Append(string.Format(
                "<div class='webgrid-pager {0}Pager' style='text-align:center;' id='Div{0}'>", _model.GridName));

            int pageCount = 1;
            if (_model.RecordCount % _model.PageRecordCount == 0)
            {
                pageCount = _model.RecordCount / _model.PageRecordCount;
            }
            else
            {
                pageCount = _model.RecordCount / _model.PageRecordCount + 1;
            }

            if (pageCount != 1)
            {
                if (pageCount < 6)
                {
                    for (int i = 1; i <= pageCount; i++)
                    {
                        _gridHtml.Append(
                            string.Format("<span class='pageNumber' pagenumber='{0}'>{0}</span>", i));
                    }
                }
                else if (_model.CurrentPageIndex <= 3)
                {
                    var forwardCount = 0;
                    for (var i = 1; i <= pageCount && ++forwardCount < 6; i++)
                    {
                        _gridHtml.Append(
                            string.Format("<span class='pageNumber' pagenumber='{0}'>{0}</span>", i));
                    }
                    if (forwardCount > 5)
                    {
                        _gridHtml.Append(
                            "<span class='pageMove' pagemovetype='Forward'><i class='fa fa-angle-left'></i></span>" +
                            "<span class='pageMove' pagemovetype='Last'><i class='fa fa-angle-double-left'></i></span>");
                    }
                }
                else
                {
                    _gridHtml.Append(
                        "<span class='pageMove' pagemovetype='First'><i class='fa fa-angle-double-right'>" +
                        "</i></span><span class='pageMove' pagemovetype='Back'><i class='fa fa-angle-right'></i></span>");
                    var forwardCount = 0;
                    for (int i = _model.CurrentPageIndex - 2; i <= pageCount && ++forwardCount < 6; i++)
                    {
                        _gridHtml.Append(
                            string.Format("<span class='pageNumber' pagenumber='{0}'>{0}</span>", i));
                    }
                    if (forwardCount > 5)
                    {
                        _gridHtml.Append(
                            "<span class='pageMove' pagemovetype='Forward'><i class='fa fa-angle-left'></i>" +
                            "</span><span class='pageMove' pagemovetype='Last'><i class='fa fa-angle-double-left'></i></span>");
                    }
                }
            }


            _gridHtml.Append("</div></div>");


            if (_model.SelectingMode != ShonizGridSelectMode.None || _model.EditingMode == ShonizGridEditMode.Single)
            {
                _gridHtml.Append("<input type='hidden' id='" + _model.GridName + "SelectedRow' />");
                _gridHtml.Append("<input type='hidden' id='" + _model.GridName + "SelectedRowValue' />");
            }

            _gridHtml.Append("</div>");

            _gridHtml.Append(ScriptGenerator.GetGridScript(_model));

            return System.Web.Mvc.MvcHtmlString.Create(_gridHtml.ToString());

        }

    }
}
