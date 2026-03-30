using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GeradorDocumentos.Aplicacao;
public static class Metadado
{
    public static Assembly GetAssembly() => typeof(Metadado).Assembly;
}