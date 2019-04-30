using System.Text;

namespace Shoniz.MVCGrid
{
    internal static class ScriptGenerator
    {
        private static GridModel _gridModel;

        /// <summary>
        /// Gets the grid script. This is the main method of generating script that gather all sripts
        /// </summary>
        /// <param name="gridModel">The grid model.</param>
        /// <returns></returns>
        public static string GetGridScript(GridModel gridModel)
        {
            _gridModel = gridModel;

            var script = new StringBuilder();

            script.Append("<script>$(function () {");
            script.Append(GenerateInitialJob());
            script.Append(GeneratePageNumberClick());
            script.Append(GeneratePageMoveClick());
            script.Append(GenerateSortingClick());
            script.Append(GenerateTdClick());
            script.Append(GenerateMouseOver());
            script.Append(GenerateDeleteLinkClick());
            script.Append("});</script>");

            return script.ToString();
        }


        /// <summary>
        /// Generates the initial job script.
        /// </summary>
        /// <returns></returns>
        private static string GenerateInitialJob()
        {
            var func = new StringBuilder();

            //if grid is selectable make it
            func.Append(string.Format("if ('{0}' != '{1}' && '{2}' != '{4}') " +
                                      "{{" +
                                      "$('#GridContainer{4} table').addClass('selectableGrid');" +
                                      "}}",
                _gridModel.SelectingMode, ShonizGridSelectMode.None,
                _gridModel.EditingMode, ShonizGridEditMode.Single, _gridModel.GridName));

            //چک کردن اینکه آیا استایل های خاص این گرید درج گشته اند یا نه و درج آنها
            func.Append(
                string.Format(
                    "if ('{0}' != '')if (!$('head style').hasClass('{1}StyleSheet'))" +
                    " {{$('head').append(\"<style class = '{1}StyleSheet'>" +
                    "{0}</style>\");}}", _gridModel.Styles.GetGridStyle(), _gridModel.GridName));

            //چک کردن اینکه آیا استایل های خاص ستون های خاص این گرید درج گشته اند یا نه و درج آنها

            func.Append(
                string.Format(
                    "if ('{1}' != '')" +
                    "if (!$('head style').hasClass('{0}ColumnStyleSheet')) " +
                    "{{" +
                    "$('head').append(\"<style class = '{0}ColumnStyleSheet'>" +
                    "{1}</style>\");" +
                    "}}", _gridModel.GridName, _gridModel.Styles.GetGridStyle()));

            //تنظیم اولیه شماره صفحه برای استایل دهی
            func.Append(
                string.Format(
                    "$('#Div{0} span[pageNumber = \"{1}\"]').addClass('ActivePage');",
                    _gridModel.GridName, _gridModel.CurrentPageIndex));

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
                "var myurl = '/{0}/{1}?PageRecordCount={2} & CurrentPageIndex=1 & GridName={3} & " +
                "Filters=' + '{4} & OrderField=' + $(this).attr(\"data-lab-name\"); " +
                "var mytarget = $(this).closest(\".webGridContainer\").parent(); " +
                "DoAjax(myurl, mytarget, \"\"); " +
                "}}); ",
                _gridModel.Controller, _gridModel.ActionName, _gridModel.PageRecordCount,
                _gridModel.GridName, _gridModel.Filters);
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
                    "$('#Div{0} .pageNumber').on('click', function () {{" +
                    "if (!$(this).hasClass('ActivePage')) " +
                    "{{var myurl = '/{1}/{2}?PageRecordCount={3}&CurrentPageIndex=' + $(this).attr('pagenumber').toString() " +
                    "+'&GridName={0}&Filters={4}&OrderField={5}';" +
                    "var mytarget = $(this).closest('.webGridContainer').parent();DoAjax(myurl, mytarget, '');" +
                    "}};}});", _gridModel.GridName, _gridModel.Controller, _gridModel.ActionName,
                    _gridModel.PageRecordCount,
                    _gridModel.Filters, _gridModel.OrderField);
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
                    "pageindex = {1} / {4} + 1;}} }}" +
                    "var myurl = '/{2}/{3}?PageRecordCount={4}&GridName={0}&CurrentPageIndex=' + pageindex +'&" +
                    "Filters={5}&OrderField={6}';" +
                    "DoAjax(myurl, mytarget, '');" +
                    "}" +
                    "});",
                    _gridModel.GridName, _gridModel.RecordCount, _gridModel.Controller, _gridModel.ActionName,
                    _gridModel.PageRecordCount,
                    _gridModel.Filters, _gridModel.OrderField
                    );
        }

        /// <summary>
        /// Generates the td click.
        /// </summary>
        /// <returns></returns>
        private static string GenerateTdClick()
        {
            return string.Format(
                "$('#GridContainer{0} tbody td').click(function () {{" +
                "var s = '';" +
                "if ('{1}' == '{2}' && $(this).attr('data-lab-name') == 'edit') {{" +
                "$(this).closest('tr').find('td[isPrimary]').each(function () {{" +
                "if (s.length > 0)s += ',';" +
                "s += $(this).attr('data-lab-name') + ':' + $(this).text();}});" +
                "$.ajax({{type: 'POST',url: '/{3}/{4}',data: JSON.stringify({{ pk: s }})," +
                "contentType: 'application/json; charset=utf-8',dataType: 'json'}});}}" +
                "else if ('{5}' == '{6}') {{" +
                "$(this).closest('tr').toggleClass('{0}SelectedRow');}}" +
                "else if ('{5}' == '{7}') " +
                "{{$(this).closest('table').find('tr').removeClass('{0}SelectedRow');" +
                "$(this).closest('tr').addClass('{0}SelectedRow');}}" +
                "var selectedPrimaryKeys = '';var selectedRow = '';var rowVal = '';" +
                "$('#GridContainer{0} tbody  .{0}SelectedRow').each(function () {{" +
                "s = '';$(this).find('td[isPrimary]').each(function () {{" +
                "if (s.length > 0)s += ',';" +
                "s += $(this).attr('data-lab-name') + ':' + $(this).text();}});" +
                "$(this).find('td').each(function () {{" +
                "if (rowVal.length > 0)rowVal += ',';rowVal += $(this).attr('data-lab-name') + ':' + $(this).text();}});" +
                "if (selectedPrimaryKeys.length > 0)selectedPrimaryKeys += '*';selectedPrimaryKeys += s;" +
                "if (selectedRow.length > 0)selectedRow += '*';selectedRow += rowVal;}});" +
                "$(\"input[name='{0}SelectedRow']\").val(selectedPrimaryKeys);" +
                "$(\"input[name='{0}SelectedRowValue']\").val(selectedRow);}});"
                , _gridModel.GridName, _gridModel.EditingMode, ShonizGridEditMode.Single, _gridModel.EditController,
                _gridModel.ActionName, _gridModel.SelectingMode, ShonizGridSelectMode.Multiple, ShonizGridSelectMode.Single);
        }

        /// <summary>
        /// Generates the mouse over.
        /// </summary>
        /// <returns></returns>
        private static string GenerateMouseOver()
        {
            return string.Format(
                "$('#GridContainer{0} .{0}Body tr td').on('mouseover', function () {{" +
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
                "$(\"#GridContainer{0} td[data-lab-name='delete']\").on('click', function () {{" +
                "if (!$(this).attr('data-lab-actOnClient')) {{" +
                "var s = '';" +
                "$(this).closest('tr').find('td[isPrimary]').each(function () {{" +
                "if (s.length > 0)s += ',';" +
                "s += $(this).attr('data-lab-name') + ':' + $(this).text();}});" +
                "var url = '/{1}/{2}?pk=' + s;" +
                "DoAjax(url, $(this).closest('.webGridContainer').parent(), '');}}" +
                "else {{$(this).closest('tr').remove();}} }});", _gridModel.GridName,
                _gridModel.DeleteController, _gridModel.DeleteAction);
        }

    }

}