using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Dto
{
        public class E080XmlDto
        {
            #region Properties
            public string Login { get; set; }
            public string Password { get; set; }
            #endregion
        }

        public class E080XmlDtoConsultores
        {
            #region Properties
            public string CodConsultor { get; set; }
            public string UserName { get; set; }
            public string Token { get; set; }
            #endregion
        }
}
