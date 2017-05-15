using System;
using System.ServiceModel.Configuration;

namespace WCFWebService.MessageInspector
{
    public class MyBehaviorExtensionElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            return new MyOutputBehavior();
        }

        public override Type BehaviorType
        {
            get
            {
                return typeof(MyOutputBehavior);
            }
        }
    }
}
