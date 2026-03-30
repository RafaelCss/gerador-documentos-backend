using System.Reflection;

namespace GeradorDocumentos.Api;

public static class Metadado
{
    public static Assembly GetAssembly() => typeof(Metadado).Assembly;
}
