using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IoC = NetSteps.Encore.Core.IoC;
using NetSteps.SOD.Common;
using NetSteps.SOD.Common.Interfaces;

namespace NetSteps.SOD.Common
{
    public class SODWrapper : ISODWrapper
    {
        public IResponse Create(ICreateParameters createParams)
        {
            return IoC.Create.NewDto<IResponse>();
        }

        public IResponse Update(IUpdateParameters updateParams)
        {
            return IoC.Create.NewDto<IResponse>();
        }

        public IResponse Login(ILoginParameters updateParams)
        {
            return IoC.Create.NewDto<IResponse>();
        }
    }
}
