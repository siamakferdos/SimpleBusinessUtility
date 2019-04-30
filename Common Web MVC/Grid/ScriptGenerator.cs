using System.Collections.Generic;
using System.Text;

namespace Shoniz.Common.Web.MVC.Grid
{
    internal static class ScriptGenerator<T>
    {
        private static GridModel<T> _gridModel;

        /// <summary>
        /// Gets the grid script. This is the main method of generating script that gather all sripts
        /// </summary>
        /// <param name="gridModel">The grid model.</param>
        /// <returns></returns>
        internal static string GetGridScript(GridModel<T> gridModel)
        {
            _gridModel = gridModel;

            var script = new StringBuilder();

            script.AppendLine("<script>");
            script.AppendLine(GenerateOffEvents());
            script.AppendLine(GenerateInitialJob());
            script.AppendLine(GeneratePageNumberClick());
            script.AppendLine(GeneratePageMoveClick());
            script.AppendLine(GenerateSortingClick());
            script.AppendLine(GenerateTdClick());
            script.AppendLine(GenerateRowDblClick());
            script.AppendLine(GenerateMouseOver());
            script.AppendLine(GenerateDeleteLinkClick());

            script.AppendLine("</script>");

            return script.ToString();
        }

        private static string GenerateOffEvents()
        {
            var func = string.Format("$('body').off('click', '#GridContainer{0} tbody tr td');", _gridModel.GridName);
            func += string.Format("$('body').off('dblclick', '#GridContainer{0} tbody tr td');", _gridModel.GridName);
            return func;
        }

        /// <summary>
        /// Generates the initial job script.
        /// </summary>
        /// <returns></returns>
        private static string GenerateInitialJob()
        {
            var func = new StringBuilder();

            //if grid is selectable make it
            func.AppendFormat("if ('{0}' != '{1}' && '{2}' != '{4}') " +
                                      "{{" +
                                      "$('#GridContainer{4} table').addClass('selectableGrid');" +
                                      "}}",
                _gridModel.SelectingMode, GridSelectMode.None,
                _gridModel.EditingMode, GridEditMode.Single, _gridModel.GridName);

            //چک کردن اینکه آیا استایل های خاص این گرید درج گشته اند یا نه و درج آنها
            func.AppendFormat(
                    "if ('{0}' != '')if (!$('head style').hasClass('{1}StyleSheet'))" +
                    " {{$('head').append(\"<style class = '{1}StyleSheet'>" +
                    "{0}</style>\");}}", _gridModel.Styles.GetGridStyle(), _gridModel.GridName);

            //چک کردن اینکه آیا استایل های خاص ستون های خاص این گرید درج گشته اند یا نه و درج آنها

            func.AppendFormat(
                    "if ('{1}' != '')" +
                    "if (!$('head style').hasClass('{0}ColumnStyleSheet')) " +
                    "{{" +
                    "$('head').append(\"<style class = '{0}ColumnStyleSheet'>" +
                    "{1}</style>\");" +
                    "}}", _gridModel.GridName, _gridModel.Styles.GetGridStyle());

            //تنظیم اولیه شماره صفحه برای استایل دهی
            func.AppendFormat(
                    "$('#Div{0} span[pageNumber = \"{1}\"]').addClass('ActivePage');",
                    _gridModel.GridName, _gridModel.CurrentPageIndex);

            return func.ToString();
        }

        /// <summary>
        /// Generates the sorting click.
        /// </summary>
        /// <returns></returns>
        private static string GenerateSortingClick()
        {
            var func = new StringBuilder();
            return string.Format(
                "$('.{3}Header th').click(function () {{" +
                "if ($(this).text().length > 0)" +
                "var myurl = '/{0}/{1}?PageRecordCount={2}&CurrentPageIndex=1&GridName={3}&" +
                "filters={4}&orderField=' + $(this).attr('data-{6}-name') + '&extraParam={5}'; " +
                "var mytarget = $(this).closest(\".webGridContainer\").parent(); " +
                "DoGlobalAjax(myurl, '', mytarget, '', ''); " +
                "}}); ",
                _gridModel.Controller, _gridModel.ActionName, _gridModel.PageRecordCount,
                _gridModel.GridName, _gridModel.Filters, _gridModel.ExtraParam, _gridModel.GridName);
        }

