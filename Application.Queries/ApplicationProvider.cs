using System.Reflection;

namespace Application.Commands
{
    public static class ApplicationQueryProvider
    {
        public static Assembly QueryAssembly => Assembly.GetExecutingAssembly();
    }
}
