using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GeradorDocumento.Ioc;

public static class Metadado
{
    public static Assembly GetAssembly() => typeof(Metadado).Assembly;
}