        /// <summary>
        /// Generates the page number click.
        /// </summary>
        /// <returns></returns>
        private static string GeneratePageNumberClick()
        {
            //متد کنترل تغییر صفحات در حین کلیک بر روی خود شماره صفحات
            return
                string.Format(
                    "$('body').off('click', '#Div{0} .pageNumber');" +
                    "$('body').on('click', '#Div{0} .pageNumber', function () {{" +
                    "if (!$(this).hasClass('ActivePage')) " +
                    "{{var myurl = '/{1}/{2}?pageRecordCount={3}&currentPageIndex=' + $(this).attr('pagenumber').toString() " +
                    "+'&gridName={0}&filters={4}&OrderField={5}&extraParam={7}';" +
                    "var mytarget = $(this).closest('.webGridContainer').parent();" +
                    "DoGlobalAjax(myurl,'', mytarget, '', '{6}');" +
                    "}};}});", _gridModel.GridName, _gridModel.Controller, _gridModel.ActionName,
                    _gridModel.PageRecordCount,
                    _gridModel.Filters, _gridModel.OrderField, string.IsNullOrWhiteSpace(_gridModel.AjaxLoaderTargetSelector) ? "body" : _gridModel.AjaxLoaderTargetSelector,
                    _gridModel.ExtraParam);
        }

        /// <summary>
        /// Generates the page move click.
        /// </summary>
        /// <returns></returns>
        private static string GeneratePageMoveClick()
        {
            //متد تغییر صفحات در زمان کلیک >> > < <<
            return
                string.Format(
                    "$('#Div{0} .pageMove').on('click', function () {{" +
                    "var mytarget = $(this).closest('.webGridContainer').parent();" +
                    "var pageindex = 1;" +
                    "if ($(this).attr('pagemovetype') == 'Forward')" +
                    "pageindex = parseInt($('#Div{0} .ActivePage').text()) + 1;" +
                    "else if ($(this).attr('pagemovetype') == 'Back')" +
                    "pageindex = parseInt($('#Div{0} .ActivePage').text()) - 1;" +
                    "else if ($(this).attr('pagemovetype') == 'Last') {{" +
                    "if (({1} % {4}) == 0)" +
                    " {{pageindex = {1} / {4};}}" +
                    "else {{" +
                    "pageindex = Math.floor({1} / {4}) + 1;}} }}" +
                    "var myurl = '/{2}/{3}?PageRecordCount={4}&GridName={0}&CurrentPageIndex=' + pageindex +'&" +
                    "Filters={5}&OrderField={6}&extraParam={7}';" +
                    "DoAjax(myurl, mytarget, '');" +
                    "}" +
                    "});",
                    _gridModel.GridName, _gridModel.RecordCount, _gridModel.Controller, _gridModel.ActionName,
                    _gridModel.PageRecordCount,
                    _gridModel.Filters, _gridModel.OrderField
                    ,_gridModel.ExtraParam
                    );
        }

        /// <summary>
        /// Generates the td click.
        /// </summary>
        /// <returns></returns>
        private static string GenerateTdClick()
        {
            

            return string.Format(
                "$(document).on('click', '#GridContainer{0} tbody tr td', function (e) {{" +
                "e.stopImmediatePropagation();var s = '';" +
                "if ('{1}' == '{2}' && $(this).attr('data-grid-name') == 'edit') {{" +
                "$(this).closest('tr').find('td[isPrimary]').each(function () {{" +
                "if (s.length > 0)s += ',';" +
                "s += $(this).attr('data-grid-name') + ':' + $(this).parent().text();}});" +
                "$.ajax({{type: 'POST',url: '/{3}/{4}',data: JSON.stringify({{ pk: s }})," +
                "contentType: 'application/json; charset=utf-8',dataType: 'json'}});}}" +
                "else if ('{5}' == '{6}') {{" +
                "$(this).closest('tr').toggleClass('{0}SelectedRow');" +
                "$(this).closest('tr').toggleClass('gridSelectedRow');}}" +
                "else if ('{5}' == '{7}') " +
                "{{" +
                "var checkSelectiveClass = $(this).hasClass('{0}SelectedRow'); " +
                "$(this).closest('table').find('tr').removeClass('{0}SelectedRow');" +
                "$(this).closest('table').find('tr').removeClass('gridSelectedRow');" +
                "if(!checkSelectiveClass){{ $(this).closest('tr').addClass('{0}SelectedRow');" +
                "$(this).closest('tr').addClass('gridSelectedRow');}}}}" +
                "var selectedPrimaryKeys = '';var selectedRow = '';var rowVal = '';" +
                "$('#GridContainer{0} tbody  .{0}SelectedRow').each(function () {{" +
                "s = '';$(this).closest('tr').find('td[isPrimary]').each(function () {{" +
                "if (s.length > 0)s += ',';" +
                "s += $(this).attr('data-grid-name') + ':' + $(this).text();}});" +
                "$(this).closest('tr').find('td').each(function () {{" +
                "if (rowVal.length > 0)rowVal += ',';rowVal += $(this).attr('data-grid-name') + ':' + $(this).text();}});" +
                "if (selectedPrimaryKeys.length > 0)selectedPrimaryKeys += '*';selectedPrimaryKeys += s;" +
                "if (selectedRow.length > 0)selectedRow += '*';selectedRow += rowVal;}});" +
                "$(\"input[name='{0}SelectedRow']\").val(selectedPrimaryKeys);" +
                "$(\"input[name='{0}SelectedRowValue']\").val(selectedRow);" +
                "if($(this).parent().hasClass('{0}SelectedRow')) $(this).parent().removeClass('{0}MouseoverRow');" +
                "if('{8}' != '') window['{8}']($(this));}});"
                , _gridModel.GridName, _gridModel.EditingMode, GridEditMode.Single, _gridModel.EditController,
                _gridModel.ActionName, _gridModel.SelectingMode, GridSelectMode.Multiple, GridSelectMode.Single, _gridModel.OnCellClick);
        }

