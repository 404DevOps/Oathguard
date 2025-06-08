using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class IdentifierService
{
    public static string GetNextId()
    {
        var guid = Guid.NewGuid().ToString();
        return guid;
    }
}