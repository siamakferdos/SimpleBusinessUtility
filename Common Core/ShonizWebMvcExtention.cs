using System.Web.Mvc;

namespace Shoniz.Extention
{
    public static class ShonizWebMvcExtention
    {
        /// <summary>
        /// Make ViewDataDictionary prefixe to the specified hepler.
        /// </summary>
        /// <param name="hepler">The hepler.</param>
        /// <param name="prefix">The prefix.</param>
        /// <returns>System.Web.Mvc.ViewDataDictionary</returns>
        public static ViewDataDictionary Prefix(this HtmlHelper hepler, string prefix)
        {
            return new ViewDataDictionary()
            {
                TemplateInfo = new TemplateInfo() { HtmlFieldPrefix = prefix }
            };
        }

        /// <summary>
        /// This extention method is for render a Partial view By it's Name and Model that can be used in a Json value
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="partialViewName">Name of the view.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static string PartialView(this Controller controller, string partialViewName, object model)
        {
            controller.ViewData.Model = model;

            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, partialViewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData,
                    controller.TempData, sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                return sw.ToString();
            }
        }

        /// <summary>
        /// Captures the specified result. This method recall an Action method
        /// </summary>
        /// <param name="result">The result in a string Format</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <returns></returns>
        public static string Capture(this ActionResult result, ControllerContext controllerContext)
        {
            var response = controllerContext.RequestContext.HttpContext.Response;
            var localWriter = new System.IO.StringWriter();
            response.Output = localWriter;
            result.ExecuteResult(controllerContext);

            localWriter.Flush();

            return localWriter.ToString();

        }

    }
}
