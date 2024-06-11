using System;
using System.Collections.Generic;
using System.Text;

namespace Authentication.BusinessLogic.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(object key) : base($"Object (with id = {key}) was not found")
        {

        }
    }
}
