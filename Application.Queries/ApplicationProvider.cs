using System.Reflection;

namespace Application.Queries
{
    public static class ApplicationQueryProvider
    {
        public static Assembly QueryAssembly => Assembly.GetExecutingAssembly();
    }
}
