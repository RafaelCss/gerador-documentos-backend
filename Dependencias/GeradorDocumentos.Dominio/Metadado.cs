using System.Reflection;


namespace GeradorDocumentos.Dominio;

public static class Metadado
{
    public static Assembly GetAssembly() => typeof(Metadado).Assembly;
}