        private static string GenerateRowDblClick()
        {
            if(!string.IsNullOrWhiteSpace(_gridModel.OnCellDblClick))
                return string.Format("$('body').on('dblclick', '#GridContainer{0} tbody tr td', function (e) {{" +
                                 "if('{1}' != '') window['{1}']($(this));return false;}});", _gridModel.GridName, _gridModel.OnCellDblClick);
            return "";
        }

        /// <summary>
        /// Generates the mouse over.
        /// </summary>
        /// <returns></returns>
        private static string GenerateMouseOver()
        {
            return string.Format(
                "$('#GridContainer{0} .{0}Body tr td').on('mouseover', function () {{" +
                "if(!$(this).parent().hasClass('{0}SelectedRow'))" +
                "$(this).closest('tr').addClass('{0}MouseoverRow');}});" +
                "$('#GridContainer{0} .{0}Body tr td').on('mouseleave', function () {{" +
                "$(this).closest('tr').removeClass('{0}MouseoverRow');}});"
                , _gridModel.GridName);
        }

        /// <summary>
        /// Generates the delete link click.
        /// </summary>
        /// <returns></returns>
        private static string GenerateDeleteLinkClick()
        {
            return string.Format(
                "$(\"#GridContainer{0} td[data-grid-name='delete']\").on('click', function () {{" +
                "if (!$(this).attr('data-grid-actOnClient')) {{" +
                "var s = '';" +
                "$(this).closest('tr').find('td[isPrimary]').each(function () {{" +
                "if (s.length > 0)s += ',';" +
                "s += $(this).attr('data-grid-name') + ':' + $(this).text();}});" +
                "var url = '/{1}/{2}?pk=' + s;" +
                "DoAjax(url, $(this).closest('.webGridContainer').parent(), '');}}" +
                "else {{$(this).closest('tr').remove();}} }} );", _gridModel.GridName,
                _gridModel.DeleteController, _gridModel.DeleteAction);
        }

        //private static string GenerateShowDetailLinkClick()
        //{
        //    return string.Format(
        //        " if($(this).parent().find(\"td[data-grid-name='detail']\").length > 0){{ $(this).parent().find(\"td[data-grid-name='detail']\").sildeDown(); return;}}" +
        //        "$(\"#GridContainer{0} td[data-grid-name='showDetail']\").on('click', function () {{" +
        //            "var s = '';" +
        //            "$(this).closest('tr').find('td[isPrimary]').each(function () {{" +
        //            "if (s.length > 0)s += ',';" +
        //            "s += $(this).attr('data-grid-name') + ':' + $(this).text(););" +
        //        "var url = '/{1}/{2}?pk=' + s;" +

        //        "DoGlobalAjax(url, null, $(this).closest('.webGridContainer').parent(), '');}}" +
        //        "else {{$(this).closest('tr').remove();}} }} );", _gridModel.GridName,
        //        _gridModel.DetailController, _gridModel.DetailAction);
        //}

        //private static string SetZeroToEmptyTextBox()
        //{
        //    return "alert(); $(document).find(\".webGridContainer input[requiredOnEditByZeroDefault]\")."+ 
        //        "each(function(){if($(this).val() = \"\")$(this).val(\"0\");}); ";
        //}

        //private static string SetZeroToEmptyTextBoxOnTextChange()
        //{
        //    return " $(document).find(\".webGridContainer input[requiredOnEditByZeroDefault]\")." +
        //        ".focusout()(function(){SetZeroToEmptyTextBox();}); ";
        //}
        

    }
}