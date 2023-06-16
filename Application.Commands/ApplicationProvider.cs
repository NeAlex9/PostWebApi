using System.Reflection;

namespace Application.Commands
{
    public static class ApplicationCommandProvider
    {
        public static Assembly CommandsAssembly => Assembly.GetExecutingAssembly();
    }
}
