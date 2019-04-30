namespace Shoniz.Common.Data.DataConvertor.Mapper
{
    /// <summary>
    /// In converting from a source  to a target, sometimes target may has none valuetype property(like class). In this situation 
    /// they will be scaned into too. These properties will list in a queue that this class provides it.
    /// </summary>
    class QueueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueObject"/> class.
        /// </summary>
        /// <param name="objectFullSpec">The object full spec.</param>
        /// <param name="instanceOfObject">The instance of object.</param>
        public QueueObject(string objectFullSpec, object instanceOfObject)
        {
            InstanceOfObject = instanceOfObject;
            ObjectFullSpec = objectFullSpec;
        }

        public object InstanceOfObject;
        public string ObjectFullSpec;
    }
}
