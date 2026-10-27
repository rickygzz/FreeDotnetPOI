using System;

namespace NPOI.Util;

public sealed class AssertFailedException : ApplicationException
{
    public AssertFailedException(string message)
        : base(message)
    {

    }
}
